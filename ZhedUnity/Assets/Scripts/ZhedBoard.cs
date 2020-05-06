using System;
using System.Collections.Generic;
using System.IO;  
using System.Linq;

namespace ZhedSolver
{
    public class ZhedBoard
    {               
        public const int EMPTY_TILE = 0;
        public const int USED_TILE = -1;
        public const int FINISH_TILE = -2;
        public const int WINNER_TILE = -3;

        public int width, height;
        public int boardValue = 0;
        private List<int[]> valueTiles = new List<int[]>{}; 
        private List<Coords> valueTilesCoords = new List<Coords>{}; 
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

        public ZhedBoard(ZhedBoard zhedBoard) {
            this.width = zhedBoard.width;
            this.height = zhedBoard.height;
            this.board = new List<List<int>>();
            this.valueTilesCoords = new List<Coords>(zhedBoard.valueTilesCoords);
            this.valueTiles = new List<int[]>(zhedBoard.valueTiles);
            this.finishTiles = new List<int[]>(zhedBoard.finishTiles);
            this.boardValue = zhedBoard.boardValue;

            for(int i = 0; i < this.height; i ++) {
                this.board.Add(new List<int>(zhedBoard.board[i]));
            }
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
                if (value > 0){
                    valueTiles.Add(new int[]{x, y, value});
                    valueTilesCoords.Add(new Coords(x,y));
                }        
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

        public ZhedBoard GoUp(Coords coords) {
            return SpreadTile(this, coords, Coords.MoveUp);
        }

        public ZhedBoard GoDown(Coords coords){
            return SpreadTile(this, coords, Coords.MoveDown);
        }

        public ZhedBoard GoLeft(Coords coords){
            return SpreadTile(this, coords, Coords.MoveLeft);
        }

        public ZhedBoard GoRight(Coords coords){
            return SpreadTile(this, coords, Coords.MoveRight);
        }

        public static ZhedBoard SpreadTile(ZhedBoard board, Coords coords, Func<Coords, Coords> moveFunction) {
            int tileValue = board.TileValue(coords);
            if(tileValue <= 0) {
                return board;
            }

            ZhedBoard newBoard = new ZhedBoard(board);

            newBoard.SetTile(coords, USED_TILE);
            newBoard.boardValue += 2*tileValue;
            newBoard.UpdateValueTiles(coords);
            while(tileValue>0){
                coords = moveFunction(coords);
                if(!newBoard.inbounds(coords))
                    break;

                switch (newBoard.TileValue(coords)) {
                    case EMPTY_TILE: newBoard.SetTile(coords, USED_TILE);  tileValue--; break;
                    case FINISH_TILE: newBoard.SetTile(coords, WINNER_TILE); newBoard.isOver = true; break;
                    default: break;
                }
            }
        
            return newBoard;
        }
        
        private void UpdateValueTiles(Coords coords) {
            this.valueTiles.RemoveAll(tile => tile[0] == coords.x && tile[1] == coords.y);
            this.valueTilesCoords.RemoveAll(coord => coord.x == coords.x && coord.y == coords.y);
        }

        public bool inbounds(Coords coords){
            return coords.x >= 0 && coords.y >= 0 && coords.x < width && coords.y < height;
        }

        public int TileValue(Coords coords) {
            return this.board[coords.y][coords.x];
        }

        private int SetTile(Coords coords, int value) {
            return this.board[coords.y][coords.x] = value;
        }

        public List<int[]> GetFinishTiles() {
            return finishTiles;
        }

        public List<int[]> GetValueTiles() {
            return valueTiles;
        }

        public List<Coords> GetValueTilesCoords() {
            return this.valueTilesCoords;
        }

        public List<List<int>> GetBoard() {
            return this.board;
        }

        public bool Winner() {
            return this.isOver;
        }

        public bool Loser() {
            return !Winner() && this.valueTiles.Count == 0;
        }

        private int checkTileExtensionValue(Coords coords, Func<Coords, Coords> moveFunction) {
            int tileValue = this.TileValue(coords);
            int tileExtensionValue = 0;
            while(tileValue>0){
                tileExtensionValue+=2;
                coords = moveFunction(coords);
                if(!inbounds(coords))
                    break;
                
                int val = TileValue(coords);
                if(val == EMPTY_TILE || val == FINISH_TILE)
                    tileValue--;
                else if(val > 0)
                    tileExtensionValue--;
            }
        
            return tileExtensionValue;
        }

        public int getBoardMaxValue(){
            int totalValue = this.boardValue;
            foreach (Coords coord in valueTilesCoords){
                int submax1 = Math.Max(checkTileExtensionValue(coord,Coords.MoveUp),checkTileExtensionValue(coord,Coords.MoveDown));
                int submax2 = Math.Max(checkTileExtensionValue(coord,Coords.MoveRight),checkTileExtensionValue(coord,Coords.MoveLeft));
                totalValue+= Math.Max(submax1,submax2);
            }
            return totalValue;
        }
        public float getBoardTotalMaxValue(){
            float totalValue = this.boardValue;
            foreach (Coords coord in valueTilesCoords){
                totalValue+= (checkTileExtensionValue(coord,Coords.MoveUp)+checkTileExtensionValue(coord,Coords.MoveDown)+checkTileExtensionValue(coord,Coords.MoveRight)+checkTileExtensionValue(coord,Coords.MoveLeft))/4;
            }
            return  totalValue;
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

        public bool AlignedWith(Coords coords) {
            return this.x == coords.x || this.y == coords.y;
        }
    };
}