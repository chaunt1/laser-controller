# PowerShell script to register DeviceInterfaceGUID for Newson Rhothor RT-CU (VID 1525, PID 0007)
# Run as Administrator in Windows 10 / Windows 11

$guid = "{FED7004F-CA54-478c-BA6F-E9EFE07066B4}"

Write-Host "Registering Rhothor DeviceInterfaceGUID $guid for Windows 10/11..." -ForegroundColor Cyan

# Find all USB devices matching VID 1525 & PID 0007
$devices = Get-PnpDevice | Where-Object { $_.InstanceId -like "*VID_1525&PID_0007*" }

if ($devices) {
    foreach ($dev in $devices) {
        Write-Host "Found Device: $($dev.FriendlyName) ($($dev.InstanceId))" -ForegroundColor Green
        $regPath = "HKLM:\SYSTEM\CurrentControlSet\Enum\$($dev.InstanceId)\Device Parameters"
        if (Test-Path $regPath) {
            Set-ItemProperty -Path $regPath -Name "DeviceInterfaceGUIDs" -Value @($guid) -Type MultiString
            Write-Host "Successfully registered GUID under $regPath" -ForegroundColor Green
        }
    }
} else {
    Write-Host "Note: Physical USB device VID_1525&PID_0007 not currently detected in Device Manager." -ForegroundColor Yellow
    Write-Host "Registering default USB flags template..." -ForegroundColor Gray
    $flagsPath = "HKLM:\SYSTEM\CurrentControlSet\Control\usbflags\152500070100"
    New-Item -Path $flagsPath -Force | Out-Null
    Set-ItemProperty -Path $flagsPath -Name "DeviceInterfaceGUIDs" -Value @($guid) -Type MultiString
    Write-Host "Registered default template under $flagsPath" -ForegroundColor Green
}

Write-Host "Done. rhothor.dll and rhothor.exe can now access the device." -ForegroundColor Cyan
