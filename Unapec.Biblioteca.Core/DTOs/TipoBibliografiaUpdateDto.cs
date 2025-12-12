using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unapec.Biblioteca.Core.DTOs
{
    public class TipoBibliografiaUpdateDto
    {
        public required string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
