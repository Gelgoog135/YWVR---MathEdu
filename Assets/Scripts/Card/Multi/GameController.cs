using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using Newtonsoft.Json;

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
                            var pc = listPlayers[p.Color].GetComponent<PlayerController>();
                            pc.SetPlayer(p);
                        }
                    }
                pendingJobPlayers = null;
            }

            if(pendingJobGame != null)
            {
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

                var json = JsonConvert.SerializeObject(player);

                socketController.SendMessageToServer("{CPlayer}" + json);
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

            if(game != null)
            {



                if (isHost)
                {
                    Game temp = new Game();
                    temp.MaxWaitingTime = game.MaxWaitingTime;
                    temp.WaitingTime = game.WaitingTime;
                    temp.CurrentQuestion = count;
                    if (equation == null)
                        temp.ExpressionId = null;
                    else
                        temp.ExpressionId = equation.ID;

                    var json = JsonConvert.SerializeObject(temp);

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

            txtQuestion.text = $"Waiting... {Mathf.RoundToInt(game.WaitingTime)}s";
            progressBar.transform.GetComponent<RectTransform>().SetRight(game.WaitingTime / (float)game.MaxWaitingTime * 5.0f);
        }

        void SetQuestion()
        {
            EquationController equationController = new EquationController();

            Equation tempEq;
            do
            {
                tempEq = equationController.GetRandomEquation();
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
