using BlazorPrestamos.DAL;
using BlazorPrestamos.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPrestamos.BLL {
    public class PersonasBLL {

        public async static Task<bool> Guardar(Persona persona) {
            if (!await Existe(persona.Id))
                return await Insertar(persona);
            else
                return await Modificar(persona);
        }

        public async static Task<bool> Insertar(Persona persona) {
            bool paso = false;
            Contexto contexto = new Contexto();

            try {
                contexto.Personas.Add(persona);
                paso = await contexto.SaveChangesAsync() > 0;
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return paso;
        }

        public async static Task<bool> Modificar(Persona persona) {
            bool paso = false;
            Contexto contexto = new Contexto();

            try {
                contexto.Entry(persona).State = EntityState.Modified;

                paso = await contexto.SaveChangesAsync() > 0;

            } catch (Exception) {

                throw;
            } finally {
                await contexto.DisposeAsync();
            }
            return paso;
        }

        public async static Task<bool> Eliminar(int id) {
            bool paso = false;
            Contexto contexto = new Contexto();
            try {
                var persona = contexto.Personas.Find(id);

                if (persona != null) {
                    contexto.Personas.Remove(persona);
                    paso = await contexto.SaveChangesAsync() > 0;
                }
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return paso;
        }

        public async static Task<Persona> Buscar(int id) {
            Contexto contexto = new Contexto();
            Persona persona;

            try {
                persona = await contexto.Personas
                    .Where(e => e.Id == id)
                    .FirstOrDefaultAsync();
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return persona;
        }


        public async static Task<bool> Existe(int id) {
            Contexto contexto = new Contexto();
            bool encontrado = false;

            try {
                encontrado = await contexto.Personas.AnyAsync(e => e.Id == id);
            } catch (Exception) {
                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return encontrado;
        }

        public async static Task<List<Persona>> GetPersonas() {
            Contexto contexto = new Contexto();

            List<Persona> personas = new List<Persona>();
            await Task.Delay(01); //Para dar tiempo a renderizar el mensaje de carga

            try {
                personas = await contexto.Personas.ToListAsync();

            } catch (Exception) {

                throw;
            } finally {
                await contexto.DisposeAsync();
            }

            return personas;

        }

    }
}
