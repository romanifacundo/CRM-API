using AutoMapper;
using CRM.Context;
using CRM.DTOs;
using CRM.Migrations;
using CRM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] ProspectoCreacionDTO prospectoCreacionDTO)
        //{
        //    if (prospectoCreacionDTO.AgentesIds == null)
        //        return BadRequest("No se puede insertar un prospecto sin asignarle al menos un agente");

        //    // obtengo la intersección entre ids recibidos e ids de la base de datos.
        //    var agentesIds = await _context.Agentes.Where(x => prospectoCreacionDTO.AgentesIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();

        //    // Con esto me aseguro que los ids que nos envíen realmente existan
        //    if (agentesIds.Count != prospectoCreacionDTO.AgentesIds.Count)
        //        return BadRequest("Se ingresó al menos un agente que no existe");

        //    var prospecto = _mapper.Map<Prospecto>(prospectoCreacionDTO);

        //    AsignarOrdenAgentes(prospecto);

        //    _context.Prospectos.Add(prospecto);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //// Método de asignación.
        //private void AsignarOrdenAgentes(Prospecto prospecto)
        //{
        //    for (int i = 0; i < prospecto.AgentesProspectos.Count; i++)
        //        prospecto.AgentesProspectos[i].Orden = i;
        //}

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

        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] ProspectoCreacionDTO prospectoCreacionDTO)
        //{
        //    // 1 Condicionar.
        //    if (prospectoCreacionDTO.AgentesIds == null)
        //    {
        //        return BadRequest("No se puede insertar un prospecto sin asignarle un agente");
        //    }

        //    // 2 consulta de LINQ para traerme la coleccción de los ids que les asigne con los ids de la DB.
        //    var agenteId = await _context.Agentes.Where(x => prospectoCreacionDTO.AgentesIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();

        //    // 3 Condicionar para comparar tamaño de colecciones con respecto a la lista que guardo en la consulta de LINQ.
        //    if (agenteId.Count != prospectoCreacionDTO.AgentesIds.Count)
        //    {
        //        return BadRequest("Se ingreso un agente con ID inexistente");
        //    }

        //    // 4 Mapeo desde la entidad Prospecto a parámetro del método.
        //    var prospectos = _mapper.Map<Prospecto>(prospectoCreacionDTO);

        //    // 5 Llamada el método para asignar prospectos a los agentes.
        //    AsignarProspectosAlosAgentes(prospectos);

        //    _context.Prospectos.Add(prospectos);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //private void AsignarProspectosAlosAgentes(Prospecto prospecto)
        //{
        //    for (int i = 0; i < prospecto.AgentesProspectos.Count; i++)
        //    {
        //        prospecto.AgentesProspectos[i].Orden = i; // Oreden de posición 
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> Post(ProspectoCreacionDTO prospectoCreacionDTO)
        //{
        //    // 1 Condicionar insertar pasando un id Obligatorio.
        //    if (prospectoCreacionDTO.AgentesIds == null)
        //    {
        //        return BadRequest("Debo asignarle un id obligatoriamente para poder insertar");
        //    }

        //    // 2 Consulta de LINQ.
        //    var agentesId = await _context.Agentes.Where(x => prospectoCreacionDTO.AgentesIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();

        //    // 3 Condición.
        //    if (agentesId.Count != prospectoCreacionDTO.AgentesIds.Count)
        //    {
        //        return BadRequest("Se ingreso un agente con ID inexistente ");
        //    }

        //    // 4 Mapeo.
        //    var prospecto = _mapper.Map<Prospecto>(prospectoCreacionDTO);

        //    // 5 llamado de método.
        //    AsignarProspectosAgentes(prospecto);

        //    // 6 Agegar al contexto.
        //    _context.Prospectos.Add(prospecto);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //private void AsignarProspectosAgentes(Prospecto prospecto)
        //{
        //    for (int i = 0; i < prospecto.AgentesProspectos.Count; i++)
        //    {
        //        prospecto.AgentesProspectos[i].Orden = i;
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProspectoCreacionDTO prospectoCreacionDTO)
        {
            // Paso 1 Condicionar.

            if (prospectoCreacionDTO.AgentesIds == null)
            {
                return BadRequest("No se puede insertar un prospecto sin asignarle al menos un agente ");
            }

            // Paso 2Consulta LINQ.
            var agenteIds = await _context.Agentes.Where(x => prospectoCreacionDTO.AgentesIds.Contains(x.Id)).Select(x => x.Id).ToListAsync(); 
            
            // Paso 3.
            if(agenteIds.Count != prospectoCreacionDTO.AgentesIds.Count) 
            {
                return BadRequest("Se ingreso un agente con ID inexistente");
            }

            // Paso 4.
            var prospectos = _mapper.Map<Prospecto>(prospectoCreacionDTO);
            
            // Paso 5.
            asignarProspectosALosAgentes(prospectos);

            // Paso 6.
            _context.Prospectos.Add(prospectos);
            await _context.SaveChangesAsync();
            return Ok();

        }

        private void asignarProspectosALosAgentes(Prospecto prospecto) 
        {
            for (int i = 0; i < prospecto.AgentesProspectos.Count; i++)
            {
                prospecto.AgentesProspectos[i].Orden = i;
            }
        }
    }
}
