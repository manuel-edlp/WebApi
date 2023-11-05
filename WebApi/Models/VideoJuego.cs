using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
