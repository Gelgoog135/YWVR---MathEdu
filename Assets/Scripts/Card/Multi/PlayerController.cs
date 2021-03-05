using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace YWVR.Card.Multi
{

    public class PlayerController : MonoBehaviour
    {
        public List<GameObject> listCards = new List<GameObject>();
        public List<TEXDraw> listChoices = new List<TEXDraw>();
        public TextMeshProUGUI txtScore;
        [ReadOnly] public int score = 0;

        public List<GameObject> listControllers = new List<GameObject>();

        public List<Equation> listEquations = new List<Equation>();


        public XRRig xRRig;

        private GameObject selectedCard;
        [ReadOnly] public int? selectedCardIndex = null;

        public Equation selectedEquation;
        public float selectedTime = 0;
        public GameController gameController;

        private GameObject placeholderPlayer;
        private GameObject placeholderBody;
        private GameObject placeholderLHand;
        private GameObject placeholderRHand;

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

        public void ChangeCardsByIDs(int[] ids)
        {
            var equationController = new EquationController();
            List<Equation> tempList = new List<Equation>();

            foreach(var id in ids)
            {
                tempList.Add(equationController.GetById(id));
            }

            listEquations = tempList;

            for (int i = 0; i < listEquations.Count; i++)
            {
                listChoices[i].text = listEquations[i].Expression;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            placeholderPlayer = gameObject.transform.Find("Player Position").gameObject;
            placeholderBody = placeholderPlayer.transform.Find("Body").gameObject;
            placeholderLHand = placeholderPlayer.transform.Find("Left Hand").gameObject;
            placeholderRHand = placeholderPlayer.transform.Find("Right Hand").gameObject;

            placeholderPlayer.SetActive(false);

            txtScore = gameObject.transform.Find("Board").Find("Canvas").Find("Score").GetComponent<TextMeshProUGUI>();
            listCards.Add(gameObject.transform.Find("Board").Find("Card 1").gameObject);
            listCards.Add(gameObject.transform.Find("Board").Find("Card 2").gameObject);
            listCards.Add(gameObject.transform.Find("Board").Find("Card 3").gameObject);
            listCards.Add(gameObject.transform.Find("Board").Find("Card 4").gameObject);
            listCards.Add(gameObject.transform.Find("Board").Find("Card 5").gameObject);
            listCards.Add(gameObject.transform.Find("Board").Find("Card 6").gameObject);

            foreach(var card in listCards)
            {
                listChoices.Add(card.transform.Find("Canvas Question").Find("TEXDraw Answer").GetComponent<TEXDraw>());
            }


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
            selectedCardIndex = listCards.IndexOf(selectedCard);
            selectedEquation = listEquations[(int)selectedCardIndex];
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
            selectedCardIndex = null;
        }

        public void SetPosition()
        {
            xRRig.transform.position = placeholderPlayer.transform.position;
            xRRig.transform.rotation = placeholderPlayer.transform.rotation;
            placeholderPlayer.SetActive(false);
        }

        public void SetPlayer(Player p)
        {
            score = p.Score;
            ChangeCardsByIDs(p.Cards);
            selectedCardIndex = p.SelectedCard;
            if (p.SelectedCard == null)
                selectedCard = null;
            else
                selectedCard = listCards[(int)p.SelectedCard];

            if(!String.IsNullOrWhiteSpace(p.BodyPosition))
            {
                placeholderBody.transform.position = StringToVector3(p.BodyPosition);
            }
            if (!String.IsNullOrWhiteSpace(p.BodyRotation))
            {
                placeholderBody.transform.rotation = StringToQuaternion(p.BodyRotation);
            }

            if (!String.IsNullOrWhiteSpace(p.LHandPosition))
            {
                placeholderLHand.transform.position = StringToVector3(p.LHandPosition);
            }
            if (!String.IsNullOrWhiteSpace(p.LHandRotation))
            {
                placeholderLHand.transform.rotation = StringToQuaternion(p.LHandRotation);
            }

            if (!String.IsNullOrWhiteSpace(p.RHandPosition))
            {
                placeholderRHand.transform.position = StringToVector3(p.RHandPosition);
            }
            if (!String.IsNullOrWhiteSpace(p.RHandRotation))
            {
                placeholderRHand.transform.rotation = StringToQuaternion(p.RHandRotation);
            }

            placeholderPlayer.SetActive(true);
        }

        public static Vector3 StringToVector3(String s)
        {
            string[] parts = s.Split(new string[] { "," }, StringSplitOptions.None);
            return new Vector3(
                float.Parse(parts[0]),
                float.Parse(parts[1]),
                float.Parse(parts[2]));
        }

        public static Quaternion StringToQuaternion(string sQuaternion)
        {
            // Remove the parentheses
            if (sQuaternion.StartsWith("(") && sQuaternion.EndsWith(")"))
            {
                sQuaternion = sQuaternion.Substring(1, sQuaternion.Length - 2);
            }

            // split the items
            string[] sArray = sQuaternion.Split(',');

            // store as a Vector3
            Quaternion result = new Quaternion(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]),
                float.Parse(sArray[3]));

            return result;
        }
    }
}