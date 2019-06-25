using System;
using tabuleiro;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int x = 0; x < tab.Linhas; x++)
            {
                for (int y = 0; y < tab.Colunas; y++)
                {
                    if (tab.peca(x, y) == null)
                    {
                        Console.Write("- ");
                    }
                    else
                    {
                        Console.Write(tab.peca(x, y) + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
