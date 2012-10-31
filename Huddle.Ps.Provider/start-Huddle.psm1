function mount-huddleProvider
{
	param( 
		[parameter( Mandatory=$true, valueFromPipelineByPropertyName=$true )]
		[string]
		$host
	)
	process
	{
		new-psdrive -psprovider Huddle.Ps.Provider -root "" -name huddle -Host $host -scope Global;
	}
}