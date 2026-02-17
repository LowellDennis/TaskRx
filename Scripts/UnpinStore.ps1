$appname = "Microsoft Store"

$shell = New-Object -Com Shell.Application
$namespace = $shell.NameSpace('shell:::{4234d49b-0245-4df3-b780-3893943456e1}')
if ($namespace -ne $null) {
    ($namespace.Items() | ?{$_.Name -eq $appname}).Verbs() | ?{$_.Name.replace('&','') -match 'Unpin from taskbar'} | %{$_.DoIt(); $exec = $true}
} else {
    Write-Host "Unable to access taskbar namespace. Microsoft Store may not be pinned or accessible."
}