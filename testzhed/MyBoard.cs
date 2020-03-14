using System;
using System.Collections.Generic;
using System.IO;  
using System.Linq;

namespace ZhedSolver
{
    public class ZhedBoard
    {               
        private const int EMPTY_TILE = 0;
        private const int USED_TILE = -1;
        private const int FINISH_TILE = -2;
        private const int WINNER_TILE = -3;

        private int width, height;
        private List<int[]> valueTiles = new List<int[]>{}; 
        private List<int[]> finishTiles = new List<int[]>{}; 
        
        private List<List<int>> board = new List<List<int>>{};

        public bool isOver { get; private set; }

        public ZhedBoard(int width, int height, List<int[]> valueTiles, List<int[]> finishTiles){
            this.width = width;
            this.height = height;
            this.valueTiles = valueTiles;
            this.finishTiles = finishTiles;
            this.ResetBoard();
        }

        public ZhedBoard(String file) {
            string[] lines = File.ReadAllLines(file);  

            this.width = int.Parse(lines[0].Split()[0]);
            this.height = int.Parse(lines[0].Split()[1]);

            foreach (string line in lines.Skip(1).ToArray())  {
                string[] nums = line.Split();
                int x = int.Parse(nums[0]);
                int y = int.Parse(nums[1]);
                int value = int.Parse(nums[2]);
                if (value > 0)
                    valueTiles.Add(new int[]{x, y, value});        
                else finishTiles.Add(new int[]{x, y});
            }

            this.ResetBoard();
        }

        private void ResetBoard(){
            for(int i = 0; i < this.height; i++){
                this.board.Add(new List<int>());
                for(int j = 0; j < this.width; j++){
                    this.board[i].Add(EMPTY_TILE);
                }
            }

            foreach (int[] tile in valueTiles)
                this.board[tile[1]][tile[0]] = tile[2];

            foreach (int[] tile in finishTiles)
                this.board[tile[1]][tile[0]] = FINISH_TILE;
        }

        public void PrintBoard() {
            Console.Write("  |");
            for(int j = 0; j < width; j++) {
                Console.Write("  " + j);
            }
            Console.Write("\n--+");
            for(int j = 0; j < width; j++) {
                Console.Write("---");
            }
            Console.WriteLine();


            for(int i = 0; i < height; i++){
                Console.Write(i + " | ");
                for(int j = 0; j < width; j++){
                    if (board[i][j] >= 0)
                        Console.Write(" ");
                    Console.Write(board[i][j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void GoUp(Coords coords) {
            SpreadTile(coords, Coords.MoveUp);
        }

        public void GoDown(Coords coords){
            SpreadTile(coords, Coords.MoveDown);
        }

        public void GoLeft(Coords coords){
            SpreadTile(coords, Coords.MoveLeft);
        }

        public void GoRight(Coords coords){
            SpreadTile(coords, Coords.MoveRight);
        }

        private void SpreadTile(Coords coords, Func<Coords, Coords> moveFunction){
            int tileValue = TileValue(coords);
            if(tileValue <= 0) {
                Console.WriteLine("Woah gamer! Calm down your horses");
                return;
            }
            SetTile(coords, USED_TILE);

            while(tileValue>0){
                coords = moveFunction(coords);
                if(!inbounds(coords))
                    break;

                switch (TileValue(coords)) {
                    case EMPTY_TILE: SetTile(coords, USED_TILE); tileValue--; break;
                    case FINISH_TILE: SetTile(coords, WINNER_TILE); this.isOver = true; break;
                    default: break;
                }
            }
        }

        public bool inbounds(Coords coords){
            return coords.x >= 0 && coords.y >= 0 && coords.x < width && coords.y < height;
        }

        private int TileValue(Coords coords) {
            return this.board[coords.y][coords.x];
        }

        private int SetTile(Coords coords, int value) {
            return this.board[coords.y][coords.x] = value;
        }
    }


    public class Coords {
        public int x { get; set; }
        public int y { get; set; }

        public Coords(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public static Coords MoveUp(Coords coords) {
            return new Coords(coords.x, coords.y - 1);
        }

        public static Coords MoveDown(Coords coords) {
            return new Coords(coords.x, coords.y + 1);
        }

        public static Coords MoveLeft(Coords coords) {
            return new Coords(coords.x - 1, coords.y);
        }
        
        public static Coords MoveRight(Coords coords) {
            return new Coords(coords.x + 1, coords.y);
        }
    };
}