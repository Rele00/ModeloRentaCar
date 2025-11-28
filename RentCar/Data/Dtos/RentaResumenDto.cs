using System;

namespace RentCar.Data.Dtos
{
    public class RentaResumenDto
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string VehiculoDescripcion { get; set; } = string.Empty;

        public DateTime FechaSalida { get; set; }
        public DateTime FechaEstimadaDevolucion { get; set; }

        public decimal MontoEstimado { get; set; }
        public string EstadoRenta { get; set; } = string.Empty;
    }
}
