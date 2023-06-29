#include <iostream>
#include <ctime>
#include <memory.h>
#include <fstream>
#include <algorithm>
#include <unordered_map>
using namespace std;

#define RESULT_NUM 5

int board[10][10];
bool line[10][10];
bool column[10][10];
bool block[4][4][10];
bool isEmpty[10][10];
int game[10][10];
int empty_num = 40;
int game_num = 10; //10 default
bool finished = false;
int res_num = 0;
int output_num = RESULT_NUM;

unordered_map<char, bool> options{ 
    {'-c', false}, 
	{'-s', false},  
	{'-n', false},  
	{'-m', false}, 
	{'-r', false},  
	{'-u', false}, 
};


void printResult(int(*board)[10], string filePath) {
    ofstream outFile;
    outFile.open(filePath, ios::out | ios::app);
    if (!outFile) {
        printf("printResult Failed!");
    }

    for (int i = 1; i <= 9; i++) {
        for (int j = 1; j <= 9; j++) {
            if (board[i][j] == 0) {
                outFile << "$ ";
            }
            else {
                outFile << board[i][j] <<" ";
            }
        }
        outFile << endl;
    }
    outFile <<"#"<<endl;
    output_num--;
    outFile.close();
}

void dfs(int(*board)[10], int x, int y) {
    if (finished == true) return;
    if (x < 10 && isEmpty[x][y] == false) {
        dfs(board, (9 * (x - 1) + y) / 9 + 1, y % 9 + 1);
    }
    else {
        if (x == 10 && y == 1) {
            printResult(board, "input_answer.txt");
            if (output_num <= 0) {
                finished = true;
            }
        }
        else {
            for (int i = 1; i <= 9; i++) {
                if (line[x][i] == false && column[y][i] == false && block[(x - 1) / 3 + 1][(y - 1) / 3 + 1][i] == false) { 
                    line[x][i] = column[y][i] = block[(x - 1) / 3 + 1][(y - 1) / 3 + 1][i] = true;
                    board[x][y] = i;
                    dfs(board, (9 * x + y - 9) / 9 + 1, y % 9 + 1);
                    if (finished == false) {
                        board[x][y] = 0;
                        line[x][i] = column[y][i] = block[(x - 1) / 3 + 1][(y - 1) / 3 + 1][i] = false;
                    }
                }
            }
        }
    }
}



void generate() {
    do {
        for (int i = 1; i <= 3; i++) {
            int rand_digits[9] = { 1,2,3,4,5,6,7,8,9 };
            random_shuffle(rand_digits, rand_digits + 9);
            int pos = 0;
            for (int j = 1 + ((i - 1) * 3); j <= 3 + ((i - 1) * 3); j++) {
                for (int k = 1 + ((i - 1) * 3); k <= 3 + ((i - 1) * 3); k++) {
                    board[j][k] = rand_digits[pos++];
                    line[j][board[j][k]] = column[k][board[j][k]] = block[(j - 1) / 3 + 1][(k - 1) / 3 + 1][board[j][k]] = true;
                    isEmpty[j][k] = false;
                }
            }
        }
        dfs(board, 1, 1);
    } while (board[9][6] == 0);
}



int main() {
    srand(time(0));
    // generate a game board
    for (int m = 0; m < game_num; m++) {
        // init
        output_num = RESULT_NUM;
        res_num = 0;
        finished = false;
        for (int i = 1; i <= 9; i++) {
            for (int j = 1; j <= 9; j++) {
                line[i][j] = false;
                column[i][j] = false;
                board[i][j] = 0;
                isEmpty[i][j] = true;
            }
        }

        for (int i = 1; i <= 3; i++) {
            for (int j = 1; j <= 3; j++) {
                for (int k = 1; k <= 9; k++) {
                    block[i][j][k] = false;
                }
            }
        }



        generate();

        int generated_board[10][10] = { 0 };
        for (int i = 1; i <= 9; i++) {
            for (int j = 1; j <= 9; j++) {
                game[i][j] = generated_board[i][j] = board[i][j];
            }
        }
        res_num = 0;
        for (int i = 1; i <= 9; i++) {
            for (int j = 1; j <= 9; j++) {
                isEmpty[i][j] = false;
            }
        }

        // set empty
        for (int i = 0; i < empty_num; i++) {
            int x = rand() % 9 + 1;
            int y = rand() % 9 + 1;
            if (isEmpty[x][y] == true)
                continue;
            else {
                isEmpty[x][y] = true;
            }
        }

        for (int i = 1; i <= 9; i++) {
            for (int j = 1; j <= 9; j++) {
                if (isEmpty[i][j] == true) {
                    game[i][j] = board[i][j] = 0;
                }
            }
        }




        // only one solution option
        if (options['-u'] == true) {
            // TODO:

        }

        // generate input file
        printResult(game, "input.txt");
    }
    return 0;
}