using System.ComponentModel.DataAnnotations;

namespace CRM.DTOs
{
    public class ContactoCreacionDTO
    {
        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(25)]
        public string Medio { get; set; }
        
        [StringLength(250)]
        public string Descripcion { get; set; }
    }
}
