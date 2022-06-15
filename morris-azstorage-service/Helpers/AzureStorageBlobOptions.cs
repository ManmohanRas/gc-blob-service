using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace morris_azstorage_service.Helpers
{
    public class AzureStorageBlobOptions
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ConnectionString { get; set; }
        public string FilePath { get; set; }

        //public IOptions As Options()
        //{
        //    return Options.Create(this);
        //}
    }
}
