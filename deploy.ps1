# deploy.ps1

# Paths
$BuildOutput = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\bin\Release\net8.0"
$DeployPath = "C:\DeployedApps\MyDotNetApp"
$TestResultsPath = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\TestResults"
$TestResultsDeployPath = "$DeployPath\TestResults"

Write-Host "==============================================="
Write-Host "        Starting Deployment Script"
Write-Host "==============================================="

# Create deployment directory if it doesn't exist
if (-Not (Test-Path $DeployPath)) {
    Write-Host "Creating deployment folder at: $DeployPath"
    New-Item -Path $DeployPath -ItemType Directory -Force | Out-Null
}

# Copy Build Output
if (Test-Path $BuildOutput) {
    Write-Host "`nBuild output found. Copying files from:"
    Write-Host "$BuildOutput"
    Copy-Item "$BuildOutput\*" "$DeployPath" -Recurse -Force
    Write-Host "✅ Build files copied successfully."
} else {
    Write-Host "`n⚠️ Build output not found. Skipping file copy."
}

# Copy Test Results
if (Test-Path $TestResultsPath) {
    Write-Host "`nTest results found. Copying from:"
    Write-Host "$TestResultsPath"

    # Ensure target folder exists
    if (-Not (Test-Path $TestResultsDeployPath)) {
        Write-Host "Creating test results folder at: $TestResultsDeployPath"
        New-Item -Path $TestResultsDeployPath -ItemType Directory -Force | Out-Null
    }

    Copy-Item "$TestResultsPath\*" "$TestResultsDeployPath" -Recurse -Force
    Write-Host "✅ Test results copied successfully."
} else {
    Write-Host "`n⚠️ No test results found to copy."
}

Write-Host "`n Hey explorer from OptimWorks. Deployment script completed."
Write-Host "==============================================="
