using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.IO;

namespace morris_azstorage_service.Models
{
    
    [AttributeUsage(AttributeTargets.Property,
                Inherited = false,
                AllowMultiple = false)]
    internal sealed class OptionalAttribute : Attribute
    {
    }
    public class metaItem 
        {
            public string Key { get; set; }
            public string Value{get;set;}
        }

   
    public class metaData
        {
            public IDictionary<string,string> metadata { get; set; }            
        }
    public class PTDownloadItem 
        {

        public string fileName { get; set; }
        [Optional]
        public string title { get; set; }
    }
    public class morrisMetaData
    {
            public string title { get; set; } = null;
            [Optional]
            public string caption { get; set; } = null;
            [Optional]
            public string altText { get; set; } = null;
            [Optional]
            public string refDate { get; set; } = null;
            [Optional]
            public string latitude { get; set; } = null;
            [Optional]
            public string longitude { get; set; } = null;
            [Optional]
            public string description { get; set; } = null;
            [Optional]
            public string category { get; set; } = null;
            [Optional]
            public string user { get; set; } = null;
            [Optional]
            public string owner { get; set; } = null;
            [Optional]
            public string component { get; set; } = null;
            [Optional]
            public string domId { get; set; } = null;
        }    
    public class morrisBlobInfo
        {
            public string blobName { get; set; }
            public string containerName { get; set; }
            
            [Optional]
            public string saveAsFileName { get; set; } = null;
            [Optional]
            public morrisMetaData metadata { get; set; } = null;
            
            }
        public class BlobAsset
        {
            public IFormFile asset { get; set; }
            public string containerName { get; set; }
            [Optional]
            public morrisMetaData metadata { get; set; } = null;
            
        }

    public class DownloadItem
    {
    
        public string blobName { get; set; }
        
        
        [Optional]
        public string friendlyName { get; set; } = null;

    }

    

    public class BlobStreamMeta
    {
        public Stream stream { get; set; }
        public string container { get; set; }
        
        public string saveAsFileName { get; set; } = null;
        [Optional]
        public string title { get; set; } = null;
        [Optional]
        public string caption { get; set; } = null;
        [Optional]
        public string altText { get; set; } = null;
        [Optional]
        public string refDate { get; set; } = null;
        [Optional]
        public string latitude { get; set; } = null;
        [Optional]
        public string longitude { get; set; } = null;
        [Optional]
        public string description { get; set; } = null;
        [Optional]
        public string category { get; set; } = null;
        [Optional]
        public string user { get; set; } = null;
        [Optional]
        public string owner { get; set; } = null;
        [Optional]
        public string component { get; set; } = null;
        [Optional]
        public string domId { get; set; } = null;

    }
    public class BlobMeta
  {
            public IFormFile asset { get; set; }
            public string container { get; set; }
             [Optional]
            public string saveAsFileName { get; set; } = null;
            [Optional]
            public string title { get; set; } = null;
            [Optional]
            public string caption { get; set; } = null;
            [Optional]
            public string altText { get; set; } = null;
            [Optional]
            public string refDate { get; set; } = null;
            [Optional]
            public string latitude { get; set; } = null;
            [Optional]
            public string longitude { get; set; } = null;
            [Optional]
            public string description { get; set; } = null;
            [Optional]
            public string category { get; set; } = null;
            [Optional]
            public string user { get; set; } = null;
            [Optional]
            public string owner { get; set; } = null;
            [Optional]
            public string component { get; set; } = null;
            [Optional]
            public string domId { get; set; } = null;
            
        }
     public class Blobi
        {
            public string containerName { get; set; }
            public string blobName { get; set; }
            [Optional]
            public IDictionary<string,string> metadata { get; set; } = null;

        }



    public class PTDownloadRequest
    {
        public string folderName { get; set; }

        public List<PTDownloadItem> downloadFiles { get; set;}
    }

}