using System;

namespace ZhedSolver
{
    enum Options {
        Play,
        SolveDFS,
        SolveBFS,
        SolveGreedy,
        SolveAstar,
        SolveUniform,
        Invalid
    }

    /* Menu class
        Shows the different menu options and gets input from user.
    */
    class Menu {
        public void ShowMenu() {
            Console.WriteLine("Zhed");
            Console.WriteLine("---------------------");
            Console.WriteLine("1 - Solve puzzle manually");
            Console.WriteLine("2 - Let AI solve it (DFS)");
            Console.WriteLine("3 - Let AI solve it (BFS)");
            Console.WriteLine("4 - Let AI solve it (Greedy)");
            Console.WriteLine("5 - Let AI solve it (A*)");
            Console.WriteLine("6 - Let AI solve it (Uniform Cost)");
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
                case '4': return Options.SolveGreedy;
                case '5': return Options.SolveAstar;
                case '6': return Options.SolveUniform;
                default: Console.WriteLine("Invalid option."); return Options.Invalid;
            }
        }
    }
}