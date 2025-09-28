Write-Host "Start checking"
if (Test-Path "C:\Program Files\WinRAR\WinRAR.exe"  -PathType Leaf){exit(0)}else{exit(1)}
