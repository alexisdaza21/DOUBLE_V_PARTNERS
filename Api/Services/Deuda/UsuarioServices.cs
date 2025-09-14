using Data;
using Dtos.Deudas;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services.Deuda
{
    public class UsuarioServices : IUsuarios
    {

        private readonly ContextPostgres _context;
        private readonly IConfiguration _configuracion;
        private readonly IDatabase _redisDb;
        public UsuarioServices(ContextPostgres context, IConfiguration configuration, IConnectionMultiplexer redis)
        {
            _configuracion = configuration;
            _context = context;
            _redisDb = redis?.GetDatabase();

        }

        public async Task<bool> CreateUsuarios(UsuarioDTO datos)
        {
            try
            {
                var encryp = new Crypter();
                datos.password = encryp.Encripta(datos.password);
                await _context.Usuarios.AddAsync(datos);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
      
        }

        /// <summary>
        /// Metodo para obtener los usuarios, almacenar en redis y actualizar si no concuerda la cantidad de registros
        /// </summary>
        /// <param name="datos">filtro de usuarios, su no se envia retorna todos los usuarios</param>
        /// <returns>listado de tipo usuariosDto</returns>
        public async Task<List<UsuarioDTO>> GetUsuarios(UsuarioDTO datos)
        {
            List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

            if (_redisDb != null)
            {
                // Generar clave única según los filtros
                string cacheKey = string.IsNullOrEmpty(datos.email) || string.IsNullOrEmpty(datos.password)
                    ? "Usuarios:All"
                    : $"Usuarios:{datos.email}:{datos.password}";

                try
                {
                    var cachedData = await _redisDb.StringGetAsync(cacheKey);
                    if (!cachedData.IsNullOrEmpty)
                    {
                        var usuariosRedis = JsonConvert.DeserializeObject<List<UsuarioDTO>>(cachedData);

                        // Comparar cantidad con la BD
                        int countDb = string.IsNullOrEmpty(datos.email) || string.IsNullOrEmpty(datos.password)
                            ? await _context.Usuarios.CountAsync()
                            : await _context.Usuarios
                                .Where(w => w.email == datos.email && w.password == new Crypter().Encripta(datos.password))
                                .CountAsync();

                        if (usuariosRedis.Count == countDb)
                        {
                            return usuariosRedis;
                        }
                    }
                }
                catch
                {
                    // Ignorar errores de Redis
                }
            }

            // Consulta a BD
            if (!string.IsNullOrEmpty(datos.email) && !string.IsNullOrEmpty(datos.password))
            {
                var encryp = new Crypter();
                datos.password = encryp.Encripta(datos.password);
                usuarios = await _context.Usuarios
                    .Where(w => w.email == datos.email && w.password == datos.password)
                    .Select(s => new UsuarioDTO { id = s.id, email = s.email })
                    .ToListAsync();
            }
            else
            {
                usuarios = await _context.Usuarios
                    .Select(s => new UsuarioDTO { id = s.id, email = s.email })
                    .ToListAsync();
            }

            // Guardar en Redis
            if (_redisDb != null)
            {
                try
                {
                    string cacheKey = string.IsNullOrEmpty(datos.email) || string.IsNullOrEmpty(datos.password)
                        ? "Usuarios:All"
                        : $"Usuarios:{datos.email}:{datos.password}";

                    await _redisDb.StringSetAsync(cacheKey, JsonConvert.SerializeObject(usuarios), TimeSpan.FromMinutes(10));
                }
                catch
                {
                    // Ignorar errores de Redis
                }
            }

            return usuarios;
        }


        public async Task<string> GetToken(string usuario, string rol, string idCliente)
        {
            return await generarToken(usuario, rol, idCliente);
        }

        /// <summary>
        /// Metodo para obtener el token con estructura clains y codificacion sha512
        /// </summary>
        /// <param name="usuario">usuario con el que se firma el token</param>
        /// <param name="rol">tipo de rol para permitir consumo de endpoinst</param>
        /// <param name="idCliente">de ser necesario para fururas validaciones y acciones</param>
        /// <returns>string con el token generado</returns>
        private async Task<string> generarToken(string usuario, string rol, string idCliente)
        {

            //cabecera
            SymmetricSecurityKey symmetricSecurityKeya = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuracion["JWT:ClaveSecreta"])
            );

            SigningCredentials signingCredentials = new SigningCredentials(
                symmetricSecurityKeya, SecurityAlgorithms.HmacSha512
            );

            JwtHeader header = new JwtHeader(signingCredentials);

            //clains
            Claim[] claims = new[] {
                new Claim("idCliente", idCliente),
                new Claim("numeroIdentificacion", usuario),
                new Claim(JwtRegisteredClaimNames.NameId, usuario),
                new Claim(ClaimTypes.Role, rol)
            };

            //payload
            JwtPayload payload = new JwtPayload(
                issuer: _configuracion["JWT:Issuer"],
                audience: _configuracion["JWT:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuracion["JWT:TimeExpire"]))
            );

            var token = new JwtSecurityToken(header, payload);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }

}