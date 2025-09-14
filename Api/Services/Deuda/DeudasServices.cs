using Data;
using Dtos.Deudas;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDeudas.Services.Deuda
{
    public class DeudasServices : IDeudas
    {

        private readonly ContextPostgres _context;
        private readonly IConfiguration _configuracion;
        public DeudasServices(ContextPostgres context, IConfiguration configuration)
        {
            _configuracion = configuration;
            _context = context;
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

        public async Task<List<AbonosDTO>> GetAbonos(int idDeuda)
        {
            return await _context.Abonos.Where(w => w.idDeuda  == idDeuda).ToListAsync();
        }
    }
}
