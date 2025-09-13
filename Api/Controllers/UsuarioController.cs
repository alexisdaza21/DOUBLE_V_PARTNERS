using Dtos;
using Dtos.Deudas;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Utils;

namespace Controllers
{
    [Route("Usuario")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class UsuarioController : ControllerBase
    {
        JsonResultHelper resultJson = new() { Status = System.Net.HttpStatusCode.OK };
        private readonly IUsuarios _IUsuarios;
        public IConfiguration _configuration { get; }

        public UsuarioController(IUsuarios IUsuarios, IConfiguration configuration)
        {
            _IUsuarios = IUsuarios;
            _configuration = configuration;
        }


    
        /// <summary>
        /// Metodo para obtener token con rol de anonimo para consumir metodos sin autenticarse con credenciales
        /// </summary>
        /// <returns>token sha 512</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("getTokenAnonimo")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        
        public async Task<JsonResultHelper> getTokenInvitado()
        {
            try
            {

                var result = await _IUsuarios.getTokenInvitado(_configuration.GetValue<string>("UsuarioAnonimo"));
                if (!string.IsNullOrEmpty(result))
                {
                    resultJson.Data = result;

                }
                else
                {
                    resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                }


            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }

            return resultJson;
        }

        /// <summary>
        /// metodo para consultar el listado de mis usuario
        /// </summary>
        /// <returns>listado de usuarios</returns>
        [HttpGet]
        [Route("GetUsuarios")]
        [Authorize(Roles = "Autenticado, Anonimo")]
        public async Task<JsonResultHelper> GetUsuarios()
        {
        
            try
            {
                resultJson.Data = await _IUsuarios.GetUsuarios();
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }

        [HttpPost]
        [Route("CreateUsuarios")]
        [Authorize(Roles = "Autenticado, Anonimo")]
        public async Task<JsonResultHelper> CreateUsuarios(UsuarioDTO datos)
        {

            try
            {
                var result = await _IUsuarios.CreateUsuarios(datos);
                resultJson.mensaje = result ? "Usuario creado correctamente" : "El usuario no se pudo crear verifique los datos";
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
