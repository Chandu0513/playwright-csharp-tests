$BuildOutput = "C:\actions-runner\_work\playwright-csharp-tests\playwright-csharp-tests\bin\Release\net8.0"
$DeployPath = "C:\AfterCIRunner\MyProject"

Write-Host "`n📦 Starting Deployment..."

# Ensure deployment folder is clean
if (Test-Path $DeployPath) {
    Write-Host "🧹 Cleaning existing deployment folder..."
    Remove-Item -Path "$DeployPath\*" -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "📁 Creating deployment folder..."
    New-Item -Path $DeployPath -ItemType Directory -Force | Out-Null
}

# Recreate folder structure and copy files manually
Write-Host "📁 Copying files..."
$items = Get-ChildItem -Path $BuildOutput -Recurse -Force

foreach ($item in $items) {
    $relativePath = $item.FullName.Substring($BuildOutput.Length).TrimStart('\')
    $targetPath = Join-Path $DeployPath $relativePath

    if ($item.PSIsContainer) {
        if (!(Test-Path $targetPath)) {
            New-Item -Path $targetPath -ItemType Directory -Force | Out-Null
        }
    } else {
        $targetDir = Split-Path $targetPath -Parent
        if (!(Test-Path $targetDir)) {
            New-Item -Path $targetDir -ItemType Directory -Force | Out-Null
        }
        Copy-Item -Path $item.FullName -Destination $targetPath -Force
    }
}

# Delete original files/folders
Write-Host "🗑️ Cleaning up original build output..."
Get-ChildItem -Path $BuildOutput -Recurse -Force | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue

Write-Host "`n✅ Deployment completed. All files copied and source cleaned."
