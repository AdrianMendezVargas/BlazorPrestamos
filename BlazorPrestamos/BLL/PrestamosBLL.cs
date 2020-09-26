using BlazorPrestamos.DAL;
using BlazorPrestamos.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPrestamos.BLL {
    public class PrestamosBLL {

        public async static Task<bool> Guardar(Prestamo prestamo) {
            if (!await Existe(prestamo.Id))
                return await Insertar(prestamo);
            else
                return await Modificar(prestamo);
        }

        public async static Task<bool> Insertar(Prestamo prestamo) {
            bool paso = false;
            Contexto contexto = new Contexto();

            try {
                contexto.Prestamos.Add(prestamo);
                paso = (await contexto.SaveChangesAsync() > 0);

                if (paso) {
                    await ActualizarBalancePersona(prestamo);
                }
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return paso;
        }

        public async static Task<bool> Modificar(Prestamo prestamo) {
            bool paso = false;
            Contexto contexto = new Contexto();

            try {
                contexto.Entry(prestamo).State = EntityState.Modified;

                paso = await contexto.SaveChangesAsync() > 0;

                if (paso) {
                    await ActualizarBalancePersona(prestamo);
                }

            } catch (Exception) {

                throw;
            } finally {
                await contexto.DisposeAsync();
            }
            return paso;
        }

        private static async Task ActualizarBalancePersona(Prestamo _prestamo) {

            var persona = await PersonasBLL.Buscar(_prestamo.PersonaId);

            if (persona != null) {

                persona.Balance = 0;
                foreach (var prestamo in await GetPrestamos()) {
                    persona.Balance += prestamo.Balance;
                }

                await PersonasBLL.Modificar(persona);
            }
        }

        public async static Task<bool> Eliminar(int id) {
            bool paso = false;
            Contexto contexto = new Contexto();
            try {
                var prestamo = await Buscar(id);

                if (prestamo != null) {
                    contexto.Prestamos.Remove(prestamo);
                    paso = await contexto.SaveChangesAsync() > 0;

                    if (paso) {
                        await ActualizarBalancePersona(prestamo);
                    }
                }
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return paso;
        }

        public async static Task<Prestamo> Buscar(int id) {
            Contexto contexto = new Contexto();
            Prestamo prestamo;

            try {
                prestamo = await contexto.Prestamos
                    .Where(p => p.Id == id)
                    .FirstOrDefaultAsync();
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return prestamo;
        }


        public async static Task<bool> Existe(int id) {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try {
                encontrado = await contexto.Prestamos.AnyAsync(p => p.Id == id);
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return encontrado;
        }

        public async static Task<List<Prestamo>> GetPrestamos() {
            Contexto contexto = new Contexto();

            List<Prestamo> prestamos = new List<Prestamo>();
            await Task.Delay(01); //Para dar tiempo a renderizar el mensaje de carga

            try {
                prestamos = await contexto.Prestamos.ToListAsync();

            } catch (Exception) {

                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return prestamos;

        }

    }
}
