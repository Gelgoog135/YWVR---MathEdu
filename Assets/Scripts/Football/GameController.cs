using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

namespace YWVR.Football
{

    public class GameController : MonoBehaviour
    {
        public Text txtLocationFootball;
        public Text txtLocationPlayer;
        public Text txtMessage;

        private bool hasCalledForEnd = false;

        public Vector2 min;
        public Vector2 max;

        public float pillarSpeed = 3.0f;

        [ReadOnly] public Vector2 goal;

        public GameObject pillar;
        public GameObject football;
        public List<XYPillar> listXYPillar = new List<XYPillar>();

        private bool isY = false;
        // Start is called before the first frame update
        void Start()
        {
            int rndX = 0;
            int rndY = 0;

            do
            {
                rndX = Random.Range(Mathf.RoundToInt(min.x), Mathf.RoundToInt(max.x) + 1);
                rndY = Random.Range(Mathf.RoundToInt(min.y), Mathf.RoundToInt(max.y) + 1);
            } while (Mathf.Abs(rndX - 3) <= 3 || Mathf.Abs(rndY - 3) <= 3);

            goal = new Vector2(rndX, rndY);
            var newPillar = Instantiate(pillar);
            newPillar.transform.position = new Vector3(rndX, -2.5f, rndY);
            listXYPillar.Add(new XYPillar(rndX, rndY, newPillar));

            var newFootball = Instantiate(football);
            newFootball.transform.position = new Vector3(rndX, 0.11f, rndY);


            txtLocationFootball.text = string.Format("{0}, {1}", Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y));
        }

        // Update is called once per frame
        void Update()
        {
            
            float step = pillarSpeed * Time.deltaTime; // calculate distance to move

            foreach(var xYPillar in listXYPillar)
            {
                var posTo = new Vector3(xYPillar.X, -2.5f, xYPillar.Y);
                if (Vector3.Distance(xYPillar.Pillar.transform.position, posTo) > 0.001f)
                {
                    xYPillar.Pillar.transform.position = Vector3.MoveTowards(xYPillar.Pillar.transform.position, posTo, step);
                }
            }


            var camPos = Camera.main.transform.position;
            var camX = Mathf.RoundToInt(camPos.x);
            var camY = camPos.y;
            var camZ = Mathf.RoundToInt(camPos.z);




            // check if right path
            if (!(camX == 0 && camZ == 0) && camPos.y > -1 && !listXYPillar.Any(x => x.X == camX && x.Y == camZ))
            {
                if (!isY)
                {
                    if(camZ == 0)
                    {
                        if(Mathf.Abs(goal.x - camX) <= Mathf.Abs(goal.x) && ((goal.x < 0 && goal.x <= camX) || (goal.x > 0 && goal.x >= camX)))
                        {
                            var newPillar = Instantiate(pillar);
                            listXYPillar.Add(new XYPillar(camX, camZ, newPillar));

                            Vector3 posFrom = new Vector3(camX, -2.5f, camZ);
                            if(goal.x < 0)
                                posFrom = new Vector3(camX + 1, -2.5f, camZ);
                            else
                                posFrom = new Vector3(camX - 1, -2.5f, camZ);
                            //Vector3 posTo = new Vector3(camX, -2.5f, camZ);

                            newPillar.transform.position = posFrom;
                        }
                    }
                }
                else
                {
                    if(camX == (int)goal.x && camZ != 0)
                    {
                        if (Mathf.Abs(goal.y - camZ) <= Mathf.Abs(goal.y) && ((goal.y < 0 && goal.y <= camZ) || (goal.y > 0 && goal.y >= camZ)))
                        {
                            var newPillar = Instantiate(pillar);
                            listXYPillar.Add(new XYPillar(camX, camZ, newPillar));

                            Vector3 posFrom = new Vector3(camX, -2.5f, camZ);
                            if (goal.y < 0)
                                posFrom = new Vector3(camX, -2.5f, camZ + 1);
                            else
                                posFrom = new Vector3(camX, -2.5f, camZ - 1);
                            //Vector3 posTo = new Vector3(camX, -2.5f, camZ);

                            newPillar.transform.position = posFrom;
                        }
                    }
                }
            }

            if (camX == goal.x && camPos.y > -1 && camZ == 0 && !isY)
            {
                isY = true;
                txtMessage.text = $"You have done {Mathf.Abs(Mathf.RoundToInt(goal.x))} units, what about Y axis?";
            }
            if (camX == goal.x && camPos.y > -1 && camZ == goal.y)
            {
                txtMessage.text = $"Great! You have reached by moving to {Mathf.RoundToInt(goal.x)}, {Mathf.RoundToInt(goal.y)}";

                if (!hasCalledForEnd)
                    StartCoroutine(EndScene(5, $"Great! You have reached by moving to ({Mathf.RoundToInt(goal.x)}, 0), then to ({Mathf.RoundToInt(goal.x)}, {Mathf.RoundToInt(goal.y)})"));
                hasCalledForEnd = true;
            }
            if ( camPos.y < -1)
            {
                txtMessage.text = "You dropped off the cliff, ending in 10 seconds";

                if(!hasCalledForEnd)
                    StartCoroutine(EndScene(10, "You dropped off the cliff"));
                hasCalledForEnd = true;
            }


            txtLocationPlayer.text = string.Format("{0}, {1}", Mathf.RoundToInt(Camera.main.transform.position.x), Mathf.RoundToInt(Camera.main.transform.position.z));

            //if (-8 <= camX && camX <= -1 && camZ == 0)
            //{
            //    foreach(var xy in listXYPillar)
            //    {
            //        if(xy.X == camX && xy.Y == camZ && xy.Pillar != null)
            //        {
            //            Destroy(xy.Pillar);
            //            xy.Pillar = null;
            //        } 
            //    }
            //}
            //if (1 <= camZ && camZ <= 10 && camX == -8)
            //{
            //    foreach (var xy in listXYPillar)
            //    {
            //        if (xy.X == camX && xy.Y == camZ && xy.Pillar != null)
            //        {
            //            Destroy(xy.Pillar);
            //            xy.Pillar = null;
            //        }
            //    }
            //}


        }

        private IEnumerator EndScene(int sec, string msg)
        {
            yield return new WaitForSeconds(sec);
            var sceneController = new SceneController(SceneController.Game.Football);
            sceneController.ChangeToGameLobby(msg);
        }
    }

    public class XYPillar
    {
        public int X { get; set; }
        public int Y { get; set; }
        public GameObject Pillar { get; set; }
        public XYPillar(int x, int y, GameObject pillar)
        {
            X = x; Y = y; Pillar = pillar;
        }
    }
}
