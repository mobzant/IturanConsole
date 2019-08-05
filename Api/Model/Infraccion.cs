using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class Infraccion
    {
        public DateTime? Fecha { get; set; }
        public DateTime? FechaInicioInfraccion { get; set; }
        public DateTime? FechaFinInfraccion { get; set; }
        public virtual Guid Gravedad { get; set; }
        public Guid VehiculoId { get; set; }
        public string VehiculoNombre { get; set; }
        public string VehiculoPatente { get; set; }
        public Guid ConductorId { get; set; }
        public string ConductorNombre { get; set; }
        public Guid EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public string Zonas { get; set; }
        public string GruposVehiculos { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string ConjuntoCoordenadas { get; set; }
        public int? VelocidadPromedio { get; set; }
        public int? VelocidadPico { get; set; }
        public int? Exceso { get; set; }
        public int? VelocidadPermitida { get; set; }
        public string Direccion { get; set; }
        public string Detalle { get; set; }
        public TimeSpan Duracion { get; set; }
        public int TipoInfraccion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Guid Id { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public string Nombre { get; set; }

    }
}
