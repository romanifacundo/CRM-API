using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class Agente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "El campo {0} no debe exceder de {1} caracteres.")]
        public string Nombre { get; set; }
    }
}
