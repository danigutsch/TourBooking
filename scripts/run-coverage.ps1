#!/usr/bin/env pwsh

# Code Coverage Collection and Report Generation Script
# This script runs tests with code coverage and generates HTML reports

param(
    [Parameter(HelpMessage = "Test configuration (Debug or Release)")]
    [string]$Configuration = "Release",
    
    [Parameter(HelpMessage = "Open HTML report after generation")]
    [switch]$OpenReport = $false,
    
    [Parameter(HelpMessage = "Verbosity level")]
    [ValidateSet("quiet", "minimal", "normal", "detailed", "diagnostic")]
    [string]$Verbosity = "normal"
)

$ErrorActionPreference = "Stop"

# Colors for output
$Green = "`e[92m"
$Yellow = "`e[93m"
$Red = "`e[91m"
$Reset = "`e[0m"

function Write-Status {
    param([string]$Message, [string]$Color = $Green)
    Write-Host "${Color}[INFO] $Message${Reset}"
}

function Write-Warning {
    param([string]$Message)
    Write-Host "${Yellow}[WARNING] $Message${Reset}"
}

function Write-Error {
    param([string]$Message)
    Write-Host "${Red}[ERROR] $Message${Reset}"
}

# Ensure we're in the right directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location (Split-Path -Parent $ScriptDir)  # Go up one level to repo root

Write-Status "Starting code coverage collection..."
Write-Status "Configuration: $Configuration"

# Define directories
$CoverageDir = "TestResults"
$ReportsDir = "CoverageReport"

# Clean previous results - only TestResults (raw coverage data)
if (Test-Path $CoverageDir) {
    Write-Status "Cleaning previous test results..."
    Remove-Item $CoverageDir -Recurse -Force
}

# Create directories
New-Item -ItemType Directory -Path $CoverageDir -Force | Out-Null
New-Item -ItemType Directory -Path $ReportsDir -Force | Out-Null

