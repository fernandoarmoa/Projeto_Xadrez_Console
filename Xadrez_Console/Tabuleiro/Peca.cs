namespace tabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public Tabuleiro Tab { get; protected set; }
        public int QtdeMovimentos { get; protected set; }

        public Peca(Tabuleiro tab, Cor cor)
        {
            Posicao = null;
            Tab = tab;
            Cor = cor;
            QtdeMovimentos = 0;
        }

        public void IncrementarQtdeMovimentos()
        {
            QtdeMovimentos++;
        }

        public void DecrementarQtdeMovimentos()
        {
            QtdeMovimentos--;
        }

        public bool ExisteMovimentosPosiveis()
        {
            bool[,] mat = MovimentosPosiveis();
            for (int x = 0; x < Tab.Linhas; x++)
            {
                for(int y = 0; y < Tab.Colunas; y++)
                {
                    if (mat[x, y])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PodeMoverPara(Posicao pos)
        {
            return MovimentosPosiveis()[pos.Linha, pos.Coluna];
        }
        public abstract bool[,] MovimentosPosiveis();

    }
}
