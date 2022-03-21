using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RediesCache_Implementation.Models
{
    public class UpdateInformationRequest
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public int Salary { get; set; }
        public string Gender { get; set; }
    }

    public class UpdateInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
