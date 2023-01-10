using AutoMapper;
using CRM.Context;
using CRM.DTOs;
using CRM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AgentesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public AgentesController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<AgenteDTO>> Get()
        {
            var agente = await _context.Agentes.ToListAsync();

            return _mapper.Map<List<AgenteDTO>>(agente);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AgenteCreacionDTO agenteCreacionDTO)
        {
            var existe = await _context.Agentes.Where(x => x.Nombre == agenteCreacionDTO.Nombre).Select(a => a).FirstOrDefaultAsync();

            if (existe != null)
            {
                return BadRequest($"Ya existe un agente con el nombre {agenteCreacionDTO.Nombre}");
            }

            var agente = _mapper.Map<Agente>(agenteCreacionDTO);
            _context.Add(agente);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgenteDTO>> GetId(int id)
        {
            var agente = await _context.Agentes.FirstOrDefaultAsync(x => x.Id == id);

            if (agente == null)
            {
                return NotFound("Agente no encontrado.");
            }

            return _mapper.Map<AgenteDTO>(agente);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AgenteDTO>> Put(AgenteDTO agenteDto)
        {
            var agenteId = await _context.Agentes.Where(x => x.Id == agenteDto.Id).Select(x => x).FirstOrDefaultAsync();

            var agente = _mapper.Map<Agente>(agenteDto);
            _context.Update(agente);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
