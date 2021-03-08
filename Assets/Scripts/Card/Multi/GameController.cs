using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace YWVR.Card.Multi
{
    public class GameController : MonoBehaviour
    {
        public int count = 10;
        public float timePerQuestion = 6.0f;
        public TEXDraw txtQuestion;
        public GameObject progressBar;

        public List<GameObject> listPlayers = new List<GameObject>();

        public PlayerController playerController;
        public SocketController socketController;

        private Equation equation;


        [ReadOnly] public bool isHost;
        [ReadOnly] public Player player;
        [ReadOnly] public Game game;
        private Game pGame;
        [ReadOnly] public List<Player> players;

        [ReadOnly] public float currentTime;
        public float remainingTime
        {
            get
            {
                return (float)timePerQuestion - currentTime;
            }
        }
        // Use this for initialization
        void Start()
        {
            if (socketController == null)
                socketController = GetComponent<SocketController>();

            Resources.UnloadUnusedAssets();
            //SetQuestion();
        }

        private Player pendingJobPlayer = null;
        private List<Player> pendingJobPlayers = null;
        private Game pendingJobGame = null;

        // Update is called once per frame
        void Update()
        {
            if(pendingJobPlayer != null)
            {
                player = pendingJobPlayer;
                playerController = listPlayers[player.Color - 1].GetComponent<PlayerController>();
                playerController.SetPosition();
                pendingJobPlayer = null;
            }

            if(pendingJobPlayers != null)
            {
                players = pendingJobPlayers;
                if(player != null)
                    foreach(var p in players)
                    {
                        if(p.Color != player.Color)
                        {
                            var pc = listPlayers[p.Color - 1].GetComponent<PlayerController>();
                            pc.SetPlayer(p);
                        }
                    }
                pendingJobPlayers = null;
            }

            if(pendingJobGame != null)
            {
                if (isHost && game != null)
                    pendingJobGame.WaitingTime = game.WaitingTime;

                game = pendingJobGame;
                if(game.Host != null && player != null)
                {
                    isHost = game.Host == player.Color;
                }
                else
                {
                    isHost = false;
                }
                pendingJobGame = null;
            }

            if(player != null)
            {
                player.Score = playerController.score;
                player.Cards = playerController.listEquations.Select(x => x.ID).ToArray();
                player.SelectedCard = playerController.selectedCardIndex;
                player.BodyPosition = playerController.xRRig.transform.Find("Camera Offset").Find("Main Camera").position.ToString("F3");
                player.BodyRotation = playerController.xRRig.transform.Find("Camera Offset").Find("Main Camera").rotation.ToString("F3");
                player.LHandPosition = playerController.xRRig.transform.Find("Camera Offset").Find("LeftHand Controller").position.ToString("F3");
                player.LHandRotation = playerController.xRRig.transform.Find("Camera Offset").Find("LeftHand Controller").rotation.ToString("F3");
                player.RHandPosition = playerController.xRRig.transform.Find("Camera Offset").Find("RightHand Controller").position.ToString("F3");
                player.RHandRotation = playerController.xRRig.transform.Find("Camera Offset").Find("RightHand Controller").rotation.ToString("F3");

                //var json = JsonConvert.SerializeObject(player);

                //socketController.SendMessageToServer("{CPlayer}" + json);
            }
            //currentTime += Time.deltaTime;
            //if(currentTime <= timePerQuestion)
            //{
            //    progressBar.transform.GetComponent<RectTransform>().SetRight(currentTime / (float)timePerQuestion * 5.0f);
            //}

            //if(remainingTime <= 0)
            //{
            //    playerController.CheckResult(equation.Value);
            //    SetQuestion();
            //    currentTime = 0;
            //    count--;
            //    if(count <= 0)
            //    {
            //        new SceneController(SceneController.Game.Card).ChangeToGameLobby($"Score: {playerController.score}");
            //    }
            //}

            if(game != null && game.WaitingTime > 0)
                UpdateWaitingTime();

            if(game != null) // && game.WaitingTime <= 0)
            {

                if (pGame != null && ( pGame.IsStarted != game.IsStarted || pGame.CurrentQuestion != game.CurrentQuestion) )
                {
                    if(playerController != null && equation != null)
                    {
                        playerController.CheckResult(equation.Value);
                        player.Score = playerController.score;
                        SetQuestion();
                    }
                    //currentTime = 0;
                    //count--;

                    //game.IsStarted = true;
                }
                if (isHost && !game.IsStarted && game.WaitingTime <= 0)
                {
                    game.WaitingTime = timePerQuestion;
                    SetQuestion();
                    game.IsStarted = true;
                }
                if (isHost && game.IsStarted && game.WaitingTime <= 0)
                {
                    count--;
                }


                if (isHost)
                {
                    game.CurrentQuestion = count;
                    if (equation == null)
                        game.ExpressionId = null;
                    else
                        game.ExpressionId = equation.ID;
                }

                if (game.CurrentQuestion <= 0)
                {
                    new SceneController(SceneController.Game.Card).ChangeToGameLobby($"Score: {playerController.score}");
                }
                pGame = game;
            }

            LoopSendToServer();
            GC.Collect();
        }

        float timeElapsedServer = 0;
        void LoopSendToServer()
        {
            timeElapsedServer += Time.deltaTime;

            if(timeElapsedServer >= 0.05f)
            {
                timeElapsedServer = 0;

                if (player != null)
                {
                    var json = JsonConvert.SerializeObject(player);
                    socketController.SendMessageToServer("{CPlayer}" + json);
                }
                if (game != null && isHost)
                {
                    var json = JsonConvert.SerializeObject(game);
                    socketController.SendMessageToServer("{CGame}" + json);
                }
            }
        }

        void UpdateWaitingTime()
        {
            if (isHost)
            {
                game.WaitingTime -= Time.deltaTime;
            }
            if (!game.IsStarted)
            {
                txtQuestion.text = $"Waiting... {Mathf.RoundToInt(game.WaitingTime)}s";
            }
            progressBar.transform.GetComponent<RectTransform>().SetRight(game.WaitingTime / (float)game.MaxWaitingTime * 5.0f);
        }

        void SetQuestion()
        {
            EquationController equationController = new EquationController();

            Equation tempEq;
            do
            {
                if (isHost)
                    tempEq = equationController.GetRandomEquation();
                else
                    tempEq = equationController.GetById((int)game.ExpressionId);
            } while (!playerController.ChangeCards(tempEq.ID, tempEq.Value));
            equation = tempEq;

            txtQuestion.text = equation.Expression;
            playerController.ClearSelected();
        }

        public void SetPlayer(Player p)
        {
            pendingJobPlayer = p;
        }
        public void SetPlayers(List<Player> p)
        {
            pendingJobPlayers = p;
        }
        public void SetGame(Game g)
        {
            pendingJobGame = g;
        }
    }
}
