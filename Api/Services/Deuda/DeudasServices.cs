using Data;
using Dtos.Deudas;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDeudas.Services.Deuda
{
    public class DeudasServices : IDeudas
    {

        private readonly ContextPostgres _context;
        private readonly IDatabase _redisDb;
        private readonly IConfiguration _configuracion;
        public DeudasServices(ContextPostgres context, IConfiguration configuration, IConnectionMultiplexer redis)
        {
            _configuracion = configuration;
            _context = context;
            _redisDb = redis?.GetDatabase();

        }


        public async Task<bool> CreateDeudas(DeudasDTO datos)
        {
            try
            {
                await _context.Deudas.AddAsync(datos);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Obtener las deudas segun su tip
        /// </summary>
        /// <param name="tipo">parametro  1 para no pagadas, >1 para pagadas</param>
        /// <returns>listado de las deudas</returns>
        public async Task<List<DeudaUsuarioDTO>> GetDeudas(int tipo)
        {
            var query = from d in _context.Deudas
                        join uPresta in _context.Usuarios on d.idUsuarioPresta equals uPresta.id
                        join uDebe in _context.Usuarios on d.idUsuarioDebe equals uDebe.id
                        join dAbono in _context.Abonos on d.id equals dAbono.idDeuda into abonosGroup
                        where d.estado == true
                        select new DeudaUsuarioDTO
                        {
                            idDeuda = d.id,
                            descripcion = d.descripcion,
                            monto = d.monto,
                            pagada = d.pagada,
                            usuarioPresta = uPresta.email,
                            usuarioDebe = uDebe.email,
                            fechaPago = abonosGroup.Max(a => (DateTime?)a.fecha) != null
                                        ? abonosGroup.Max(a => a.fecha).ToString("yyyy/MM/dd")
                                        : null
                        };


            if (tipo == 1)
            {
                return await query.Where(d => d.pagada == false).ToListAsync();
            }
            else
            {
                return await query.Where(d => d.pagada == true).ToListAsync();
            }
        }

        /// <summary>
        /// metodo para generar el pago de una deuda en su totalidad, inserta un abono con el todal de la deuda y descontando los abonos realizados
        /// </summary>
        /// <param name="idDeuda">id de la deuda a pagar</param>
        /// <returns>retorna boleano con el resultado del proceso</returns>

        public async Task<bool> PagarDeuda(int idDeuda)
        {

            try
            {
                var deuda = await _context.Deudas.FindAsync(idDeuda);

                if (deuda == null || deuda.pagada)
                    return false;

                var totalAbonos = await _context.Abonos
                    .Where(a => a.idDeuda == idDeuda)
                    .SumAsync(a => (decimal?)a.monto) ?? 0m;

                var montoRestante = deuda.monto - totalAbonos;

                if (montoRestante <= 0)
                {
                    deuda.pagada = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                var abono = new AbonosDTO
                {
                    idDeuda = idDeuda,
                    monto = montoRestante,
                };

                await _context.Abonos.AddAsync(abono);
                deuda.pagada = true;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// metodo para borrado logico de las deudas
        /// </summary>
        /// <param name="idDeuda">id de la deuda a eliminar</param>
        /// <returns>retorna boleano con el resultado del proceso</returns>
        public async Task<bool> EliminarDeuda(int idDeuda)
        {
            try
            {
                var deuda = await _context.Deudas.FindAsync(idDeuda);

                if (deuda == null)
                    return false;

                deuda.estado = false;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para generar Abonos a la deuda
        /// </summary>
        /// <param name="idDeuda">id de la deuda a abonar</param>
        /// <param name="monto">valore del monto a abonar</param>
        /// <returns>retorna boleano con el resultado del proceso</returns>
        public async Task<bool> CreateAbono(int idDeuda, decimal monto)
        {
            try
            {
                var deuda = await _context.Deudas.FindAsync(idDeuda);
                if (deuda == null)
                    return false;

                var abono = new AbonosDTO
                {
                    idDeuda = idDeuda,
                    monto = monto,
                };

                await _context.Abonos.AddAsync(abono);
                await _context.SaveChangesAsync();

                var sumaAbonos = await _context.Abonos
                    .Where(a => a.idDeuda == idDeuda)
                    .SumAsync(a => a.monto);

                if (sumaAbonos >= deuda.monto)
                {
                    deuda.pagada = true;
                    _context.Deudas.Update(deuda);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para consultar los abonos, se inserta o actualiza en redis si la cantidad de registro no concuerrda
        /// </summary>
        /// <param name="idDeuda">id de la deuda para consultar los abonos</param>
        /// <returns>retorna listado de tipo AbonosDTO</returns>
        public async Task<List<AbonosDTO>> GetAbonos(int idDeuda)
        {

            if (_redisDb != null)
            {
                string cacheKey = $"Abonos:{idDeuda}";
                try
                {
                    var cachedData = await _redisDb.StringGetAsync(cacheKey);
                    if (!cachedData.IsNullOrEmpty)
                    {
                        var abonosRedis = JsonConvert.DeserializeObject<List<AbonosDTO>>(cachedData);

                        // Comparar cantidad con la BD
                        int countDb = await _context.Abonos.CountAsync(w => w.idDeuda == idDeuda);
                        if (abonosRedis.Count == countDb)
                        {
                            return abonosRedis;
                        }
                    }
                }
                catch
                {
                   
                }
            }


            var abonosDb = await _context.Abonos
                .Where(w => w.idDeuda == idDeuda)
                .ToListAsync();

            if (_redisDb != null)
            {
                try
                {
                    string cacheKey = $"Abonos:{idDeuda}";
                    await _redisDb.StringSetAsync(cacheKey, JsonConvert.SerializeObject(abonosDb), TimeSpan.FromMinutes(10));
                }
                catch
                {
                   
                }
            }

            return abonosDb;
        }

        /// <summary>
        /// Metodo para edirar el monto de una deuda
        /// </summary>
        /// <param name="idDeuda">id de la deuda a editar</param>
        /// <param name="monto">valor del moto a actulizar</param>
        /// <returns>retorna boleano con el resultado del proceso</returns>
        public async Task<bool> EditDeuda(int idDeuda, decimal monto)
        {
            try
            {
                var deuda = await _context.Deudas.FindAsync(idDeuda);

                if (deuda == null)
                    return false;

                deuda.monto = monto;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
