//using Microsoft.AspNetCore.Mvc;
//using Swashbuckle.AspNetCore.Annotations;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using BusTrackingSystem.Models;
//using BusTrackingSystem.Services;

//namespace BusTrackingSystem.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BusController : ControllerBase
//    {
//        private readonly IBusService _busService;

//        public BusController(IBusService busService)
//        {
//            _busService = busService;
//        }

//        [HttpGet]
//        [SwaggerOperation(Summary = "Get All Buses", Description = "Retrieve a list of all buses.", Tags = new[] { "Bus Management" })]
//        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Bus>))]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
//        public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
//        {
//            try
//            {
//                var buses = await _busService.GetBusesAsync();
//                return Ok(buses);
//            }
//            catch
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = "An error occurred while retrieving buses." });
//            }
//        }



//        [HttpPost]
//        [SwaggerOperation(Summary = "Add New Bus", Description = "Add a new bus to the system.", Tags = new[] { "Bus Management" })]
//        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Bus))]
//        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
//        public async Task<ActionResult<Bus>> PostBus([FromBody] Bus bus)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new ErrorResponse { Message = "Invalid bus data." });
//            }

//            try
//            {
//                var createdBus = await _busService.AddBusAsync(bus);
//                return CreatedAtAction(nameof(GetBuses), new { id = createdBus.Id }, createdBus);
//            }
//            catch
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Message = "An error occurred while adding the bus." });
//            }
//        }
//    }
//}
