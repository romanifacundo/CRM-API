using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs
{
    public class ProspectoCreacionDTO
    {
        [Required]
        [StringLength(70, ErrorMessage = "El campo {0} no debe exceder de {1} caracteres.")]
        public string Name { get; set; }
        public string UrlPerfil { get; set; }
        public List<int> AgentesIds { get; set; }
    }
}
