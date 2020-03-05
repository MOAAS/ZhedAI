using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var b1 = new MyBoard();
            b1.print();
            b1.goUp(new int[]{1,3});
            Console.WriteLine();
            b1.print();
            b1.goRight(new int[]{0,1});
            Console.WriteLine();
            b1.print();
            b1.goDown(new int[]{3,0});
            Console.WriteLine();
            b1.print();
        }
    }
}
