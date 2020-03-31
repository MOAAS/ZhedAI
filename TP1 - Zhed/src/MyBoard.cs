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

        private int width, height;
        private int boardValue = 0;
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

        /* Copies another board onto this board.
            All fields are copied by value.
        */
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

        /* Reads a board from a file.
           File structure is specified on the powerpoint.
        */
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

        /* Resets the board
            Gets the value tiles and finish tile fields and sets the board field accordingly.
            Used only when board is build.
        */
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

        /* Prints the board on the console.
        */
        public void PrintBoard() {
            Console.Write("   |");
            for(int j = 0; j < width; j++) {
                if (j < 10)
                    Console.Write(" ");
                Console.Write(" " + j);
            }
            Console.Write("\n---+");
            for(int j = 0; j < width; j++) {
                Console.Write("---");
            }
            Console.WriteLine();


            for(int i = 0; i < height; i++){
                if (i < 10)
                    Console.Write(" ");
                Console.Write(i + " | ");
                for(int j = 0; j < width; j++){
                    if (board[i][j] >= 0 && board[i][j] < 10)
                        Console.Write(" ");
                    Console.Write(board[i][j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /* Spreads a tile up.
            Does not change the board. Instead it returns the new changed board.
        */
        public ZhedBoard GoUp(Coords coords) {
            return SpreadTile(this, coords, Coords.MoveUp);
        }

        /* Spreads a tile down.
            Does not change the board. Instead it returns the new changed board.
        */
        public ZhedBoard GoDown(Coords coords){
            return SpreadTile(this, coords, Coords.MoveDown);
        }

        /* Spreads a tile left.
            Does not change the board. Instead it returns the new changed board.
        */
        public ZhedBoard GoLeft(Coords coords){
            return SpreadTile(this, coords, Coords.MoveLeft);
        }

        /* Spreads a tile right.
            Does not change the board. Instead it returns the new changed board.
        */
        public ZhedBoard GoRight(Coords coords){
            return SpreadTile(this, coords, Coords.MoveRight);
        }

        /* Spreads a tile in a specified direction.
            Does not change the board. Instead it returns the new changed board.
        */
        private static ZhedBoard SpreadTile(ZhedBoard board, Coords coords, Func<Coords, Coords> moveFunction) {
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
        
        /* Removes a coordinate from the value tiles arrays.        
        */
        private void UpdateValueTiles(Coords coords) {
            this.valueTiles.RemoveAll(tile => tile[0] == coords.x && tile[1] == coords.y);
            this.valueTilesCoords.RemoveAll(coord => coord.x == coords.x && coord.y == coords.y);
        }

        /* Checks if a coordinate pair is valid within the board.
            Returns true if it is, false otherwise.
        */
        public bool inbounds(Coords coords){
            return coords.x >= 0 && coords.y >= 0 && coords.x < width && coords.y < height;
        }

        /* Gets the element at the specified coordinates.
            Assumes valid coordinates.
        */
        public int TileValue(Coords coords) {
            return this.board[coords.y][coords.x];
        }

        /* Sets a board tile to a specified value.
            Assumes valid coordinates;
        */
        private int SetTile(Coords coords, int value) {
            return this.board[coords.y][coords.x] = value;
        }


        /* Gets a list with the finish tiles.
            A tile consists of 2 elements, x, y.
        */
        public List<int[]> GetFinishTiles() {
            return finishTiles;
        }

        /* Gets a list with the value tiles.
            A tile consists of 3 elements, x, y and value.
        */
        public List<int[]> GetValueTiles() {
            return valueTiles;
        }


        /* Gets a list with the value tiles' coordinates.

        */
        public List<Coords> GetValueTilesCoords() {
            return this.valueTilesCoords;
        }

        /* Checks how far a tile can reach.
            Empty and used tiles count as 2, value tiles count as 1.
        */
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

        /* Sum of the number of tiles that can be covered with the current layout.
            Is used for heuristic 3 of the solver.
        */
        public int getBoardMaxValue(){
            int totalValue = this.boardValue;
            foreach (Coords coord in valueTilesCoords){
                int submax1 = Math.Max(checkTileExtensionValue(coord,Coords.MoveUp),checkTileExtensionValue(coord,Coords.MoveDown));
                int submax2 = Math.Max(checkTileExtensionValue(coord,Coords.MoveRight),checkTileExtensionValue(coord,Coords.MoveLeft));
                totalValue += Math.Max(submax1,submax2);
            }
            return totalValue;
        }
        public float getBoardTotalValue(){
            float totalValue = this.boardValue;
            foreach (Coords coord in valueTilesCoords){
                totalValue += (
                    checkTileExtensionValue(coord,Coords.MoveUp) +
                    checkTileExtensionValue(coord,Coords.MoveDown) +
                    checkTileExtensionValue(coord,Coords.MoveRight) +
                    checkTileExtensionValue(coord,Coords.MoveLeft)
                ) / 4;
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