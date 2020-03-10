// Very Simple Base Code for Connect4 Game written in C
// Author: Luis Paulo Reis
// Date: March, 04, 2020

#include <iostream>
#include <cstdlib> 
#include <algorithm>   

#define HEI 6
#define WID 7

using namespace std;

int INF = std::numeric_limits<int>::max();

//Simple State representation. Only board is needed but other fields are useful.
struct state{
	int board[HEI][WID];
	int lastMoveX, lastMoveY;
	int player;
	int nmoves;
};


//Move is trivial in this game (only col - column where to play). 
struct movem{
	int col;
};
  
//Basic functions to implement a simple C game with Human/Computer game and Minimax 
void draw_state(state st);
void init_state(state &st);
movem get_human_mov(state st);
movem get_pc_rand_mov(state st);
movem get_pc_greedy_mov(state st);
bool valid_movement(state st, movem mov);
state execute_movement(state st, movem mov);
int check_winner(state st);
int evaluate(state st, int pl);

movem get_pc_minimax_mov(state st, int depth);
int max_value(state state, int alfa, int beta, int remaining_depth);
int min_value(state state, int alfa, int beta, int remaining_depth);
movem action_with_val(state state, int depth, int desired_val);



//Main function with very basic game play engine
int main()
{
   state st; movem mov; srand(1);  //srand(Time(NULL));
   init_state(st);
   draw_state(st);
   
   do {
      	if (st.player==1) mov = get_pc_minimax_mov(st, 8); else mov=get_human_mov(st);
      	st = execute_movement(st, mov);
      	draw_state(st);
   } while (check_winner(st)==-1);
   cout << "End of Game! Winner: " << check_winner(st) << endl;
   
   //system("PAUSE"); system("PAUSE");
}

//Initializes the game state to an empty board
void init_state(state &st)
{
	for(int i=0; i<HEI; i++)	
		for(int j=0; j<WID; j++)
			st.board[i][j]=0;
	st.nmoves=0; st.player=1; //st.player=2;
}

//Draws the state in a very simple textual manner, including the current evaluation
void draw_state(state st)
{
   	cout << "| 1 | 2 | 3 | 4 | 5 | 6 | 7 |" << endl;
   	cout << "-----------------------------" << endl;
   	for (int y=HEI-1; y>=0; y--) {
      	for (int x=0; x<WID; x++) {
         	if (st.board[y][x] == 0) cout << "|   ";
         	else if (st.board[y][x] == 1) cout << "| X ";
         	else if (st.board[y][x] == 2) cout << "| O ";
         	if (x==WID-1) cout << "|\n";
      }
      cout << "-----------------------------" << endl;
   }
   cout << "Evaluation: " << evaluate(st, st.player) << endl;
}

//Verifies if a given movement is valid in the current state
bool valid_movement(state st, movem mov)
{
	return (mov.col>=1 && mov.col<= WID && st.board[HEI-1][mov.col-1]==0);
}

//Execute a movement in the given state calculating the new successor state
state execute_movement(state st, movem mov)
{
	int i=0;
   	while (st.board[i][mov.col-1] != 0) i++;
   	st.board[i][mov.col-1]=st.player;
   	st.lastMoveY = i; st.lastMoveX = mov.col-1;
   	st.player= 3-st.player;
   	st.nmoves++;
   	return st;
}

// Get Human Movement from keyboard
movem get_human_mov(state st)
{
   movem mov;  
   do {
   		cout << "\nPlayer" << st.player << ", Please Select Move (1-7): ";
      	cin >> mov.col;
   } while (!valid_movement(st, mov));
   return mov;
}

//Get PC Random Movement
movem get_pc_rand_mov(state st)
{
   movem mov;  
   do {
   		mov.col=rand()%7+1;
   } while (!valid_movement(st, mov));
   return mov;	
}

//Get PC Greedy Movement (wins but also looses game easily)
movem get_pc_greedy_mov(state st)
{	
   movem mov, movfin;
   int aval=-1000; 
   state st2; 
   for(mov.col=0; mov.col<WID; mov.col++){
   		if (valid_movement(st, mov)) {
   			st2 = execute_movement(st,mov);
			if (evaluate(st2, st.player)>aval) {aval=evaluate(st2, st.player); movfin=mov;}
		}  	
	}	   
   	return movfin;	
}

//Get PC Minimax Movement (TODO)
movem get_pc_minimax_mov(state st, int depth)
{	
	
	int value = max_value(st, -INF, INF, depth);

	cout << value << " is value " << endl;

	return action_with_val(st, depth, value);	
}

