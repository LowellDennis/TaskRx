param(
    [string]$OutputFile = "hpe-copilot.vsix"
)

$url = "https://github.hpe.com/firmware/hpe-copilot-prompts/releases/latest/download/hpe-copilot-prompts.vsix"

try {
    $token = Read-Host "Enter HPE GitHub personal access token (or press Enter to skip)"

    $headers = @{}
    if (-not [string]::IsNullOrWhiteSpace($token)) {
        $headers["Authorization"] = "token $token"
    }

    if (Test-Path $OutputFile) {
        Remove-Item $OutputFile -Force -ErrorAction SilentlyContinue
    }

    if ($headers.Count -gt 0) {
        Invoke-WebRequest -Uri $url -OutFile $OutputFile -Headers $headers -ErrorAction Stop
    }
    else {
        Invoke-WebRequest -Uri $url -OutFile $OutputFile -ErrorAction Stop
    }

    if (-not (Test-Path $OutputFile)) {
        throw "Download did not produce a VSIX file."
    }

    $fileInfo = Get-Item $OutputFile
    if ($fileInfo.Length -lt 4) {
        throw "Downloaded file is too small to be a valid VSIX."
    }

    $stream = [System.IO.File]::OpenRead($OutputFile)
    try {
        $signature = New-Object byte[] 2
        [void]$stream.Read($signature, 0, 2)
    }
    finally {
        $stream.Dispose()
    }

    if ($signature[0] -ne 0x50 -or $signature[1] -ne 0x4B) {
        throw "Downloaded file is not a valid VSIX/ZIP (PK signature missing)."
    }

    & ".\RunVSCode.cmd" --install-extension $OutputFile
}
catch {
    Write-Host "HPE Copilot extension install failed: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "Verify access/token/VPN and retry, or install manually from HPE GitHub release." -ForegroundColor Yellow
}