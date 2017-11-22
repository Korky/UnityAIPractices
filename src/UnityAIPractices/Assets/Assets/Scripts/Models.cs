﻿//System libraries
using UnityEngine;
using System.Collections.Generic;

//Custom libraries
using Enums;

namespace Models
{
    
    public class Player
    {
        public PlayerIndex Index;
        public PlayerType Type;
    }
    public class Board
    {
        public BoardOption[,] BoardData;

        public void Init()
        {
            BoardData = new BoardOption[3, 3];
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    BoardData[y, x] = BoardOption.NO_VAL;
                    
                }
              
            }
            
        }

        public bool PlaceMove(int x, int y, BoardOption opt)
        {
            
            Debug.Log("Model Cord:" + x + ", " + y);
            if (BoardData[y,x] == BoardOption.NO_VAL) {
                BoardData[y, x] = opt;
                printBoardDebug();
                
                
                return true;
            }
           
            return false;
        }

        public GameOver CheckGameOver()
        {
            GameOver ret;
            ret = checkAllRows();
            if (ret != GameOver.IDLE) return ret;
            
            ret = checkAllDiagonals();
            if (ret != GameOver.IDLE) return ret;

            if (checkForEmpty() == GameState.GAMEOVER) return GameOver.TIE;

            return GameOver.IDLE;
        }

        private void printBoardDebug()
        {
            string[] p = new string[3];
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    int t = (int)BoardData[y, x];
                    p[x] = t.ToString();

                }
                Debug.Log(string.Join(",", p)+"\n");
            }

        }
        private void printRowDebug(List<BoardOption> row)
        {

                List<string> r = new List<string>();
                foreach (BoardOption g in row)
                {
                    int q = (int)g;
                    r.Add(q.ToString());
                }

                string s = string.Join("\t", r.ToArray());
                Debug.Log(s + "\n");
          
        }

        private BoardOption getBoardValue(int x, int y)
        {
            return BoardData[y,x];
        }

        private GameOver checkAllRows()
        {
            int countforP1 = 0;
            int countforP2 = 0;

            for (int y = 0; y < 3; y++)
            {
                countforP1 = 0;
                countforP2 = 0;
                for (int x = 0; x < 3; x++)
                {
                    if (getBoardValue(x, y) == BoardOption.X)
                    {
                        countforP1++;
                    }
                    if (getBoardValue(x, y) == BoardOption.O)
                    {
                        countforP2++;
                    }

                    if (countforP1 == 3)
                    {
                        //Player 1 Wins
                        return GameOver.P1;
                    }
                    if (countforP2 == 3)
                    {
                        //Player 2 Wins
                        return GameOver.P2;
                    }
                }
            }
            return GameOver.IDLE;
        }

        private GameOver checkAllDiagonals()
        {
            int countforP1 = 0;
            int countforP2 = 0;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (y == x && getBoardValue(x, y) == BoardOption.X)
                    {
                        countforP1++;
                    }

                    if (y == x && getBoardValue(x, y) == BoardOption.O)
                    {
                        countforP2++;
                    }

                    if (countforP1 == 3)
                    {
                        //Player 1 Wins
                        return GameOver.P1;
                    }
                    if (countforP2 == 3)
                    {
                        //Player 2 Wins
                        return GameOver.P2;
                    }
                }
            }
            return GameOver.IDLE;
        }

        private GameState checkForEmpty()
        {

            for (int y = 0; y < 3; y++)
            {
                for(int x = 0; x < 3; x++)
                {
                    if(BoardData[y,x] == BoardOption.NO_VAL)
                    {
                        return GameState.IN_PROGRESS;
                    }
                }
                
            }

            return GameState.GAMEOVER;
        }

    }

}