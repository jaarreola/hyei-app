using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class ClienteCreacionDTO
    {
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
        [StringLength(40)]
        public string Apellido { get; set; }
        [StringLength(10)]
        public string Telefono { get; set; }
        [StringLength(40)]
        public string Correo { get; set; }
        [StringLength(150)]
        public string Direccion { get; set; }
        [StringLength(13)]
        public string RFC { get; set; }
        public bool EsFrecuente { get; set; }
        public bool? EsProblema { get; set; }
        public bool? PuedeRentar { get; set; }
        public bool? PersonaFisica { get; set; }
        public bool? PersonaMoral { get; set; }
    }
}
