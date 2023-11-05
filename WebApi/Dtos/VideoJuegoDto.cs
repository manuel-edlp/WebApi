using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class VideoJuegoDto
    {
        public string nombre { get; set; }
        public int año { get; set; }
        public string desarrollador { get; set; }
        public float peso { get; set; }
    }
}
