$ErrorActionPreference = 'Stop'

$project = Join-Path $PSScriptRoot 'BackendMecanicaElEnano\BackendMecanicaElEnano\BackendMecanicaElEnano.csproj'
$frontend = Join-Path $PSScriptRoot 'FrontendMecanicaElEnano'
$frontendOutput = Join-Path $frontend 'dist\frontend-mecanica-el-enano'
$webRoot = Join-Path $PSScriptRoot 'BackendMecanicaElEnano\BackendMecanicaElEnano\wwwroot'
$output = Join-Path $PSScriptRoot 'publicacion'

& npm.cmd --prefix $frontend run build

if ($LASTEXITCODE -ne 0) {
    throw "Frontend build failed with exit code $LASTEXITCODE."
}

if (Test-Path -LiteralPath $webRoot) {
    Remove-Item -LiteralPath $webRoot -Recurse -Force
}

New-Item -ItemType Directory -Path $webRoot | Out-Null
Copy-Item -Path (Join-Path $frontendOutput '*') -Destination $webRoot -Recurse

if (Test-Path -LiteralPath $output) {
    Remove-Item -LiteralPath $output -Recurse -Force
}

dotnet publish $project --configuration Release --output $output

if ($LASTEXITCODE -ne 0) {
    throw "Publishing failed with exit code $LASTEXITCODE."
}

Write-Host ''
Write-Host "Executable created at: $output\BackendMecanicaElEnano.exe" -ForegroundColor Green
