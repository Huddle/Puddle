In order for this provider to work run the setup.ps1 in the scripts folder this will take care of putting the formatter in the right place setting up powershell to work with .net 4.0 etc

----When Running The Provider----
When you run the provider for the first time you'll get an error as you have never set a token before so run the following command:
set-token -clientId "clientId Here" you should only need to do this once as the provider will automatically refresh the token as it expires. This provider also supports getting a token with the password type authentication but please use this only as a last resort.

An example of how to hit your entry page of your account can be seen in the scripts folder.

Currently the provider supports the following commands : set-item, get-item, get-childitem, new-item, remove-item. The provider does not support tab completion.

The provider has the following cmdlets with it set-token, UndoDelete (will restore a deleted folder or document), new-object (used for set-item and get-item), new-FileLocation (used to specifiy the path of the file you wish to upload to the huddle).

uploading files is currently a two step process 1. create the new blank file 2. set-item with the path of the item you wish to upload (to create this upload object use the newFileLocation cmdlet).



