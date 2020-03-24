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
        Astar
    }

    class Solver {
        private ZhedBoard board;

        public Solver(ZhedBoard board) {
            this.board = board;
        }

        public List<ZhedStep> Solve(SearchMethod searchMethod) {
            Func<ZhedBoard, int> heuristic = Heuristic3;


            PriorityQueue<Node> queue = new PriorityQueue<Node>();
            queue.Enqueue(new Node(this.board, null, null, 1), 1);

            DFSPriority = int.MaxValue;
            int visitedNodes = 0;
            while(queue.Count > 0) {
                visitedNodes++;
                Node nextNode = queue.Dequeue();

                Console.WriteLine("Visit number {0}", visitedNodes); nextNode.board.PrintBoard();
                Console.WriteLine("Value of board: {0}" , nextNode.board.getBoardMaxValue());

                if (nextNode.board.isOver) {
                    Console.WriteLine("Visited {0} nodes", visitedNodes);
                    return GetPath(nextNode);
                }
                List<Node> children = GetNextGeneration(nextNode, heuristic);
                foreach(Node node in children)
                    queue.Enqueue(node, NodePriority(searchMethod, node));
            }
            return null;

           // return BFS(root);
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

        public int Heuristic2(ZhedBoard board) {
            if (board.isOver)
                return 0;
            List<int[]> valueTiles = board.GetValueTiles2();
            List<int[]> finishTiles = board.GetFinishTiles();

            int numberOfTilesOnTheZhedBoardThatNotOnlyHaveANaturalNumberOnThemButAlsoHappenToBeAlignedThatIsOnTheSameRowOrColumnWithAFinishTile = 0;
            foreach (int[] finishTile in finishTiles)
                foreach (int[] valueTile in valueTiles)
                    if (new Coords(valueTile[0], valueTile[1]).AlignedWith(new Coords(finishTile[0], finishTile[1])))
                        numberOfTilesOnTheZhedBoardThatNotOnlyHaveANaturalNumberOnThemButAlsoHappenToBeAlignedThatIsOnTheSameRowOrColumnWithAFinishTile++;
            if (numberOfTilesOnTheZhedBoardThatNotOnlyHaveANaturalNumberOnThemButAlsoHappenToBeAlignedThatIsOnTheSameRowOrColumnWithAFinishTile == 0)
                return int.MaxValue;
            return 1 / numberOfTilesOnTheZhedBoardThatNotOnlyHaveANaturalNumberOnThemButAlsoHappenToBeAlignedThatIsOnTheSameRowOrColumnWithAFinishTile;
        }

        public int Heuristic3(ZhedBoard board){
            if(board.isOver)
                return 0;
            return 1 / board.getBoardMaxValue();
        }

        /*

        public Node NextToExpand(PriorityQueue<Node> queue, SearchMethod method) {
            return queue.Dequeue();
            Node node = null;
            int bestValue = int.MaxValue;
            switch (method) {
                case SearchMethod.BFS: return queue.Dequeue(); break;
               // case SearchMethod.DFS: node = queue.Last.Value; queue.RemoveLast(); break;
               // case SearchMethod.Greedy: 
                    foreach (Node item in queue) {
                        if (item.value < bestValue) {
                            node = item;
                            bestValue = item.value;
                        }
                    }
                  //  queue.Remove(node);
                  //  return node;
                case SearchMethod.Astar: 
                    foreach (Node item in queue) {
                        if (item.value + item.height < bestValue) {
                            node = item;
                            bestValue = item.value + item.height;
                        }
                    }
                    queue.Remove(node);
                    return node;
            }
            return node;
        }
            */


/*
        private Node CreateNewNode(Node parent, Coords coords, Operations operations, int value) {
            ZhedBoard boardCopy = new ZhedBoard(parent.board);

            switch (operations) {
                case Operations.MoveUp: boardCopy.GoUp(coords); break;
                case Operations.MoveDown: boardCopy.GoDown(coords); break;
                case Operations.MoveLeft: boardCopy.GoLeft(coords); break;
                case Operations.MoveRight: boardCopy.GoRight(coords); break;
            }
            return new Node(boardCopy, parent, new ZhedStep(operations, coords), value);
        }
        */ 

        private List<Node> GetNextGeneration(Node parent, Func<ZhedBoard, int> heuristic) {
            List<Node> nextGeneration = new List<Node>();
            List<Coords> valueTiles = parent.board.GetValueTiles();

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

/*
        private List<ZhedStep> BFS(Node root) {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);
            while(queue.Count > 0) {
                Node nextNode = queue.Dequeue();
                if (nextNode.board.isOver)
                    return GetPath(nextNode);
                List<Node> children = GetNextGeneration(nextNode);
                foreach(Node node in children)
                    queue.Enqueue(node);
            }
            return null;
        }
        */
/*
        private List<ZhedStep> DFS() {
            return new List<ZhedStep>();
        }

        private List<ZhedStep> Greedy() {
            return new List<ZhedStep>();
        }

        private List<ZhedStep> Astar() {
            return new List<ZhedStep>();
        }
        */

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

    class Node {
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

/*             while(parent != null) {
                this.height += 1;
                parent = parent.parent;
            } */
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
            Console.Write("Coords[" + coords.x + ":" + coords.y + "] : ");
            switch (operations) {
                case Operations.MoveUp: Console.Write("Move Up\n"); break;
                case Operations.MoveDown: Console.Write("Move Down\n"); break;
                case Operations.MoveLeft: Console.Write("Move Left\n"); break;
                case Operations.MoveRight: Console.Write("Move Right\n"); break;
            }
        }
    }
}