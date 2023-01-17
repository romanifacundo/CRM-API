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
            CreateMap<Agente, AgenteConProspectosDTO>()
                .ForMember(x => x.Prospectos, options => options.MapFrom(MapFromAgentesProspectosToProspectoDTO));

            //CreateMap<ProspectoCreacionDTO, Prospecto>();
            CreateMap<ProspectoCreacionDTO, Prospecto>()
                .ForMember(x => x.AgentesProspectos, options => options.MapFrom(MapIntToAgenteProspecto));

            CreateMap<Prospecto, ProspectoDTO>();
            CreateMap<Prospecto, ProspectoConAgentesDTO>()
                .ForMember(x => x.Agentes, options => options.MapFrom(MapFromAgentesProspectosToAgenteDTO));

            //CreateMap<ContactoCreacionDTO, Contacto>(); 
            CreateMap<ContactoCreacionDTO, Contacto>(); 
            CreateMap<Contacto, ContactoDTO>(); 
        }

        private List<ProspectoDTO> MapFromAgentesProspectosToProspectoDTO(Agente agente, AgenteDTO agenteDTO)
        {
            List<ProspectoDTO> response = new List<ProspectoDTO>();
            if (agente.AgentesProspectos == null)
                return response; //no hacemos validaciones adicionales para respetar el principio SRP de responsabilidad única

            foreach (var item in agente.AgentesProspectos)
                response.Add(new ProspectoDTO { Id = item.ProspectoId, Name = item.Prospecto.Name, UrlPerfil = item.Prospecto.UrlPerfil });

            return response;
        }

        private List<AgenteDTO> MapFromAgentesProspectosToAgenteDTO(Prospecto prospecto, ProspectoDTO prospectoDTO)
        {
            List<AgenteDTO> response = new List<AgenteDTO>();
            if (prospecto.AgentesProspectos == null)
                return response; //no hacemos validaciones adicionales para respetar el principio SRP de responsabilidad única

            foreach (var item in prospecto.AgentesProspectos)
                response.Add(new AgenteDTO { Id = item.AgenteId, Nombre = item.Agente.Nombre });

            return response;
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
