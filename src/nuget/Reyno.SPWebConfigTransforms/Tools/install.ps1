 
<#
	Need to locate the package manifest and add a solution
	dependency.  
 #>

$solutionId = "8007a5f3-e5b3-4121-a64d-b3ba3e5dc174"

function updateManifest($path) {
    
	# load the xml
	$xml = new-object XML
	$xml.Load($path)


	# set up the namespace    
    $ns = $xml.DocumentElement.NamespaceURI
	$nsm = new-object System.Xml.XmlNamespaceManager $xml.NameTable
	$nsm.AddNamespace("x", $ns)

	# get or create the activation dependencies element
	$dependencies = $xml.SelectSingleNode("/x:Solution/x:ActivationDependencies", $nsm)
	if($xml.Solution.ActivationDependencies -eq $null) {
		$dependencies = $xml.CreateElement("ActivationDependencies", $xml.DocumentElement.NamespaceURI) 
		$xml.Solution.AppendChild($dependencies)| Out-Null
	}


	# create the dependency if it is not there already
	if(-Not $xml.Solution.ActivationDependencies.ActivationDependency.SolutionId -eq $solutionId) {
		
		$dependency = $xml.CreateElement("ActivationDependency", $xml.DocumentElement.NamespaceURI)

		$dependency.SetAttribute("SolutionId", $solutionId)
		$dependency.SetAttribute("SolutionName", "Reyno.SPWebConfigTransforms")
		$dependency.SetAttribute("SolutionTitle", "Reyno - Web Config Transforms")
		$dependency.SetAttribute("SolutionUrl", "https://github.com/reyno-uk/SPWebConfigModifications")

		$dependencies.AppendChild($dependency)| Out-Null

		write-host "Added solution dependency to package" -ForegroundColor Green

	}

	# save the changes
	$xml.Save($path)

}

# try to find the package and update if found 
$path = "$installPath\Package\Package.Template.xml"
if(Test-Path $path) {
	updateManifest $path
}

 