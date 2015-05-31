param($installPath, $toolsPath, $package, $project) 

[System.Reflection.Assembly]::LoadWithPartialName("System.Xml.Linq") | Out-Null
<#
	Need to locate the package manifest and add a solution
	dependency.  
 #>

 
$solutionId = "8007a5f3-e5b3-4121-a64d-b3ba3e5dc174"
$path = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($project.FileName), "Package\Package.Template.xml")

$xml = [System.Xml.Linq.XDocument]::Load($path)

# get a reference to the default namespace
$ns = $xml.Root.GetDefaultNamespace()

# get or create the activation dependencies element
$dependencies = $xml.Root.Element($ns + "ActivationDependencies")
if($dependencies -eq $null) {

    # create the element and add it to the root node
    $dependencies = New-Object System.Xml.Linq.XElement($ns + "ActivationDependencies")
    $xml.Root.Add($dependencies) | Out-Null

}

# try and find the specific dependency
$dependency = $dependencies.Elements($ns + "ActivationDependency") `
    | ? { $_.Attribute("SolutionId") -ne $null } `
    | ? { $_.Attribute("SolutionId").Value -eq $solutionId } `

# create the dependency
if($dependency -eq $null) {

    # create the dependency element
	$dependency = New-Object System.Xml.Linq.XElement($ns + "ActivationDependency")

	# add the attributes
    $dependency.Add($(New-Object System.Xml.Linq.XAttribute("SolutionId", $solutionId)))
    $dependency.Add($(New-Object System.Xml.Linq.XAttribute("SolutionName", "Reyno.SPWebConfigTransforms")))
    $dependency.Add($(New-Object System.Xml.Linq.XAttribute("SolutionTitle", "Reyno - Web Config Transforms")))
    $dependency.Add($(New-Object System.Xml.Linq.XAttribute("SolutionUrl", "https://github.com/reyno-uk/SPWebConfigModifications")))
    
	# add to the list of dependencies
    $dependencies.Add($dependency)

}

# save the changes
$xml.Save($path)

