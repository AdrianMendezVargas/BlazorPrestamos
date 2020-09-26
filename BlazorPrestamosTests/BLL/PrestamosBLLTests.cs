using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorPrestamos.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using BlazorPrestamos.Entidades;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace BlazorPrestamos.BLL.Tests {
    [TestClass()]
    public class PrestamosBLLTests {

        [TestMethod()]
        public async Task InsertarTest() {

            int personaId = 1;//Ya existe

            bool FuePrestamoInsertado = false;
            bool FuePersonaBalanceModificado = false;

            Prestamo prestamo = new Prestamo {
                Id = 0 ,
                PersonaId = personaId ,
                Monto = 1000 ,
                Balance = 1000 ,
                Fecha = DateTime.Now
            };

            var persona = await PersonasBLL.Buscar(personaId);

            if (persona != null) {

                decimal balaceInicial = persona.Balance;

                FuePrestamoInsertado = await PrestamosBLL.Insertar(prestamo);

                var personaModificada = await PersonasBLL.Buscar(personaId);

                decimal balanceFinal = personaModificada.Balance;

                FuePersonaBalanceModificado = (balanceFinal == (balaceInicial + prestamo.Monto));

            } else {
                Console.WriteLine("La Persona no existe");
            }

            Assert.IsTrue(FuePrestamoInsertado && FuePersonaBalanceModificado);
        }

        [TestMethod()]
        public async Task ModificarTest() {
            int PrestamoId = 1;
            decimal BalanceRestado = 500m;

            var prestamo = await PrestamosBLL.Buscar(PrestamoId);
            var persona = await PersonasBLL.Buscar(prestamo.PersonaId);

            decimal BalancePrestamoInicial = prestamo.Balance;
            decimal BalacePersonaInicial = persona.Balance;

            prestamo.Balance -= BalanceRestado;

            await PrestamosBLL.Modificar(prestamo);

            var personaModificada = await PersonasBLL.Buscar(prestamo.PersonaId);
            var prestamoModificado = await PrestamosBLL.Buscar(PrestamoId);

            bool FuePestamoModificado = (prestamoModificado.Balance == (BalancePrestamoInicial - BalanceRestado));
            bool FuePersonaBalanceModificado = (personaModificada.Balance == (BalacePersonaInicial - BalanceRestado));

            Assert.IsTrue(FuePestamoModificado && FuePersonaBalanceModificado);
        }

        [TestMethod()]
        public void BuscarTest() {

            var prestamo = PrestamosBLL.Buscar(1);

            Assert.IsNotNull(prestamo);
        }

        [TestMethod()]
        public async Task EliminarTest() {
            int prestamoId = 1;
            int personaId = 1;

            var persona = await PersonasBLL.Buscar(personaId);
            decimal BalancePersonaInicial = persona.Balance;

            var prestamo = await PrestamosBLL.Buscar(prestamoId);
            decimal BalancePrestamo = prestamo.Balance;

            await PrestamosBLL.Eliminar(prestamoId);

            var personaModificada = await PersonasBLL.Buscar(personaId);

            bool FueBalancePersonaModificado = (personaModificada.Balance == (BalancePersonaInicial - BalancePrestamo));

            Assert.IsTrue(FueBalancePersonaModificado);
        }
    }
}