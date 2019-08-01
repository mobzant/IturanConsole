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
        public List<ReposoNocturno> infracciones = new List<ReposoNocturno>();
        public List<ReposoNocturno> iFuel = new List<ReposoNocturno>();
        public List<ReposoNocturno> kilometraje = new List<ReposoNocturno>();


        public ApiController(IOptions<Connection> Connection, IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        [Route("TraerReposoNocturno")]
        public async Task<ObjectResult> TraerReposoNocturno(string empresaId, string fechaDesde, string fechaHasta)
        {
            ObjectResult result;

            empresaId = "5A4AB8D4-7ECF-45CE-BED3-DA6AAE369A9F";
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

            ReposoNocturno reposo = new ReposoNocturno();
            try
            {
                result = TraerReposoNocturnoPorEmpresa(ConnectionString, empresaId, fechaDesde, fechaHasta);

                
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Database Error");
            }


            return StatusCode((int)HttpStatusCode.OK, result.Value);
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
                        query += " and FechaFin ='" + fechaHasta + "'";
                    }

                    if (!String.IsNullOrWhiteSpace(fechaDesde))
                    {
                        query += " and FechaInicio ='" + fechaDesde + "'";

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

        [HttpPost]
        [Route("TraerIfuel")]
        public async Task<ObjectResult> TraerIfuel(string msg)
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
        [HttpPost]
        [Route("TraerInfracciones")]
        public async Task<ObjectResult> TraerInfracciones(string msg)
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
        [HttpPost]
        [Route("TraerKilometros")]
        public async Task<ObjectResult> TraerKilometros(string msg)
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

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
