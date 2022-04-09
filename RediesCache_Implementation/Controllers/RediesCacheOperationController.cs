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
                //await _distributedCache.RemoveAsync(RedisCacheKey);
                response = await _rediesCacheOperationDL.AddInformation(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetInformation(GetInformationRequest request)
        {
            GetInformationResponse response = new GetInformationResponse()
            {
                IsSuccess = true,
                Message = "Successful"
            };
            try
            {
                string SerializeList = string.Empty;
                //var EncodedList = await _distributedCache.GetAsync(request.UserID.ToString());
                var Lists = await _distributedCache.GetStringAsync(request.UserID.ToString());
                if (/*EncodedList*/ Lists != null)
                {
                    response.data = new GetInformation();
                    
                    //SerializeList = Encoding.UTF8.GetString(EncodedList);
                    //response.data = JsonConvert.DeserializeObject<GetInformation>(SerializeList);

                    response.data = JsonConvert.DeserializeObject<GetInformation>(Lists);
                }
                else
                {
                    response = await _rediesCacheOperationDL.GetInformation(request);
                    if (response.IsSuccess)
                    {
                        SerializeList = JsonConvert.SerializeObject(response.data);
                        
                        //EncodedList = Encoding.UTF8.GetBytes(SerializeList);
                        
                        var Option = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(20)) // The Cache will be expired after a particular time only if it has not been used during that time span
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(6)); // The Cache will be expired after a perticular time irrespective of the fact whether it has been used or not in that time span
                                                                              //.AbsoluteExpirationRelativeToNow(DateTime.Now.AddHours(6)); // The Cache work similar to absolute expiration
                        //await _distributedCache.SetAsync(request.UserID.ToString(), EncodedList, Option);
                        
                        await _distributedCache.SetStringAsync(request.UserID.ToString(), SerializeList, Option);
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
                    await _distributedCache.RemoveAsync(request.UserID.ToString());
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
                    await _distributedCache.RemoveAsync(request.UserID.ToString());
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
                    foreach (RefreshRecordTime data in response.data)
                    {
                        if (data.UserId != -1)
                        {
                            Console.WriteLine($"Refresh Sliding Time User ID : {data.UserId}");
                            await _distributedCache.RefreshAsync(data.UserId.ToString());
                        }
                    }
                }
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }
    }
}
