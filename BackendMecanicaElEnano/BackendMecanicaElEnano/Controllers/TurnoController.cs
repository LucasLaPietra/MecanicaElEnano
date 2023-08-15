using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendMecanicaElEnano.Controllers
{
    [ApiController]
    [Route("api/turnos")]
    public class TurnoController : ControllerBase
    {

        private readonly ITurnoRepository repository;
        public TurnoController(ITurnoRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TurnoDto>> Create(CreateTurnoDto vechiculo)
        {
            var TurnoCreated = await this.repository.CreateAsync(vechiculo);
            return TurnoCreated;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TurnoDto>> GetAll()
        {
            var Turnos = this.repository.FindAll().ToList();
            return Turnos;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TurnoDto>> GetById(Guid id)
        {
            var Turno = await this.repository.FindByIdAsync(id);
            return Turno;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TurnoDto>> Update(TurnoDto Turno)
        {
            var TurnoUpdated = await this.repository.UpdateAsync(Turno);
            return TurnoUpdated;
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
