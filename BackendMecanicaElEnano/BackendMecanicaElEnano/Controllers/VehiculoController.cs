using BackendMecanicaElEnano.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BackendMecanicaElEnano.Controllers
{
    [ApiController]
    [Route("[vehiculos]")]
    public class VehiculoController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VehiculoDto> Create(CreateVehiculoDto vechiculo)
        {

            return CreatedAtAction("","");
        }
    }
}
