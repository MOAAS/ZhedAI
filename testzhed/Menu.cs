using System;

namespace ZhedSolver
{
    class Menu {
        public void showMenu() {
            Console.WriteLine("Zhed");
            Console.WriteLine("---------------------");
            Console.WriteLine("1 - Solve Puzzle");
            Console.WriteLine("2 - Let AI solve it");
            Console.WriteLine("---------------------");
            Console.Write("Choose an option: ");
        }

        public Options getOption() {
            char dirInput = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (char.ToUpper(dirInput)) {
                case '1': return Options.Play;
                case '2': return Options.Solve;
                default: Console.WriteLine("Invalid option."); return Options.Invalid;
            }
        }
    }
}