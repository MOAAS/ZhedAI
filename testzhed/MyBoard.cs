using System;
using System.Collections.Generic;

namespace HelloWorld
{
    public class MyBoard
    {
        List<List<int>> board;
        private int width,height;

        
        public MyBoard(){
            
            width = 5;
            height = 5;
            board = new List<List<int>>{
                new List<int> {0,0,0,3,0},
                new List<int> {2,0,0,0,0},
                new List<int> {0,0,0,0,0},
                new List<int> {0,2,0,0,0},
                new List<int> {0,0,0,-2,0}  
            }; 
        }

        public MyBoard(int width, int heigth){

        }

        public void print(){
            for(int i = 0; i < width; i++){
                for(int j=0; j < height; j++){
                    Console.Write(board[i][j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public void goUp(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[y][x];
            if(tileValue>0){
                board[y][x] = -1;
                untapLoop(tileValue,coordinates,moveUp);
            }
        }

        public void goDown(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[y][x];
            if(tileValue>0){
                board[y][x] = -1;
                untapLoop(tileValue,coordinates,moveDown);
            }
        }

        public void goLeft(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[y][x];
            if(tileValue>0){
                board[y][x] = -1;
                untapLoop(tileValue,coordinates,moveLeft);
            }
        }

        public void goRight(int[] coordinates){
            int x = coordinates[0];
            int y = coordinates[1];
            int tileValue = board[y][x];
            if(tileValue>0){
                board[y][x] = -1;
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
                if(board[coordinates[1]][coordinates[0]]==0){
                    Console.WriteLine(board[coordinates[1]][coordinates[0]]);
                    board[coordinates[1]][coordinates[0]]=-1;
                    tileValue--;
                }
                if(board[coordinates[1]][coordinates[0]]==-2){
                    board[coordinates[1]][coordinates[0]]=-3;
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