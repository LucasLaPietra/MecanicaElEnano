using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendMecanicaElEnano.Controllers
{
    [ApiController]
    [Route("api/presupuestos")]
    public class PresupuestoController : ControllerBase
    {

        private readonly IPresupuestoRepository repository;
        public PresupuestoController(IPresupuestoRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PresupuestoDto>> Create(Guid vehiculoId)
        {
            var presupuestoCreated = await this.repository.CreateAsync(vehiculoId);
            return presupuestoCreated;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<PresupuestoDto>> GetAll()
        {
            var presupuestos = this.repository.FindAll().ToList();
            return presupuestos;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PresupuestoDto>> GetById(Guid id)
        {
            var presupuesto = await this.repository.FindByIdAsync(id);
            return presupuesto;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PresupuestoDto>> Update(UpdatePresupuestoDto presupuesto)
        {
            var presupuestoUpdated = await this.repository.UpdateAsync(presupuesto);
            return presupuestoUpdated;
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
