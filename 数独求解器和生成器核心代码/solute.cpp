#include <iostream>
#include <ctime>
#include <memory.h>
#include <fstream>
#include <algorithm>
#include <unordered_map>

using namespace std;

#define SOLUTE_NUM 5

int s_board[10][10];
bool s_line[10][10];
bool s_column[10][10];
bool s_block[4][4][10];
bool s_isEmpty[10][10];
int s_empty_num = 40;
bool s_finished = false;
int s_output_num = SOLUTE_NUM;


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
                outFile << board[i][j] << " ";
            }
        }
        outFile << endl;
    }
    outFile << "#" << endl;
    s_output_num--;
    outFile.close();
}

void solute(int(*board)[10], int x, int y) {
    if (s_finished == true) return;
    if (x < 10 && s_isEmpty[x][y] == false) {
        solute(board, (9 * (x - 1) + y) / 9 + 1, y % 9 + 1);
    }
    else {
        if (x == 10 && y == 1) {
            printResult(board, "solution.txt");
            if (s_output_num <= 0) {
                s_finished = true;
            }
        }
        else {
            for (int i = 1; i <= 9; i++) {
                if (s_line[x][i] == false && s_column[y][i] == false && s_block[(x - 1) / 3 + 1][(y - 1) / 3 + 1][i] == false) {
                    s_line[x][i] = s_column[y][i] = s_block[(x - 1) / 3 + 1][(y - 1) / 3 + 1][i] = true;
                    board[x][y] = i;
                    solute(board, (9 * x + y - 9) / 9 + 1, y % 9 + 1);
                    if (s_finished == false) {
                        board[x][y] = 0;
                        s_line[x][i] = s_column[y][i] = s_block[(x - 1) / 3 + 1][(y - 1) / 3 + 1][i] = false;
                    }
                }
            }
        }
    }
}




int main() {
        fstream inFile;
        inFile.open("input.txt", ios::in);
        if (!inFile) {
            printf("open input file failed!\n");
            return -1;
        }

        int cnt = 0;
        while (!inFile.eof()) {
            cnt++;

            char line[19];
            for (int i = 1; i <= 10; i++) {
                inFile.getline(line, 19);

                if (i < 10) {
                    for (int j = 1; j <= 9; j++) {
                        if (line[2 * (j - 1)] == '$') {
                            s_board[i][j] = 0;
                        }
                        else {
                            s_board[i][j] = line[2 * (j - 1)] - '0';
                        }
                    }
                }
            }

			if (line[0] != '#' && line[0] != '$' && (line[0] < '1' || line[0] > '9')) {
				break;
			}

            // init
            s_output_num = SOLUTE_NUM;
            s_finished = false;
            for (int i = 1; i <= 9; i++) {
                for (int j = 1; j <= 9; j++) {
                    s_isEmpty[i][j] = true;
                    s_line[i][j] = s_column[i][j] = false;
                }
            }

            for (int i = 1; i <= 3; i++) {
                for (int j = 1; j <= 3; j++) {
                    for (int k = 1; k <= 9; k++){
                        s_block[i][j][k] = false;
                    }
                }
            }



            for (int i = 1; i <= 9; i++) {
                for (int j = 1; j <= 9; j++) {
                    if (s_board[i][j] != 0) {
                        s_isEmpty[i][j] = false;
                        s_line[i][s_board[i][j]] = s_column[j][s_board[i][j]] = s_block[(i - 1) / 3 + 1][(j - 1) / 3 + 1][s_board[i][j]] = true;
                    }
                }
            }

            ofstream outFile;
            outFile.open("solution.txt", ios::out | ios::app);
            if (!outFile) {
                printf("printResult Failed!");
            }
            outFile << "===============input "<< cnt <<" solutions:==================" << endl;
            outFile.close();

            solute(s_board, 1, 1);
            
        }
        inFile.close();

        return 0;
    }