namespace RentCar.Data.Dtos
{
    public class MasUsadosDto
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Año { get; set; }
        public int VecesUsado { get; set; }
    }
}
