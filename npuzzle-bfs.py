from copy import copy, deepcopy


class State:
    def __init__(self, matrix, x, y):
        self.matrix = matrix
        self.x = x
        self.y = y

class Node:
  def __init__(self, state, parent, dist):
    self.state = state
    self.parent = parent
    self.dist = dist
    self.height = 0
    while parent:
      self.height += 1
      parent = parent.parent

initial_state = State([    
    [5,1,3,4], 
    [2,0,7,8], 
    [10,6,11,12],
    [9,13,14,15],
], 1, 1)

final_state_matrix = [[1,2,3,4], [5,6,7,8], [9,10,11,12],[13,14,15,0]]

"""
initial_state = State([    
    [1,2,3], 
    [0,5,6], 
    [4,7,8]
], 0, 1)
final_state_matrix = [[1,2,3], [4,5,6], [7,8,0]]
"""

puzzle_size = len(initial_state.matrix)


def canMoveLeft(state):
  return state.x != 0

def canMoveRight(state):
  return state.x != puzzle_size - 1

def canMoveUp(state):
  return state.y != 0

def canMoveDown(state):
  return state.y != puzzle_size - 1

def moveLeft(state):
  return switchPositions(state.matrix, (state.x, state.y), (state.x - 1, state.y))

def moveRight(state):
  return switchPositions(state.matrix, (state.x, state.y), (state.x + 1, state.y))

def moveUp(state):
  return switchPositions(state.matrix, (state.x, state.y), (state.x, state.y - 1))

def moveDown(state):
  return switchPositions(state.matrix, (state.x, state.y), (state.x, state.y + 1))

def switchPositions(stateMatrix, posFrom, posTo):
  x1,y1 = posFrom
  x2,y2 = posTo
  newStateMatrix = deepcopy(stateMatrix)

  newStateMatrix[y1][x1] = stateMatrix[y2][x2]
  newStateMatrix[y2][x2] = stateMatrix[y1][x1]
  return State(newStateMatrix, x2, y2)

def printMatrix(state):
    for row in state:
        for element in row:
            print(element, end=' ')
        print("")
    print("")

def getPossibleMoves(state):
    moves = []
    if canMoveLeft(state): moves.append(moveLeft(state))
    if canMoveRight(state): moves.append(moveRight(state)) 
    if canMoveUp(state): moves.append(moveUp(state)) 
    if canMoveDown(state): moves.append(moveDown(state)) 
    return moves
    
  
def solve_search(state):
    queue = [Node(state, None, foraDoSitio(state))]
        
    while queue:
        ### Depending on solving method, determine expanded node

        # BFS
        # node = queue.pop(0)

        # Greedy Fora do Sitio
        # greediestIndex = 0
        # greediestValue = queue[0].dist
        # for i in range(len(queue)):
        #     if queue[i].dist < greediestValue: 
        #         greediestIndex = i
        #         greediestValue = queue[i].dist
        # node = queue.pop(greediestIndex)
        

        # A* Fora do Sitio
        greediestIndex = 0
        greediestValue = queue[0].dist + queue[0].height
        for i in range(len(queue)):
            if queue[i].dist + queue[i].height < greediestValue: 
                greediestIndex = i
                greediestValue = queue[i].dist + queue[i].height
        node = queue.pop(greediestIndex)

        # Check if final state
        if (node.state.matrix == final_state_matrix):
            return node

        # Expand node
        states = getPossibleMoves(node.state)
        for state in states:
            queue.append(Node(state, node, foraDoSitio(state)))


def foraDoSitio(state):
    numForaDoSitio = 0
    for y in range(puzzle_size):
      for x in range(puzzle_size):
        if (state.matrix[y][x] != final_state_matrix[y][x]):
            numForaDoSitio += 1
    return numForaDoSitio


print("Search")
node = solve_search(initial_state)
print("End Search")

print("Result")
while node:
    printMatrix(node.state.matrix)
    node = node.parent
