# deploy.ps1

$BuildOutput = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\bin\Release\net8.0"
$DeployPath = "C:\AfterCIRunner\MyProject"
$TraceZipPath = "C:\AfterCIRunner\MyProject\TestTrace\trace.zip"
$PlaywrightCLI = "$env:USERPROFILE\.dotnet\tools\playwright.cmd"

Write-Host "Starting Deployment..."

# Clean previous deployment
if (Test-Path $DeployPath) {
    Write-Host "Clearing existing deployment folder..."
    Remove-Item -Path "$DeployPath\*" -Recurse -Force
} else {
    Write-Host "Creating deployment folder..."
    New-Item -Path $DeployPath -ItemType Directory -Force
}

# Copy new files
Write-Host "Copying new build output..."
Copy-Item "$BuildOutput\*" "$DeployPath" -Recurse -Force

Write-Host "Deployment completed."

# Open the trace.zip file in Playwright Trace Viewer
if (Test-Path $TraceZipPath) {
    Write-Host "Launching Playwright Trace Viewer..."
    Start-Process -FilePath $PlaywrightCLI -ArgumentList "trace", "viewer", "`"$TraceZipPath`""
} else {
    Write-Host "Trace file not found at: $TraceZipPath"
}
