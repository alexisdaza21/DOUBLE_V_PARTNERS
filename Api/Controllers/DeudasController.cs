using Dtos.Deudas;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Utils;

namespace ApiDeudas.Controllers
{
    [Route("Deudas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeudasController : ControllerBase
    {
        JsonResultHelper resultJson = new() { Status = System.Net.HttpStatusCode.OK };
        private readonly IDeudas _IDeudas;
        public IConfiguration _configuration { get; }

        public DeudasController(IDeudas IDeudas, IConfiguration configuration)
        {
            _IDeudas = IDeudas;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CreateDeudas")]
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> CreateDeudas(DeudasDTO datos)
        {

            try
            {
                var result = await _IDeudas.CreateDeudas(datos);
                resultJson.mensaje = result ? "La deuda se creo correctamente" : "La deuda no se pudo crear verifique los datos";
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }


        [HttpGet]
        [Route("GetDeudas")]
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> GetDeudas(int tipo)
        {

            try
            {
                resultJson.Data = await _IDeudas.GetDeudas(tipo);
               
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }

        [HttpPost]
        [Route("PagarDeuda")]
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> PagarDeuda(int idDeuda)
        {

            try
            {
                var result = await _IDeudas.PagarDeuda(idDeuda);
                resultJson.mensaje = result ? "La deuda se pago correctamente" : "La deuda no se pudo pagar";
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }

        [HttpPost]
        [Route("EliminarDeuda")]
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> EliminarDeuda(int idDeuda)
        {

            try
            {
                var result = await _IDeudas.EliminarDeuda(idDeuda);
                resultJson.mensaje = result ? "La deuda se elimino correctamente" : "La deuda no se pudo eliminar";
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }

        [HttpPost]
        [Route("CreateAbono")]
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> CreateAbono(int idDeuda, decimal monto)
        {

            try
            {
                var result = await _IDeudas.CreateAbono(idDeuda, monto);
                resultJson.mensaje = result ? "El abono se creo correctamente" : "El abono no se pudo crear";
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }

        [HttpGet]
        [Route("GetAbonos")]
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> GetAbonos(int idDeuda)
        {

            try
            {
                resultJson.Data = await _IDeudas.GetAbonos(idDeuda);
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }

        [HttpPost]
        [Route("EditDeuda")]
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> EditDeuda(int idDeuda, decimal monto)
        {

            try
            {
                var result = await _IDeudas.EditDeuda(idDeuda, monto);
                resultJson.mensaje = result ? "La deuda se edito correctamente" : "La deuda no se pudo editar";
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }
    }
}
