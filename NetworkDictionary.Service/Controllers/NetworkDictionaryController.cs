using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetworkDictionary.Dispatcher.Interfaces;
using NetworkDictionary.Domain.Dto;

namespace NetworkDictionary.Service.Controllers
{
    [Route("api/v1/[controller]")]
    public class NetworkDictionaryController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public NetworkDictionaryController(IDispatcher dispatcher) { _dispatcher = dispatcher; }

        /// <summary>
        /// Get keys
        /// </summary>
        /// <response code="200">Execution success</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal service error</response>
        [HttpGet("keys")]
        [ProducesResponseType(typeof(GetKeysResponseDto), 200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetKeys(GetKeysRequestDto request)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var result = await _dispatcher.GetKeys(request);
            return Ok(result);
        }
    }
}
