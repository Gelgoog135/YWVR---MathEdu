using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
namespace YWVR.RE
{
    public class TimerController : MonoBehaviour
    {
        public TextMeshProUGUI txtTime;
        public GameObject progressBar;
        public int time;

        private float currentTime = 0;
        private float remainingTime
        {
            get
            {
                return (float)time - currentTime;
            }
        }
        private float totalLength;



        // Start is called before the first frame update
        void Start()
        {
            totalLength = progressBar.transform.GetComponent<RectTransform>().GetRight();
            //Debug.Log(totalLength);
        }

        // Update is called once per frame
        void Update()
        {
          
            if (remainingTime > 0)
            {
                currentTime += Time.deltaTime;

                if ((int)currentTime % 2 == 0)
                {
                    txtTime.SetText(TimeSpan.FromSeconds((double)remainingTime).ToString(@"mm\:ss"));
                }
                else
                {
                    txtTime.SetText(TimeSpan.FromSeconds((double)remainingTime).ToString(@"mm\ ss"));
                }

                //280 is the length of the "progress bar"
                progressBar.transform.GetComponent<RectTransform>().SetRight(currentTime / (float)time * 280);
            }
            else
            {
                new SceneController(SceneController.Game.Kitchen).ChangeToGameLobby("You ran out of time!");
            }
        }
    }

}
