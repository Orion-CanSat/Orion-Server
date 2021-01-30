#!/usr/pwsh

if (& dotnet --list-sdks | Select-String "5.0") {
    Write-Host "Net 5.0 was detected"
} else {
    Write-Error -Message "Net 5.0 could not be located."
    Write-Error -Message "Please follow instruction in https://dotnet.microsoft.com/download/dotnet/5.0"
    exit 1
}


if (& dotnet tool list -g | Select-String "libman")  {
    Write-Host "Libman was located."
} else {
    Write-Error -Message "Libman could not be located."
    Write-Error -Message "Please follow instruction in https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/libman-cli"
    exit 1
}


Set-Location -Path "Orion Server"
libman restore


if ($IsWindows){
    if (-not [Environment]::Is64BitOperatingSystem) {
        dotnet publish -c Release -f net5.0 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained true -r win-x86
        Set-Location -Path "bin/Release/net5.0/win-x86/publish"
    } else {
        dotnet publish -c Release -f net5.0 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained true -r win-x64
        Set-Location -Path "bin/Release/net5.0/win-x64/publish"
    }
} elseif ($IsLinux) {
    if (-not [Environment]::Is64BitOperatingSystem) {
        dotnet publish -c Release -f net5.0 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained true -r linux-x86
        Set-Location -Path "bin/Release/net5.0/linux-x86/publish"
    } else {
        dotnet publish -c Release -f net5.0 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained true -r linux-x64
        Set-Location -Path "bin/Release/net5.0/linux-x64/publish"
    }
} elseif ($IsMacOS) {
    dotnet publish -c Release -f net5.0 -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained true -r osx-x64
    Set-Location -Path "bin/Release/net5.0/osx-x64/publish"
}