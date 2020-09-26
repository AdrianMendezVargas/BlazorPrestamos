using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPrestamos.Entidades {
    public class Prestamo {

        [Key]
        public int Id { get; set; }

        [Required]
        public int PersonaId { get; set; }

        public DateTime Fecha { get; set; }

        public string Concepto { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public decimal Balance { get; set; }

        public Prestamo() {
            Monto = 0m;
            Balance = 0m;
            Fecha = DateTime.Now;
        }


    }
}
