# Documentation generation script
# This script generates DocFX documentation from XML comments and markdown

param(
    [switch]$ViewLocal = $false
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Generating Documentation" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$docsDir = Join-Path $projectRoot "docs" "docfx_project"
$outputDir = Join-Path $projectRoot "artifacts" "docs"

# Check if DocFX is installed
$docfxVersion = docfx --version 2>$null
if (-not $docfxVersion) {
    Write-Host "DocFX not found. Installing..." -ForegroundColor Yellow
    dotnet tool install -g docfx
}

# Build documentation
Write-Host "Building documentation from XML comments and markdown..." -ForegroundColor Yellow
Push-Location $docsDir

docfx docfx.json -o $outputDir

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✓ Documentation generated successfully!" -ForegroundColor Green
    Write-Host "Output location: $outputDir" -ForegroundColor Cyan
    Write-Host ""
    
    if ($ViewLocal) {
        Write-Host "Starting local server..." -ForegroundColor Yellow
        Push-Location $outputDir
        python -m http.server 8000
    } else {
        Write-Host "To view the documentation locally:" -ForegroundColor Cyan
        Write-Host "  cd $outputDir" -ForegroundColor Gray
        Write-Host "  python -m http.server 8000  # or: python3 -m http.server 8000" -ForegroundColor Gray
        Write-Host "  Then open: http://localhost:8000" -ForegroundColor Gray
    }
} else {
    Write-Host "✗ Documentation generation failed" -ForegroundColor Red
    exit 1
}

Pop-Location
