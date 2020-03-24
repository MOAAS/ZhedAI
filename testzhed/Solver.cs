using System;
using System.Collections.Generic;

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
            Func<ZhedBoard, int> heuristic = (ZhedBoard) => {
                return 1;
            };

            LinkedList<Node> queue = new LinkedList<Node>();

            queue.AddLast(new Node(this.board, null, null, 1));

            while(queue.Count > 0) {
                            foreach (var item in queue)
            {
                Console.WriteLine(item.value);
            }
                Node nextNode = NextToExpand(queue, searchMethod);
                if (nextNode.board.isOver)
                    return GetPath(nextNode);
                List<Node> children = GetNextGeneration(nextNode, heuristic);
                foreach(Node node in children)
                    queue.AddLast(node);
            }
            return null;

           // return BFS(root);
        }

        public Node NextToExpand(LinkedList<Node> queue, SearchMethod method) {
            Node node;
            switch (method) {
                case SearchMethod.BFS: node = queue.First.Value; queue.RemoveFirst(); break;
                case SearchMethod.DFS: node = queue.Last.Value; queue.RemoveLast(); break;
                case SearchMethod.Greedy: return null;


                default: return null;

            }
            return node;
        }


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
            List<Coords> positiveTiles = parent.board.GetPositiveTiles();

            foreach (Coords coords in positiveTiles) {
                nextGeneration.Add(new Node(parent.board.GoUp(coords), parent, new ZhedStep(Operations.MoveUp, coords), heuristic(parent.board)));
                nextGeneration.Add(new Node(parent.board.GoDown(coords), parent, new ZhedStep(Operations.MoveDown, coords), heuristic(parent.board)));
                nextGeneration.Add(new Node(parent.board.GoLeft(coords), parent, new ZhedStep(Operations.MoveLeft, coords), heuristic(parent.board)));
                nextGeneration.Add(new Node(parent.board.GoRight(coords), parent, new ZhedStep(Operations.MoveRight, coords), heuristic(parent.board)));
               // nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveUp, 1));
               // nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveDown, 1));
               // nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveLeft, 1));
               // nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveRight, 1));
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

        private List<ZhedStep> DFS() {
            return new List<ZhedStep>();
        }

        private List<ZhedStep> Greedy() {
            return new List<ZhedStep>();
        }

        private List<ZhedStep> Astar() {
            return new List<ZhedStep>();
        }

        public ZhedBoard GetBoard() {
            return this.board;
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