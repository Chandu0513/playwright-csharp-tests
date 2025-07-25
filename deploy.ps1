# deploy.ps1

$BuildOutput = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\bin\Release\net8.0"
$DeployPath = "C:\AfterCIRunner\MyProject"
$TraceZipPath = Join-Path $DeployPath "trace.zip"

Write-Host "Starting Deployment..."

if (Test-Path $DeployPath) {
    Write-Host "Clearing existing deployment folder..."
    Remove-Item -Path "$DeployPath\*" -Recurse -Force
} else {
    Write-Host "Creating deployment folder..."
    New-Item -Path $DeployPath -ItemType Directory -Force
}

Write-Host "Copying new build output..."
Copy-Item "$BuildOutput\*" "$DeployPath" -Recurse -Force

Write-Host "Deployment completed. Launching trace if available..."

# Optional: Automatically open trace.zip using Playwright CLI viewer
if (Test-Path $TraceZipPath) {
    $PlaywrightCLI = "C:\Users\runner_user\.dotnet\tools\playwright.cmd"  # Adjust path if needed
    Start-Process $PlaywrightCLI "trace viewer `"$TraceZipPath`""
} else {
    Write-Host "Trace file not found: $TraceZipPath"
}
