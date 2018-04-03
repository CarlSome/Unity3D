using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
    private int[,] GameBoard = new int[3, 3];   //棋盘
    private int Player = 1; //两位玩家分别用1和-1表示
    private int StepCount = 0;  //计步器，用于判断平局
    private bool Flag = false;  //游戏结束时为true,否则为false

    //开局初始化
    void Start()
    {
        Initialize();
    }

    //游戏主体
    void OnGUI()
    {
        GUI.Box(new Rect((Screen.width) / 2 - 150, 20, 300, 290), "Tic-Tac-Toe");

        int result = GameOver();
        if(result == 1)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 100, 200, 200, 50), "O has won! Reset and play again!");
        }
        else if(result == -1)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 100, 200, 200, 50), "X has won! Reset and play again!");
        }
        else if(result == 0)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 110, 200, 220, 50), "End in a draw, reset and play again!");
        }

        if(Player == 1)
        {
            GUI.Label(new Rect((Screen.width) / 2 - 60, 220, 120, 20), "Current Player: O");
        }
        else
        {
            GUI.Label(new Rect((Screen.width) / 2 - 60, 220, 120, 20), "Current Player: X");
        }

        if(GUI.Button(new Rect((Screen.width) / 2 - 50, 250, 100, 40), "Reset"))
        {
            Initialize();
        }

        for (int i=0;i<3;i++)
        {
            for(int ii=0;ii<3;ii++)
            {
                if(GameBoard[i,ii] == 1)
                {
                    GUI.Button(new Rect((Screen.width) / 2 - 75 + i * 50, 50 + ii * 50, 50, 50), "O");
                }
                else if (GameBoard[i, ii] == -1)
                {
                    GUI.Button(new Rect((Screen.width) / 2 - 75 + i * 50, 50 + ii * 50, 50, 50), "X");
                }
                if(GUI.Button(new Rect((Screen.width) / 2 - 75 + i * 50, 50 + ii * 50, 50, 50), ""))
                {
                    if(Flag == false)
                    {
                        StepCount++;
                        GameBoard[i, ii] = Player;
                        Player = -Player;
                    }
                }
            }
        }
    }

    //初始化函数
    void Initialize()
    {
        for(int i=0;i<3;i++)
        {
            for(int ii=0;ii<3;ii++)
            {
                GameBoard[i, ii] = 0;   //未激活的位置为0
            }
        }
        Player = 1;
        StepCount = 0;
        Flag = false;
    }

    //游戏结束判断函数：玩家1胜返回1，玩家-1胜返回-1.平局返回0，未结束返回666
    int GameOver()
    {
        int Sum = 0;
        for(int i=0;i<3;i++)    //横向判断
        {
            if(GameBoard[i,0] == 0) //第一位没被点击，不可能存在
            {
                continue;
            }
            Sum = GameBoard[i, 0] + GameBoard[i, 1] + GameBoard[i, 2];
            if(Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if(Sum == -3)
            {
                Flag = true;
                return -1;
            }
        }
        for(int i=0;i<3;i++)    //纵向判断
        {
            if(GameBoard[0,i] == 0)
            {
                continue;
            }
            Sum = GameBoard[0, i] + GameBoard[1, i] + GameBoard[2, i];
            if (Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if (Sum == -3)
            {
                Flag = true;
                return -1;
            }
        }
        if (GameBoard[1, 1] != 0)   //斜向判断
        {
            Sum = GameBoard[0, 0] + GameBoard[1, 1] + GameBoard[2, 2]; 
            if (Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if (Sum == -3)
            {
                Flag = true;
                return -1;
            }

            Sum = GameBoard[0, 2] + GameBoard[1, 1] + GameBoard[2, 0];
            if (Sum == 3)
            {
                Flag = true;
                return 1;
            }
            else if (Sum == -3)
            {
                Flag = true;
                return -1;
            }
        }
        if(StepCount == 9) //平局
        {
            Flag = true;
            return 0;
        }
        return 666;
    }
}
