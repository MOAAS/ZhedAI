using System;
using System.Collections.Generic;

namespace ZhedSolver
{
    class Program
    {
        static void Main(string[] args)
        {   

            List<int[]> valueTiles =  new List<int[]> {
                new int[] { 2, 2, 1},
                new int[] { 1, 3, 2},
                new int[] { 0, 0, 3}
            };
            List<int[]> finishTiles =  new List<int[]> {
                new int[] {4, 0}
            };

            ZhedBoard board = new ZhedBoard(5, 4, valueTiles, finishTiles);

            while (!board.isOver) {
                board.PrintBoard();
                Console.Write("Select tile (X, Y): ");
                string[] coordsInput = Console.ReadLine().Split();
                int X, Y;
                if (coordsInput.Length != 2 || !int.TryParse(coordsInput[0], out X) || !int.TryParse(coordsInput[1], out Y)) {
                    Console.WriteLine("Invalid input");
                    continue;
                }
                Console.Write("Select direction (U D L R): ");
                char dirInput = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch(char.ToUpper(dirInput)) {
                    case 'U': board.GoUp(new Coords(X, Y)); break;
                    case 'D': board.GoDown(new Coords(X, Y)); break;
                    case 'L': board.GoLeft(new Coords(X, Y)); break;
                    case 'R': board.GoRight(new Coords(X, Y)); break;
                    default: Console.WriteLine("Invalid direction."); break;
                }
            }

            Console.WriteLine("You win!");
        }
    }
}
