#!/bin/bash

# Build script for the midterm banking system project
# This script handles:
# 1. Restoration of dependencies
# 2. Code formatting check with StyleCopAnalyzers
# 3. Building the project (debug and release modes)
# 4. Running unit tests
# 5. Generating documentation

set -e

echo "==============================================="
echo "Build Script - Midterm Banking System"
echo "==============================================="

BUILD_CONFIG="${1:-Debug}"
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BACKEND_ROOT="$PROJECT_ROOT/backend"
DOCS_OUTPUT="$PROJECT_ROOT/artifacts/docs"

echo "Project Root: $PROJECT_ROOT"
echo "Build Configuration: $BUILD_CONFIG"

# Function to print section headers
print_section() {
    echo ""
    echo "========================================"
    echo "$1"
    echo "========================================"
}

# Clean previous builds
print_section "Cleaning previous builds"
rm -rf "$BACKEND_ROOT"/**/bin
rm -rf "$BACKEND_ROOT"/**/obj
echo "✓ Cleaned build artifacts"

# Restore dependencies
print_section "Restoring dependencies"
cd "$BACKEND_ROOT"
dotnet restore backend.slnx
if [ $? -eq 0 ]; then
    echo "✓ Dependencies restored successfully"
else
    echo "✗ Failed to restore dependencies"
    exit 1
fi

# Run code analysis with StyleCopAnalyzers
print_section "Running code analysis (StyleCopAnalyzers)"
cd "$BACKEND_ROOT"
dotnet build backend.slnx -c $BUILD_CONFIG /p:TreatWarningsAsErrors=false \
    /p:EnforceCodeStyleInBuild=true \
    /p:StyleCopTreatErrorsAsWarnings=false \
    --no-restore
if [ $? -eq 0 ]; then
    echo "✓ Code analysis completed"
else
    echo "⚠ Code analysis found issues (check output above)"
fi

# Build Debug
print_section "Building Debug configuration"
cd "$BACKEND_ROOT"
dotnet build backend.slnx -c Debug --no-restore
if [ $? -eq 0 ]; then
    echo "✓ Debug build successful"
else
    echo "✗ Debug build failed"
    exit 1
fi

# Build Release
print_section "Building Release configuration"
cd "$BACKEND_ROOT"
dotnet build backend.slnx -c Release --no-restore
if [ $? -eq 0 ]; then
    echo "✓ Release build successful"
else
    echo "✗ Release build failed"
    exit 1
fi

# Run tests
print_section "Running unit tests"
cd "$BACKEND_ROOT"
dotnet test tests/tests.csproj -c Release --no-build --no-restore \
    /p:CollectCoverage=true \
    /p:CoverletOutput=tests/TestResults/Coverage/ \
    /p:CoverletOutputFormat=cobertura \
    /p:Include="[model*]*%2c[api*]*" \
    /p:Exclude="[tests*]*" \
    /p:ExcludeByFile="**/obj/**%2c**/Program.cs"

if [ $? -eq 0 ]; then
    echo "✓ All tests passed"
else
    echo "✗ Tests failed"
    exit 1
fi

# Generate XML documentation (already in bin output from build)
print_section "Documentation generation"
echo "✓ XML documentation generated during build"
echo "Documentation files located in:"
echo "  - $BACKEND_ROOT/api/bin/Release/net10.0/api.xml"
echo "  - $BACKEND_ROOT/model/bin/Release/net10.0/model.xml"
echo "  - $BACKEND_ROOT/dal/bin/Release/net10.0/dal.xml"

print_section "Build Summary"
echo "✓ Dependency restoration: SUCCESS"
echo "✓ Code analysis: COMPLETED"
echo "✓ Debug build: SUCCESS"
echo "✓ Release build: SUCCESS"
echo "✓ Unit tests: SUCCESS"
echo "✓ Documentation: GENERATED"
echo ""
echo "==============================================="
echo "Build completed successfully!"
echo "==============================================="
