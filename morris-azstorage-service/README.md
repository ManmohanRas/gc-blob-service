# morris-storage-service
![Build and deploy ASP.Net Core app to Azure Web App - morris-azstorageservice](https://github.com/morrisgis/morris-azstorage-service/workflows/Build%20and%20deploy%20ASP.Net%20Core%20app%20to%20Azure%20Web%20App%20-%20morris-azstorageservice/badge.svg?branch=master&event=push)

https://morris-azstorageservice.azurewebsites.net/v1/index.html

> Server Version: 2019-07-07 and 2019-02-02

Azure Blob storage is Microsoft's object storage solution for the cloud. Blob
storage is optimized for storing massive amounts of unstructured data.
Unstructured data is data that does not adhere to a particular data model or
definition, such as text or binary data.

[Source code][source] | [Package (NuGet)][package] | [API reference documentation][docs] | [REST API documentation][rest_docs] | [Product documentation][product_docs]

## Getting started

### Install the package

Install the Azure Storage Blobs client library for .NET with [NuGet][nuget]:

```Powershell
dotnet add package Azure.Storage.Blobs
```

### Sample Blob Storage Hello World Async repository

Follow this link to view sample .NET CORE async code for the Azure.Storage v12 library.
<https://github.com/Azure/azure-sdk-for-net/blob/4033f3a7b094b5f3d82b35eb04d5ba1151ef6d85/sdk/storage/Azure.Storage.Blobs/samples/Sample01b_HelloWorldAsync.cs>

#### Endpoints
|**Num.**|**URL Endpoint**   | **Description**
|:---:|:----------------------|:---------------------------|
|1    |.../Files/ListFiles    | Lists files in a container
|2    |.../Files/UploadFile   | Upload a file to a container
|3    |.../Files/DownloadFile | Download a file from a container
|4    |.../Files/RenameFile   | Rename a file in a container
|5    |.../Files/DeleteFile   | Delete a file from a container


### Publishing Service to IIS

Reference: <https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1>

Use the following command to generate a Release package to a sub folder called publish:

```Powershell
dotnet publish -c Release -f netcoreapp3.1 -r win-x64 -o ./publish
```

**!Application Requirement**
For security, we're hiding the Azure connection string from our code to avoid accidentally sharing it during code distribution.  The abstraction of the connection string adds an extra step in configuration for both development and production.

    1. For development, create a new system environment variable
    2. For production add an environmentVariable in IIS using the Configuration Editor
        (Section:system.webServer/aspNetCore From: .../Web.config)

The program requires a valid Azure storage account to be set up.  Once established, you can copy the connection string from the Azure Portal into an environment variable on the host Server. Set a new System Environment Variable for the Azure connection string on your development machine and the host server:


**name**: AzureStorageBlobOptions__MCPRIMAConnectionString

**value**: DefaultEndpointsProtocol=https;AccountName={_ACCOUNT NAME_};AccountKey={_ACCOUNT KEY_};EndpointSuffix=core.windows.net


#### Open API (Swagger)
The OpenAPI documentation publishing option has been enabled for this web API, allowing endpoints to be described and tested.  

Live Swagger Endpoint: <https://morris-azstorageservice.azurewebsites.net/v1/index.html>
