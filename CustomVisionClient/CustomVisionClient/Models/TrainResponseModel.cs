using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomVisionClient.Models
{
    class TrainResponseModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    class RateLimitResponseModel
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }
}