movem action_with_val(state state, int depth, int desired_val) {
	int alfa = -INF, beta = INF, value = -INF;
	movem move;
	for(move.col = 0; move.col < WID; move.col++){
   		if (valid_movement(state, move)) { 
			value = max(value, min_value(execute_movement(state, move), alfa, beta, depth - 1));			
			if (value == desired_val)
				return move;

			alfa = max(alfa, value); 
		}  	
	}	

	cout << "YEET YEET YEET YEET" << endl;
	cout << "YEET YEET YEET YEET" << endl;
	cout << "YEET YEET YEET YEET" << endl;
	cout << "YEET YEET YEET YEET" << endl;
	cout << "YEET YEET YEET YEET" << endl;

	return move;
}

int max_value(state state, int alfa, int beta, int remaining_depth) {
	if (check_winner(state) != -1 || remaining_depth == 0) return evaluate(state, 1);

	int value = -INF;
	movem move;
	for(move.col = 0; move.col < WID; move.col++){
   		if (valid_movement(state, move)) { 
			value = max(value, min_value(execute_movement(state, move), alfa, beta, remaining_depth - 1)); // 3
			//cout << value << endl;
			if (value >= beta)
				return value;
			alfa = max(alfa, value); 
		}  	
	}	

	return value;
}

int min_value(state state, int alfa, int beta, int remaining_depth) {
	if (check_winner(state) != -1 || remaining_depth == 0) return evaluate(state, 1);

	int value = INF;
	movem move;
	for(move.col = 0; move.col < WID; move.col++){
   		if (valid_movement(state, move)) { 
			value = min(value, max_value(execute_movement(state, move), alfa, beta, remaining_depth - 1)); 
			if (value <= alfa)
				return value;
			beta = min(beta, value); 
		}  	
	}	

	return value;
}

// Returns if four consecutive positions have a line or 3 in a line menace
bool count(int n, int pla, int p1, int p2, int p3, int p4)
{
	int pec = (p1==pla) + (p2==pla) + (p3==pla) + (p4==pla);
	if (n==4) return pec==4;
	int vaz = (p1==0) + (p2==0) + (p3==0) + (p4==0);
	if (n==3) return pec==3 && vaz==1;

	return false;
}

//calculates lines of 4 or lines of 3 with an empty cell for horizontal, vertical and diagonal
int line(int n, state st, int pl)
{
	int lin=0;
    for(int i=0; i<HEI; i++)
        for(int j=0; j<WID; j++) {
			if(j<WID-3 && count(n, pl, st.board[i][j],st.board[i][j+1],st.board[i][j+2],st.board[i][j+3])) 
				lin++;
			if(i<HEI-3 && count(n, pl, st.board[i][j],st.board[i+1][j],st.board[i+2][j],st.board[i+3][j]))
				lin++;
			if(j<WID-3 && i<HEI-3 && 
				count(n, pl, st.board[i][j],st.board[i+1][j+1],st.board[i+2][j+2],st.board[i+3][j+3])) 
				lin++;
        	if(j>3 && i<HEI-3 && 
				count(n, pl, st.board[i][j],st.board[i+1][j-1],st.board[i+2][j-2],st.board[i+3][j-3]))
				lin++;
    }
    return lin;
}

// Checks if there is any winner (1,2) or draw (0) or returns -1
int check_winner(state st)
{
   	if (line(4, st, 1)>0) return 1;
   	if (line(4, st, 2)>0) return 2;
   	if (st.nmoves==42) return 0;
  	return -1;
}

// Board very simple positional value: 2 points for pieces in column 3, 1 point for column 2 or 4
int posit(state st, int pl)
{
	int val=0;
    for(int i=0; i<HEI; i++)
        for(int j=0; j<WID; j++) 
			if (st.board[i][j]==pl)
				if (j==3) val+=2; else if (j==2 || j==4) val++;	
    return val;
}

// Evaluation function combining winning, 3 in a row menaces and positional value
int evaluate(state st, int pl)
{
	int pos = posit(st,pl) - posit(st,3-pl);
	int lin3 = line(3, st, pl) - line(3, st, 3-pl);
	int lin4 = line(4, st, pl) - line(4, st, 3-pl);
	int val = lin4*100 + lin3*5 + pos;
	//cout << "Val: " << val << "  = Pos: " << pos << "  Lin3: " << lin3 << "  Lin4: " << lin4 << endl;
	return val;
}
