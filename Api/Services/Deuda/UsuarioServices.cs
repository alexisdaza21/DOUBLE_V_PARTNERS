using ApiDeudas;
using Data;
using Dtos.Deudas;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        public UsuarioServices(ContextPostgres context, IConfiguration configuration)
        {
            _configuracion = configuration;
            _context = context;
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

        public async Task<List<UsuarioDTO>> GetUsuarios(UsuarioDTO datos)
        {
            var usuarios = new List<UsuarioDTO>();
            if (!string.IsNullOrEmpty(datos.email) && !string.IsNullOrEmpty(datos.password))
            {
                var encryp = new Crypter();
                datos.password = encryp.Encripta(datos.password);
                usuarios = await _context.Usuarios.Where(w => w.email == datos.email && w.password == datos.password)
                    .Select(s => new UsuarioDTO { id = s.id, email = s.email }).ToListAsync();

            }
            else {
                usuarios = await _context.Usuarios.Select(s => new UsuarioDTO { id = s.id, email = s.email }).ToListAsync();
            }
            return usuarios;
        }

        public async Task<string> GetToken(string usuario, string rol, string idCliente)
        {
            return await generarToken(usuario, rol, idCliente);
        }

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