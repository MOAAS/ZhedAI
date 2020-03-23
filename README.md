# ZHED - Single Player Game​

## Description

-  ZHED is a grid based puzzle game where in order to complete each level a numbered cell must be expanded to reach the goal cell.​

- Each numbered cell can be expanded in one of four directions and overlapped. Each cell expands n cells in the direction chosen, decreasing by one for each empty cell. When a expanding cell overlaps an already filled cell, the number of cells to be filled in the direction of the expansion is not decreased.

---

## Related Work​

https://www.wilgysef.com/articles/zhed-solver/

https://github.com/WiLGYSeF/zhed-solver

https://www.cin.ufpe.br/~if684/EC/aulas-IASimbolica/korf96-search.pdf

http://archive.oreilly.com/oreillyschool/courses/data-structures-algorithms/singlePlayer.html

http://www.pvv.ntnu.no/~spaans/spec-cs.pdf

---

## Problem Formulation​

For each level, the puzzle size and the numbered cells' positions are diferent. Therefore we are using as an example for the formulation a puzzle size of 4.​

- State Representation: NxN Matrix (List of Lists of integers, N = Puzzle Size), where each cell can have a value, Val, of: ​

    - A positive number, representing the expandable length of the cell;​

    - 0, representing an empty cell;​

    - -1, representing an expanded cell;​

    - -2, representing the goal cell.​

    - -3, representing the reached goal cell.  ​

---

## Operators

| **Operator** | **Pre-conditions**​ | 
| --- | --- | --- | --- |
| Move [X, Y] Up | Board(X, Y) > 0 | 
| Move [X,Y] Down | Board(X,Y) > 0​ |
| Move [X,Y] Left | Board(X,Y) > 0 |
| Move [X,Y] Right | Board(X,Y) > 0 |

---

## Heuristics

- We plan on implementing several search algorithms, such as breadth-first, depth-first, greedy, A*, and comparing the results we achieve.

- ​For the heuristic methods (greedy, A*), we will try different heuristics, such as: ​

    - H1 = Minimum Zhed Distance between a Value Cell and a Finish Tile. ​

    - H2 = (1 - Number of Reached Finish Tiles)  / (Number of tiles aligned with a Finish Tile)​

    - H3 = (1 - Number of Reached Finish Tiles)  / Sum(Maximum Tile Reach)​

    - Hx = A combination of previous heuristics​

The Zhed Distance between a Value Cell and a Tile, only applicable when they are in the same row or column, consists of:​

- The actual distance between them 
- The number of used tiles between them 
- The Cell's Value ​