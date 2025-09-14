using Dtos.Deudas;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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
        [Route("GetTokenAnonimo")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        
        public async Task<JsonResultHelper> GetTokenAnonimo()
        {
            try
            {
                var result = await _IUsuarios.GetToken(_configuration.GetValue<string>("UsuarioAnonimo"), "Anonimo", "");
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
        [Authorize(Roles = "Autenticado")]
        public async Task<JsonResultHelper> GetUsuarios()
        {
        
            try
            {
                var usuariNull = new UsuarioDTO();
                resultJson.Data = await _IUsuarios.GetUsuarios(usuariNull);
            }
            catch (Exception e)
            {
                resultJson.Status = System.Net.HttpStatusCode.BadRequest;
                resultJson.Errors.Add(e.Message);
            }
            return resultJson;
        }

        /// <summary>
        /// Crear usuario para logue del aplicativo
        /// </summary>
        /// <param name="datos">clase de usuario</param>
        /// <returns>boleano dependiendo del proceso de la transacción</returns>
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


        /// <summary>
        /// metodo para validar existebcua de usuario
        /// </summary>
        /// <returns>usuario</returns>
        [HttpPost]
        [Route("GetUsuarioLogin")]
        [Authorize(Roles = "Autenticado, Anonimo")]
        public async Task<JsonResultHelper> GetUsuarioLogin(UsuarioDTO datos)
        {

            try
            {
                var result = await _IUsuarios.GetUsuarios(datos);

                if (result.Count > 0)
                {
                    result[0].password = _IUsuarios.GetToken(datos.email, "Autenticado", "").Result;
                    resultJson.Data = result;
                    resultJson.mensaje = "Usuario encontrado";
                }
                else {
                    resultJson.mensaje = "Usuario no encontrado";
                    resultJson.Status = System.Net.HttpStatusCode.NoContent;
                }
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
