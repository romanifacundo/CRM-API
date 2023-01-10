using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class Contacto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(25)]
        public string Medio { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }
        public int ProspectoId { get; set; } //sirve de nexo con la entidad Prospecto.

        //Propiedades de navegación
        public Prospecto Prospecto { get; set; } //es para poder navegar hasta los datos del prospecto.
    }

}

