using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RediesCache_Implementation.Models
{
    public class DeleteInformationRequest
    {
        public int UserID { get; set; }
    }

    public class DeleteInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
