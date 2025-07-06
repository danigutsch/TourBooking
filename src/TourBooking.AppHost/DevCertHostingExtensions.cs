using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TourBooking.AppHost;

internal static class DevCertHostingExtensions
{
    /// <summary>
    /// Injects the ASP.NET Core HTTPS developer certificate into the resource via the specified environment variables when
    /// <paramref name="builder"/>.<see cref="IResourceBuilder{T}.ApplicationBuilder">ApplicationBuilder</see>.<see cref="IDistributedApplicationBuilder.ExecutionContext">ExecutionContext</see>.<see cref="DistributedApplicationExecutionContext.IsRunMode">IsRunMode</see><c> == true</c>.<br/>
    /// If the resource is a <see cref="ContainerResource"/>, the certificate files will be bind mounted into the container.
    /// </summary>
    /// <remarks>
    /// This method <strong>does not</strong> configure an HTTPS endpoint on the resource.
    /// Use <see cref="ResourceBuilderExtensions.WithHttpsEndpoint{TResource}"/> to configure an HTTPS endpoint.
    /// </remarks>
    public static IResourceBuilder<TResource> RunWithHttpsDevCertificate<TResource>(
        this IResourceBuilder<TResource> builder, string certFileEnv, string certKeyFileEnv, Action<string, string>? onSuccessfulExport = null)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode && builder.ApplicationBuilder.Environment.IsDevelopment())
        {
            builder.ApplicationBuilder.Eventing.Subscribe<BeforeStartEvent>(async (e, ct) =>
            {
                var logger = e.Services.GetRequiredService<ResourceLoggerService>().GetLogger(builder.Resource);

                // Export the ASP.NET Core HTTPS development certificate & private key to files and configure the resource to use them via
                // the specified environment variables.
                var (exported, certPath, certKeyPath) = await TryExportDevCertificateAsync(builder.ApplicationBuilder, logger);

                if (!exported)
                {
                    // The export failed for some reason, don't configure the resource to use the certificate.
                    return;
                }

                if (builder.Resource is ContainerResource containerResource)
                {
                    // Bind-mount the certificate files into the container.
                    const string DEV_CERT_BIND_MOUNT_DEST_DIR = "/dev-certs";

                    var certFileName = Path.GetFileName(certPath);
                    var certKeyFileName = Path.GetFileName(certKeyPath);

                    var bindSource = Path.GetDirectoryName(certPath) ?? throw new UnreachableException();

                    var certFileDest = $"{DEV_CERT_BIND_MOUNT_DEST_DIR}/{certFileName}";
                    var certKeyFileDest = $"{DEV_CERT_BIND_MOUNT_DEST_DIR}/{certKeyFileName}";

                    builder.ApplicationBuilder.CreateResourceBuilder(containerResource)
                        .WithBindMount(bindSource, DEV_CERT_BIND_MOUNT_DEST_DIR, isReadOnly: true)
                        .WithEnvironment(certFileEnv, certFileDest)
                        .WithEnvironment(certKeyFileEnv, certKeyFileDest);
                }
                else
                {
                    builder
                        .WithEnvironment(certFileEnv, certPath)
                        .WithEnvironment(certKeyFileEnv, certKeyPath);
                }

                if (onSuccessfulExport is not null)
                {
                    onSuccessfulExport(certPath, certKeyPath);
                }
            });
        }

        return builder;
    }

    private static async Task<(bool, string CertFilePath, string CertKeyFilPath)> TryExportDevCertificateAsync(IDistributedApplicationBuilder builder, ILogger logger)
    {
        // Exports the ASP.NET Core HTTPS development certificate & private key to PEM files using 'dotnet dev-certs https' to a temporary
        // directory and returns the path.
        var appNameHash = builder.Configuration["AppHost:Sha256"]![..10];
        var tempDir = Path.Combine(Path.GetTempPath(), $"aspire.{appNameHash}");
        var certExportPath = Path.Combine(tempDir, "dev-cert.pem");
        var certKeyExportPath = Path.Combine(tempDir, "dev-cert.key");

        if (File.Exists(certExportPath) && File.Exists(certKeyExportPath))
        {
            // Certificate already exported, return the path.
            logger.UsingPreviouslyExportedDevCertFiles(certExportPath, certKeyExportPath);
            return (true, certExportPath, certKeyExportPath);
        }

        if (File.Exists(certExportPath))
        {
            logger.DeletingPreviouslyExportedDevCertFile(certExportPath);
            File.Delete(certExportPath);
        }

        if (File.Exists(certKeyExportPath))
        {
            logger.DeletingPreviouslyExportedDevCertKeyFile(certKeyExportPath);
            File.Delete(certKeyExportPath);
        }

        if (!Directory.Exists(tempDir))
        {
            logger.CreatingExportDirectory(tempDir);
            Directory.CreateDirectory(tempDir);
        }

        string[] args = ["dev-certs", "https", "--export-path", $"\"{certExportPath}\"", "--format", "Pem", "--no-password"];
        var argsString = string.Join(' ', args);

        logger.RunningExportCommand($"dotnet {argsString}");
        var exportStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = argsString,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        using (var exportProcess = new Process { StartInfo = exportStartInfo })
        {
            Task? stdOutTask = null;
            Task? stdErrTask = null;

            try
            {
                try
                {
                    if (exportProcess.Start())
                    {
                        stdOutTask = ConsumeOutput(exportProcess.StandardOutput, logger.StandardOutput);
                        stdErrTask = ConsumeOutput(exportProcess.StandardError, logger.StandardError);
                    }
                }
                catch (Exception ex)
                {
                    logger.FailedToStartExportProcess(ex);
                    throw;
                }

                var timeout = TimeSpan.FromSeconds(5);
                using var cts = new CancellationTokenSource(timeout);
                await exportProcess.WaitForExitAsync(cts.Token);

                if (File.Exists(certExportPath) && File.Exists(certKeyExportPath))
                {
                    logger.DevCertExported(certExportPath, certKeyExportPath);
                    return (true, certExportPath, certKeyExportPath);
                }

                if (exportProcess.HasExited && exportProcess.ExitCode != 0)
                {
                    logger.ExportFailedWithExitCode(exportProcess.ExitCode);
                }
                else if (!exportProcess.HasExited)
                {
                    exportProcess.Kill(true);
                    logger.ExportTimedOut(timeout.TotalSeconds);
                }
                else
                {
                    logger.ExportFailedUnknownReason();
                }

                return default;
            }
            finally
            {
#pragma warning disable CA1508
                await Task.WhenAll(stdOutTask ?? Task.CompletedTask, stdErrTask ?? Task.CompletedTask);
#pragma warning restore CA1508
            }

            static async Task ConsumeOutput(TextReader reader, Action<string> callback)
            {
                var buffer = new char[256];
                int charsRead;

                while ((charsRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    callback(new string(buffer, 0, charsRead));
                }
            }
        }
    }
}

