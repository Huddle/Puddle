function Get-FrameworkDirectory()  
{  
	$([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory())  
}  

#get there powershell directory
$path = $PsHome

#first lets move the module to the powershell folder
cd $path\Modules\

if(!(Test-Path $path\Modules\PsHuddle))
{
	MD 	PsHuddle
}

$currentpath = Split-Path $MyInvocation.MyCommand.Path

copy-Item  "$currentpath\Provider\*" $path\Modules\PsHuddle\ 

#now its time to register that blasted .dll
Invoke-Expression "$(Get-FrameworkDirectory)installutil $path\Modules\PSHuddle\Provider.dll"

#finally check if they are on .net 4
if([System.Environment]::Version.Major -lt 4)
{
	Write-Host Please update to .net Version 4.0 otherwise this will not run.
}
$configFile = "$PsHome\powershell.exe.config"
$configExists = Test-Path $configFile

if($configExists)
{
	Write-Host You already have a config file please manually edit this file so it can run .net 4.0 files -foregroundcolor "yellow"
	Write-Host http://stackoverflow.com/questions/2094694/how-can-i-run-powershell-with-the-net-4-runtime -foregroundcolor "yellow"
}
else
{
$xml = "<?xml version='1.0' encoding='utf-8' ?> 
<configuration> 
  <!-- http://msdn.microsoft.com/en-us/library/w4atty68.aspx --> 
  <startup useLegacyV2RuntimeActivationPolicy='true'> 
    <supportedRuntime version='v4.0.30319'/> 
    <supportedRuntime version='v3.5' /> 
    <supportedRuntime version='v3.0' /> 
    <supportedRuntime version='v2.0.50727'/> 
  </startup> 
</configuration>"

$xml | Out-File $configFile
}

