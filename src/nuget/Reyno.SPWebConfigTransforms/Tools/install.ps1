
<#
	Need to locate the package manifest and add a solution
	dependency.  
 #>


 $manifest = Get-ChildItem "$installPath\Package\Package.Template.xml"
