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
        public bool xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Amarelo;
            terminada = false;
            xeque = false;
            VulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQtdeMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            //  #jogadaEspecial RoquePequeno

            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQtdeMovimentos();
                tab.ColocarPeca(T, destinoT);
            }

            //  #jogadaEspecial RoqueGrande

            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQtdeMovimentos();
                tab.ColocarPeca(T, destinoT);
            }

            //  #jogadaEspecial - En Passant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if(p.Cor == Cor.Amarelo)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = tab.RetirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.RetirarPeca(destino);
            p.DecrementarQtdeMovimentos();
            if(pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, origem);

            //  #jogadaEspecial RoquePequeno

            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQtdeMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            //  #jogadaEspecial RoqueGrande

            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQtdeMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            //  #jogadaEspecial - En Passant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = tab.RetirarPeca(destino);
                    Posicao posP;
                    if(p.Cor == Cor.Amarelo)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    tab.ColocarPeca(peao, posP);

                }
            }

        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EstaEmCheque(jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (EstaEmCheque(CorAdversario(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TesteXequeMate(CorAdversario(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }

            Peca p = tab.peca(destino);

            //  #jogadaEspecial - En Passant

            if(p is Peao && destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2)
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }
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
            if (!tab.peca(origem).MovimentoPosivel(destino))
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

        private Cor CorAdversario(Cor cor)
        {
            if(cor == Cor.Amarelo)
            {
                return Cor.Vermelho;
            }
            else
            {
                return Cor.Amarelo;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if(x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmCheque(Cor cor)
        {
            Peca R = Rei(cor);
            if(R == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca x in PecasEmJogo(CorAdversario(cor)))
            {
                bool[,] mat = x.MovimentosPosiveis();
                if(mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmCheque(cor))
            {
                return false;
            }
            foreach (Peca pc in PecasEmJogo(cor))
            {
                bool[,] mat = pc.MovimentosPosiveis();
                for (int x = 0; x < tab.Linhas; x++)
                {
                    for (int y = 0; y < tab.Colunas; y++)
                    {
                        if (mat[x, y])
                        {
                            Posicao origem = pc.Posicao;
                            Posicao destino = new Posicao(x, y);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmCheque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }

                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }
        private void ColocarPecas()
        {
            char[] letras = new char[8] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

            //  PEÇAS AMARELAS

            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Amarelo));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Amarelo));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Amarelo));
            ColocarNovaPeca('d', 1, new Dama(tab, Cor.Amarelo));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Amarelo, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Amarelo));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Amarelo));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Amarelo));

            for (int i = 0; i < letras.Length; i++)
            {
                ColocarNovaPeca(letras[i], 2, new Peao(tab, Cor.Amarelo, this));
            }


            //  PEÇAS VERMELHAS

            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Vermelho));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Vermelho));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Vermelho));
            ColocarNovaPeca('d', 8, new Dama(tab, Cor.Vermelho));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Vermelho, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Vermelho));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Vermelho));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Vermelho));

            for (int i = 0; i < letras.Length; i++)
            {
                ColocarNovaPeca(letras[i], 7, new Peao(tab, Cor.Vermelho, this));
            }

        }
    }
}
