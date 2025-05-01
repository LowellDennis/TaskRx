param (
    # Private Key file (must exist)
    [Parameter(Mandatory=$true)]
    [string]$pem,
	# Target PuTTY Private Key file (must not exist)
    [Parameter(Mandatory=$true)]
	[string]$ppk
)

try {
	# Add Forms assembly
    Add-Type -AssemblyName System.Windows.Forms

    # Make sure Private Key file exists
    if (-not (Test-Path -Path $pem)) {
        Write-Host "Private Key file not found: $pem"
		exit 1
    }

    # Make sure PuTTY Private Key file does not exists
    if (Test-Path -Path $ppk) {
        Write-Host "PuTTY PPK file already exists: $ppk"
		exit 2
    }

	# Start PuTTYGen loading PEM file
	Start-Process  "C:\Program Files\putty\puttygen.exe" "$pem"
	Start-Sleep -Seconds 1

	# Close pop-up window saying the PEM file has been loaded
	[System.Windows.Forms.SendKeys]::SendWait("{ENTER}")
	Start-Sleep -Milliseconds 500

	# Set format to EdDSA
	[System.Windows.Forms.SendKeys]::SendWait("%ks")
	Start-Sleep -Milliseconds 500

	# Save private key
	[System.Windows.Forms.SendKeys]::SendWait("%s")
	Start-Sleep -Seconds 1

	# Yes, save without a passphrase
	[System.Windows.Forms.SendKeys]::SendWait("y")
	Start-Sleep -Seconds 1

	# Save to target PPK file
	[System.Windows.Forms.SendKeys]::SendWait("$ppk{ENTER}")
	Start-Sleep -Seconds 1

	# Done!
	Stop-Process -Name puttygen
	exit 0
} catch {
    Write-Host "An error occurred: $($_.Exception.Message)"
	exit 3
}
