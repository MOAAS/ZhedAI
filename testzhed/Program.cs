using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;

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

            ZhedBoard board = new ZhedBoard("levels/testlevel.txt");
            Solver solver = new Solver(board);

            Menu menu = new Menu();
            menu.ShowMenu();
            Options option;

            do {
                option = menu.GetOption();
            } while(option == Options.Invalid);

            switch(option) {
                case Options.Play: Play(board); break;
                case Options.SolveDFS: ShowZhedSteps(solver, SearchMethod.DFS); break;
                case Options.SolveBFS: ShowZhedSteps(solver, SearchMethod.BFS); break;
            }
        }

        private static void Play(ZhedBoard board) {

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
                    case 'U': board = board.GoUp(new Coords(X, Y)); break;
                    case 'D': board = board.GoDown(new Coords(X, Y)); break;
                    case 'L': board = board.GoLeft(new Coords(X, Y)); break;
                    case 'R': board = board.GoRight(new Coords(X, Y)); break;
                    default: Console.WriteLine("Invalid direction."); break;
                }
            }

            Console.WriteLine("You win!");
            board.PrintBoard();
        }

        private static void ShowZhedSteps(Solver solver, SearchMethod method) {
            solver.GetBoard().PrintBoard();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<ZhedStep> steps = solver.Solve(method);

	        stopwatch.Stop();
	        Console.WriteLine("{0} method: Elapsed Time is {1} ms\n", method, stopwatch.ElapsedMilliseconds);

            foreach (ZhedStep step in steps) 
                step.Print(); 
        }
    } 

    
}
