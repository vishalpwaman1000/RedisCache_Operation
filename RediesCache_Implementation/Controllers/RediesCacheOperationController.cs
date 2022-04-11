using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RediesCache_Implementation.DataAccessLayer;
using RediesCache_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RediesCache_Implementation.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class RediesCacheOperationController : ControllerBase
    {
        public readonly IRediesCacheOperationDL _rediesCacheOperationDL;
        public readonly IDistributedCache _distributedCache;
        string RedisCacheKey = "Master1";
        public RediesCacheOperationController(IRediesCacheOperationDL rediesCacheOperationDL, IDistributedCache distributedCache)
        {
            _rediesCacheOperationDL = rediesCacheOperationDL;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse();
            try
            {
                await _distributedCache.RemoveAsync(RedisCacheKey);
                response = await _rediesCacheOperationDL.AddInformation(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetInformation()
        {
            GetInformationResponse response = new GetInformationResponse()
            {
                IsSuccess = true,
                Message = "Successful"
            };
            try
            {
                string SerializeList = string.Empty;
                var EncodedList = await _distributedCache.GetAsync(RedisCacheKey);
                if (EncodedList != null)
                {
                    await _distributedCache.RemoveAsync(RedisCacheKey);
                    response.data = new List<GetInformation>();
                    SerializeList = Encoding.UTF8.GetString(EncodedList);
                    response.data = JsonConvert.DeserializeObject<List<GetInformation>>(SerializeList);
                }
                else
                {
                    response = await _rediesCacheOperationDL.GetInformation();
                    if (response.IsSuccess)
                    {
                        SerializeList = JsonConvert.SerializeObject(response.data);
                        EncodedList = Encoding.UTF8.GetBytes(SerializeList);
                        var Option = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(20)) // After 20 min Entry will be Inactive
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(6)); // Expired in 6 hour
                        await _distributedCache.SetAsync(RedisCacheKey, EncodedList, Option);
                    }
                }



            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateInformation(UpdateInformationRequest request)
        {
            UpdateInformationResponse response = new UpdateInformationResponse();
            try
            {
                response = await _rediesCacheOperationDL.UpdateInformation(request);
                if (response.IsSuccess)
                {
                    await _distributedCache.RemoveAsync(RedisCacheKey);
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInformation(DeleteInformationRequest request)
        {
            DeleteInformationResponse response = new DeleteInformationResponse();
            try
            {

                response = await _rediesCacheOperationDL.DeleteInformation(request);
                if (response.IsSuccess)
                {
                    await _distributedCache.RemoveAsync(RedisCacheKey);
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> RefreshRecordTime()
        {
            RefreshRecordTimeResponse response = new RefreshRecordTimeResponse();
            try
            {

                response = await _rediesCacheOperationDL.RefreshRecordTime();
                if (response.IsSuccess)
                {
                    await _distributedCache.RefreshAsync(RedisCacheKey);
                    response.Message = "Cache Refresh Successfully";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

    }
}
