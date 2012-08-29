# PS.Huddle

## A Powershell provider for Huddle


### Installation
- Pull the source (just Scripts directory needed for existing Binary)
- Run setup.ps1 in the scripts folder 

### Example use

####supports powershell syntax for:
* get-item
* get-childitem
* new-item
* remove-item
*set-item

####Cmdlets for:
* setting a auth token --> set-token -clientId foo
* Undoing a deleted folder or file --> undo-delete -path foo
* creating a blank item --> new-item -title foo -desc bar
* pointing to a path for upload --> new-fileLocation -path foo

### Developer Notes

* On first running of the project you will be prompted to log in to huddle so it can set a token. 

* Your Token is stored in your %appdata% (don't worry its encrypted).

* You will need to modify the config file in the token project to get the source working as I have removed
oauth details such as the projects clientId or redirectURI.

* To Upload a file from your computer to huddle is a two step apporach 1. create a blank file with new-item then
call set-item with the value of new-fileLocation object.




