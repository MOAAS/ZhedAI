import random
import math

inscricoes = [
    [1,2,3,4,5],
    [6,7,8,9],
    [10,11,12],
    [1,2,3,4],
    [5,6,7,8],
    [9,10,11,12],
    [1,2,3,5],
    [6,7,8],
    [4,9,10,11,12],
    [1,2,4,5],
    [3,6,7,8],
    [9,10,11,12]
]

numSlots = 4

numDisciplinas = len(inscricoes) # 4
numAlunos = 12 # 12

def evaluate(result : list) -> int:
    alunos = [[] for i in range(numAlunos)]
    for disciplina in range(len(result)):
        slot = result[disciplina] # slot = 4  disciplina = 0
        for inscricao in inscricoes[disciplina]: # inscricao = 1
            alunos[inscricao - 1].append(slot)
            
    numOverlappings = 0
    for aluno in alunos:
        if len(aluno) != len(set(aluno)):
            numOverlappings += 1
    return numOverlappings


def makeInitial():
    return [random.randint(1, numSlots) for i in range(numAlunos)]
    return [1,1,1,1,1,1,1,1,1,1,1,1]
    return [4,1,2,3,2,4,1,1,2,1,2,3]
    return [1,1,1,2,2,2,3,3,3,4,4,1]
    return [1,2,3,4,1,2,3,4,1,2,3,4]
    return [1,1,4,2,2,2,3,3,3,4,4,1]

def printState(state):
    print(str(state) + " : " + str(evaluate(state)))


##### Hill climbing #####

def hillClimbing():
    current = makeInitial()
    while True:
        #printState(current)
        neighbor = bestSuccessor(current)

        if evaluate(neighbor) >= evaluate(current):
            return current
        current = neighbor


def bestSuccessor(current):
    minValue = numAlunos + 1
    bestSolution = []
    for i, slot in enumerate(current):
        for newSlot in range(1, numSlots + 1):
            if newSlot != slot:
                possibleSolution = current.copy()
                possibleSolution[i] = newSlot
                possibleValue = evaluate(possibleSolution)
                if possibleValue < minValue:
                    minValue = possibleValue
                    bestSolution = possibleSolution
    return bestSolution

##### Sumulated Annealing #####

def simulatedAnnealing(maxSteps):
    current = makeInitial()
    for step in range(maxSteps):
        #printState(current)
        temp = temperature(step / float(maxSteps))   
        nextState = randomSuccessor(current)
        diff = evaluate(current) - evaluate(nextState)
        if diff > 0 or math.exp(diff / temp) > random.random():
            current = nextState
    return current

def temperature(fraction):
    return max(0.01, min(1, 1 - fraction))

def randomSuccessor(current):
    index = random.randint(0, len(current) - 1)

    choices = [i for i in range(1, numSlots + 1)]
    choices.remove(current[index])
    choice = random.choice(choices)
    
    successor = current.copy()    
    successor[index] = choice
    return successor

#print(evaluate([4,1,2,3,2,4,1,1,2,1,2,3]))

printState(hillClimbing())
printState(simulatedAnnealing(100000))
#bestSuccessor([4,1,2,3,2,4,1,1,2,1,2,3])

