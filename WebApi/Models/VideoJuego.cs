using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class VideoJuego
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string nombre { get; set; }

        public int año { get; set; }

        [Required]
        [ForeignKey("desarrolladorId")]
        public int desarrolladorId { get; set; }

        public virtual Desarrollador desarrollador { get; set; }

        [Required]
        public float peso { get; set; }


    }
}
