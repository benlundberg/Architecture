param ([string] $ProjectDir, [string] $ConfigurationName)
Write-Host "ProjectDir: $ProjectDir"
Write-Host "ConfigurationName: $ConfigurationName"

$PlistPath = $ProjectDir + "Info.plist"

Write-Host "PlistPath: $PlistPath"

[xml] $xdoc = Get-Content $PlistPath

$bundleName =	$xdoc.plist.dict.ChildNodes[1].'#text'
$displayName =	$xdoc.plist.dict.ChildNodes[3].'#text'
$identifier =	$xdoc.plist.dict.ChildNodes[5].'#text'

Write-Host "Current bundle name: $bundleName"
Write-Host "Current display name: $displayName"
Write-Host "Current identifier: $identifier"

if ($ConfigurationName -eq "Prod")
{
	#BundleName
	$xdoc.plist.dict.ChildNodes[1].'#text' = "Architecture"
	#DisplayName
	$xdoc.plist.dict.ChildNodes[3].'#text' = "Architecture"
	#Identifer
	$xdoc.plist.dict.ChildNodes[5].'#text' = "com.yourcompany.Architecture"
	#Save new list
	$xdoc.Save($PlistPath)
} 
else
{
    #BundleName
	$xdoc.plist.dict.ChildNodes[1].'#text' = "ArchitectureTest"
	#DisplayName
	$xdoc.plist.dict.ChildNodes[3].'#text' = "ArchitectureTest"
	#Identifer
	$xdoc.plist.dict.ChildNodes[5].'#text' = "com.yourcompany.ArchitectureTest"
	#Save new list
	$xdoc.Save($PlistPath)
}