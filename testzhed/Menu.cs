using System;

namespace ZhedSolver
{
    enum Options {
        Play,
        SolveDFS,
        SolveBFS,
        Invalid
    }

    class Menu {
        public void ShowMenu() {
            Console.WriteLine("Zhed");
            Console.WriteLine("---------------------");
            Console.WriteLine("1 - Solve Puzzle");
            Console.WriteLine("2 - Let AI solve it (DFS)");
            Console.WriteLine("3 - Let AI solve it (BFS)");
            Console.WriteLine("---------------------");
            Console.Write("Choose an option: ");
        }

        public Options GetOption() {
            char dirInput = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (char.ToUpper(dirInput)) {
                case '1': return Options.Play;
                case '2': return Options.SolveDFS;
                case '3': return Options.SolveBFS;
                default: Console.WriteLine("Invalid option."); return Options.Invalid;
            }
        }
    }
}