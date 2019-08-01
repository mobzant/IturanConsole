using ListenerTCP.Enum;
using ListenerTCP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Security.AccessControl;
using System.Text;

namespace ListenerTCP.Helpers
{
    public class ProcesarMensajeHelper
    {
        public void Procesar(string data)
        {
            MemoryCache cache = MemoryCache.Default;            

            var valores = new List<Data>();

            string msgRecuperado = cache["msgRecuperado"] as string;

            var msgs = data.Split('|').ToList();
            msgs.RemoveAll(m => string.IsNullOrEmpty(m));
            var primerMsg = msgs.FirstOrDefault();
            var ultimoMsg = msgs.LastOrDefault();

            foreach (var item in msgs)
            {
                var campos = item.Split("!");
                var patente = campos[(int)CamposEnum.Patente].Trim();
                var equipoID = campos[(int)CamposEnum.EquipoID].Trim();

                if (campos.Length == 33 && (ValidarPatenteHelper.ValidarPatente(patente) || patente == equipoID))
                {
                    try
                    {
                        valores.Add(ObtenerValores(campos));
                        Console.WriteLine(string.Join('!', campos));
                    }
                    catch (Exception ex)
                    {
                        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IturanDB"].ConnectionString);
                        connection.Open();
                        SqlCommand commandRowCount = new SqlCommand("INSERT INTO [dbo].[RawDataIncompleta] ([Valor], [Exception]) VALUES ('" + string.Join('!', campos).Replace("'", "''") + "', '" + ex.Message.Replace("'", "''") + "')", connection);
                        commandRowCount.CommandTimeout = 0;
                        commandRowCount.ExecuteScalar();
                        connection.Close();
                    }
                }

                else if (ultimoMsg == item)
                {
                    CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                    cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);

                    cache.Set("msgRecuperado", ultimoMsg, cacheItemPolicy);
                }

                else if (primerMsg == item && !string.IsNullOrEmpty(msgRecuperado))
                {
                    msgRecuperado = msgRecuperado + primerMsg;
                    var camposRecuperados = msgRecuperado.Split("!");
                    patente = camposRecuperados[(int)CamposEnum.Patente].Trim();
                    equipoID = camposRecuperados[(int)CamposEnum.EquipoID].Trim();

                    if (camposRecuperados.Length == 33 && (ValidarPatenteHelper.ValidarPatente(patente) || patente == equipoID))
                    {
                        try
                        {
                            valores.Add(ObtenerValores(camposRecuperados));
                            Console.WriteLine(string.Join('!', camposRecuperados));
                        }
                        catch (Exception ex)
                        {
                            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IturanDB"].ConnectionString);
                            connection.Open();
                            SqlCommand commandRowCount = new SqlCommand("INSERT INTO [dbo].[RawDataIncompleta] ([Valor], [Exception]) VALUES ('" + string.Join('!', campos).Replace("'", "''") + "', '" + ex.Message.Replace("'", "''") + "')", connection);
                            commandRowCount.CommandTimeout = 0;
                            commandRowCount.ExecuteScalar();
                            connection.Close();
                        }
                    }

                    else
                    {
                        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IturanDB"].ConnectionString);
                        connection.Open();
                        SqlCommand commandRowCount = new SqlCommand("INSERT INTO [dbo].[RawDataIncompleta] ([Valor]) VALUES ('" + string.Join('!', campos).Replace("'", "''") + "')", connection);
                        commandRowCount.CommandTimeout = 0;
                        commandRowCount.ExecuteScalar();
                        connection.Close();
                    }

                    cache.Remove("msgRecuperado");
                }

                else
                {
                    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IturanDB"].ConnectionString);
                    connection.Open();
                    SqlCommand commandRowCount = new SqlCommand("INSERT INTO [dbo].[RawDataIncompleta] ([Valor]) VALUES ('" + string.Join('!', campos).Replace("'", "''") + "')", connection);
                    commandRowCount.CommandTimeout = 0;
                    commandRowCount.ExecuteScalar();
                    connection.Close();
                }

                new BulkCopyHelper().CargaRawDataBulkCopy(valores);
                valores = new List<Data>();
            }
        }

