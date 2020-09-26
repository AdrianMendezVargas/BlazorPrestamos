using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlazorPrestamos.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using BlazorPrestamos.Entidades;
using System.Threading.Tasks;

namespace BlazorPrestamos.BLL.Tests {
    [TestClass()]
    public class PersonasBLLTests {
        [TestMethod()]
        public async Task InsertarTest() {

            Persona persona = new Persona { Nombres = "Adrian Mendez" , Balance = 0 };

            Assert.IsTrue(await PersonasBLL.Insertar(persona));
        }

        [TestMethod()]
        public async Task ModificarTest() {

            Persona persona = new Persona { Id = 1 , Nombres = "Adrian Mendez Vargas" , Balance = 0 };

            Assert.IsTrue(await PersonasBLL.Modificar(persona));
        }

        [TestMethod()]
        public async Task BuscarTest() {

            var persona = await PersonasBLL.Buscar(1);

            Assert.IsNotNull(persona);
        }

        [TestMethod()]
        public async Task GetPersonasTest() {

            var personas = await PersonasBLL.GetPersonas();

            Assert.IsTrue(personas.Count > 0);
        }

        [TestMethod()]
        public async Task EliminarTest() {

            Assert.IsTrue(await PersonasBLL.Eliminar(1));
        }

    }
}