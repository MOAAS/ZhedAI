using System;

namespace ZhedSolver
{
    enum SearchOption {
        Human,
        SolveDFS,
        SolveBFS,
        SolveGreedy,
        SolveAstar,
        SolveUniform,
    }

    /* Menu class
        Shows the different menu options and gets input from user.
    */
    class Menu {
        public static void ShowMenu() {
            Console.WriteLine("Search options");
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

        public static SearchOption GetOption() {
            while (true) {
                char dirInput = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (char.ToUpper(dirInput)) {
                    case '1': return SearchOption.Human;
                    case '2': return SearchOption.SolveDFS;
                    case '3': return SearchOption.SolveBFS;
                    case '4': return SearchOption.SolveGreedy;
                    case '5': return SearchOption.SolveAstar;
                    case '6': return SearchOption.SolveUniform;
                    default: Console.WriteLine("Invalid option"); break;
                }
            }
        }

        public static int HeuristicMenu() {
            Console.Write("Choose Heuristic (1 - 5): ");
            return GetInt(1, 5);
        }

        public static string LevelPickerMenu() {
            Console.WriteLine("+--------------------+");
            Console.WriteLine("|        Zhed        |");
            Console.WriteLine("+--------------------+");
            Console.Write("Choose Level (1 - 99): ");
            return "levels/level" + GetInt(1, 99) + ".txt";
        }

        private static int GetInt(int min, int max) {
            while (true) {
                string input = Console.ReadLine();
                int number;
                if (int.TryParse(input, out number) && number >= min && number <= max)
                    return number;
                else Console.WriteLine("Invalid number. Must be between {0} and {1}.", min, max);
            }
        }
    }
}