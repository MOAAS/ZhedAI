using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZhedSolver
{
    public enum Operations {
        MoveUp, 
        MoveDown, 
        MoveLeft,
        MoveRight
    }

    public enum SearchMethod {
        BFS,
        DFS,
        Greedy,
        Astar
    }

    public class Solver {
        private ZhedBoard board;

        public Solver(ZhedBoard board) {
            this.board = board;
        }

        public List<ZhedStep> Solve(SearchMethod searchMethod) {
            Func<ZhedBoard, int> heuristic = Heuristic5;

            PriorityQueue<Node> queue = new PriorityQueue<Node>();
            queue.Enqueue(new Node(this.board, null, null, 1), 1);

            DFSPriority = int.MaxValue;
            int visitedNodes = 0;
            while(queue.Count > 0) {
                visitedNodes++;
                Node nextNode = queue.Dequeue();

              //  Console.WriteLine("Visit number {0}", visitedNodes); nextNode.board.PrintBoard();
              //  Console.WriteLine("Value of board: {0}" , nextNode.board.getBoardMaxValue());

                if (nextNode.board.isOver) {
                    Console.WriteLine("Visited {0} nodes", visitedNodes);
                    return GetPath(nextNode);
                }
                List<Node> children = GetNextGeneration(nextNode, heuristic);
                foreach(Node node in children)
                    queue.Enqueue(node, NodePriority(searchMethod, node));
            }
            return null;
        }

        private int DFSPriority;

        public int NodePriority(SearchMethod method, Node node) {
            DFSPriority--;
            switch (method) {
                case SearchMethod.BFS: return node.height;
                case SearchMethod.DFS: return DFSPriority; 
                case SearchMethod.Greedy: return node.value;
                case SearchMethod.Astar: return node.value + node.height;
                default: return 1;
            }
        }

        public int Heuristic0(ZhedBoard board) {
            return 1;
        }

        private int Heuristic1(ZhedBoard board) {
            int minZhedDistance = int.MaxValue;

            foreach (int[] valueTile in board.GetValueTiles()) {
                foreach(int[] finishTile in board.GetFinishTiles()) {
                    int zhedDistance;

                    if (valueTile[0] == finishTile[0]) 
                        zhedDistance = CalcZhedDistance(valueTile, finishTile, board, true);
                    else if (valueTile[1] == finishTile[1])
                        zhedDistance = CalcZhedDistance(valueTile, finishTile, board, false);
                    else continue;

                    //Console.WriteLine("Min zhed distance calculated: {0}", zhedDistance);

                    if (zhedDistance < minZhedDistance) 
                        minZhedDistance = zhedDistance;
                }
            }
            return minZhedDistance;
        }

        private int CalcZhedDistance(int [] valueTile, int[] finishTile, ZhedBoard board, Boolean alignedVertically) {
            int actualDistance = ((alignedVertically) ? Math.Abs(finishTile[1] - valueTile[1]) : Math.Abs(finishTile[0] - valueTile[0]));
            return (actualDistance - valueTile[2] - GetNumUsedTiles(valueTile, finishTile, alignedVertically));
        }

        private int GetNumUsedTiles(int[] valueTile, int[] finishTile, Boolean alignedVertically) {
            int numUsedTiles = 0;
            
            Func<Coords, Coords> moveFunction;
            if (alignedVertically) {
                if (valueTile[1] > finishTile[1]) moveFunction = Coords.MoveUp;
                else moveFunction = Coords.MoveDown;
            }
            else { 
                if (valueTile[0] < finishTile[0]) moveFunction = Coords.MoveRight;
                else moveFunction = Coords.MoveLeft;
            }

            Coords coords = moveFunction(new Coords(valueTile[0], valueTile[1]));
            int tileValue = board.TileValue(coords);
            while (tileValue != ZhedBoard.FINISH_TILE) {

                if (tileValue == ZhedBoard.USED_TILE) {
                    numUsedTiles++; 
                    Console.WriteLine("Num used tiles: {0}", numUsedTiles);
                }

                coords = moveFunction(coords);
                tileValue = board.TileValue(coords);
            }

            if (numUsedTiles != 0) 
                Console.WriteLine("Num used tiles: {0}", numUsedTiles);

            return numUsedTiles;
        }

        public static int Heuristic2(ZhedBoard board) {
            if (board.isOver)
                return 0;
            List<int[]> valueTiles = board.GetValueTiles();
            List<int[]> finishTiles = board.GetFinishTiles();

            int numAligned = 0;
            foreach (int[] finishTile in finishTiles)
                foreach (int[] valueTile in valueTiles)
                    if (new Coords(valueTile[0], valueTile[1]).AlignedWith(new Coords(finishTile[0], finishTile[1])))
                        numAligned++;
            if (numAligned == 0)
                return 9999;
            int a = 1 / numAligned;
            //Console.WriteLine(a);
            return a;
        }

        public int Heuristic3(ZhedBoard board){
            if(board.isOver)
                return 0;
            int a = 1000 / board.getBoardMaxValue();
            //Console.WriteLine(a);
            return a;
        }


        public int Heuristic4(ZhedBoard board){
            if(board.isOver)
                return 0;
            int a = (int)(1000 / board.getBoardTotalMaxValue());
            //Console.WriteLine(a);
            return a;
        }

        public int Heuristic5(ZhedBoard board){
            int a = Heuristic2(board);
            int b = Heuristic4(board);
            //Console.WriteLine(a + " " + b);
            return a+b;
        }

        private List<Node> GetNextGeneration(Node parent, Func<ZhedBoard, int> heuristic) {
            List<Node> nextGeneration = new List<Node>();
            List<Coords> valueTiles = parent.board.GetValueTilesCoords();

            foreach (Coords coords in valueTiles) {
                ZhedBoard up = parent.board.GoUp(coords);
                ZhedBoard down = parent.board.GoDown(coords);
                ZhedBoard left = parent.board.GoLeft(coords);
                ZhedBoard right = parent.board.GoRight(coords);
                nextGeneration.Add(new Node(up, parent, new ZhedStep(Operations.MoveUp, coords), heuristic(up)));
                nextGeneration.Add(new Node(down, parent, new ZhedStep(Operations.MoveDown, coords), heuristic(down)));
                nextGeneration.Add(new Node(left, parent, new ZhedStep(Operations.MoveLeft, coords), heuristic(left)));
                nextGeneration.Add(new Node(right, parent, new ZhedStep(Operations.MoveRight, coords), heuristic(right)));
            }
            return nextGeneration;
        }
        
        private List<ZhedStep> GetPath(Node solutionNode) {
            List<ZhedStep> path = new List<ZhedStep>();
            Node currentNode = solutionNode;
            while (currentNode.parent != null && currentNode.zhedStep != null) { //reached root
                path.Add(currentNode.zhedStep);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }

        public ZhedBoard GetBoard() {
            return this.board;
        }

        public ZhedStep GetHint() {
            var task = Task.Run(() => this.Solve(SearchMethod.Greedy)[0]);
            if (task.Wait(TimeSpan.FromSeconds(1)))
                return task.Result;
            else return null;
        }
    }

    public class Node {
        public ZhedBoard board;
        public Node parent;
        public int height;
        public int value;

        public ZhedStep zhedStep; //Zhed Step that created this node

        public Node(ZhedBoard board, Node parent, ZhedStep zhedStep, int value) {
            this.board = board;
            this.parent = parent;
            this.zhedStep = zhedStep;
            this.value = value;
            this.height = 0;

            while(parent != null) {
                this.height += 1;
                parent = parent.parent;
            } 
        }
    }

    public class ZhedStep {
        public Operations operations;
        public Coords coords;

        public ZhedStep(Operations operations, Coords coords) {
            this.operations = operations;
            this.coords = coords;
        }

        public void Print() {
            Console.WriteLine("Coords({0}, {1}) : {2}.", coords.x, coords.y, operations);
        }
    }
}