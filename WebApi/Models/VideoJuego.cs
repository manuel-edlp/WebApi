using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class VideoJuego
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string nombre { get; set; }

        public int año { get; set; }

        public string desarrollador { get; set; }

        [Required]
        public float peso { get; set; }


    }
}
