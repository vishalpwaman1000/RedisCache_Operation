using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RediesCache_Implementation.Models
{
    public class RefreshRecordTimeResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<RefreshRecordTime> data { get; set; }
    }

    public class RefreshRecordTime
    {
        public int UserId { get; set; }
    }
}
