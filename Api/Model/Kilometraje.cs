using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class Kilometraje
    {
        public int Kilometros { get; set; }
        public Guid EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public Guid ConductorId { get; set; }
        public string ConductorNombre { get; set; }
        public Guid VehiculoId { get; set; }
        public string VehiculoPatente { get; set; }
        public string VehiculoTipo { get; set; }
        public string GruposVehiculos { get; set; }
        public DateTime? Fecha { get; set; }
        public Guid? DiaNoLaborableId { get; set; }
        public string DiaNoLaborableNombre { get; set; }
        public int DiaSemana { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Guid Id { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public string Nombre { get; set; }
    }
}
