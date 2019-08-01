using System;
using System.Collections.Generic;
using System.Text;

namespace ListenerTCP.Model
{
    public class Data
    {
        public Guid Id { get; set; }
        public bool SincronizadoEnDB { get;set; }
        public string Patente { get; set; }
        public string EquipoID { get; set; }
        public DateTime? FechaUltLocalizacion { get; set; }
        public string Longitud { get; set; }
        public string Latitud { get; set; }
        public int? Velocidad { get; set; }
        public int? Calidad { get; set; }
        public int? OdometroTotal { get; set; }
        public int? Rumbo { get; set; }
        public string GeoZona { get; set; }
        public string NombreConductor { get; set; }
        public string Direccion { get; set; }
        public string TiempoIgnicion { get; set; }
        public string OdometroParcial { get; set; }
        public string NombreEvento1 { get; set; }
        public string NombreEvento2 { get; set; }
        public string NombreEvento3 { get; set; }
        public string CodigoECM { get; set; }
        public string CodigoLim { get; set; }
        public string Reglas { get; set; }
        public string Temperatura1 { get; set; }
        public string Temperatura2 { get; set; }
        public string Temperatura3 { get; set; }
        public string Temperatura4 { get; set; }
        public string NombrePlataforma { get; set; }
        public bool ValidacionConductor { get; set; }
        public string NombreUsuario { get; set; }
        public int? UsuarioID { get; set; }
        public int? PlataformaID { get; set; }
        public int? ConductorID { get; set; }
        public int? ClienteID { get; set; }
        public string HorarioUTCActual { get; set; }
        public string UAID { get; set; }
        public DateTime FechaRecepcion { get; set; }
    }
}
