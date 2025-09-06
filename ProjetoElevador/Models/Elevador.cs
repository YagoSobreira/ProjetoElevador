using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoElevador.Models
{
    public class Elevador
    {
        private int _andarAtual = 0;
        private StatusElevador _status = StatusElevador.Parado;
        private StatusPorta _porta = StatusPorta.Aberta;
        private int _passageiros = 0;
        private List<int> _rota = new List<int>();

        public int CapacidadeMaxima { get; } = 8;
        public int AndarAtual => _andarAtual;
        public StatusElevador Status => _status;
        public StatusPorta Porta => _porta;
        public int Passageiros => _passageiros;
        public List<int> Rota => _rota.ToList();
        public bool TemRota => _rota.Count > 0;

        public Elevador()
        {
            InicializarElevador();
        }

        public void EmbarcarPassageiros(int quantidade)
        {
            ValidarEmbarque(quantidade);
            _passageiros += quantidade;
        }

        public void DesembarcarPassageiros(int quantidade)
        {
            ValidarDesembarque(quantidade);
            _passageiros -= quantidade;
        }

        public void SelecionarAndares(List<int> andares)
        {
            ValidarSelecao();
            AdicionarAndares(andares);
        }

        public void FecharPorta()
        {
            ValidarFechamento();
            _porta = StatusPorta.Fechada;
            AtualizarStatusMovimento();
        }

        public void AbrirPorta()
        {
            ValidarAbertura();
            _porta = StatusPorta.Aberta;
        }

        public void Mover()
        {
            ValidarMovimento();
            ExecutarMovimento();
        }

        private void InicializarElevador()
        {
            // Regra 1: Elevador inicia no térreo, parado, porta aberta
            _andarAtual = 0;
            _status = StatusElevador.Parado;
            _porta = StatusPorta.Aberta;
        }

        private void ValidarEmbarque(int quantidade)
        {
            // Regra 7: Só embarca parado com porta aberta
            if (_status != StatusElevador.Parado || _porta != StatusPorta.Aberta)
                throw new InvalidOperationException("Só pode embarcar com elevador parado e porta aberta");

            if (_passageiros + quantidade > CapacidadeMaxima)
                throw new InvalidOperationException("Capacidade excedida");
        }

        private void ValidarDesembarque(int quantidade)
        {
            // Regra 7: Só desembarca parado com porta aberta
            if (_status != StatusElevador.Parado || _porta != StatusPorta.Aberta)
                throw new InvalidOperationException("Só pode desembarcar com elevador parado e porta aberta");

            if (quantidade > _passageiros)
                throw new InvalidOperationException("Não há passageiros suficientes");
        }

        private void ValidarSelecao()
        {
            // Regra 12: Só seleciona com porta aberta
            if (_porta != StatusPorta.Aberta)
                throw new InvalidOperationException("Só pode selecionar andares com porta aberta");
        }

        private void AdicionarAndares(List<int> andares)
        {
            // Regra 11: Ignora andar atual
            var novosAndares = andares
                .Where(andar => andar != _andarAtual && !_rota.Contains(andar))
                .ToList();

            _rota.AddRange(novosAndares);
            OrganizarRota();
        }

        private void OrganizarRota()
        {
            // Regra 3: Se subindo, vai até o último andar antes de descer
            var andaresAcima = _rota.Where(a => a > _andarAtual).OrderBy(a => a).ToList();
            var andaresAbaixo = _rota.Where(a => a < _andarAtual).OrderByDescending(a => a).ToList();

            _rota.Clear();
            _rota.AddRange(andaresAcima);
            _rota.AddRange(andaresAbaixo);
        }

        private void ValidarFechamento()
        {
            // Regra 4: Só fecha com rota
            if (_rota.Count == 0)
                throw new InvalidOperationException("Precisa ter uma rota para fechar a porta");
        }

        private void AtualizarStatusMovimento()
        {
            // Regra 6: Status muda quando porta fecha
            if (_rota.Count > 0)
            {
                _status = _rota[0] > _andarAtual ? StatusElevador.Subindo : StatusElevador.Descendo;
            }
        }

        private void ValidarAbertura()
        {
            if (_status != StatusElevador.Parado)
                throw new InvalidOperationException("Só pode abrir porta parado");
        }

        private void ValidarMovimento()
        {
            // Regra 9: Só move com porta fechada
            if (_porta != StatusPorta.Fechada || _rota.Count == 0)
                throw new InvalidOperationException("Precisa estar com porta fechada e ter rota");
        }

        private void ExecutarMovimento()
        {
            int destino = _rota[0];

            // Regra 2: Atualiza status ao mover
            MoverParaDestino(destino);
            VerificarChegada(destino);
        }

        private void MoverParaDestino(int destino)
        {
            if (destino > _andarAtual)
            {
                _andarAtual++;
                _status = StatusElevador.Subindo;
            }
            else if (destino < _andarAtual)
            {
                _andarAtual--;
                _status = StatusElevador.Descendo;
            }
        }

        private void VerificarChegada(int destino)
        {
            if (_andarAtual == destino)
            {
                _rota.RemoveAt(0);
                _status = StatusElevador.Parado;
                _porta = StatusPorta.Aberta; // Regra 6: Para e abre porta
            }
        }
    }
}