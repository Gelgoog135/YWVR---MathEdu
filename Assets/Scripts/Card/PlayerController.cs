using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace YWVR.Card
{

    public class PlayerController : MonoBehaviour
    {
        public List<GameObject> listCards = new List<GameObject>();
        public List<TEXDraw> listChoices = new List<TEXDraw>();
        public TextMeshProUGUI txtScore;
        [ReadOnly] public int score = 0;

        public List<GameObject> listControllers = new List<GameObject>();

        private List<Equation> listEquations = new List<Equation>();

        private GameObject selectedCard;
        public Equation selectedEquation;
        public float selectedTime = 0;
        public GameController gameController;

        public bool ChangeCards(int id, string value)
        {
            var equationController = new EquationController();
            List<Equation> tempList = new List<Equation>();
            tempList.AddRange(equationController.GetRandomEquations(listChoices.Count, id));

            if(!tempList.Any(x => x.Value == value))
            {
                var eq = equationController.GetRandomEquationByValue(id, value);
                if (eq == null)
                    return false;

                tempList[0] = eq;
                tempList.Shuffle();
            }

            listEquations = tempList;

            for(int i = 0; i < listChoices.Count; i++)
            {
                listChoices[i].text = "-";
                listChoices[i].text = listEquations[i].Expression;
            }
            return true;
        }
        // Start is called before the first frame update
        void Start()
        {
            foreach (var card in listCards)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = new Color(214f / 255f, 214f / 255f, 214f / 255f);
                card.GetComponent<MeshRenderer>().material = mat;
            }

            var controller = listControllers[0].GetComponent<ActionBasedController>();
            controller.selectAction.action.performed += LeftSelectAction_Performed;

            controller = listControllers[1].GetComponent<ActionBasedController>();
            controller.selectAction.action.performed += RightSelectAction_Performed;
            //selectedCard = listCards[1];
        }

        // Update is called once per frame
        void Update()
        {

            GameObject nearestCard = null;
            float? nearestDistance = null;

            foreach (var controller in listControllers)
            {
                foreach (var card in listCards)
                {
                    if(card != selectedCard)
                    {
                        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                        mat.color = new Color(214f / 255f, 214f / 255f, 214f / 255f);
                        card.GetComponent<MeshRenderer>().material = mat;
                    }

                    var posDiff = controller.transform.position - card.transform.position;
                    if ((nearestDistance == null && posDiff.magnitude < 0.25f) || (nearestDistance != null && posDiff.magnitude < nearestDistance))
                    {
                        nearestDistance = posDiff.magnitude;
                        nearestCard = card;
                    }
                }

                if (nearestCard != null)
                {
                    Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    mat.color = new Color(112f / 255f, 230f / 255f, 230f / 255f);
                    nearestCard.GetComponent<MeshRenderer>().material = mat;

                    //Debug Only
                    //selectedCard = nearestCard;
                    //var index = listCards.IndexOf(selectedCard);
                    //selectedEquation = listEquations[index];
                    //selectedTime = gameController.remainingTime;
                }

                if(selectedCard != null && selectedCard != nearestCard)
                {
                    Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    mat.color = new Color(230f / 255f, 112f / 255f, 112f / 255f);
                    selectedCard.GetComponent<MeshRenderer>().material = mat;
                }
            }
        }


        private void LeftSelectAction_Performed(InputAction.CallbackContext obj)
        {
            Check(listControllers[0]);
            Debug.Log("pressed");
        }

        private void RightSelectAction_Performed(InputAction.CallbackContext obj)
        {
            Check(listControllers[1]);
            Debug.Log("pressed");
        }

        private void Check(GameObject controller)
        {
            GameObject nearestCard = null;
            float? nearestDistance = null;

            foreach (var card in listCards)
            {
                var posDiff = controller.transform.position - card.transform.position;
                if ((nearestDistance == null && posDiff.magnitude < 0.25f) || (nearestDistance != null && posDiff.magnitude < nearestDistance))
                {
                    nearestDistance = posDiff.magnitude;
                    nearestCard = card;
                }
            }

            if (nearestCard == null) return;
            selectedCard = nearestCard;
            var index = listCards.IndexOf(selectedCard);
            selectedEquation = listEquations[index];
            selectedTime = gameController.remainingTime;
        }

        public void CheckResult(string value)
        {
            if(selectedEquation != null)
            {
                if(selectedEquation.Value == value)
                {
                    score += Mathf.RoundToInt(selectedTime * 10.0f);
                }
            }
            txtScore.text = score.ToString();
        }

        public void ClearSelected()
        {
            selectedEquation = null;
            selectedTime = 0;
            selectedCard = null;
        }
    }
}