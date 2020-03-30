using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZhedSolver
{
    enum Operations {
        MoveUp, 
        MoveDown, 
        MoveLeft,
        MoveRight
    }

    enum SearchMethod {
        BFS,
        DFS,
        Greedy,
        Astar,
        Uniform,
    }

    class Solver {
        private ZhedBoard board;

        public Solver(ZhedBoard board) {
            this.board = board;
        }

        /* Main solver function

            Solves a zhed puzzle with a provided Search method.
            Returns a list of steps, which contain coordinates and operators.
        */
        public List<ZhedStep> Solve(SearchMethod searchMethod) {
            Func<ZhedBoard, int> heuristic = Heuristic5;
            if (searchMethod != SearchMethod.Greedy && searchMethod != SearchMethod.Astar)
                heuristic = Heuristic0;

            PriorityQueue<Node> queue = new PriorityQueue<Node>();
            Node root = new Node(this.board, null, null, 1, 0);
            queue.Enqueue(root, NodePriority(searchMethod, root));

            DFSPriority = int.MaxValue;
            int visitedNodes = 0;
            while(queue.Count > 0) {
                visitedNodes++;
                Node nextNode = queue.Dequeue();

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

        /* Gets node priority

            Depending on the search method the result may be different for same node.
            Lower result means higher priority.
        */
        public int NodePriority(SearchMethod method, Node node) {
            DFSPriority--;
            switch (method) {
                case SearchMethod.BFS: return node.height;
                case SearchMethod.DFS: return DFSPriority; 
                case SearchMethod.Greedy: return node.value;
                case SearchMethod.Astar: return node.value + node.cost;
                case SearchMethod.Uniform: return node.cost;
                default: return 1;
            }
        }

        // No heuristic
        public int Heuristic0(ZhedBoard board) {
            return 1;
        }


        /* Heuristic 1
            Minimum zhed Distance between a value tile and a finish tile.
        */
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

                    if (zhedDistance < minZhedDistance) 
                        minZhedDistance = zhedDistance;
                }
            }
            return minZhedDistance;
        }

        /* Heuristic 1 auxiliary
            Calculates Zhed distance between value tile and finish tile
        */
        private int CalcZhedDistance(int [] valueTile, int[] finishTile, ZhedBoard board, Boolean alignedVertically) {
            int actualDistance = ((alignedVertically) ? Math.Abs(finishTile[1] - valueTile[1]) : Math.Abs(finishTile[0] - valueTile[0]));
            return (actualDistance - valueTile[2] - GetNumUsedTiles(valueTile, finishTile, alignedVertically));
        }

        /* Heuristic 1 auxiliary
            Calculates number of used tiles between a value tile and finish tile
        */
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

        /* Heuristic 2
            1 / Number of cells aligned with finish tile
        */
        public int Heuristic2(ZhedBoard board) {
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

        /* Heuristic 3
            1 / Sum of the number of tiles that can be covered with the current layout
        */
        public int Heuristic3(ZhedBoard board){
            if(board.isOver)
                return 0;
            return 1000 / board.getBoardMaxValue();
        }

        /* Heuristic 4
            Similar to heuristic 3, but takes into account the average in all 4 directions on each tile
        */
        public int Heuristic4(ZhedBoard board){
            if(board.isOver)
                return 0;
            return (int)(1000 / board.getBoardTotalMaxValue());
        }

        /* Heuristic 5
            Combines heuristic 3 and 4
        */
        public int Heuristic5(ZhedBoard board){
            return Heuristic2(board) + Heuristic4(board);
        }

        /* Cost function
            Simply assumes that expanding a tile is more beneficial than not expanding it
        */
        private int Cost(ZhedBoard board, Coords coords) {
            return -30;
        }

        /* Node expansion
            Expands every tile in every direction, creating a new node for each resulting board.
            The nodes will contain information about the cost and heuristic value.
            Returns the node list.
        */
        private List<Node> GetNextGeneration(Node parent, Func<ZhedBoard, int> heuristic) {
            List<Node> nextGeneration = new List<Node>();
            List<Coords> valueTiles = parent.board.GetValueTilesCoords();

            foreach (Coords coords in valueTiles) {
                int cost = Cost(parent.board, coords);
                ZhedBoard up = parent.board.GoUp(coords);
                ZhedBoard down = parent.board.GoDown(coords);
                ZhedBoard left = parent.board.GoLeft(coords);
                ZhedBoard right = parent.board.GoRight(coords);
                nextGeneration.Add(new Node(up, parent, new ZhedStep(Operations.MoveUp, coords), heuristic(up), cost));
                nextGeneration.Add(new Node(down, parent, new ZhedStep(Operations.MoveDown, coords), heuristic(down), cost));
                nextGeneration.Add(new Node(left, parent, new ZhedStep(Operations.MoveLeft, coords), heuristic(left), cost));
                nextGeneration.Add(new Node(right, parent, new ZhedStep(Operations.MoveRight, coords), heuristic(right), cost));
            }
            return nextGeneration;
        }


        /* Gets total path to a solution node
            Goes from the node to the root building a list of the moves.
            Reverses the path in the end so it's in order.
        */       
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

        /* Gets a hint from the computer
            After 3 seconds it stops looking for one and returns null.
        */        

        public ZhedStep GetHint() {
            var task = Task.Run(() => this.Solve(SearchMethod.Greedy)[0]);
            if (task.Wait(TimeSpan.FromSeconds(3)))
                return task.Result;
            else return null;
        }
    }

    class Node {
        public ZhedBoard board;
        public Node parent;
        public int height;
        public int value;
        public int cost;

        public ZhedStep zhedStep; //Zhed Step that created this node

        public Node(ZhedBoard board, Node parent, ZhedStep zhedStep, int value, int cost) {
            this.board = board;
            this.parent = parent;
            this.zhedStep = zhedStep;
            this.value = value;
            if (parent == null)
                this.cost = cost;
            else this.cost = parent.cost + cost;

            while(parent != null) {
                this.height += 1;
                parent = parent.parent;
            } 
        }
    }

    class ZhedStep {
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