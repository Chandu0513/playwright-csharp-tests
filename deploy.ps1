# deploy.ps1

$BuildOutput = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\bin\Release\net8.0"
$DeployPath = "C:\DeployedApps\MyDotNetApp"

Write-Host "Starting Deployment..."

if (-Not (Test-Path $DeployPath)) {
    Write-Host "Creating deployment folder..."
    New-Item -Path $DeployPath -ItemType Directory -Force
}

Write-Host "Copying files..."
Copy-Item "$BuildOutput\*" "$DeployPath" -Recurse -Force

Write-Host "Hey explorer from optimworks. The Deployment completed successfully.Check"
