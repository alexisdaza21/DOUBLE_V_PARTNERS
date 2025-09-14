using Dtos.Deudas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUsuarios
    {
        Task<List<UsuarioDTO>> GetUsuarios(UsuarioDTO datos);
        Task<bool> CreateUsuarios(UsuarioDTO datos);
        Task<string> GetToken(string usuario, string rol, string idCliente);
    }
}
