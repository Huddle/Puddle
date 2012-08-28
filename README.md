In order to use this Powershell provider you need to be running Powershell using v4 of the .NET Runtime.

To ensure Powershell runs with v4, you need to add the following lines to your powershell.exe.config, or powershell_ise.exe.config in your System32\WindowsPowerShell\v1.0\ directory (and your SystWOW64\WindowsPowerShell\v1.0\)

<?xml version="1.0" encoding="utf-8" ?> 
<configuration> 
  <!-- http://msdn.microsoft.com/en-us/library/w4atty68.aspx --> 
  <startup useLegacyV2RuntimeActivationPolicy="true"> 
    <supportedRuntime version="v4.0.30319" /> 
    <supportedRuntime version="v3.5" /> 
    <supportedRuntime version="v3.0" /> 
    <supportedRuntime version="v2.0.50727" /> 
  </startup> 
</configuration>

Next you will need to add the pssnapin to windows so it can recognise it when you register it. The easiest way to do this is to open up Visual Studio's command prompt and run the command installutil provider.dll

Finally if you want to format the output of the provider so it mirriors a windows file system move the Puddle.format.PS1XML file to your powershell directory.



----When Running The Provider----
When you run the provider for the first time you'll get an error as you have never set a token before so run the following command:
set-token -clientId "clientId Here" you should only need to do this once as the provider will automatically refresh the token as it expires. This provider also supports getting a token with the password type authentication but please use this only as a last resort.

An example of how to hit your entry page of your account can be seen in the scripts folder.

Currently the provider supports the following commands : set-item, get-item, get-childitem, new-item, remove-item. The provider does not support tab completion.

The provider has the following cmdlets with it set-token, UndoDelete (will restore a deleted folder or document), new-object (used for set-item and get-item), new-FileLocation (used to specifiy the path of the file you wish to upload to the huddle).

uploading files is currently a two step process 1. create the new blank file 2. set-item with the path of the item you wish to upload (to create this upload object use the newFileLocation cmdlet).



