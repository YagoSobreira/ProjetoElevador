using System;
using ProjetoElevador.Models;
using ProjetoElevador.Services;
using ProjetoElevador.UI;

namespace ProjetoElevador
{
    class Program
    {
        static void Main()
        {
            try
            {
                IniciarAplicacao();
            }
            catch (Exception ex)
            {
                TratarErrosCriticos(ex);
            }
        }

        private static void IniciarAplicacao()
        {
            Console.WriteLine("=== SIMULADOR DE ELEVADOR ===\n");

            var elevador = CriarElevador();
            var controller = new ConsoleController(elevador);

            controller.IniciarInterface();
        }

        private static Elevador CriarElevador()
        {
            var factory = new ElevadorFactory();
            return factory.CriarElevador();
        }

        private static void TratarErrosCriticos(Exception ex)
        {
            Console.WriteLine($"ERRO CRÍTICO: {ex.Message}");
            Console.WriteLine("Pressione ENTER para sair...");
            Console.ReadLine();
        }
    }
}