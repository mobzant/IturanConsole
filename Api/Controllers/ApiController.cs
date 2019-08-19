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
using System.IO;

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
        [Route("MsgToConsole")]
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
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message.ToString() + "/ Error in the message");
                }

            }

            return StatusCode((int)HttpStatusCode.OK, msg);
        }
        #endregion

    }
}
