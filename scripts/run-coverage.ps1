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
    [string]$Verbosity = "minimal"
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

    # Run all tests with coverage in a single command
    Write-Status "Running all TUnit tests with coverage..."
    
    $coverageSettingsPath = Join-Path (Get-Location) "CodeCoverage.runsettings"
    
    dotnet test `
        --configuration $Configuration `
        --verbosity $Verbosity `
        --no-build `
        -- `
        --coverage `
        --coverage-output-format cobertura `
        --coverage-output "coverage.cobertura.xml" `
        --coverage-settings $coverageSettingsPath `
        --results-directory $CoverageDir
    
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Some tests failed with exit code $LASTEXITCODE, but continuing with coverage collection"
    }

    # Find coverage files
    $CoverageFiles = @()
    
    # Look in the main TestResults directory for the single coverage file
    $CoverageFiles += Get-ChildItem -Path $CoverageDir -Recurse -Filter "*.cobertura.xml" -ErrorAction SilentlyContinue
    $CoverageFiles += Get-ChildItem -Path $CoverageDir -Recurse -Filter "*coverage*.xml" -ErrorAction SilentlyContinue
    
    # Also check for coverage files in test project directories (TUnit generates them there)
    $testProjectDirs = @(
        "tests\TourBooking.Tests.Domain",
        "tests\TourBooking.WebTests", 
        "tests\TourBooking.Tests.EndToEnd"
    )
    
    foreach ($dir in $testProjectDirs) {
        if (Test-Path $dir) {
            $projectCoverageFiles = Get-ChildItem -Path $dir -Recurse -Filter "*coverage*.xml" -ErrorAction SilentlyContinue
            if ($projectCoverageFiles.Count -gt 0) {
                foreach ($file in $projectCoverageFiles) {
                    $CoverageFiles += $file
                }
            }
        }
    }
    
    # Remove duplicates based on full path
    $CoverageFiles = $CoverageFiles | Sort-Object FullName | Get-Unique -AsString
    
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
        -filefilters:"-https://**;+**"

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
