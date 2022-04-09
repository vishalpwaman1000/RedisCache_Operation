using RediesCache_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RediesCache_Implementation.DataAccessLayer
{
    public interface IRediesCacheOperationDL
    {
        public Task<AddInformationResponse> AddInformation(AddInformationRequest request);

        public Task<GetInformationResponse> GetInformation();

        public Task<UpdateInformationResponse> UpdateInformation(UpdateInformationRequest request);

        public Task<DeleteInformationResponse> DeleteInformation(DeleteInformationRequest request);

        public Task<RefreshRecordTimeResponse> RefreshRecordTime();
    }
}
