using EPAM1.Cargos.Interfaces;

namespace EPAM1.Cargos
{
    public class Baggage : IBasicCargo
    {
        public int Weight { get; set; }
    }
}