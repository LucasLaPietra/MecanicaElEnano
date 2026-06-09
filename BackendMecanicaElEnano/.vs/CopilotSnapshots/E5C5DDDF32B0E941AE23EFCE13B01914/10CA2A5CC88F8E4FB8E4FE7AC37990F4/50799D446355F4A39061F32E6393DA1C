using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendMecanicaElEnano.Controllers
{
    [ApiController]
    [Route("api/ordenTrabajos")]
    public class OrdenTrabajoController : ControllerBase
    {

        private readonly IOrdenTrabajoRepository repository;
        public OrdenTrabajoController(IOrdenTrabajoRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrdenTrabajoDto>> Create(Guid vehiculoId)
        {
            var ordenTrabajoCreated = await this.repository.CreateAsync(vehiculoId);
            return ordenTrabajoCreated;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<OrdenTrabajoDto>> GetAll()
        {
            var ordenTrabajos = this.repository.FindAll().ToList();
            return ordenTrabajos;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrdenTrabajoDto>> GetById(Guid id)
        {
            var ordenTrabajo = await this.repository.FindByIdAsync(id);
            return ordenTrabajo;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrdenTrabajoDto>> Update(UpdateOrdenTrabajoDto ordenTrabajo)
        {
            var ordenTrabajoUpdated = await this.repository.UpdateAsync(ordenTrabajo);
            return ordenTrabajoUpdated;
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
