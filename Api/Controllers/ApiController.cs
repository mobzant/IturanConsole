using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ListenerTCP.Helpers;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Api.Model;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using Api.Helpers;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        public ProcesarMensajeHelper msgHelper = new ProcesarMensajeHelper();

        public IConfiguration Configuration { get; }
        public string ConnectionString { get; set; }
        public List<ReposoNocturno> reposos = new List<ReposoNocturno>();
        public List<Infraccion> infracciones = new List<Infraccion>();
        public List<EventoIfuelProcesado> iFuels = new List<EventoIfuelProcesado>();
        public List<Kilometraje> kilometrajes = new List<Kilometraje>();
        public HttpClientHelper httpHelper = new HttpClientHelper();

        public ApiController(IOptions<Connection> Connection, IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        #region EndPoints

        [HttpPost]
        [Route("Test")]
        public async Task<ObjectResult> Test(List<ReposoNocturno> dataExample)
        {
            var hola = Request;

            return StatusCode((int)HttpStatusCode.OK, dataExample);
        }


        [HttpPost]
        [Route("TraerReposoNocturno")]
        public async Task<ObjectResult> TraerReposoNocturno(string empresaId, string fechaDesde, string fechaHasta, string uri = null)
        {

            var token = Request.Headers.Where(d => d.Key.Equals("Authorization")).Select(x => x.Value).FirstOrDefault().ToString();
            token = token.Replace("bearer ","");
            if (VerificarTokenUsuario(token))
            {
                ObjectResult result;
                
                if (String.IsNullOrWhiteSpace(empresaId))
                {
                    return StatusCode((int)HttpStatusCode.NoContent, "Parameter is null, empty or whitespace.");
                }
                if (String.IsNullOrWhiteSpace(fechaDesde))
                {
                    fechaDesde = "";
                }
                if (String.IsNullOrWhiteSpace(fechaHasta))
                {
                    fechaHasta = "";
                }
                try
                {
                    result = TraerReposoNocturnoPorEmpresa(ConnectionString, empresaId, fechaDesde, fechaHasta);

                
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message.ToString());
                }

                if (!String.IsNullOrEmpty(uri))
                {
                    try
                    {
                        var postResult = await httpHelper.Post(uri, result.Value.ToString());
                        return StatusCode((int)HttpStatusCode.OK,"Ok");

                    }
                    catch (Exception ex)
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message.ToString());

                    }
                }
                return StatusCode((int)HttpStatusCode.OK, result.Value);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, "Unauthorized");
            }
           
        }

        private bool VerificarTokenUsuario(string token)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "Select * from Usuario where UserToken = '" + token + "'";

                using (var sqlCommand = connection.CreateCommand())
                {

                    sqlCommand.CommandText = query;
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            connection.Close();
                            return true;
                        }
                        else
                        {
                            connection.Close();
                            return false;

                        }
                        
                    }

                }

            }

        }

        [HttpPost]
        [Route("TraerIfuel")]
        public async Task<ObjectResult> TraerIfuel(string empresaId, string fechaDesde, string fechaHasta, string uri = null)
        {
            var token = Request.Headers.Where(d => d.Key.Equals("Authorization")).Select(x => x.Value).FirstOrDefault().ToString();
            token = token.Replace("bearer ", "");
            if (VerificarTokenUsuario(token))
            {
                ObjectResult result;

                if (String.IsNullOrWhiteSpace(empresaId))
                {
                    return StatusCode((int)HttpStatusCode.NoContent, "Parameter is null, empty or whitespace.");
                }
                if (String.IsNullOrWhiteSpace(fechaDesde))
                {
                    fechaDesde = "";
                }
                if (String.IsNullOrWhiteSpace(fechaHasta))
                {
                    fechaHasta = "";
                }

                try
                {
                    result = TraerIfuelPorEmpresa(ConnectionString, empresaId, fechaDesde, fechaHasta);


                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Database Error");
                }

                if (!String.IsNullOrEmpty(uri))
                {
                    try
                    {
                        var postResult = await httpHelper.Post(uri, result.Value.ToString());
                        return StatusCode((int)HttpStatusCode.OK, postResult);

                    }
                    catch (Exception ex)
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message.ToString());

                    }
                }

                return StatusCode((int)HttpStatusCode.OK, result.Value);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, "Unauthorized");
            }
            
        }

        [HttpPost]
        [Route("TraerInfracciones")]
        public async Task<ObjectResult> TraerInfracciones(string empresaId, string fechaDesde, string fechaHasta, string uri = null)
        {
            var token = Request.Headers.Where(d => d.Key.Equals("Authorization")).Select(x => x.Value).FirstOrDefault().ToString();
            token = token.Replace("bearer ", "");
            if (VerificarTokenUsuario(token))
            {
                ObjectResult result;

                if (String.IsNullOrWhiteSpace(empresaId))
                {
                    return StatusCode((int)HttpStatusCode.NoContent, "Parameter is null, empty or whitespace.");
                }
                if (String.IsNullOrWhiteSpace(fechaDesde))
                {
                    fechaDesde = "";
                }
                if (String.IsNullOrWhiteSpace(fechaHasta))
                {
                    fechaHasta = "";
                }
                try
                {
                    result = TraerInfraccionesPorEmpresa(ConnectionString, empresaId, fechaDesde, fechaHasta);


                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Database Error");
                }

                if (!String.IsNullOrEmpty(uri))
                {
                    try
                    {
                        var postResult = await httpHelper.Post(uri, result.Value.ToString());
                        return StatusCode((int)HttpStatusCode.OK, postResult);

                    }
                    catch (Exception ex)
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message.ToString());

                    }
                }

                return StatusCode((int)HttpStatusCode.OK, result.Value);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, "Unauthorized");
            }
            
        }

        [HttpPost]
        [Route("TraerKilometros")]
        public async Task<ObjectResult> TraerKilometros(string empresaId, string fechaDesde, string fechaHasta, string uri = null)
        {
            var token = Request.Headers.Where(d => d.Key.Equals("Authorization")).Select(x => x.Value).FirstOrDefault().ToString();
            token = token.Replace("bearer ", "");

            if (VerificarTokenUsuario(token))
            {
                ObjectResult result;

                if (String.IsNullOrWhiteSpace(empresaId))
                {
                    return StatusCode((int)HttpStatusCode.NoContent, "Parameter is null, empty or whitespace.");
                }
                if (String.IsNullOrWhiteSpace(fechaDesde))
                {
                    fechaDesde = "";
                }
                if (String.IsNullOrWhiteSpace(fechaHasta))
                {
                    fechaHasta = "";
                }

                try
                {
                    result = TraerKilometrosPorEmpresa(ConnectionString, empresaId, fechaDesde, fechaHasta);


                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Database Error");
                }

                if (!String.IsNullOrEmpty(uri))
                {
                    try
                    {
                        var postResult = await httpHelper.Post(uri, result.Value.ToString());
                        return StatusCode((int)HttpStatusCode.OK, postResult);

                    }
                    catch (Exception ex)
                    {
                        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message.ToString());

                    }
                }

                return StatusCode((int)HttpStatusCode.OK, result.Value);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, "Unauthorized");
            }

            
        }

        [HttpPost]
        [Route("Send")]
        public async Task<ObjectResult> Send(string msg)
        {
            //Request.
            if (msg == null || msg == string.Empty)
            {
                return StatusCode((int)HttpStatusCode.NoContent, msg);
            }
            else
            {
                try
                {
                    msgHelper.Procesar(msg);
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message.ToString());
                }

            }

            return StatusCode((int)HttpStatusCode.OK, msg);
        }
        #endregion

        #region Methods        
        private ObjectResult TraerIfuelPorEmpresa(string connectionString, string empresaId, string fechaDesde, string fechaHasta)
        {
            DateTimeFormatInfo arCulture = new CultureInfo("es-ES", false).DateTimeFormat;
            EventoIfuelProcesado iFuel = new EventoIfuelProcesado();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "Select * from EventoIfuelProcesado ";

                using (var sqlCommand = connection.CreateCommand())
                {

                    if (empresaId != string.Empty)
                    {
                        query += " where EmpresaId ='" + empresaId + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaHasta))
                    {
                        query += " and Fecha <='" + fechaHasta + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaDesde))
                    {
                        query += " and Fecha >='" + fechaDesde + "'";

                    }

                    sqlCommand.CommandText = query;
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            try
                            {
                                while (reader.Read())
                                {

                                    iFuel.ConductorId = new Guid(reader[3].ToString());
                                    iFuel.ConductorNombre = reader[4].ToString();
                                    iFuel.VehiculoNombre = reader[6].ToString();
                                    iFuel.Grupos = reader[8].ToString();
                                    iFuel.LitrosAntes = (int)reader[11];
                                    iFuel.LitrosEventos = (int)reader[12];
                                    iFuel.LitrosActuales = (int)reader[13];
                                    iFuel.CapacidadTanque = (int)reader[14];
                                    iFuel.PrecioNafta = (double)reader[18];
                                    iFuel.Gasto = (double)reader[23];
                                    iFuel.TipoEventoIfuel = new Guid(reader[0].ToString()); ;
                                    iFuel.Id = new Guid(reader[0].ToString());
                                    iFuel.VehiculoId = new Guid(reader[5].ToString());
                                    iFuel.VehiculoPatente = reader[7].ToString();
                                    iFuel.EmpresaId = new Guid(reader[1].ToString());
                                    iFuel.EmpresaNombre = reader[2].ToString();
                                    iFuel.Latitud = (double)reader[15];
                                    iFuel.Longitud = (double)reader[16];
                                    iFuel.Direccion = reader[17].ToString();
                                    iFuel.Fecha = reader[10].Equals(DBNull.Value) ? null : (DateTime?)reader[10];
                                    iFuel.Zonas = reader[9].ToString();
                                    iFuel.FechaCreacion = reader[20].Equals(DBNull.Value) ? null : (DateTime?)reader[20]; ;
                                    iFuel.FechaEdicion = reader[21].Equals(DBNull.Value) ? null : (DateTime?)reader[21]; ;
                                    iFuels.Add(iFuel);



                                }
                            }
                            catch (Exception ex)
                            {
                                reader.Close();
                                connection.Close();
                                return StatusCode((int)HttpStatusCode.InternalServerError, "Mapping Error");

                            }
                        }
                        else
                        {
                            reader.Close();
                            connection.Close();
                            return StatusCode((int)HttpStatusCode.NoContent, "No rows found.");

                        }
                        reader.Close();
                    }

                }

                connection.Close();

            }

            var iFuelJson = JsonConvert.SerializeObject(iFuels);

            return StatusCode((int)HttpStatusCode.OK, iFuelJson);


        }

        private ObjectResult TraerInfraccionesPorEmpresa(string connectionString, string empresaId, string fechaDesde, string fechaHasta)
        {
            DateTimeFormatInfo arCulture = new CultureInfo("es-ES", false).DateTimeFormat;
            Infraccion infraccion = new Infraccion();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "Select * from Infraccion ";

                using (var sqlCommand = connection.CreateCommand())
                {

                    if (empresaId != string.Empty)
                    {
                        query += " where EmpresaId ='" + empresaId + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaHasta))
                    {
                        query += " and FechaFin <='" + fechaHasta + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaDesde))
                    {
                        query += " and FechaInicio >='" + fechaDesde + "'";

                    }

                    sqlCommand.CommandText = query;
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            try
                            {
                                while (reader.Read())
                                {
                                    infraccion.Nombre = reader[4].ToString();

                                    infraccion.ConductorId = new Guid(reader[18].ToString());
                                    infraccion.ConductorNombre = reader[19].ToString();
                                    infraccion.VehiculoNombre = reader[17].ToString();
                                    infraccion.Id = new Guid(reader[0].ToString());
                                    infraccion.VehiculoId = new Guid(reader[16].ToString());
                                    infraccion.VehiculoPatente = reader[18].ToString();
                                    infraccion.EmpresaId = new Guid(reader[20].ToString());
                                    infraccion.EmpresaNombre = reader[24].ToString();
                                    infraccion.Latitud = (double)reader[2];
                                    infraccion.Longitud = (double)reader[3];

                                    infraccion.FechaInicioInfraccion = reader[21].Equals(DBNull.Value) ? null : (DateTime?)reader[21];
                                    infraccion.FechaFinInfraccion = reader[22].Equals(DBNull.Value) ? null : (DateTime?)reader[22];
                                    infraccion.ConjuntoCoordenadas = reader[23].ToString();

                                    infraccion.VelocidadPromedio = (int)reader[9];
                                    infraccion.VelocidadPico = (int)reader[10];
                                    infraccion.Exceso = (int)reader[11];
                                    infraccion.TipoInfraccion = (int)reader[25];


                                    infraccion.VelocidadPermitida = (int)reader[12];
                                    infraccion.Detalle = reader[13].ToString();
                                    infraccion.Duracion = (TimeSpan)reader[14];


                                    infraccion.Direccion = reader[15].ToString();
                                    infraccion.Gravedad = new Guid(reader[8].ToString());
                                    infraccion.Fecha = reader[1].Equals(DBNull.Value) ? null : (DateTime?)reader[1];
                                    infraccion.Zonas = reader[26].ToString();
                                    infraccion.FechaCreacion = reader[5].Equals(DBNull.Value) ? null : (DateTime?)reader[5]; ;
                                    infraccion.FechaEdicion = reader[6].Equals(DBNull.Value) ? null : (DateTime?)reader[6]; ;

                                    infracciones.Add(infraccion);
                                }
                            }
                            catch (Exception ex)
                            {
                                reader.Close();
                                connection.Close();
                                return StatusCode((int)HttpStatusCode.InternalServerError, "Mapping Error");

                            }
                        }
                        else
                        {
                            reader.Close();
                            connection.Close();
                            return StatusCode((int)HttpStatusCode.NoContent, "No rows found.");

                        }
                        reader.Close();
                    }

                }

                connection.Close();

            }

            var infraccionJson = JsonConvert.SerializeObject(infracciones);

            return StatusCode((int)HttpStatusCode.OK, infraccionJson);


        }

        private ObjectResult TraerReposoNocturnoPorEmpresa(string connectionString, string empresaId, string fechaDesde, string fechaHasta)
        {
            DateTimeFormatInfo arCulture = new CultureInfo("es-ES", false).DateTimeFormat;
            ReposoNocturno reposo = new ReposoNocturno();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "Select * from ReposoNocturno ";

                using (var sqlCommand = connection.CreateCommand())
                {

                    if (empresaId != string.Empty)
                    {
                        query += " where EmpresaId ='" + empresaId + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaHasta))
                    {
                        query += " and Fecha <='" + fechaHasta + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaDesde))
                    {
                        query += " and Fecha >='" + fechaDesde + "'";

                    }

                    sqlCommand.CommandText = query;
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            try
                            {
                                while (reader.Read())
                                {
                                    reposo.Id = new Guid(reader[0].ToString());
                                    reposo.VehiculoId = new Guid(reader[1].ToString());
                                    reposo.VehiculoPatente = reader[2].ToString();
                                    reposo.EmpresaId = new Guid(reader[3].ToString());
                                    reposo.EmpresaNombre = reader[4].ToString();
                                    reposo.Latitud = (double)reader[5];
                                    reposo.Longitud = (double)reader[6];
                                    reposo.Direccion = reader[7].ToString();
                                    reposo.Fecha = reader[8].Equals(DBNull.Value) ? null : (DateTime?)reader[8];
                                    reposo.FechaInicio = reader[9].Equals(DBNull.Value) ? null : (DateTime?)reader[9];
                                    reposo.FechaFin = reader[10].Equals(DBNull.Value) ? null : (DateTime?)reader[10];
                                    reposo.Duracion = reader[11].Equals(DBNull.Value) ? null : (DateTime?)reader[11];
                                    reposo.ReposoCerrado = (bool)reader[12];
                                    reposo.FechaCreacion = reader[14].Equals(DBNull.Value) ? null : (DateTime?)reader[14]; ;
                                    reposo.FechaEdicion = reader[15].Equals(DBNull.Value) ? null : (DateTime?)reader[15]; ;
                                    reposo.Zonas = reader[17].ToString();
                                    reposo.GruposVehiculos = reader[18].ToString();

                                    reposos.Add(reposo);
                                }
                            }
                            catch (Exception ex)
                            {
                                reader.Close();
                                connection.Close();
                                return StatusCode((int)HttpStatusCode.InternalServerError, "Mapping Error");

                            }
                        }
                        else
                        {
                            reader.Close();
                            connection.Close();
                            return StatusCode((int)HttpStatusCode.NoContent, "No rows found.");

                        }
                        reader.Close();
                    }

                }

                connection.Close();

            }

            var reposoJson = JsonConvert.SerializeObject(reposos);

            return StatusCode((int)HttpStatusCode.OK, reposoJson);


        }

        private ObjectResult TraerKilometrosPorEmpresa(string connectionString, string empresaId, string fechaDesde, string fechaHasta)
        {
            DateTimeFormatInfo arCulture = new CultureInfo("es-ES", false).DateTimeFormat;
            Kilometraje kilometraje = new Kilometraje();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "Select * from Kilometraje ";

                using (var sqlCommand = connection.CreateCommand())
                {

                    if (empresaId != string.Empty)
                    {
                        query += " where EmpresaId ='" + empresaId + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaHasta))
                    {
                        query += " and Fecha <='" + fechaHasta + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaDesde))
                    {
                        query += " and Fechao >='" + fechaDesde + "'";

                    }

                    sqlCommand.CommandText = query;
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            try
                            {
                                while (reader.Read())
                                {

                                    kilometraje.Id = new Guid(reader[0].ToString());
                                    kilometraje.Kilometros = (int)reader[1];

                                    kilometraje.Nombre = reader[3].ToString();

                                    kilometraje.ConductorId = new Guid(reader[7].ToString());
                                    kilometraje.ConductorNombre = reader[8].ToString();
                                    kilometraje.VehiculoId = new Guid(reader[9].ToString());
                                    kilometraje.VehiculoPatente = reader[10].ToString();
                                    kilometraje.VehiculoTipo = reader[11].ToString();

                                    kilometraje.DiaNoLaborableId = new Guid(reader[12].ToString());

                                    kilometraje.DiaNoLaborableNombre = reader[13].ToString();

                                    kilometraje.DiaSemana = (int)reader[14];
                                    kilometraje.GruposVehiculos = reader[17].ToString();
                                    kilometraje.EmpresaId = new Guid(reader[15].ToString());
                                    kilometraje.EmpresaNombre = reader[16].ToString();
                                     kilometraje.Fecha = reader[2].Equals(DBNull.Value) ? null : (DateTime?)reader[2];
                                    kilometraje.FechaCreacion = reader[4].Equals(DBNull.Value) ? null : (DateTime?)reader[4]; ;
                                    kilometraje.FechaEdicion = reader[5].Equals(DBNull.Value) ? null : (DateTime?)reader[5]; ;


                                    kilometrajes.Add(kilometraje);
                                }
                            }
                            catch (Exception ex)
                            {
                                reader.Close();
                                connection.Close();
                                return StatusCode((int)HttpStatusCode.InternalServerError, "Mapping Error");

                            }
                        }
                        else
                        {
                            reader.Close();
                            connection.Close();
                            return StatusCode((int)HttpStatusCode.NoContent, "No rows found.");

                        }
                        reader.Close();
                    }

                }

                connection.Close();

            }

            var kilometrajeJson = JsonConvert.SerializeObject(kilometrajes);

            return StatusCode((int)HttpStatusCode.OK, kilometrajeJson);


        }
        #endregion

    }
}
