using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace morris_azstorage_service.Models
{
        
    // [AttributeUsage(AttributeTargets.Property,
    //             Inherited = false,
    //             AllowMultiple = false)]
    // internal sealed class OptionalAttribute : Attribute
    // {
    // }
    public class ContainerFilter
    {
        public string containerName { get; set; }
          [Optional]
        public string prefix { get; set; } = null;
          [Optional]
        public string partialstring { get; set; } = null;
          [Optional]
        public string metadatakey { get; set; } = null;
          [Optional]
        public string metadatavalue { get; set; } = null;
        [Optional]
        public string[] extensions { get; set; } = null;
      
     
    }

    
}