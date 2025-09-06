using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using ProjetoElevador.Models;

namespace ProjetoElevador.UI
{
    public class ConsoleController
    {
        private readonly Elevador _elevador;

        public ConsoleController(Elevador elevador)
        {
            _elevador = elevador;
        }

        public void IniciarInterface()
        {
            while (true)
            {
                ExibirStatus();
                ExibirMenu();

                var opcao = Console.ReadLine();

                if (!ProcessarComando(opcao))
                    break;

                AguardarContinuacao();
            }
        }

        private void ExibirStatus()
        {
            Console.Clear();
            Console.WriteLine($"Andar: {_elevador.AndarAtual} | Status: {_elevador.Status} | Porta: {_elevador.Porta}");
            Console.WriteLine($"Passageiros: {_elevador.Passageiros}/{_elevador.CapacidadeMaxima}");
            ExibirRota();
            Console.WriteLine();
        }

        private void ExibirRota()
        {
            if (_elevador.TemRota)
                Console.WriteLine($"Rota: [{string.Join(", ", _elevador.Rota)}]");
            else
                Console.WriteLine("Rota: Nenhuma");
        }

        private void ExibirMenu()
        {
            Console.WriteLine("1 - Embarcar passageiros");
            Console.WriteLine("2 - Desembarcar passageiros");
            Console.WriteLine("3 - Selecionar andares");
            Console.WriteLine("4 - Fechar porta");
            Console.WriteLine("5 - Abrir porta");
            Console.WriteLine("6 - Mover");
            Console.WriteLine("7 - Executar rota completa");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha: ");
        }

        private bool ProcessarComando(string opcao)
        {
            try
            {
                return opcao switch
                {
                    "1" => ExecutarEmbarque(),
                    "2" => ExecutarDesembarque(),
                    "3" => ExecutarSelecaoAndares(),
                    "4" => ExecutarFechamento(),
                    "5" => ExecutarAbertura(),
                    "6" => ExecutarMovimento(),
                    "7" => ExecutarRotaCompleta(),
                    "0" => false,
                    _ => ExecutarOpcaoInvalida()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
                return true;
            }
        }

        private bool ExecutarEmbarque()
        {
            var quantidade = LerQuantidade("Quantos passageiros? ");
            _elevador.EmbarcarPassageiros(quantidade);
            Console.WriteLine($"{quantidade} passageiros embarcaram!");
            return true;
        }

        private bool ExecutarDesembarque()
        {
            var quantidade = LerQuantidade("Quantos passageiros? ");
            _elevador.DesembarcarPassageiros(quantidade);
            Console.WriteLine($"{quantidade} passageiros desembarcaram!");
            return true;
        }

        private bool ExecutarSelecaoAndares()
        {
            Console.Write("Digite os andares separados por vírgula: ");
            var input = Console.ReadLine();

            var andares = ParsearAndares(input);
            _elevador.SelecionarAndares(andares);
            Console.WriteLine("Andares selecionados!");
            return true;
        }

        private bool ExecutarFechamento()
        {
            _elevador.FecharPorta();
            Console.WriteLine("Porta fechada!");
            return true;
        }

        private bool ExecutarAbertura()
        {
            _elevador.AbrirPorta();
            Console.WriteLine("Porta aberta!");
            return true;
        }

        private bool ExecutarMovimento()
        {
            _elevador.Mover();
            Console.WriteLine("Elevador moveu!");
            return true;
        }

        private bool ExecutarRotaCompleta()
        {
            if (!_elevador.TemRota)
            {
                Console.WriteLine("Não há rota!");
                return true;
            }

            ExecutarMovimentoCompleto();
            return true;
        }

        private bool ExecutarOpcaoInvalida()
        {
            Console.WriteLine("Opção inválida!");
            return true;
        }

        private void ExecutarMovimentoCompleto()
        {
            _elevador.FecharPorta();

            while (_elevador.TemRota)
            {
                var andarAnterior = _elevador.AndarAtual;
                _elevador.Mover();

                if (_elevador.AndarAtual != andarAnterior)
                    Console.WriteLine($"Andar {andarAnterior} -> {_elevador.AndarAtual}");

                if (_elevador.Status == StatusElevador.Parado)
                {
                    Console.WriteLine($"Chegou no andar {_elevador.AndarAtual}!");

                    if (_elevador.TemRota)
                        _elevador.FecharPorta();
                }
            }

            Console.WriteLine("Rota completa!");
        }

        private int LerQuantidade(string mensagem)
        {
            Console.Write(mensagem);
            return int.TryParse(Console.ReadLine(), out int quantidade) ? quantidade : 0;
        }

        private List<int> ParsearAndares(string input)
        {
            try
            {
                return input?.Split(',').Select(int.Parse).ToList() ?? new List<int>();
            }
            catch
            {
                throw new ArgumentException("Formato inválido! Use números separados por vírgula.");
            }
        }

        private void AguardarContinuacao()
        {
            Console.WriteLine("\nPressione ENTER...");
            Console.ReadLine();
        }
    }
}