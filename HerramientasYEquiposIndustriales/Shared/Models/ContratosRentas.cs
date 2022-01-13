using System;
using System.ComponentModel.DataAnnotations;

namespace HerramientasYEquiposIndustriales.Shared.Models
{
    public class ContratosRentas
    {
        public int ContratoRentaId { get; set; }
        public int RentaId { get; set; }
        public string FolioRenta { get; set; }
        public int ClienteId { get; set; }
        public int AvalId { get; set; }
        public DateTime? FechaContratoInici { get; set; }
        public DateTime? FechaContratoFin { get; set; }

        public virtual Rentas Renta { get; set; }
        public virtual Aval Aval { get; set; }
    }
}
