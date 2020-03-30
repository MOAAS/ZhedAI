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
            ZhedBoard board = new ZhedBoard(Menu.LevelPickerMenu());
            Solver solver = new Solver(board);

            Menu.ShowMenu();            
            switch(Menu.GetOption()) {
                case SearchOption.Human: Play(board); break;
                case SearchOption.SolveDFS: ShowZhedSteps(solver, SearchMethod.DFS, Solver.Heuristic0); break;
                case SearchOption.SolveBFS: ShowZhedSteps(solver, SearchMethod.BFS, Solver.Heuristic0); break;
                case SearchOption.SolveGreedy: ShowZhedSteps(solver, SearchMethod.Greedy, GetHeuristic()); break;
                case SearchOption.SolveAstar: ShowZhedSteps(solver, SearchMethod.Astar, GetHeuristic()); break;
                case SearchOption.SolveUniform: ShowZhedSteps(solver, SearchMethod.Uniform, Solver.Heuristic0); break;
            }
        }

        private static Func<ZhedBoard, int> GetHeuristic() {
            switch (Menu.HeuristicMenu()) {
                case 1: return Solver.Heuristic1;
                case 2: return Solver.Heuristic2;
                case 3: return Solver.Heuristic3;
                case 4: return Solver.Heuristic4;
                case 5: return Solver.Heuristic5;
                default: Console.WriteLine("Invalid heuristic!"); return Solver.Heuristic0;
            }
        }

        private static void Play(ZhedBoard board) {

            while (!board.isOver) {
                board.PrintBoard();

                Console.Write("Select tile (X, Y) / H for hint: ");
                string input = Console.ReadLine();

                if (input.ToLower() == "h") {
                    ShowHint(board);
                    continue;
                }

                string[] coordsInput = input.Split(); int X, Y;
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

        private static void ShowHint(ZhedBoard board) {
            ZhedStep hint = new Solver(board).GetHint();
            if (hint == null)
                Console.WriteLine("We couldn't solve this...\n");
            else {
                Console.Write("Perhaps... ");
                hint.Print();
                Console.WriteLine();
            }            
        }

        private static void ShowZhedSteps(Solver solver, SearchMethod method, Func<ZhedBoard, int> heuristic) {
            solver.GetBoard().PrintBoard();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<ZhedStep> steps = solver.Solve(method, heuristic);

	        stopwatch.Stop();
	        Console.WriteLine("{0} method: Elapsed Time is {1} ms\n", method, stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Steps:");
            foreach (ZhedStep step in steps) 
                step.Print(); 
        }
    } 
 
}
   