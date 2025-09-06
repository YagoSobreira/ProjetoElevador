using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProjetoElevador.Models;

namespace ProjetoElevador.Services
{
    // Regra 14: Factory sem parâmetros no construtor
    public class ElevadorFactory
    {
        public ElevadorFactory() { }

        public Elevador CriarElevador() => new Elevador();
    }
}