<<<<<<< HEAD
Puddle
======

Huddle Provider
=======
In order to use this Powershell provider you need to be running Powershell using v4 of the .NET Runtime.

To ensure Powershell runs with v4, you need to add the following lines to your `powershell.exe.config`, or `powershell_ise.exe.config` in your `System32\WindowsPowerShell\v1.0\` directory (and your `SystWOW64\WindowsPowerShell\v1.0\`)

```xml
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
```

We support the following provider paths:

* Drive-qualified: `huddle:/files/folders/12345`
* Provider-qualified: `Puddle::huddle:/files/folders/12345`
* Provider-internal: this is identical to the drive-qualified path syntax

To register a provider use `new-psadrive`, e.g.

```
new-psdrive -psprovider puddle -name myHuddle -root '' -Host api.huddle.local
```

You can list drives with 

```
get-psdrive
```

And your new drive should be in the list

You can then get a folder using a variety of syntaxes such as:

```
get-item myHuddle:/files/folders/29282765
```

Today this is hard coded to use a test user id, and only has access to folders against that test user. Future releases need to deal with user management.
>>>>>>> c5282a831a7a225b9676b4cce05ccb31093e22db
