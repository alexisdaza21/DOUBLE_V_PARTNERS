using Dtos;
using Dtos.Deudas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUsuarios
    {
        Task<List<UsuarioDTO>> GetUsuarios();
        Task<string> getTokenInvitado(string usuario);
    }
}
