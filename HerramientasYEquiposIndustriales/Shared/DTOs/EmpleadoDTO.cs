﻿using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class EmpleadoDTO
    {
        public int EmpleadoId { get; set; }
        [Required]
        [StringLength(10)]
        public string NumeroEmpleado { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(200)]
        public string Direccion { get; set; }
        [StringLength(10)]
        public string Telefono { get; set; }
        public int PuestoId { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaBaja { get; set; }

        public virtual Puesto Puesto { get; set; }
    }
}
