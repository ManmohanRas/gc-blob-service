using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace morris_azstorage_service.Models
{
    public class DownloadResult
    {
        public DownloadResult(bool isSuccessful, Response response, string errorMessage = "")
        {
            IsSuccessful = isSuccessful;
            Response = response;
            ErrorMessage = errorMessage;
        }

        public bool IsSuccessful { get; set; }
        public Response Response { get; set; }
        public string ErrorMessage { get; set; }
    }
}
