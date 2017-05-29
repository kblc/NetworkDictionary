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

        /// <summary>
        /// Set value
        /// </summary>
        /// <response code="200">Execution success</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal service error</response>
        [HttpPost("key")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SetValue([FromBody]SetValueRequestDto request)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            await _dispatcher.SetValue(request);
            return Ok();
        }

        /// <summary>
        /// Get value
        /// </summary>
        /// <response code="200">Execution success</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal service error</response>
        [HttpGet("key")]
        [ProducesResponseType(typeof(GetValueResponseDto), 200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetValue(GetValueRequestDto request)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var result = await _dispatcher.GetValue(request);
            return Ok(result);
        }

        /// <summary>
        /// Delete Key
        /// </summary>
        /// <response code="200">Execution success</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal service error</response>
        [HttpDelete("key")]
        [ProducesResponseType(typeof(DeleteKeyResponseDto), 200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteKey([FromBody]DeleteKeyRequestDto request)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var result = await _dispatcher.DeleteKey(request);
            return Ok(result);
        }

        /// <summary>
        /// Get options
        /// </summary>
        /// <response code="200">Execution success</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal service error</response>
        [HttpGet("options")]
        [ProducesResponseType(typeof(GetOptionsResponseDto), 200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOptions()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var result = await _dispatcher.GetOptions();
            return Ok(result);
        }

        /// <summary>
        /// Set options
        /// </summary>
        /// <response code="200">Execution success</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal service error</response>
        [HttpPost("options")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SetOptions([FromBody]SetOptionsRequestDto request)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            await _dispatcher.SetOptions(request);
            return Ok();
        }

        /// <summary>
        /// Packet execution
        /// </summary>
        /// <response code="200">Execution success</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal service error</response>
        [HttpPost("execute")]
        [ProducesResponseType(typeof(PacketResponseDto), 200)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PacketExecution([FromBody]PacketRequestDto request)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            var result = await _dispatcher.GetPacketExecutionResult(request);
            return Ok(result);
        }
    }
}
