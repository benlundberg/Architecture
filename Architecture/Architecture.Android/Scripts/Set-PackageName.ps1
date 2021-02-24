param ([string] $ProjectDir, [string] $ConfigurationName)
Write-Host "ProjectDir: $ProjectDir"
Write-Host "ConfigurationName: $ConfigurationName"

$ManifestPath = $ProjectDir + "Properties\AndroidManifest.xml"

Write-Host "ManifestPath: $ManifestPath"

[xml] $xdoc = Get-Content $ManifestPath

$package = $xdoc.manifest.package

Write-Host "Origin package name $package"

$fileprovider = $xdoc.manifest.application.provider.authorities

Write-Host "Fileprovider: $fileprovider"

$appname = $xdoc.manifest.application.label

Write-Host "Origin app name: $appname"

# If production
If ($ConfigurationName -eq "Production")
{
    # Change package name to production
    If ($package.EndsWith("Test")) 
    {
        $package = $package.Replace("Test", "")
    }

    # Change app name to production
    If ($appname.EndsWith("Test"))
    {
        $appname = $appname.Replace("Test", "");
    }
}

# If NOT production
If ($ConfigurationName -ne "Production")
{
    # Add Test to package name
    If (-not $package.EndsWith("Test")) 
    {
        $package = $package + "Test"
    }

    # Add Test to app name
    If (-not $appname.EndsWith("Test"))
    {
        $appname = $appname + "Test"
    }
}

Write-Host "New package name $package"

If ($package -ne $xdoc.manifest.package -or $appname -ne $xdoc.manifest.application.label)
{
    # Set package name
    $xdoc.manifest.package = $package

    # Set app name    
    $xdoc.manifest.application.label = $appname

    # Change authorities to correct package name
    $xdoc.manifest.application.provider.authorities = $package + ".fileprovider"

    $xdoc.Save($ManifestPath)
    Write-Host "AndroidManifest.xml package name updated to $package"
}