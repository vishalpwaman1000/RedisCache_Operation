using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RediesCache_Implementation.DataAccessLayer;
using RediesCache_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RediesCache_Implementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RediesCacheOperationController : ControllerBase
    {
        public readonly IRediesCacheOperationDL _rediesCacheOperationDL; 
        public RediesCacheOperationController(IRediesCacheOperationDL rediesCacheOperationDL)
        {
            _rediesCacheOperationDL = rediesCacheOperationDL;
        }

        [HttpPost]
        public async Task<IActionResult> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse();
            try
            {

                response = await _rediesCacheOperationDL.AddInformation(request);

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetInformation()
        {
            GetInformationResponse response = new GetInformationResponse();
            try
            {

                response = await _rediesCacheOperationDL.GetInformation();

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
