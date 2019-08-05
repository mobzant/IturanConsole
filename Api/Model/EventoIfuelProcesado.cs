using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class EventoIfuelProcesado
    {

        public Guid EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public Guid ConductorId { get; set; }
        public string ConductorNombre { get; set; }
        public Guid VehiculoId { get; set; }
        public string VehiculoNombre { get; set; }
        public string VehiculoPatente { get; set; }
        public string Grupos { get; set; }
        public string Zonas { get; set; }
        public DateTime? Fecha { get; set; }
        public int LitrosAntes { get; set; }
        public int LitrosEventos { get; set; }
        public int LitrosActuales { get; set; }
        public int CapacidadTanque { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Direccion { get; set; }
        public double PrecioNafta { get; set; }
        public double? Gasto { get; set; }
        public Guid TipoEventoIfuel { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Guid Id { get; set; }
        public DateTime? FechaEdicion { get; set; }
    }
}
