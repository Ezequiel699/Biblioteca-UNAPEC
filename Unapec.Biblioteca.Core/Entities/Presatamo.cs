using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unapec.Biblioteca.Core.Entities
{
    // Ejemplo conceptual de la nueva entidad
    public class Prestamo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LibroId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; } // null si no se ha devuelto
        public bool Devuelto { get; set; } // o puedes inferirlo de FechaDevolucion
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public DateTime? ActualizadoEn { get; set; }

        // Propiedades de Navegación
        public Usuario Usuario { get; set; } = default!;
        public Libro Libro { get; set; } = default!;
    }
}
