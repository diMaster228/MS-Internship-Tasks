# SharePoint Sandbox Solution scanner #

### Summary ###
Using this command line utility you can scan, download, analyze and if possible fix the sandbox solutions in your SharePoint environment. This tool uses multi-threading to improve performance, uses app-only permissions to be able to access all sites and can deal with throttling in case that would happen. If you like watching a short video then check out [this session on the PnP YouTube channel](https://www.youtube.com/watch?v=pK4p2mGYXpU).

### Applies to ###
-  Office 365 Multi Tenant (MT)
-  Office 365 Dedicated (D)
-  SharePoint 2013 on-premises
-  SharePoint 2016 on-premises

### Solution ###
Solution | Author(s)
---------|----------
SharePoint.SandBoxTool | Bert Jansen (**Microsoft**)

### Version history ###
Version  | Date | Comments
---------| -----| --------
1.0  | August 19th 2016 | Initial release

### Disclaimer ###
**THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**


----------

# What will this tool do for you? #
The main purpose of this tool is to give you a detailed view on the sandbox solutions in your environment. You'll not only be able to see which sites do have sandbox solutions, whether they've an assembly and are activated, but the tool also can download and analyze the solution for you giving you information about what's inside (is it an InfoPath solution, does it contain web parts, does it contain event receivers,...). This information will be helpful in finding the needed transformation guidance and it will help assessing the sandbox remediation needs. If you want to learn more about how to transform sandbox solutions then please checkout the [sandbox solution transformation guidance on MSDN](https://msdn.microsoft.com/en-us/pnp_articles/sandbox-solution-transformation-guidance).

# Quick start guide #
## Download the tool ##
You can download the tool from here:
 - [Sandbox tool for SharePoint Online](https://github.com/OfficeDev/PnP-Tools/blob/master/Solutions/SharePoint.SandBoxTool/Releases/SandboxTool%20For%20SharePoint%20Online.zip?raw=true "SandboxTool for SharePoint Online")
 - [Sandbox tool for SharePoint 2016](https://github.com/OfficeDev/PnP-Tools/blob/master/Solutions/SharePoint.SandBoxTool/Releases/SandboxTool%20For%20SharePoint%202016.zip?raw=true "Sandbox tool for SharePoint 2016")
 - [Sandbox tool for SharePoint 2013](https://github.com/OfficeDev/PnP-Tools/blob/master/Solutions/SharePoint.SandBoxTool/Releases/SandboxTool%20For%20SharePoint%202013.zip?raw=true "Sandbox tool for SharePoint 2013")

Once you've downloaded the tool (or alternatively you can also compile it yourself using Visual Studio) you have a folder containing the tool **sandboxtool.exe**. Start a (PowerShell) command prompt and navigate to that folder so that you can use the tool.

## Using the tool for SharePoint Online ##
Since this tool needs to be able to scan all site collections it's recommended to use an app-only principal with tenant scoped permissions for the scan. This approach will ensure the tool has access, if you use an account (e.g. your SharePoint tenant admin account) then the tool can only access the sites where this user also has access.

### Setting up an app-only principal with tenant permissions ###
Below steps show how to setup the app-only principal with the needed permissions. Note that you'll need to he a SharePoint tenant administrator to complete these steps.

#### Create a new principal ####
Navigate to a site in your tenant (e.g. https://contoso.sharepoint.com) and then call the **appregnew.aspx** page (e.g. https://contoso.sharepoint.com/_layouts/15/appregnew.aspx). In this page click on the **Generate** button to generate a client id and client secret and fill the remaining information like shown in the screen-shot below.

![create app-only principal](http://i.imgur.com/pKoD872.png)

>**Important**
> - Store the retrieved information (client id and client secret) since you'll need this in the next step!

#### Grant permissions to the created principal ####
Next step is granting permissions to the newly created principal. Since we're granting tenant scoped permissions this granting can only be done via the **appinv.aspx** page on the tenant administration site. You can reach this site via https://contoso-admin.sharepoint.com/_layouts/15/appinv.aspx. Once the page is loaded add your client id and look up the created principal:

![Grant permissions to app-only principal](http://i.imgur.com/L0AkFKZ.png)

In order to grant permissions you'll need to provide the permission XML that describes the needed permissions. Since the sandbox solution scanner needs to be able to access all sites + also uses search with app-only it **requires** below permissions:

```xml
<AppPermissionRequests AllowAppOnlyPolicy="true">
  <AppPermissionRequest Scope="http://sharepoint/content/tenant" Right="FullControl" />
</AppPermissionRequests>
```

When you click on **Create** you'll be presented with a permission consent dialog. Press **Trust It** to grant the permissions:

![Trust principal](http://i.imgur.com/0izhKlX.png)

>**Important**
> - Please safeguard the created client id/secret combination as would it be your administrator account. Using this client id/secret one **can read/update all data in your SharePoint Online environment**!

With the preparation work done let's continue with doing a scan.

### Scanning and analyzing your SharePoint Online MT environment ###
Below option is the typical usage of the tool for most customers: you specify a mode, your tenant name and the created client id and secret:

```console
sandboxtool -m scanandanalyze -t <tenant> -c <clientid> -s <clientsecret>
```

A real life sample:

```console
sandboxtool -m scanandanalyze -t contoso -c 7a5c1615-997a-4059-a784-db2245ec7cc1 -s eOb6h+s805O/V3DOpd0dalec33Q6ShrHlSKkSra1FFw=
```

You'll see the following output during the run:

```console
=====================================================
Scanning is starting...19/08/2016 12:41:52
=====================================================
Building a list of site collections...
Processing site https://contoso.sharepoint.com/...
Processing site https://contoso.sharepoint.com/sites/130041...
Processing site https://contoso.sharepoint.com/portals/community...
Processing site https://contoso.sharepoint.com/sites/130042...
Processing site https://contoso.sharepoint.com/sites/test1...
Processing site https://contoso.sharepoint.com/portals/hub...
Site https://contoso.sharepoint.com/sites/test1 processed. Found 5 solutions in total of which 2 have assemblies
and are activated
Processing site https://contoso.sharepoint.com/sites/records...
Processing site https://contoso.sharepoint.com/sites/redirectbug...
=====================================================
Scanning is done...now dump the results to a CSV file
=====================================================
Outputting scan results to 636072073126632445\sandboxreport.csv
Outputting errors to 636072073126632445\errors.csv
=====================================================
All done. Took 00:00:33.9736871 for 187 sites
=====================================================
```

After the run you'll find a new sub folder (e.g. 636072073126632445) which contains the following:
 - **sandboxreport.csv**: this file contains the output of the scan. Depending on the chosen scan mode you'll see more columns in this csv file. 
 - **error.csv**: if the scan tool encountered errors then these are logged in this file.
 - **Multiple folder with a guid as name**: if you've chosen the **scanandanalyze** or **scananddownload** mode then whenever we find an sandbox solution with an assembly it will be downloaded by the tool. The downloaded file is placed in a folder per site collection (we use site id as the folder name). If a site collection contains multiple sandbox solutions with an assembly then these are all added to the same site collection folder. If you've chosen the **scanandanalyze** method then next to the downloaded sandbox solution you'll also see an XML file containing information about the analyzed sandbox solution. This information is useful for planning your remediation activities. And finally, but not least, you'll also can see a sandbox solution ending on **_fixed.wsp**: if the tool detected that the sandbox solution only contained an empty assembly it will recreate the solution file without assembly. 

![Analysis results folder](http://i.imgur.com/iYtcxIM.png)

## Using the tool for SharePoint 2013 or SharePoint 2016 ##
When using this tool for SharePoint 2013 or SharePoint 2016 you'll need to use regular credentials. In on-premises SharePoint you can easily grant an given account full control on all the site collections using web application policies.

>Note:
> - Being able to scan on-premises is important if you're currently migrating to SharePoint Online since in SharePoint Online you'll need to deal with your sandbox solutions.

### Scanning and analyzing your SharePoint 2013 or SharePoint 2016 environment ###
Below option is the typical usage of the tool for most customers: you specify a mode, the sites to scan, your account, password and domain:

```console
sandboxtool -m scanandanalyze -r <sitestoscan> -u <user> -o <domain> -p <pwd>
```

A real life sample:

```console
sandboxtool -m scanandanalyze -r https://portal2013.pnp.com/*,https://mysites2013.pnp.com/* -u user -o domain -p pwd
```

The output you'll see is similar than the output shown previously in the SharePoint Online documentation.

# Understanding the scan output (sandboxreport.csv file) #
Depending on the chosen scan mode you'll see the below columns in the report:

**Mode: scan or scananddownload**: 
 - **SiteURL**: Url of the scanned site collection
 - **SiteOwner**: If the scanned sandbox solution contains assemblies then we list the site owners as you might want to contact these owners
 - **WSPName**: Name of the sandbox solution 
 - **Author**: Author who originally authored the sandbox solution
 - **CreatedDate**: Date when the solution was created
 - **Activated**: Is this sandbox solution activated
 - **HasAssemblies**: Does this sandbox solution have assemblies
 - **SolutionHash**: The hash that uniquely describes the sandbox solution
 - **SolutionID**: The ID of the sandbox solution
 - **SiteID**: the ID of the site collection

**Mode: scanandanalyze**: the columns from above +
 - **IsEmptyAssembly**: Does this sandbox solution contain an empty assembly
 - **IsInfoPath**: Does this sandbox solution belong to a published InfoPath form with code behind
 - **IsEmptyInfoPathAssembly**: Does this InfoPath sandbox solution contain an empty assembly
 - **HasWebParts**: Does the sandbox solution contain web parts
 - **HasWebTemplate**: Does the sandbox solution contain web templates
 - **HasFeatureReceivers**: Does the sandbox solution have feature receivers
 - **HasEventReceivers**: Does the sandbox solution have event receivers
 - **HasListDefinition**: Does the sandbox solution contain list definitions
 - **HasWorkflowAction**: Does the sandbox solution have workflow actions

# Advanced topics #

## I'm running SharePoint Online dedicated ##
In SharePoint Online Dedicated one can have vanity url's like teams.contoso.com which implies that the tool cannot automatically determine the used url's and tenant admin center url. Using below command line switches you can specify the site url's to scan and the tenant admin center url. Note that the urls need to be separated by a comma.

```console
sandboxtool -m scanandanalyze -r <urls> -a <tenantadminsite> -c <clientid> -s <clientsecret>
```

A real life sample:

```console
sandboxtool -m scanandanalyze -r https://team.contoso.com/*,https://mysites.contoso.com/* 
            -a https://contoso-admin.contoso.com -c 7a5c1615-997a-4059-a784-db2245ec7cc1 
            -s eOb6h+s805O/V3DOpd0dalec33Q6ShrHlSKkSra1FFw=
```

>**Note**
> If you're not interested in also scanning your OD4B sites then you can add the **-x parameter** which will prevent the sandbox scanner from enumerating all OD4B sites

## I don't want to use app-only ##
The best option is to use app-only since that will ensure that the tool can read all site collections but you can also run the tool using credentials.

```console
sandboxtool -m scanandanalyze -t <tenant> -u <user> -p <password>
```

A real life sample:

```console
sandboxtool -m scanandanalyze -t contoso -u admin@contoso.onmicrosoft.com -p mysecret
```

## I only want to scan a few sites ##
Using the urls command line switch you can control which sites are scanned. You can specify one or more url's which can have a wild card. Samples of valid url's are:
 - https://contoso.sharepoint.com/*
 - https://contoso.sharepoint.com/sites/mysite
 - https://contoso-my.sharepoint.com/personal/*

To specify the url's you can use the -r parameter as shown below:

```console
sandboxtool -m scanandanalyze -r https://contoso.sharepoint.com/*,https://contoso.sharepoint.com/sites/mysite,https://contoso-my.sharepoint.com/personal/* 
-c 7a5c1615-997a-4059-a784-db2245ec7cc1 -s eOb6h+s805O/V3DOpd0dalec33Q6ShrHlSKkSra1FFw=
```


# Complete list of command line switches for the SharePoint Online version #

```Console
-m, --mode             Required. (Default: scananddownload) Execution mode. Choose scan for a basic scan,
                       scananddownload for also downloading the sandbox solutions or scanandanalyze for downloading
                       + analyzing the SB solutions

-t, --tenant           Tenant name, e.g. contoso when your sites are under https://contoso.sharepoint.com/sites.
                       This is the recommended model for SharePoint Online MT as this way all site collections will
                       be scanned

-r, --urls             List of (wildcard) urls (e.g.
                       https://contoso.sharepoint.com/*,https://contoso-my.sharepoint.com,https://contoso-my.sharepo
                       int.com/personal/*) that you want to get scanned. When you specify the --tenant option then
                       this parameter is ignored

-c, --clientid         Client ID of the app-only principal used to scan your site collections

-s, --clientsecret     Client Secret of the app-only principal used to scan your site collections

-u, --user             User id used to scan/enumerate your site collections

-p, --password         Password of the user used to scan/enumerate your site collections

-a, --tenantadminsite  Url to your tenant admin site (e.g. https://contoso-admin.contoso.com): only needed when
                       your not using SPO MT

-x, --excludeOD4B      (Default: False) Exclude OD4B sites from the scan

-e, --separator        (Default: ,) Separator used in output CSV files

-h, --threads          (Default: 10) Number of parallel threads, maximum = 100

-d, --duplicates       (Default: False) Download and process all sandbox solutions, not just the unique ones

-v, --verbose          (Default: False) Show more execution details

--help                 Display this help screen.
```

# Complete list of command line switches for the SharePoint 2013/2016 version #

```Console

-m, --mode          Required. (Default: scananddownload) Execution mode. Choose scan for a basic scan,
                    scananddownload for also downloading the sandbox solutions or scanandanalyze for downloading +
                    analyzing the SB solutions

-r, --urls          List of (wildcard) urls (e.g.
                    https://team.contoso.com/*,https://mysites.contoso.com,https://mysites.contoso.com/personal/*)
                    that you want to get scanned

-u, --user          User id used to scan/enumerate your site collections

-o, --domain        Domain of the user used to scan/enumerate your site collections

-p, --password      Password of the user used to scan/enumerate your site collections

-e, --separator     (Default: ,) Separator used in output CSV files

-h, --threads       (Default: 10) Number of parallel threads, maximum = 100

-d, --duplicates    (Default: False) Download and process all sandbox solutions, not just the unique ones

-v, --verbose       (Default: False) Show more execution details

--help              Display this help screen.
```

<img src="https://telemetry.sharepointpnp.com/pnp-tools/solutions/sharepoint-sandboxtool" /> 


