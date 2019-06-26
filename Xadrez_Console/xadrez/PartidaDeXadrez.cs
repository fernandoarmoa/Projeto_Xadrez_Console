using System.Collections.Generic;
using tabuleiro;
using tabuleiro.Exceptions;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Amarelo;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQtdeMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            turno++;
            MudaJogador();
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if(jogadorAtual != tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).ExisteMovimentosPosiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador()
        {
            if(jogadorAtual == Cor.Amarelo)
            {
                jogadorAtual = Cor.Vermelho;
            }
            else
            {
                jogadorAtual = Cor.Amarelo;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if(x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }


        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }
        private void ColocarPecas()
        {
            //  PEÇAS AMARELAS
            ColocarNovaPeca('c', 1, new Torre(tab, Cor.Amarelo));
            ColocarNovaPeca('c', 2, new Torre(tab, Cor.Amarelo));
            ColocarNovaPeca('d', 2, new Torre(tab, Cor.Amarelo));
            ColocarNovaPeca('e', 2, new Torre(tab, Cor.Amarelo));
            ColocarNovaPeca('e', 1, new Torre(tab, Cor.Amarelo));
            ColocarNovaPeca('d', 1, new Rei(tab, Cor.Amarelo));

            //tab.ColocarPeca(new Torre(tab, Cor.Amarelo), new PosicaoXadrez('c', 1).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Amarelo), new PosicaoXadrez('c', 2).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Amarelo), new PosicaoXadrez('d', 2).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Amarelo), new PosicaoXadrez('e', 2).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Amarelo), new PosicaoXadrez('e', 1).ToPosicao());
            //tab.ColocarPeca(new Rei(tab, Cor.Amarelo), new PosicaoXadrez('d', 1).ToPosicao());

            //  PEÇAS VERMELHAS

            ColocarNovaPeca('c', 8, new Torre(tab, Cor.Vermelho));
            ColocarNovaPeca('c', 7, new Torre(tab, Cor.Vermelho));
            ColocarNovaPeca('d', 7, new Torre(tab, Cor.Vermelho));
            ColocarNovaPeca('e', 8, new Torre(tab, Cor.Vermelho));
            ColocarNovaPeca('e', 7, new Torre(tab, Cor.Vermelho));
            ColocarNovaPeca('d', 8, new Rei(tab, Cor.Vermelho));

            //tab.ColocarPeca(new Torre(tab, Cor.Vermelho), new PosicaoXadrez('c', 8).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Vermelho), new PosicaoXadrez('c', 7).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Vermelho), new PosicaoXadrez('d', 7).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Vermelho), new PosicaoXadrez('e', 8).ToPosicao());
            //tab.ColocarPeca(new Torre(tab, Cor.Vermelho), new PosicaoXadrez('e', 7).ToPosicao());
            //tab.ColocarPeca(new Rei(tab, Cor.Vermelho), new PosicaoXadrez('d', 8).ToPosicao());
        }
    }
}
