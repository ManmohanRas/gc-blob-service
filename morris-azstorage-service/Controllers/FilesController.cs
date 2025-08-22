
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using morris_azstorage_service.Models;
using morris_azstorage_service.Helpers;
using Microsoft.Extensions.Configuration;

using System.Diagnostics;
using System.Net.Http;

using Azure;
using Azure.Core.Pipeline;
using NUnit.Framework;
using Azure.Storage.Sas;

namespace morris_azstorage_service.Controllers
{

    //[Authorize(Policy = "ApiScope")]
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        // private readonly IOptions<AzureStorageBlobOptions> config;
        //     private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=mcprima;AccountKey=0Alzc05DLS+JdI4BRgaVM0KWqIX5qZ4Ua+NFiPW9XBKsA5YbHF+x/iA45300nR9CITG3drKxkedSrauX/Zfq1Q==;EndpointSuffix=core.windows.net";
        public IConfiguration Configuration { get; }



        public FilesController(IConfiguration configuration)
        {

            Configuration = configuration;
        }



        [HttpGet("ListFilesTest")]
        [ProducesResponseType(typeof(List<string>), 200)]

        public async Task<IActionResult> ListFilesTest()
        {
            // Define the cancellation token.
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationtoken = source.Token;

            // Get a connection string to our Azure Storage account.
            //string connectionString = config.Value.ConnectionString;
            string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, "pres-trust-client");
            try
            {
                // List all the blobs
                List<Blobi> blobs = new List<Blobi>();

                AsyncPageable<BlobItem> blobitems = container.GetBlobsAsync(
                          traits: BlobTraits.Metadata, //include metadata
                          states: BlobStates.Snapshots, //Snapshots
                          prefix: null,
                          cancellationToken: cancellationtoken

                  );
                await foreach (BlobItem blob in blobitems)
                {

                    blobs.Add(new Blobi()
                    {
                        containerName = "pres-trust-client",
                        blobName = blob.Name,
                        metadata = blob.Metadata
                    }
                          );

                }
                return Ok(blobs);
            }
            catch (Exception ex)
            {
                // Clean up after the test when we're finished
                return Ok($"Unexpected error: {ex}");
            }

        }




        /// <summary>
        /// List files within a container, with optional filters.
        /// </summary>
        /// <returns> Returns a list of files inside an Azure container.</returns>
        /// <remarks> Three additional parameters allow containers to be filtered by: **file prefix_**, **file name_** and **file type_**. **The prefix filter is the most efficient**,
        /// as it is applied directly to the Azure REST API query!<br></br>
        /// File extensions and partial string searches must loop through each file name to determine equality AFTER the prefix is applied, so it is most efficient to use
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     POST/ListFilesByContainer
        ///     {
        ///         "containerName": "pres-trust-client",
        ///         "prefix": "ostf",
        ///         "partialstring": ""
        ///     }
        ///</remarks>
        /// <param name="cf">The Blob container filter object. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>

        [HttpPost("ListFiles")]
        //    
        // [ProducesResponseType(typeof(List<string>),200)]
        // [AllowAnonymous]
        public async Task<IActionResult> ListFiles([FromBody] ContainerFilter cf)
        {

            // Define the cancellation token.
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationtoken = source.Token;

            // Get a connection string to our Azure Storage account.
            //string connectionString = config.Value.ConnectionString;
            string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];
            string _container = User.FindFirst("client_id").Value;
            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, cf.containerName);
            if (cf.containerName != _container)
            {
                return Ok("Container is disabled until authorization is enabled");
            }

            if (!container.Exists())
            {
                return Ok("Invalid container");
            }
            try
            {
                // List all the blobs
                List<Blobi> blobs = new List<Blobi>();

                await foreach (BlobItem blob in container.GetBlobsAsync(
                        traits: BlobTraits.Metadata, //include metadata
                        states: BlobStates.Snapshots, //Snapshots
                        prefix: cf.prefix,
                        cancellationToken: cancellationtoken

                ))
                {

                    if (cf.partialstring.Length > 0)
                    {
                        if (blob.Name.ToLower().Contains(cf.partialstring.ToLower()))
                        {
                            blobs.Add(new Blobi()
                            {
                                containerName = cf.containerName,
                                blobName = blob.Name,
                                metadata = blob.Metadata
                            }
                             );
                        }
                    }
                    else
                    {
                        blobs.Add(new Blobi()
                        {
                            containerName = cf.containerName,
                            blobName = blob.Name,
                            metadata = blob.Metadata
                        }
                                     );
                    }


                }

                return Ok(blobs);
            }

            catch (Exception ex)
            {
                // Clean up after the test when we're finished
                return Ok($"Unexpected error: {ex}");
            }
        }

        /// <summary>
        /// Get a Blob's metadata.
        /// </summary>
        /// <returns> Returns a blob's metadata.</returns>
        /// <remarks> Three additional parameters allow containers to be filtered by: **file prefix_**, **file name_** and **file type_**. **The prefix filter is the most efficient**,
        /// as it is applied directly to the Azure REST API query!<br></br>
        /// File extensions and partial string searches must loop through each file name to determine equality AFTER the prefix is applied, so it is most efficient to use
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     POST/GetBlobMetadata
        ///     {
        ///         "containerName": "pres-trust-client",
        ///         "blobName": "morris"
        ///     }
        ///</remarks>
        /// <param name="blobi">The Blob container. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>
        //[AllowAnonymous]
        [HttpPost("GetFile")]
        //    
        // [ProducesResponseType(typeof(List<string>),200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile([FromBody] Blobi blobi)
        {

            // Define the cancellation token.
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationtoken = source.Token;

            // Get a connection string to our Azure Storage account.
            string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, blobi.containerName);

            if (!container.Exists())
            {
                return Ok("Invalid container");
            }
            try
            {
                // List all the blobs
                BlobClient blob = container.GetBlobClient(blobi.blobName);

                if (!blob.Exists())
                {

                    return Ok("File " + blobi.blobName + " not found.");
                }
                // Get the blob's properties and metadata.
                BlobProperties properties = await blob.GetPropertiesAsync();
                Blobi results = new Blobi()
                {
                    containerName = blobi.containerName,
                    blobName = blobi.blobName,
                    metadata = properties.Metadata
                };
                return Ok(results);

            }

            catch (Exception ex)
            {
                // Clean up after the test when we're finished
                return Ok($"Unexpected error: {ex}");
            }
        }



        /// <summary>
        /// Upload a file to a container, with optional metadata.
        /// </summary>
        /// <returns> Uploads a file to an Azure container.</returns>
        /// <remarks> The original file name is maintained, and suffixed with a numerical (1) counter if the file already exists.<br></br>
        /// Use the other method to include a renamed file.
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     POST/UploadFile
        ///     {
        ///        "asset": {IFormFile},
        ///        "container": "pres-trust-client",
        ///        "saveAsFileName":null,
        ///        "title":null,
        ///        "caption":null,
        ///        "altText":null,
        ///        "refDate":null,
        ///        "latitude":0,
        ///        "longitude":0,
        ///        "description":null,
        ///        "category":null,
        ///        "user":null,
        ///        "owner":null,
        ///        "component":null,
        ///        "domId":null
        ///     }
        ///</remarks>
        /// <param name="bm">The Blob file object. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>
        [HttpPost("UploadFile")]
        //  [AllowAnonymous]
        public async Task<ActionResult<bool>> UploadFile([FromForm] BlobMeta bm)
        {

            try
            {
                // temporary protection of non-prestrust containers
                string _container = User.FindFirst("client_id").Value;
                if (bm.container != _container)
                {
                    return BadRequest(false);

                }

                // Replace user's extension(or lack-thereof) with the extension from the uploaded file
                if (bm.saveAsFileName is null)
                {
                    bm.saveAsFileName = bm.asset.FileName;
                }
                if (bm.saveAsFileName != bm.asset.FileName)
                {
                    bm.saveAsFileName = Path.ChangeExtension(bm.saveAsFileName, GetExtension(bm.asset.FileName).ToLower());
                }


                // Define the cancellation token.
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken cancellationtoken = source.Token;

                // Get a connection string to our Azure Storage account.
                string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

                // Get a reference to a container named "sample-container" and then create it
                BlobContainerClient container = new BlobContainerClient(connectionString, bm.container);


                // Get a reference to a blob
                string fn = bm.saveAsFileName ?? bm.asset.FileName;
                BlobClient blob = container.GetBlobClient(fn);

                await blob.UploadAsync(bm.asset.OpenReadStream());

                // Add metadata to the blob.

                IDictionary<string, string> dict = new Dictionary<string, string>();

                if (bm.title is null) { dict.Add("title", bm.saveAsFileName); } else { dict.Add("title", bm.title); };
                if (bm.caption != null) { dict.Add("caption", bm.caption); };
                if (bm.altText != null) { dict.Add("altText", bm.altText); };
                if (bm.refDate != null) { dict.Add("refDate", bm.refDate); };
                if (bm.latitude != "0" && bm.latitude != null) { dict.Add("latitude", bm.latitude.ToString()); };
                if (bm.longitude != "0" && bm.longitude != null) { dict.Add("longitude", bm.longitude.ToString()); };
                if (bm.description != null) { dict.Add("description", bm.description); };
                if (bm.category != null) { dict.Add("category", bm.category); };
                if (bm.user != null) { dict.Add("creator", bm.user); };
                if (bm.owner != null) { dict.Add("owner", bm.owner); };
                if (bm.component != null) { dict.Add("component", bm.component); };
                if (bm.domId != null) { dict.Add("domId", bm.domId); };

                await blob.SetMetadataAsync(dict);

                return Ok(true);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    false);
            }
        }
        /// <summary>
        /// Update blob metadata.
        /// </summary>
        /// <returns> Updates metadata for blob.</returns>
        /// <remarks> The original file name is maintained, and suffixed with a numerical (1) counter if the file already exists.<br></br>
        /// Use the other method to include a renamed file.
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     PUT/UpdateFile
        ///     {
        ///        "blobName": null,
        ///        "containerName": "pres-trust-client",
        ///        "saveAsFileName":null,
        ///        "metadata": {
        ///             "title":null,
        ///             "caption":null,
        ///             "altText":null,
        ///             "refDate":null,
        ///             "latitude":0,
        ///             "longitude":0,
        ///             "description":null,
        ///             "category":null,
        ///             "user":null,
        ///             "owner":null,
        ///             "component":null,
        ///             "domId":null
        ///             }
        ///     }
        ///</remarks>
        /// <param name="bm">The Blob file object. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>
        [HttpPut("UpdateMetadata")]
        // [AllowAnonymous]
        public async Task<string> UpdateFile([FromBody] Blobi bm)
        {


            // Get a connection string to our Azure Storage account.
            string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, bm.containerName);



            // Get Handle to the blob
            BlobClient existsblob = container.GetBlobClient(bm.blobName);
            //  BlobClient newblob = container.GetBlobClient(bm.saveAsFileName);

            // Define the cancellation token.
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationtoken = source.Token;

            try
            {
                // Convert metadat to a dictionary, including only the keys containing values.
                IDictionary<string, string> dict = new Dictionary<string, string>();

                await existsblob.SetMetadataAsync(bm.metadata);

                return "Blob metadata updated";


            }
            catch
            {
                return "Error updating blob";
            }
        }





        /// <summary>
        /// Download a file from a container.
        /// </summary>
        /// <returns> Downloads a file to an Azure container.</returns>
        /// <remarks> The original file name is maintained, and suffixed with a numerical (1) counter if the file already exists.<br></br>
        /// Use the other method to include a renamed file.
        /// Prefix filters to reduce the RBAR.<br></br> 
        ///</remarks>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>
        [HttpGet("GetSASFromApplication")]
        public Uri GetSASFromApplication()
        {
            string _container = User.FindFirst("client_id").Value;

            // Get a connection string to our Azure Storage account.
            string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, _container);

            string sasContainerToken;

            // Create a SAS token that's valid for one hour.
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _container,
                Resource = "c"
            };


            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
            sasBuilder.SetPermissions(BlobContainerSasPermissions.All);

            Uri sasUri = container.GenerateSasUri(sasBuilder);
            Console.WriteLine("SAS URI for blob container is: {0}", sasUri);
            Console.WriteLine();

            return sasUri;


        }


        /// <summary>
        /// Download a file from a container.
        /// </summary>
        /// <returns> Downloads a file to an Azure container.</returns>
        /// <remarks> The original file name is maintained, and suffixed with a numerical (1) counter if the file already exists.<br></br>
        /// Use the other method to include a renamed file.
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     POST/UploadFile
        ///     {
        ///        "containerName": "pres-trust-client",
        ///        "blobName":"MyFilename.pdf",
        ///     }
        ///</remarks>
        /// <param name="containerName">The Blob container name. Required</param>
        /// <param name="blobName">The Blob file name. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>
        [HttpPost("DownloadFile")]
        //  [AllowAnonymous]
        public async Task<IActionResult> DownloadFile([FromBody] Blobi blobi)
        {
            // // Get a connection string to our Azure Storage account.
            string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

            // Get a temporary path on disk where we can download the file
            string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, blobi.containerName);

            await container.ExistsAsync();

            try
            {
                // Get Handle to the blob
                BlobClient blob = container.GetBlobClient(blobi.blobName);
                BlobProperties properties = await blob.GetPropertiesAsync();
                string mediatype = properties.ContentType;


                // Download the blob's contents and save it to a file
                BlobDownloadInfo download = await blob.DownloadAsync();
                var ms = new MemoryStream();


                StorageTransferOptions options = new StorageTransferOptions();
                options.InitialTransferLength = 1024 * 1024;
                options.MaximumConcurrency = 20;
                options.MaximumTransferLength = 4 * 1024 * 1024;
                await blob.DownloadToAsync(ms, null, options);
                ms.Position = 0;


                return Ok(File(ms.ToArray(), mediatype, true));

            }
            finally
            {
                // Clean up after the test when we're finished

            }


        }


        /// <summary>
        /// List files within a container, with optional filters.
        /// </summary>
        /// <returns> Returns a list of files inside an Azure container.</returns>
        /// <remarks> Three additional parameters allow containers to be filtered by: **file prefix_**, **file name_** and **file type_**. **The prefix filter is the most efficient**,
        /// as it is applied directly to the Azure REST API query!<br></br>
        /// File extensions and partial string searches must loop through each file name to determine equality AFTER the prefix is applied, so it is most efficient to use
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     POST/DownloadFilesAsync
        ///         [
        ///             {
        ///                 "fileName": "ostf-62-easement_deed_language-1613667562001.txt",
        ///                 "title": "bulkcopy data.txt"
        ///             },
        ///             {
        ///                 "fileName": "ostf-62-site_file-1610733374841.txt",
        ///                 "title": "ostf-72-existing_property_survey-1600457399678.txt"
        ///             }
        ///         ]
        /// </remarks>
        /// <param name="bloblist">The Blob container filter object. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>

        [HttpPost("DownloadFilesAsync")]
        public async Task<IActionResult> DownloadFilesAsync([FromBody] PTDownloadRequest downloadRequest)
        {
            try
            {


                string _container = User.FindFirst("client_id").Value;
                if (_container == "pres-trust-client-localhost")
                {
                    _container = "pres-trust-client-dev";
                }
                // Define the cancellation token.
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken cancellationtoken = source.Token;

                // Get a connection string to our Azure Storage account.
                string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

                // Get a reference to a container named "sample-container" and then create it
                BlobContainerClient container = new BlobContainerClient(connectionString, _container);

                // Path to the directory to upload
                string downloadPath = downloadRequest.folderName + "\\download_" + DateTime.Now.ToString("yymmdd_hhmmss") + "\\";
                Directory.CreateDirectory(downloadPath);
                Console.WriteLine($"Created directory {downloadPath}");

                // Specify the StorageTransferOptions
                var options = new StorageTransferOptions
                {
                    // Set the maximum number of workers that may be used in a parallel transfer.
                    MaximumConcurrency = 8,

                    // Set the maximum length of a transfer to 50MB.
                    MaximumTransferLength = 50 * 1024 * 1024
                };

                // Download the blobs

                int count = 0;

                // Create a queue of tasks that will each upload one file.
                var tasks = new Queue<Task<Response>>();

                // Iterate through the files
                foreach (PTDownloadItem blobItem in downloadRequest.downloadFiles)
                {
                    string fileName = downloadPath + blobItem.title ?? blobItem.fileName;
                    Console.WriteLine($"Downloading {blobItem.title ?? blobItem.fileName} to {downloadPath}");

                    BlobClient blob = container.GetBlobClient(blobItem.fileName);

                    // Add the download task to the queue
                    tasks.Enqueue(blob.DownloadToAsync(fileName, default, options));
                    count++;
                }

                // Run all the tasks asynchronously.
                await Task.WhenAll(tasks);

                Console.WriteLine($"Downloaded {count} files.");
                return Ok($"Downloaded {count} files to {downloadPath}.");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Azure request failed: {ex.Message}");
                return BadRequest($"Azure request failed: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Error parsing files in the directory: {ex.Message}");
                return BadRequest($"Error parsing files in the directory: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest($"Exception: {ex.Message}");
            }
        }


        /// <summary>
        /// Rename a file in a container.
        /// </summary>
        /// <returns> Copies the original blob to the new, renamed blob - then deletes the old blob.</returns>
        /// <remarks> The original file name is maintained, and suffixed with a numerical (1) counter if the file already exists.<br></br>
        /// Use the other method to include a renamed file.
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     POST/UploadFile
        ///     {
        ///        "containerName": "pres-trust-client",
        ///        "blobName":"MyFilename.pdf",
        ///        "saveAsFileName":"MyNewFilename.pdf",
        ///     }
        ///</remarks>
        /// <param name="containerName">The Blob container name. Required</param>
        /// <param name="blobName">The Blob file name. Required</param>
        /// <param name="saveAsFileName">The Blob file name. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>
        [Route("RenameFile")]
        [HttpPut]
        // [AllowAnonymous]
        public async Task<string> RenameFile([FromBody] morrisBlobInfo bi)
        {
            try
            {
                string _container = User.FindFirst("client_id").Value;
                // temporary protection of non-prestrust containers
                if (bi.containerName != _container)
                {
                    return ("Container is disabled until authorization is enabled");
                }

                // Get a connection string to our Azure Storage account.
                string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];


                // Get a reference to a container named "sample-container" and then create it
                BlobContainerClient container = new BlobContainerClient(connectionString, bi.containerName);



                //Replace user's extension(or lack-thereof) with the extension from the uploaded file
                if (bi.saveAsFileName.Length > 1)
                {
                    bi.saveAsFileName = Path.ChangeExtension(bi.saveAsFileName, GetExtension(bi.blobName).ToLower());
                }


                // Get Handle to the blob
                BlobClient existsblob = container.GetBlobClient(bi.blobName);
                BlobClient newblob = container.GetBlobClient(bi.saveAsFileName);

                var olduri = existsblob.Uri;
                await newblob.StartCopyFromUriAsync(existsblob.Uri);

                await Task.Delay(10000); //wait 10 seconds for copy to complete ?
                bool res = existsblob.DeleteIfExists();

                return "File renamed to " + bi.saveAsFileName;

            }
            catch
            {
                return "Rename failed";
            }
        }

        /// <summary>
        /// Delete a file in a container.
        /// </summary>
        /// <returns> Deletes the blob.</returns>
        /// <remarks> The original file name is maintained, and suffixed with a numerical (1) counter if the file already exists.<br></br>
        /// Use the other method to include a renamed file.
        /// Prefix filters to reduce the RBAR.<br></br> 
        /// Sample request:
        ///
        ///     POST/UploadFile
        ///     {
        ///        "containerName": "pres-trust-client",
        ///        "blobName":"MyFilename.pdf"
        ///     }
        ///</remarks>
        /// <param name="containerName">The Blob container name. Required</param>
        /// <param name="blobName">The Blob file name. Required</param>
        /// <response code="200">Success, return a collection of projectLiteDto. Or an empty array if the the query does not find any projects.</response>
        /// <response code="400">If query has a bad format or has unknown fields.</response> 
        /// <response code="501">Something wrong occurs with this operation.</response>

        [Route("DeleteFile")]
        [HttpPut]
        public async Task<ActionResult<bool>> DeleteFile([FromBody] Blobi blobi)
        {
            try
            {
                string _container = User.FindFirst("client_id").Value;
                // temporary protection of non-prestrust containers
                if (blobi.containerName != _container)
                {
                    return BadRequest(false);
                }

                // Get a connection string to our Azure Storage account.
                string connectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

                // Get a reference to a container named "sample-container" and then create it
                BlobContainerClient container = new BlobContainerClient(connectionString, blobi.containerName);



                // Get Handle to the blob
                bool res = await container.DeleteBlobIfExistsAsync(blobi.blobName);
                //  BlobClient blob = container.GetBlobClient(blobi.blobName);
                //  bool res = blob.DeleteIfExists();

                return Ok(true);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return NotFound(false);
            }
        }
        private string GetExtension(string filename)
        {
            string ext = Path.GetExtension(filename);

            return ext;
        }

        public static async Task AddContainerMetadataAsync(BlobClient blob, IDictionary<string, string> metadata)
        {
            try
            {

                // Set the container's metadata.
                await blob.SetMetadataAsync(metadata);
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

    }
}