        private Data ObtenerValores(string[] data)
        {
            var valores = new Data();
            valores.Id = Guid.NewGuid();
            valores.SincronizadoEnDB = false;
            valores.Patente = data[(int)CamposEnum.Patente].Trim();
            valores.EquipoID = data[(int)CamposEnum.EquipoID].Trim();
            valores.FechaUltLocalizacion = data[(int)CamposEnum.FechaUltLocalizacion].Trim() != "" && data[(int)CamposEnum.FechaUltLocalizacion].Trim() != "0" ? (DateTime?)Convert.ToDateTime(data[(int)CamposEnum.FechaUltLocalizacion].Trim()) : null;
            valores.Longitud = data[(int)CamposEnum.Longitud].Trim();
            valores.Latitud = data[(int)CamposEnum.Latitud].Trim();
            valores.Velocidad = data[(int)CamposEnum.Velocidad].Trim() != "" ? (int?)Convert.ToInt32(data[(int)CamposEnum.Velocidad].Trim()) : null;
            valores.Calidad = data[(int)CamposEnum.Calidad].Trim() != "" ? (int?)Convert.ToInt32(data[(int)CamposEnum.Calidad].Trim()) : null;
            valores.OdometroTotal = data[(int)CamposEnum.OdometroTotal].Trim() != "" ? (int?)Convert.ToInt32(data[(int)CamposEnum.OdometroTotal].Trim()) : null;
            valores.Rumbo = data[(int)CamposEnum.Rumbo].Trim() != "" ? (int?)Convert.ToInt32(data[(int)CamposEnum.Rumbo].Trim()) : null;
            valores.GeoZona = data[(int)CamposEnum.GeoZona].Trim();
            valores.NombreConductor = string.IsNullOrEmpty(data[(int)CamposEnum.NombreConductor].Trim()) ? "CONDUCTOR NO IDENTIFICADO" : data[(int)CamposEnum.NombreConductor].Trim();
            valores.Direccion = data[(int)CamposEnum.Direccion].Trim();
            valores.TiempoIgnicion = data[(int)CamposEnum.TiempoIgnicion].Trim();
            valores.OdometroParcial = data[(int)CamposEnum.OdometroParcial].Trim();
            valores.NombreEvento1 = data[(int)CamposEnum.NombreEvento1].Trim();
            valores.NombreEvento2 = data[(int)CamposEnum.NombreEvento2].Trim();
            valores.NombreEvento3 = data[(int)CamposEnum.NombreEvento3].Trim();
            valores.CodigoECM = data[(int)CamposEnum.CodigoECM].Trim();
            valores.CodigoLim = data[(int)CamposEnum.CodigoLim].Trim();
            valores.Reglas = data[(int)CamposEnum.Reglas].Trim();
            valores.Temperatura1 = data[(int)CamposEnum.Temperatura1].Trim();
            valores.Temperatura2 = data[(int)CamposEnum.Temperatura2].Trim();
            valores.Temperatura3 = data[(int)CamposEnum.Temperatura3].Trim();
            valores.Temperatura4 = data[(int)CamposEnum.Temperatura4].Trim();
            valores.NombrePlataforma = data[(int)CamposEnum.NombrePlataforma].Trim();
            valores.NombreUsuario = data[(int)CamposEnum.NombreUsuario].Trim();
            valores.UsuarioID = data[(int)CamposEnum.UsuarioID].Trim() != "" ? (int?)Convert.ToInt32(data[(int)CamposEnum.UsuarioID].Trim()) : null;
            valores.PlataformaID = data[(int)CamposEnum.PlataformaID].Trim() != "" ? (int?)Convert.ToInt32(data[(int)CamposEnum.PlataformaID].Trim()) : null;
            valores.ConductorID = Convert.ToInt32(data[(int)CamposEnum.ConductorID].Trim()) != 0 ? (int?)Convert.ToInt32(data[(int)CamposEnum.ConductorID].Trim()) : null;
            valores.ValidacionConductor = valores.ConductorID != null ? true : false;
            valores.ClienteID = data[(int)CamposEnum.ClienteID].Trim() != "" ? (int?)Convert.ToInt32(data[(int)CamposEnum.ClienteID].Trim()) : null;
            valores.HorarioUTCActual = data[(int)CamposEnum.HorarioUTCActual].Trim();
            valores.UAID = data[(int)CamposEnum.UAID].Trim();
            valores.FechaRecepcion = DateTime.Now;

            if (valores.Patente.Contains("E/T"))
            {
                valores.Patente = "E/T " + valores.EquipoID;
            }

            var nombreConductor = valores.NombreConductor.ToUpper().Split(' ').ToList();
            nombreConductor.RemoveAll(c => string.IsNullOrEmpty(c));
            valores.NombreConductor = string.Join(' ', nombreConductor);

            return valores;
        }
    }
}
