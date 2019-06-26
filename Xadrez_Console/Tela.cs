using System;
using System.Collections.Generic;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirPartida(PartidaDeXadrez partida)
        {
            ImprimirTabuleiro(partida.tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.turno);
            Console.WriteLine("Aguardando jogador: " + partida.jogadorAtual);
        }

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Peças capturadas:");
            Console.Write("Amarelas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Amarelo), Cor.Amarelo);
            Console.WriteLine();
            Console.Write("Vermelhas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Vermelho), Cor.Vermelho);
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto, Cor cor)
        {
            ConsoleColor aux = Console.ForegroundColor;
            Console.Write("[ ");
            foreach (Peca x in conjunto)
            {
                if(cor == Cor.Amarelo)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(x + " ");
                }
                else if(cor == Cor.Vermelho)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(x + " ");
                }
            }
            Console.ForegroundColor = aux;
            Console.Write("]");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int x = 0; x < tab.Linhas; x++)
            {
                Console.Write(8 - x + " ");
                for (int y = 0; y < tab.Colunas; y++)
                {
                    ImprimirPeca(tab.peca(x, y));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPosiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoPosicoesPossiveis = ConsoleColor.DarkGray;

            for (int x = 0; x < tab.Linhas; x++)
            {
                Console.Write(8 - x + " ");
                for (int y = 0; y < tab.Colunas; y++)
                {
                    if (posicoesPosiveis[x, y])
                    {
                        Console.BackgroundColor = fundoPosicoesPossiveis;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    ImprimirPeca(tab.peca(x, y));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
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
            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                switch (peca.Cor)
                {
                    case Cor.Branco:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case Cor.Preto:
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case Cor.Amarelo:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case Cor.Azul:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case Cor.Vermelho:
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
                Console.Write(" ");
            }
        }

    }
}