try {
    # Note: We'll skip building separately and let 'dotnet test' handle the build
    # This avoids issues with .slnx files in PowerShell context
    Write-Status "Building will be handled by dotnet test..."

    # Define test projects
    $TestProjects = @(
        "tests\TourBooking.Tests.Domain\TourBooking.Tests.Domain.csproj",
        "tests\TourBooking.WebTests\TourBooking.WebTests.csproj",
        "tests\TourBooking.Tests.EndToEnd\TourBooking.Tests.EndToEnd.csproj"
    )

    # Run tests with coverage
    foreach ($project in $TestProjects) {
        $projectName = Split-Path -Leaf (Split-Path -Parent $project)
        Write-Status "Running tests for $projectName..."
        
        # Check if project exists
        if (-not (Test-Path $project)) {
            Write-Warning "Project file not found: $project"
            continue
        }
        
        # For Microsoft Testing Platform projects, use dotnet run with coverage arguments
        $mtpCommand = "dotnet run --project `"$project`" --configuration $Configuration --verbosity normal -- --coverage --coverage-output-format cobertura --coverage-output `"${projectName}-coverage.cobertura.xml`" --coverage-settings CodeCoverage.runsettings --results-directory `"$CoverageDir`""
        Write-Host "Executing: $mtpCommand" -ForegroundColor Gray
        
        dotnet run --project $project `
            --configuration $Configuration `
            --verbosity normal `
            -- `
            --coverage `
            --coverage-output-format cobertura `
            --coverage-output "${projectName}-coverage.cobertura.xml" `
            --coverage-settings CodeCoverage.runsettings `
            --results-directory $CoverageDir `
            2>&1 | Where-Object { 
                $_ -notmatch "^\(\d+,\d+s\)$" -and 
                $_ -notmatch "^  " -and 
                $_ -notmatch "Restore" -and
                $_ -notmatch "Determining projects to restore" -and
                $_ -notmatch "Nothing to do" -and
                $_ -match "(Test run summary|total:|failed:|succeeded:|skipped:|duration:|coverage|error|warning)"
            }
        
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "Tests failed for $projectName with exit code $LASTEXITCODE, but continuing with coverage collection"
        }
    }

    # Find coverage files (multiple formats)
    $CoverageFiles = @()
    $CoverageFiles += Get-ChildItem -Path $CoverageDir -Recurse -Filter "coverage.cobertura.xml" -ErrorAction SilentlyContinue
    $CoverageFiles += Get-ChildItem -Path $CoverageDir -Recurse -Filter "*-coverage.cobertura.xml" -ErrorAction SilentlyContinue
    $CoverageFiles += Get-ChildItem -Path $CoverageDir -Recurse -Filter "*.cobertura.xml" -ErrorAction SilentlyContinue
    
    # Also check for coverage files in project-specific bin directories (MTP projects)
    $testProjectDirs = @(
        "tests\TourBooking.Tests.Domain\bin\$Configuration\net9.0\TestResults",
        "tests\TourBooking.WebTests\bin\$Configuration\net9.0\TestResults",
        "tests\TourBooking.Tests.EndToEnd\bin\$Configuration\net9.0\TestResults"
    )
    
    foreach ($dir in $testProjectDirs) {
        if (Test-Path $dir) {
            $projectCoverageFiles = @()
            $projectCoverageFiles += Get-ChildItem -Path $dir -Recurse -Filter "*.cobertura.xml" -ErrorAction SilentlyContinue
            $projectCoverageFiles += Get-ChildItem -Path $dir -Recurse -Filter "*coverage*.xml" -ErrorAction SilentlyContinue
            if ($projectCoverageFiles.Count -gt 0) {
                foreach ($file in $projectCoverageFiles) {
                    $CoverageFiles += $file
                }
            }
        }
    }
    
    if ($CoverageFiles.Count -eq 0) {
        throw "No coverage files found. Make sure tests ran successfully and coverage collection is working."
    }

    Write-Status "Found $($CoverageFiles.Count) coverage file(s)"

    # Generate HTML report
    Write-Status "Generating HTML coverage report..."
    $coverageFilePaths = $CoverageFiles | ForEach-Object { $_.FullName }
    $reports = $coverageFilePaths -join ";"
    
    # Use filefilters to exclude https URLs and filter out 404 errors from output
    $reportCommand = "reportgenerator -reports:`"$reports`" -targetdir:`"$ReportsDir`" -reporttypes:`"Html;HtmlSummary;Badges;TextSummary`" -verbosity:Warning -title:`"TourBooking Code Coverage Report`" -tag:`"$((Get-Date).ToString('yyyy-MM-dd_HH-mm-ss'))`" -filefilters:`"-https://**;+**`""
    Write-Host "Executing: $reportCommand" -ForegroundColor Gray
    
    reportgenerator `
        -reports:$reports `
        -targetdir:$ReportsDir `
        -reporttypes:"Html;HtmlSummary;Badges;TextSummary" `
        -verbosity:Warning `
        -title:"TourBooking Code Coverage Report" `
        -tag:"$((Get-Date).ToString('yyyy-MM-dd_HH-mm-ss'))" `
        -filefilters:"-https://**;+**" `
        2>&1 | Where-Object { 
            $_ -notmatch "Error during reading file 'https://" -and
            $_ -match "(Writing report|Report generation took|ERROR|WARNING|generated|Processing)" 
        }

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to generate coverage report"
    }

    # Display summary
    Write-Status "Coverage collection completed successfully!"
    
    $summaryFile = Join-Path $ReportsDir "Summary.txt"
    if (Test-Path $summaryFile) {
        Write-Status "Coverage Summary:"
        Get-Content $summaryFile | ForEach-Object {
            if ($_ -match "Line coverage|Branch coverage") {
                Write-Host "  $_" -ForegroundColor Cyan
            }
        }
    }

    $indexFile = Join-Path $ReportsDir "index.html"
    Write-Status "HTML Report: $indexFile"
    Write-Status "Raw coverage files preserved in: $CoverageDir"

    # Copy latest report to root CoverageReport folder for consistency
    if (Test-Path $indexFile) {
        Write-Status "Coverage report generated successfully!"
    } else {
        Write-Warning "HTML report was not generated at expected location: $indexFile"
    }

    # Open report if requested
    if ($OpenReport -and (Test-Path $indexFile)) {
        Write-Status "Opening coverage report in default browser..."
        if ($IsWindows) {
            Start-Process $indexFile
        } elseif ($IsMacOS) {
            Start-Process "open" -ArgumentList $indexFile
        } else {
            Start-Process "xdg-open" -ArgumentList $indexFile
        }
    }

} catch {
    Write-Error "Coverage collection failed: $($_.Exception.Message)"
    exit 1
}
