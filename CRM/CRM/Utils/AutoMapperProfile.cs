using AutoMapper;
using CRM.DTOs;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Aquí van las reglas de mapeo <origen,destino>
            CreateMap<AgenteCreacionDTO, Agente>();
            CreateMap<Agente, AgenteDTO>();
          
            CreateMap<ProspectoCreacionDTO, Prospecto>()
            .ForMember(x => x.AgentesProspectos, options => options.MapFrom(MapIntToAgenteProspecto));
            CreateMap<Prospecto, ProspectoDTO>();
            
            CreateMap<ContactoCreacionDTO, Contacto>();
            CreateMap<Contacto, ContactoDTO>();
        }

        private List<AgenteProspecto> MapIntToAgenteProspecto(ProspectoCreacionDTO prospectoCreacionDTO, Prospecto prospecto)
        {
            List<AgenteProspecto> response = new List<AgenteProspecto>();

            if (prospectoCreacionDTO.AgentesIds == null)
                return response;

            foreach (int id in prospectoCreacionDTO.AgentesIds)
            {
                response.Add(new AgenteProspecto { AgenteId = id });
            }
            return response;
        }
    }
}
