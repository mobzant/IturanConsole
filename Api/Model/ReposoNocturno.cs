using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class ReposoNocturno
    {

        public Guid VehiculoId { get; set; }
        public string VehiculoPatente { get; set; }
        public Guid EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public string Zonas { get; set; }
        public string GruposVehiculos { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Direccion { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? Duracion { get; set; }
        public bool ReposoCerrado { get; set; }

    }
}
