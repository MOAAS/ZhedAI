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
            Node root = new Node(this.board, null, null, 1);
            return BFS(root);
        }
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

        private List<Node> GetNextGeneration(Node parent) {
            List<Node> nextGeneration = new List<Node>();
            List<Coords> positiveTiles = parent.board.GetPositiveTiles();

            foreach (Coords coords in positiveTiles) {
                nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveUp, 1));
                nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveDown, 1));
                nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveLeft, 1));
                nextGeneration.Add(CreateNewNode(parent, coords, Operations.MoveRight, 1));
            }
            return nextGeneration;
        }

        private Node CreateNewNode(Node parent, Coords coords, Operations operations, int value) {
            ZhedBoard boardCopy = new ZhedBoard(parent.board);

            switch (operations) {
                case Operations.MoveUp: boardCopy.SpreadTile(coords, Coords.MoveUp); break;
                case Operations.MoveDown: boardCopy.SpreadTile(coords, Coords.MoveDown); break;
                case Operations.MoveLeft: boardCopy.SpreadTile(coords, Coords.MoveLeft); break;
                case Operations.MoveRight: boardCopy.SpreadTile(coords, Coords.MoveRight); break;
            }
            return new Node(boardCopy, parent, new ZhedStep(operations, coords), value);
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

        private List<ZhedStep> DFS() {
            return new List<ZhedStep>();
        }

        private List<ZhedStep> Greedy() {
            return new List<ZhedStep>();
        }

        private List<ZhedStep> Astar() {
            return new List<ZhedStep>();
        }
    }

    class Node {
        public ZhedBoard board;
        public Node parent;
        public int value;
        public int height;

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