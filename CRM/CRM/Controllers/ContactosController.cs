using AutoMapper;
using CRM.Context;
using CRM.DTOs;
using CRM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/prospectos/{prospectoId:int}/comentarios")]

    public class ContactosController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public ContactosController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactoDTO>>> Get(int prospectoId)
        {
            var existeProspecto = await _context.Prospectos.Where(x => x.Id == prospectoId).Select(x => x).FirstOrDefaultAsync();

            if (existeProspecto == null) 
            {
                return NotFound("Prospecto no existe");
            }

            var contactos = await _context.Contactos.Where(x => x.ProspectoId == prospectoId).ToListAsync();

            return _mapper.Map<List<ContactoDTO>>(contactos); 
        }

        [HttpPost] 
        public async Task<ActionResult> Post(int prospectoId , ContactoCreacionDTO contactoCreacionDTO)
        {
            var existe = await _context.Prospectos.AnyAsync(x => x.Id == prospectoId);

            if (!existe) 
            {
                return NotFound("El prospecto no existe");
            }

            var contacto = _mapper.Map<Contacto>(contactoCreacionDTO);
            contacto.ProspectoId = prospectoId;

            _context.Contactos.Add(contacto); 
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
