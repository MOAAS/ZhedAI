from copy import copy, deepcopy

state = [[1,3,2], [4,6,5], [0,8,7]]
final_state = [[1,2,3], [4,5,6], [7,8,0]]

print(final_state)




def canMoveLeft(state):
  return findZero(state)[0] != 0

def canMoveRight(state):
  return findZero(state)[0] != 2

def canMoveUp(state):
  return findZero(state)[1] != 0

def canMoveDown(state):
  return findZero(state)[1] != 2

def moveLeft(state):
  zeroPos = findZero(state)
  leftPos = [zeroPos[0] - 1, zeroPos[1]]
  return switchPositions(state, zeroPos, leftPos)

def moveRight(state):
  zeroPos = findZero(state)
  leftPos = [zeroPos[0] + 1, zeroPos[1]]
  return switchPositions(state, zeroPos, leftPos)

def moveUp(state):
  zeroPos = findZero(state)
  leftPos = [zeroPos[0], zeroPos[1] - 1]
  return switchPositions(state, zeroPos, leftPos)

def moveDown(state):
  zeroPos = findZero(state)
  leftPos = [zeroPos[0], zeroPos[1] + 1]
  return switchPositions(state, zeroPos, leftPos)

def switchPositions(state, pos1, pos2):
  item1 = state[pos1[1]][pos1[0]]
  item2 = state[pos2[1]][pos2[0]]

  newState = deepcopy(state)
  newState[pos1[1]][pos1[0]] = item2
  newState[pos2[1]][pos2[0]] = item1
  return newState

def findZero(state):
  for y in range(len(state)):
    for x in range(len(state[y])):
      if (state[y][x] == 0):
        return [x, y]
  return 0

print(state)
print(switchPositions(state, [0,0], [1,1]))
print(switchPositions(state, [0,0], [1,2]))


def breadth_first(state, list):
    moves = [moveUp(state), moveDown(state), moveLeft(state), moveRight(state)]
    for move in moves:
        if (acabado(move)):
            return listademovesateagora

  

    


