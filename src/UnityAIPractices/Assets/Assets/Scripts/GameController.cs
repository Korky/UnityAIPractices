﻿//System Libraries
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Custom Libraries
using Models;
using Enums;
using AI;

public class GameController : MonoBehaviour
{
    //views
    public Image[] GUIRef = new Image[9];
    public Image[,] GridRef = new Image[3, 3];
    public Canvas[] MenuFlow = new Canvas[4];
    public Text GameOverText;
    public Text CurrentPlayerText;

    //models
    private Player p1, p2;
    private Player currentPlayer;
    private Board GameBoard;
    private int currentMenuIndex;

    //utils
    private GameState currentGameState;
    private MinimaxAI AI;

    // Overrides
    void Awake()
    {
        currentGameState = GameState.NOT_INIT;
        p1 = new Player();
        p2 = new Player();
        currentMenuIndex = 0;

    }
	void Start ()
    {

        //Setup UI Pointers
        GridRef[0, 0] = GUIRef[0];
        GridRef[0, 1] = GUIRef[1];
        GridRef[0, 2] = GUIRef[2];
        GridRef[1, 0] = GUIRef[3];
        GridRef[1, 1] = GUIRef[4];
        GridRef[1, 2] = GUIRef[5];
        GridRef[2, 0] = GUIRef[6];
        GridRef[2, 1] = GUIRef[7];
        GridRef[2, 2] = GUIRef[8];

        //start Menu Flow
        MenuFlow[currentMenuIndex].gameObject.SetActive(true);
    }
    void Update()
    {
        if (currentGameState != GameState.IN_PROGRESS ) return;
        
        //update Current Player Text
        if(p1 == currentPlayer) {
            string des = (currentPlayer.Type == PlayerType.AI) ? "CPU" : "Human";
            CurrentPlayerText.text = "Player 1 - " + des;
        }
        else {
            string des = (currentPlayer.Type == PlayerType.AI) ? "CPU" : "Human";
            CurrentPlayerText.text = "Player 2 - " + des;
        }

        

    }
	
    //public
    public void ClickOption(string cord)
    {
        int x, y;
        x = int.Parse(cord[0].ToString());
        y = int.Parse(cord[1].ToString());
        //Debug.Log("UI Clicked Cord:" + x + ", " + y);


        if (GameBoard.PlaceMove(x, y, currentPlayer.Icon))
        {
            ModifyOptionView(x, y, currentPlayer.Icon);            
        }
        currentPlayer = (currentPlayer == p1) ? p2 : p1;


        GameOver checker = GameBoard.CheckGameOver();
        Debug.Log(checker);
        if(checker != GameOver.IDLE)
        {
            //End Game
            GameEnd(checker);
        }
        CheckforAI();
    }

    public void ClickMenuOption(string opt)
    {
        switch (opt)
        {
            case "PVP":
                p1.Type = PlayerType.HUMAN;
                p2.Type = PlayerType.HUMAN;
                ChangeMenu();
                break;
            case "PVE":
                p1.Type = PlayerType.HUMAN;
                p2.Type = PlayerType.AI;
                ChangeMenu();
                break;
            case "EVE":
                p1.Type = PlayerType.AI;
                p2.Type = PlayerType.AI;
                ChangeMenu();
                break;
            case "X":
                p1.Icon = BoardOption.X;
                p1.Index = PlayerIndex.PLAYER1;
                p2.Icon = BoardOption.O;
                p2.Index = PlayerIndex.PLAYER2;
                GameSetup();
                break;
            case "O":
                p2.Icon = BoardOption.X;
                p2.Index = PlayerIndex.PLAYER1;
                p1.Icon = BoardOption.O;
                p1.Index = PlayerIndex.PLAYER2;
                GameSetup();
                break;
            case "RETRY":
                p1 = new Player();
                p2 = new Player();
                ChangeMenu();
                currentGameState = GameState.NOT_INIT;
                break;
            default:
                break;
        }
    }

    //private
    private void CheckforAI()
    {
        //AICode
        if (currentPlayer.Type == PlayerType.AI)
        {
            Move AIMove = AI.PerformAIMove(ref GameBoard);
            if (GameBoard.PlaceMove(AIMove.x, AIMove.y, currentPlayer.Icon))
                ModifyOptionView(AIMove.x, AIMove.y, currentPlayer.Icon);
            else
                Debug.Log("FATAL ERROR: Something went wrong with AICode in Game Controller Update");

            currentPlayer = (currentPlayer == p1) ? p2 : p1;
        }
    }

    private void ChangeMenu()
    {
        MenuFlow[currentMenuIndex++].gameObject.SetActive(false);

        if (currentMenuIndex > 3) currentMenuIndex = 0;
        
        MenuFlow[currentMenuIndex].gameObject.SetActive(true);

    }

    private void ModifyOptionView(int x,int y,BoardOption opt)
    {
        GridRef[x,y].GetComponent<OptionController>().ChangeOption(opt);

    }

    private void GameSetup()
    { 
        //MenuFlow[currentMenuIndex+1].enabled = true;
        currentPlayer = new Player();
        //Initialiaze Game
        currentGameState = GameState.INIT;
        GameInit();
    }

    private void ClearView()
    {
        for (int i = 0; i < 9; i++)
        {
            GUIRef[i].GetComponent<OptionController>().Init();
        }
    }

    private void GameEnd(GameOver winner)
    {
        switch (winner)
        {
            case GameOver.P1:
                if(p1.Index == PlayerIndex.PLAYER1)
                    GameOverText.text = "Player 1 wins";
                else
                    GameOverText.text = "Player 2 wins";
                break;
            case GameOver.P2:
                if (p1.Index == PlayerIndex.PLAYER2)
                    GameOverText.text = "Player 1 wins";
                else
                    GameOverText.text = "Player 2 wins";
                break;
            case GameOver.TIE:
                GameOverText.text = "Tie";
                break;
        }
        //MenuFlow[currentMenuIndex].enabled = false;
        ClearView();
        ChangeMenu();
    }

    private void GameInit()
    {
        //start Game Init
        GameBoard = new Board();
        GameBoard.Init();


        //finish Game Init
        if (p1.Type != PlayerType.NULL || p2.Type != PlayerType.NULL) { 
            currentPlayer = (p1.Index == PlayerIndex.PLAYER1) ? p1 : p2;
            currentGameState = GameState.IN_PROGRESS;
            ChangeMenu();
        }
        else { Debug.Log("FATAL ERROR: Still Haven't Assigned Players in Game Model Class"); }

        if (p1.Type == PlayerType.AI || p2.Type == PlayerType.AI)
            AI = new MinimaxAI(p1,p2);


    }
}
