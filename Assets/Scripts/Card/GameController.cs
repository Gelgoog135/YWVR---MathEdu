using UnityEngine;
using System.Collections;
using TexDrawLib;

namespace YWVR.Card
{
    public class GameController : MonoBehaviour
    {
        public int count = 10;
        public float timePerQuestion = 5.0f;
        public TEXDraw txtQuestion;
        public GameObject progressBar;
        public PlayerController playerController;

        private Equation equation;

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
            SetQuestion();
            //TEXPreference.Initialize();
            //TEXPreference.main.CallRedraw();
        }

        // Update is called once per frame
        void Update()
        {
            currentTime += Time.deltaTime;
            if(currentTime <= timePerQuestion)
            {
                progressBar.transform.GetComponent<RectTransform>().SetRight(currentTime / (float)timePerQuestion * 5.0f);
            }

            if(remainingTime <= 0)
            {
                playerController.CheckResult(equation.Value);
                SetQuestion();
                //TEXPreference.Initialize();
                //TEXPreference.main.CallRedraw();
                currentTime = 0;
                count--;
                if(count <= 0)
                {
                    new SceneController(SceneController.Game.Card).ChangeToGameLobby($"Score: {playerController.score}");
                }
            }

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
    }
}