internal static partial class DevCertHostingExtensionsLogger
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Using previously exported dev cert files '{CertPath}' and '{CertKeyPath}'", EventName = "UsingPreviouslyExportedDevCertFiles")]
    public static partial void UsingPreviouslyExportedDevCertFiles(this ILogger logger, string certPath, string certKeyPath);

    [LoggerMessage(EventId = 1, Level = LogLevel.Trace, Message = "Deleting previously exported dev cert file '{CertPath}'", EventName = "DeletingPreviouslyExportedDevCertFile")]
    public static partial void DeletingPreviouslyExportedDevCertFile(this ILogger logger, string certPath);

    [LoggerMessage(EventId = 2, Level = LogLevel.Trace, Message = "Deleting previously exported dev cert key file '{CertKeyPath}'", EventName = "DeletingPreviouslyExportedDevCertKeyFile")]
    public static partial void DeletingPreviouslyExportedDevCertKeyFile(this ILogger logger, string certKeyPath);

    [LoggerMessage(EventId = 3, Level = LogLevel.Trace, Message = "Creating directory to export dev cert to '{ExportDir}'", EventName = "CreatingExportDirectory")]
    public static partial void CreatingExportDirectory(this ILogger logger, string exportDir);

    [LoggerMessage(EventId = 4, Level = LogLevel.Trace, Message = "Running command to export dev cert: {ExportCmd}", EventName = "RunningExportCommand")]
    public static partial void RunningExportCommand(this ILogger logger, string exportCmd);

    [LoggerMessage(EventId = 5, Level = LogLevel.Debug, Message = "Dev cert exported to '{CertPath}' and '{CertKeyPath}'", EventName = "DevCertExported")]
    public static partial void DevCertExported(this ILogger logger, string certPath, string certKeyPath);

    [LoggerMessage(EventId = 6, Level = LogLevel.Error, Message = "Failed to start HTTPS dev certificate export process", EventName = "FailedToStartExportProcess")]
    public static partial void FailedToStartExportProcess(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 7, Level = LogLevel.Error, Message = "HTTPS dev certificate export failed with exit code {ExitCode}", EventName = "ExportFailedWithExitCode")]
    public static partial void ExportFailedWithExitCode(this ILogger logger, int exitCode);

    [LoggerMessage(EventId = 8, Level = LogLevel.Error, Message = "HTTPS dev certificate export timed out after {TimeoutSeconds} seconds", EventName = "ExportTimedOut")]
    public static partial void ExportTimedOut(this ILogger logger, double timeoutSeconds);

    [LoggerMessage(EventId = 9, Level = LogLevel.Error, Message = "HTTPS dev certificate export failed for an unknown reason", EventName = "ExportFailedUnknownReason")]
    public static partial void ExportFailedUnknownReason(this ILogger logger);

    [LoggerMessage(EventId = 10, Level = LogLevel.Information, Message = "> {StandardOutput}", EventName = "StandardOutput")]
    public static partial void StandardOutput(this ILogger logger, string standardOutput);

    [LoggerMessage(EventId = 11, Level = LogLevel.Error, Message = "> {StandardError}", EventName = "StandardError")]
    public static partial void StandardError(this ILogger logger, string standardError);
}
