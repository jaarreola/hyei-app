using HerramientasYEquiposIndustriales.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HerramientasYEquiposIndustriales.Shared.DTOs
{
    public class ContadorDTO
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
    }
}
