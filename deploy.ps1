$BuildOutput = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\bin\Release\net8.0"
$DeployPath = "C:\AfterCIRunner\MyProject"

Write-Host "Starting Deployment..."

# Ensure deployment path exists
if (Test-Path $DeployPath) {
    Write-Host "Clearing existing deployment folder..."
    Remove-Item -Path "$DeployPath\*" -Recurse -Force
} else {
    Write-Host "Creating deployment folder..."
    New-Item -Path $DeployPath -ItemType Directory -Force
}

Write-Host "Copying all files from build output to deployment folder..."
Get-ChildItem -Path $BuildOutput -Recurse | ForEach-Object {
    $dest = Join-Path $DeployPath ($_.FullName.Substring($BuildOutput.Length))
    $destDir = Split-Path $dest
    if (-not (Test-Path $destDir)) {
        New-Item -Path $destDir -ItemType Directory -Force | Out-Null
    }
    Copy-Item -Path $_.FullName -Destination $dest -Force
}

Write-Host "Removing original files from build output..."
Remove-Item -Path "$BuildOutput\*" -Recurse -Force

Write-Host "✅ Deployment complete. All files copied and original build output cleaned."
