using System;
using System.Collections.Generic;

namespace HelloWorld
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

            var b1 = new MyBoard(5,4,valueTiles,finishTiles);
            b1.print();
            
            b1.goLeft(new int[]{2,2});
            Console.WriteLine();
            b1.print();

            b1.goUp(new int[]{1,3});
            Console.WriteLine();
            b1.print();

            b1.goRight(new int[]{0,0});
            Console.WriteLine();
            b1.print();
        }
    }
}
