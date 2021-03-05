using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Game game;
    public enum Game
    {
        None = 0,
        Kitchen = 1,
        Triangle = 2,
        Football = 3,
        Card = 4,
    }

    public SceneController() : this(Game.None)
    {
    }
    public SceneController(Game g)
    {
        this.game = g;
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ChangeToLobby()
    {
        Global.Score = null;
        ChangeScene("LobbyScene");
    }

    public void ChangeToGameLobby(string score = null)
    {
        Global.Score = score;
        switch (game)
        {
            case Game.Kitchen:
                ChangeScene("KitchenLobbyScene");
                break;
            case Game.Triangle:
                ChangeScene("TriangleLobbyScene");
                break;
            case Game.Football:
                ChangeScene("FootballLobbyScene");
                break;
            case Game.Card:
                ChangeScene("CardLobbyScene");
                break;
            default:
                break;
        }
    }

    public void ChangeToGame(int diff)
    {
        Global.Difficulty = diff;
        switch (game)
        {
            case Game.Kitchen:
                ChangeScene("KitchenScene");
                break;
            case Game.Triangle:
                ChangeScene("TriangleScene");
                break;
            case Game.Football:
                ChangeScene("FootballScene");
                break;
            case Game.Card:
                if (diff != 1)
                {
                    ChangeScene("CardScene");
                }
                else
                {
                    ChangeScene("CardMultiScene");
                }
                break;
            default:
                break;
        }
    }
}
