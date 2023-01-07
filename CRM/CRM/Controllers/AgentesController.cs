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
        public async Task<ICollection<AgenteDTO>> GetAllAsync()
        {
            var agente = await _context.Agentes.ToListAsync();

            return _mapper.Map<ICollection<AgenteDTO>>(agente);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] AgenteCreacionDTO agenteCreacionDTO)
        {
            var existe = await _context.Agentes.Where(x => x.Nombre == agenteCreacionDTO.Nombre).Select(a => a).FirstOrDefaultAsync();

            if (existe != null)
            {
                return BadRequest($"Ya existe un agente con el nombre {agenteCreacionDTO.Nombre}");
            }

            _context.Add(agenteCreacionDTO);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgenteDTO>> GetIdAsync(int id)
        {
            var agente = await _context.Agentes.FirstOrDefaultAsync(x => x.Id == id);

            if (agente == null)
            {
                return NotFound("Agente no encontrado.");
            }

            return _mapper.Map<AgenteDTO>(agente);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<AgenteDTO>> UpdateAsync(AgenteDTO agenteDto)
        //{
        //    var agente = _context.Agentes.Where(x => x.Id == agenteDto.Id).Select(x => x.Nombre).FirstOrDefaultAsync();


        //    _context.Update(agente);
        //    _mapper.Map<AgenteDTO>(agente);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}
    }
}
