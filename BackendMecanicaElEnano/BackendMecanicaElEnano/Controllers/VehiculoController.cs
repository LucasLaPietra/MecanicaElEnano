using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendMecanicaElEnano.Controllers
{
    [ApiController]
    [Route("api/vehiculos")]
    public class VehiculoController : ControllerBase
    {

        private readonly IVehiculoRepository repository;
        public VehiculoController(IVehiculoRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VehiculoDto>> Create(CreateVehiculoDto vechiculo)
        {
            var vehiculoCreated = await this.repository.CreateAsync(vechiculo);
            return vehiculoCreated;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<VehiculoDto>> GetAll()
        {
            var vehiculos = this.repository.FindAll().ToList();
            return vehiculos;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VehiculoDto>> Update(VehiculoDto vehiculo)
        {
            var vehiculoUpdated = await this.repository.UpdateAsync(vehiculo);
            return vehiculoUpdated;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task Delete(Guid id)
        {
            await this.repository.DeleteAsync(id);
        }
    }
}
