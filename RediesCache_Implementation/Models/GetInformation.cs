using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RediesCache_Implementation.Models
{
    public class GetInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<GetInformation> data { get; set; }
    }

    public class GetInformation
    {
        //UserName, EmailID, MobileNumber, Salary, Gender
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public int Salary { get; set; }
        public string Gender { get; set; }
    }
}
