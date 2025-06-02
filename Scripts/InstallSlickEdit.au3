#include <File.au3>
#include <AutoItConstants.au3>

; Invoke the SlickEdit installer
Local $title  = "SlickEdit® Pro 2017 Setup"
Local $msi    = "se_22000201_win64.msi"
Local $path   = _PathFull(@UserProfileDir & "\Downloads")
Local $exe    = _PathFull($path & "\" & $msi)
Local $server = "l27001@liclin3.its.hpecorp.net,27001@liclin2.its.hpecorp.net,27001@liclin1.its.hpecorp.net"
ShellExecute($exe, "SLICKEDIT_LICENSE_SERVER="&$server, $path)
sleep(1000)

; Click Next button
; (not sure why ALT-A is needed here (it's not applicable until the next screen) but the scripts does not work work without it
Send("{ENTER}{ALT}A")
Sleep(1000)

; Click accept terms checkbox
Send("{ALT}A")
Sleep(500)

; Click Next button
Send("{ENTER}")
Sleep(500)

; Add to Send to menu and Quick Launch bar then click Next
Send("{ALT}S{ALT}Q{ENTER}")
Sleep(1000)

; Click Install button
Send("{ENTER}")

; Wait until the Finish button appears then click Finish
Do
    $Finish = ControlGetHandle("SlickEdit® Pro 2017 Setup", "", "[ID:1373]")
	Sleep(1000)
Until $Finish <> ""
Send("{ENTER}")
