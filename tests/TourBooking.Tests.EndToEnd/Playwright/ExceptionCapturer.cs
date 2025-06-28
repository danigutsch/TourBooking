using JetBrains.Annotations;

namespace TourBooking.Tests.EndToEnd.Playwright;

/// <summary>
///     ExceptionCapturer is a best-effort way of detecting if a test did pass or fail in xUnit.
///     This class uses the AppDomain's FirstChanceException event to set a flag indicating
///     whether an exception has occurred during the test execution.
///     Note: There is no way of getting the test status in xUnit in the dispose method.
///     For more information, see: https://stackoverflow.com/questions/28895448/current-test-status-in-xunit-net
/// </summary>
[PublicAPI]
public class ExceptionCapturer : IAsyncLifetime
{
    protected static bool TestOk => TestContext.Current.TestState?.Result is not TestResult.Failed;

    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    public virtual ValueTask InitializeAsync()
    {
        return ValueTask.CompletedTask;
    }
}