using System;
using System.Collections.Generic;

namespace HelloWorld
{
    public class MyBoard
    {
        private List<List<int>> board = new List<List<int>>{};
        private int width,height;
        private List<int[]> valueTiles;
        private List<int[]> finishTiles;
        
        public MyBoard(int width, int height, List<int[]> valueTiles, List<int[]> finishTiles){
            this.width = width;
            this.height = height;
            this.valueTiles = valueTiles;
            this.finishTiles = finishTiles;
            makeEmptyBoard();
            insertValueTiles();
            insertFinishTiles();
        }

        private void makeEmptyBoard(){
            for(var i = 0; i < this.width; i++){
                this.board.Add(new List<int>());
                for(var j = 0; j < this.height; j++){
                      this.board[i].Add(0);
                }
            }
        }
        private void insertValueTiles(){
            foreach (var tile in valueTiles)
                this.board[tile[0]][tile[1]] = tile[2];
        }

        private void insertFinishTiles(){
            foreach (var tile in finishTiles)
                this.board[tile[0]][tile[1]] = -2;
        }

        public void print(){
            for(int j=0; j < height; j++){
                for(int i = 0; i < width; i++){
                    Console.Write(board[i][j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public void goUp(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[x][y];
            if(tileValue>0){
                board[x][y] = -1;
                untapLoop(tileValue,coordinates,moveUp);
            }
        }

        public void goDown(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[x][y];
            if(tileValue>0){
                board[x][y] = -1;
                untapLoop(tileValue,coordinates,moveDown);
            }
        }

        public void goLeft(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[x][y];
            if(tileValue>0){
                board[x][y] = -1;
                untapLoop(tileValue,coordinates,moveLeft);
            }
        }

        public void goRight(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[x][y];
            if(tileValue>0){
                board[x][y] = -1;
                untapLoop(tileValue,coordinates,moveRight);
            }
        }


        private int[] moveUp(int[] coordinates){
            coordinates[1]--;
            return coordinates;
        }
        private int[] moveDown(int[] coordinates){
            coordinates[1]++;
            return coordinates;
        }
        private int[] moveLeft(int[] coordinates){
            coordinates[0]--;
            return coordinates;
        }
        private int[] moveRight(int[] coordinates){
            coordinates[0]++;
            return coordinates;
        }
        private void untapLoop(int tileValue,int[] coordinates,Func<int[], int[]> moveFunction){
            while(tileValue>0){
                coordinates = moveFunction(coordinates);
                if(!inbounds(coordinates))
                    break;
                if(board[coordinates[0]][coordinates[1]]==0){
                    board[coordinates[0]][coordinates[1]]=-1;
                    tileValue--;
                }
                if(board[coordinates[0]][coordinates[1]]==-2){
                    board[coordinates[0]][coordinates[1]]=-3;
                    Console.WriteLine("Finished");
                }
            }
        }

        public bool inbounds(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            if(x<0 || y<0 || x>=width || y>=height)
                return false;
            return true;
        }
    }
}