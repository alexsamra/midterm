# Build script for the midterm banking system project
# This script handles:
# 1. Restoration of dependencies
# 2. Code formatting check with StyleCopAnalyzers
# 3. Building the project (debug and release modes)
# 4. Running unit tests
# 5. Generating documentation

param(
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

Write-Host "===============================================" -ForegroundColor Cyan
Write-Host "Build Script - Midterm Banking System" -ForegroundColor Cyan
Write-Host "===============================================" -ForegroundColor Cyan

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$backendRoot = Join-Path $projectRoot "backend"
$docsOutput = Join-Path $projectRoot "artifacts/docs"

Write-Host "Project Root: $projectRoot"
Write-Host "Build Configuration: $Configuration"

# Function to print section headers
function Print-Section {
    param([string]$title)
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Yellow
    Write-Host $title -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Yellow
}

# Clean previous builds
Print-Section "Cleaning previous builds"
Get-ChildItem -Path $backendRoot -Recurse -Directory -Filter "bin" | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
Get-ChildItem -Path $backendRoot -Recurse -Directory -Filter "obj" | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
Write-Host "✓ Cleaned build artifacts" -ForegroundColor Green

# Restore dependencies
Print-Section "Restoring dependencies"
Push-Location $backendRoot
dotnet restore backend.slnx
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Dependencies restored successfully" -ForegroundColor Green
} else {
    Write-Host "✗ Failed to restore dependencies" -ForegroundColor Red
    exit 1
}

# Run code analysis with StyleCopAnalyzers
Print-Section "Running code analysis (StyleCopAnalyzers)"
dotnet build backend.slnx -c $Configuration `
    /p:TreatWarningsAsErrors=false `
    /p:EnforceCodeStyleInBuild=true `
    /p:StyleCopTreatErrorsAsWarnings=false `
    --no-restore

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Code analysis completed" -ForegroundColor Green
} else {
    Write-Host "⚠ Code analysis found issues (check output above)" -ForegroundColor Yellow
}

# Build Debug
Print-Section "Building Debug configuration"
dotnet build backend.slnx -c Debug --no-restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Debug build successful" -ForegroundColor Green
} else {
    Write-Host "✗ Debug build failed" -ForegroundColor Red
    exit 1
}

# Build Release
Print-Section "Building Release configuration"
dotnet build backend.slnx -c Release --no-restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Release build successful" -ForegroundColor Green
} else {
    Write-Host "✗ Release build failed" -ForegroundColor Red
    exit 1
}

# Run tests
Print-Section "Running unit tests"
dotnet test tests/tests.csproj -c Release --no-build --no-restore `
    /p:CollectCoverage=true `
    /p:CoverletOutput=tests/TestResults/Coverage/ `
    /p:CoverletOutputFormat=cobertura `
    /p:Include="[model*]*%2c[api*]*" `
    /p:Exclude="[tests*]*" `
    /p:ExcludeByFile="**/obj/**%2c**/Program.cs"

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ All tests passed" -ForegroundColor Green
} else {
    Write-Host "✗ Tests failed" -ForegroundColor Red
    exit 1
}

# Generate XML documentation (already in bin output from build)
Print-Section "Documentation generation"
Write-Host "✓ XML documentation generated during build" -ForegroundColor Green
Write-Host "Documentation files located in:" -ForegroundColor Cyan
Write-Host "  - $backendRoot/api/bin/Release/net10.0/api.xml"
Write-Host "  - $backendRoot/model/bin/Release/net10.0/model.xml"
Write-Host "  - $backendRoot/dal/bin/Release/net10.0/dal.xml"

Pop-Location

Print-Section "Build Summary"
Write-Host "✓ Dependency restoration: SUCCESS" -ForegroundColor Green
Write-Host "✓ Code analysis: COMPLETED" -ForegroundColor Green
Write-Host "✓ Debug build: SUCCESS" -ForegroundColor Green
Write-Host "✓ Release build: SUCCESS" -ForegroundColor Green
Write-Host "✓ Unit tests: SUCCESS" -ForegroundColor Green
Write-Host "✓ Documentation: GENERATED" -ForegroundColor Green
Write-Host ""
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Cyan
