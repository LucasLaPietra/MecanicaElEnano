using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendMecanicaElEnano.Controllers
{
    [ApiController]
    [Route("api/trabajos")]
    public class TrabajoController : ControllerBase
    {

        private readonly ITrabajoRepository repository;
        public TrabajoController(ITrabajoRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TrabajoDto>> Create(Guid vehiculoId)
        {
            var trabajoCreated = await this.repository.CreateAsync(vehiculoId);
            return trabajoCreated;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TrabajoDto>> GetAll()
        {
            var trabajos = this.repository.FindAll().ToList();
            return trabajos;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrabajoDto>> GetById(Guid id)
        {
            var trabajo = await this.repository.FindByIdAsync(id);
            return trabajo;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TrabajoDto>> Update(UpdateTrabajoDto trabajo)
        {
            var trabajoUpdated = await this.repository.UpdateAsync(trabajo);
            return trabajoUpdated;
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
