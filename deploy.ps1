# deploy.ps1

$BuildOutput = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\bin\Release\net8.0"
$DeployPath = "C:\AfterCIRunner\MyProject"

Write-Host "Starting Deployment..."

# Clean the target folder if it exists
if (Test-Path $DeployPath) {
    Write-Host "Clearing existing deployment folder..."
    Remove-Item -Path "$DeployPath\*" -Recurse -Force
} else {
    Write-Host "Creating deployment folder..."
    New-Item -Path $DeployPath -ItemType Directory -Force
}

Write-Host "Copying new build output..."
Copy-Item "$BuildOutput\*" "$DeployPath" -Recurse -Force

Write-Host "Hey explorer from optimworks. The deployment completed successfully. Check the latest report in the deployment path."
