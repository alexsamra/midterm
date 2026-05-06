#!/bin/bash

# Documentation generation script
# This script generates DocFX documentation from XML comments and markdown

set -e

echo "========================================"
echo "Generating Documentation"
echo "========================================"

PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DOCS_DIR="$PROJECT_ROOT/docs/docfx_project"
OUTPUT_DIR="$PROJECT_ROOT/artifacts/docs"

# Check if DocFX is installed
if ! command -v docfx &> /dev/null; then
    echo "DocFX not found. Installing..."
    dotnet tool install -g docfx
fi

# Build documentation
echo "Building documentation from XML comments and markdown..."
cd "$DOCS_DIR"

docfx docfx.json -o "$OUTPUT_DIR"

if [ $? -eq 0 ]; then
    echo ""
    echo "✓ Documentation generated successfully!"
    echo "Output location: $OUTPUT_DIR"
    echo ""
    echo "To view the documentation locally:"
    echo "  cd $OUTPUT_DIR"
    echo "  python -m http.server 8000  # or: python3 -m http.server 8000"
    echo "  Then open: http://localhost:8000"
else
    echo "✗ Documentation generation failed"
    exit 1
fi
