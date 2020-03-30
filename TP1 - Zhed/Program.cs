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
            ZhedBoard board = new ZhedBoard("levels/level10.txt");
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
                case Options.SolveGreedy: ShowZhedSteps(solver, SearchMethod.Greedy); break;
                case Options.SolveAstar: ShowZhedSteps(solver, SearchMethod.Astar); break;
                case Options.SolveUniform: ShowZhedSteps(solver, SearchMethod.Uniform); break;
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
                Console.WriteLine("We give up, this is too hard for us. Hmm but we believe in you :D\n");
            else {
                Console.Write("Perhaps... ");
                hint.Print();
                Console.WriteLine();
            }            
        }

        private static void ShowZhedSteps(Solver solver, SearchMethod method) {
            solver.GetBoard().PrintBoard();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<ZhedStep> steps = solver.Solve(method);

	        stopwatch.Stop();
	        Console.WriteLine("{0} method: Elapsed Time is {1} ms\n", method, stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Steps:");
            foreach (ZhedStep step in steps) 
                step.Print(); 
        }
    } 
 
}
   