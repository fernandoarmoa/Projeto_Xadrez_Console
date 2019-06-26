using System;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int x = 0; x < tab.Linhas; x++)
            {
                Console.Write(8 - x + " ");
                for (int y = 0; y < tab.Colunas; y++)
                {
                    if (tab.peca(x, y) == null)
                    {
                        Console.Write("- ");
                    }
                    else
                    {
                        ImprimirPeca(tab.peca(x, y));
                        Console.Write(" ");
                        //Console.Write(tab.peca(x, y) + " ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }
        public static void ImprimirPeca(Peca peca)
        {
            ConsoleColor corOriginal = Console.ForegroundColor;
            switch (peca.Cor)
            {
                case Cor.Branca:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Cor.Preta:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case Cor.Amarela:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Cor.Azul:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Cor.Vermelha:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Cor.Verde:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.Write(peca);
            Console.ForegroundColor = corOriginal;
        }

    }
}
