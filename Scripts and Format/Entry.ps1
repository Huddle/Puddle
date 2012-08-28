function addSnapinIfNotAdded($snapIn)  
{  
 	if ( (Get-PSSnapin -Name $snapIn -ErrorAction SilentlyContinue) -eq $null )  
	{  
		add-pssnapin $snapIn  
	}  
}  

function Install_Puddle()
{
	addSnapinIfNotAdded "foobarbaz"
	#if you move the format script to the powershell directory you shouldn't need this
	#update-formatdata -prependpath "C:\Users\adam.flax\Documents\Visual Studio 2010\Projects\Provider\Provider\bin\Debug\Puddle.format.ps1xml" -verbose
	new-psdrive -psprovider foobar -name baz -root "" -Host api.huddle.net/ -Scope "Global"

}

$loc = "\"
##go to the root point
cd baz:$loc
#