from copy import copy, deepcopy


class State:
    def __init__(self, matrix, x, y):
        self.matrix = matrix
        self.x = x
        self.y = y

class Node:
  def __init__(self, state, parent):
    self.state = state
    self.parent = parent

initial_state = State([    
    [5,1,3,4], 
    [2,0,7,8], 
    [10,6,11,12],
    [9,13,14,15],
], 1, 1)

final_state_matrix = [[1,2,3,4], [5,6,7,8], [9,10,11,12],[13,14,15,0]]

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
    
def breadth_first(state):
    queue = [Node(state, None)]
        
    while queue:
        node = queue.pop(0)

        if (node.state.matrix == final_state_matrix):
            return node

        moves = getPossibleMoves(node.state)
        for move in moves:
            queue.append(Node(move, node))

print("BREADTH")
node = breadth_first(initial_state)

print("Result")
while node:
    printMatrix(node.state.matrix)
    node = node.parent
print("END BREADTH")
