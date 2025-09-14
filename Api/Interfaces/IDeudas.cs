using Dtos.Deudas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDeudas
    {
        Task<bool> CreateDeudas(DeudasDTO datos);
        Task<List<DeudaUsuarioDTO>> GetDeudas(int tipo);
        Task<bool> PagarDeuda(int idDeuda);
        Task<bool> EliminarDeuda(int idDeuda);
        Task<bool> CreateAbono(int idDeuda, decimal monto);
        Task<List<AbonosDTO>> GetAbonos(int idDeuda);
    }
}
