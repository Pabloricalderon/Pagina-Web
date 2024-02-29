using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginaWeb
{
    public class Navegador
    {
        string pagina;
        int vecesIngreso;
        DateTime fechaIngreso;

        public string Pagina { get => pagina; set => pagina = value; }
        public int VecesIngreso { get => vecesIngreso; set => vecesIngreso = value; }
        public DateTime FechaIngreso { get => fechaIngreso; set => fechaIngreso = value; }
    }
}
