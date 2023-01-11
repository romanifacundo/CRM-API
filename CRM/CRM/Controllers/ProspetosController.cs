using AutoMapper;
using CRM.Context;
using CRM.DTOs;
using CRM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProspetosController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public ProspetosController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<ProspectoDTO>> Get()
        {
            var prospetos = await _context.Prospectos.ToListAsync();

            return _mapper.Map<List<ProspectoDTO>>(prospetos);
        }

        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] ProspectoCreacionDTO prospectoCreacionDTO)
        //{
        //    var existe = await _context.Prospectos.Where(x => x.Name == prospectoCreacionDTO.Name).Select(x => x).FirstOrDefaultAsync();

        //    if (existe != null)
        //    {
        //        return BadRequest($"El {prospectoCreacionDTO.Name} ya existe");
        //    }

        //    var prospecto = _mapper.Map<Prospecto>(prospectoCreacionDTO);
        //    _context.Prospectos.Add(prospecto);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}


        // Asignar Clientes(Prospectos) a Vendedores(Agentes). 
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProspectoCreacionDTO prospectoCreacionDTO)
        {
            if (prospectoCreacionDTO.AgentesIds == null)
                return BadRequest("No se puede insertar un prospecto sin asignarle al menos un agente");

            // obtengo la intersección entre ids recibidos e ids de la base de datos.
            var agentesIds = await _context.Agentes.Where(x => prospectoCreacionDTO.AgentesIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();
            
            // Con esto me aseguro que los ids que nos envíen realmente existan
            if (agentesIds.Count != prospectoCreacionDTO.AgentesIds.Count)
                return BadRequest("Se ingresó al menos un agente que no existe");

            var prospecto = _mapper.Map<Prospecto>(prospectoCreacionDTO);

            AsignarOrdenAgentes(prospecto);

            _context.Prospectos.Add(prospecto);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Método de asignación.
        private void AsignarOrdenAgentes(Prospecto prospecto)
        {
            for (int i = 0; i < prospecto.AgentesProspectos.Count; i++)
                prospecto.AgentesProspectos[i].Orden = i;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProspectoDTO>> GetId(int id)
        {
            var prospecto = await _context.Prospectos.FirstOrDefaultAsync(x => x.Id == id);

            if (prospecto == null)
            {
                return NotFound("Registro no encontrado.");
            }

            return _mapper.Map<ProspectoDTO>(prospecto);
        }
    }
}
