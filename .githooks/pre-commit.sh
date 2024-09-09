#!/bin/bash
echo "Running git pre-commit hook"
for f in $(find . -name "*.sln"); do dotnet build $f; done
