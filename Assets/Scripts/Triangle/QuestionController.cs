using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;

namespace YWVR.Triangle
{
    [ExecuteAlways]
    public class QuestionController : MonoBehaviour
    {
        public GameObject triangleContainer;

        [Range(0, 107)]
        public int index = 0;

        public float speedRatio = 1.0f;

        public bool isWon = false;
        public bool isLost = false;

        private List<GameObject> listControllers = new List<GameObject>();

        private Question question;

        private Question GetQuestion(int index = -1){
            List<Question> listQuestions = new List<Question>();
            listQuestions.Add(new Question("sin θ = a/?", "c", "a", "b", "c", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = b/?", "c", "a", "b", "c", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = a/?", "b", "a", "c", "b", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = c/?", "b", "a", "c", "b", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = b/?", "c", "b", "a", "c", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = a/?", "c", "b", "a", "c", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = b/?", "a", "b", "c", "a", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = c/?", "a", "b", "c", "a", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = c/?", "b", "c", "a", "b", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = a/?", "b", "c", "a", "b", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = c/?", "a", "c", "b", "a", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = b/?", "a", "c", "b", "a", "", "θ", ""));

            listQuestions.Add(new Question("cos θ = b/?", "c", "a", "b", "c", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = a/?", "c", "a", "b", "c", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = c/?", "b", "a", "c", "b", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = a/?", "b", "a", "c", "b", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = a/?", "c", "b", "a", "c", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = b/?", "c", "b", "a", "c", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = c/?", "a", "b", "c", "a", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = b/?", "a", "b", "c", "a", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = a/?", "b", "c", "a", "b", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = c/?", "b", "c", "a", "b", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = b/?", "a", "c", "b", "a", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = c/?", "a", "c", "b", "a", "", "θ", ""));

            listQuestions.Add(new Question("tan θ = a/?", "b", "a", "b", "c", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = b/?", "a", "a", "b", "c", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = a/?", "c", "a", "c", "b", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = c/?", "a", "a", "c", "b", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = b/?", "a", "b", "a", "c", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = a/?", "b", "b", "a", "c", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = b/?", "c", "b", "c", "a", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = c/?", "b", "b", "c", "a", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = c/?", "a", "c", "a", "b", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = a/?", "c", "c", "a", "b", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = c/?", "b", "c", "b", "a", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = b/?", "c", "c", "b", "a", "", "θ", ""));


            listQuestions.Add(new Question("sin θ = ?/c", "a", "a", "b", "c", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = ?/c", "b", "a", "b", "c", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = ?/b", "a", "a", "c", "b", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = ?/b", "c", "a", "c", "b", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = ?/c", "b", "b", "a", "c", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = ?/c", "a", "b", "a", "c", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = ?/a", "b", "b", "c", "a", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = ?/a", "c", "b", "c", "a", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = ?/b", "c", "c", "a", "b", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = ?/b", "a", "c", "a", "b", "", "θ", ""));
            listQuestions.Add(new Question("sin θ = ?/a", "c", "c", "b", "a", "θ", "", ""));
            listQuestions.Add(new Question("sin θ = ?/a", "b", "c", "b", "a", "", "θ", ""));

            listQuestions.Add(new Question("cos θ = ?/c", "b", "a", "b", "c", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = ?/c", "a", "a", "b", "c", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = ?/b", "c", "a", "c", "b", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = ?/b", "a", "a", "c", "b", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = ?/c", "a", "b", "a", "c", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = ?/c", "b", "b", "a", "c", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = ?/a", "c", "b", "c", "a", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = ?/a", "b", "b", "c", "a", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = ?/b", "a", "c", "a", "b", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = ?/b", "c", "c", "a", "b", "", "θ", ""));
            listQuestions.Add(new Question("cos θ = ?/a", "b", "c", "b", "a", "θ", "", ""));
            listQuestions.Add(new Question("cos θ = ?/a", "c", "c", "b", "a", "", "θ", ""));

            listQuestions.Add(new Question("tan θ = ?/b", "a", "a", "b", "c", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = ?/a", "b", "a", "b", "c", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = ?/c", "a", "a", "c", "b", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = ?/a", "c", "a", "c", "b", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = ?/a", "b", "b", "a", "c", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = ?/b", "a", "b", "a", "c", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = ?/c", "b", "b", "c", "a", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = ?/b", "c", "b", "c", "a", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = ?/a", "c", "c", "a", "b", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = ?/c", "a", "c", "a", "b", "", "θ", ""));
            listQuestions.Add(new Question("tan θ = ?/b", "c", "c", "b", "a", "θ", "", ""));
            listQuestions.Add(new Question("tan θ = ?/c", "b", "c", "b", "a", "", "θ", ""));


            listQuestions.Add(new Question("sin ? = a/c", "A", "a", "b", "c", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = b/c", "B", "a", "b", "c", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = a/b", "A", "a", "c", "b", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = c/b", "B", "a", "c", "b", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = b/c", "A", "b", "a", "c", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = a/c", "B", "b", "a", "c", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = b/a", "A", "b", "c", "a", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = c/a", "B", "b", "c", "a", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = c/b", "A", "c", "a", "b", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = a/b", "B", "c", "a", "b", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = c/a", "A", "c", "b", "a", "A", "B", "C"));
            listQuestions.Add(new Question("sin ? = b/a", "B", "c", "b", "a", "A", "B", "C"));

            listQuestions.Add(new Question("cos ? = b/c", "A", "a", "b", "c", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = a/c", "B", "a", "b", "c", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = c/b", "A", "a", "c", "b", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = a/b", "B", "a", "c", "b", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = a/c", "A", "b", "a", "c", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = b/c", "B", "b", "a", "c", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = c/a", "A", "b", "c", "a", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = b/a", "B", "b", "c", "a", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = a/b", "A", "c", "a", "b", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = c/b", "B", "c", "a", "b", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = b/a", "A", "c", "b", "a", "A", "B", "C"));
            listQuestions.Add(new Question("cos ? = c/a", "B", "c", "b", "a", "A", "B", "C"));

            listQuestions.Add(new Question("tan ? = a/b", "A", "a", "b", "c", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = b/a", "B", "a", "b", "c", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = a/c", "A", "a", "c", "b", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = c/a", "B", "a", "c", "b", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = b/a", "A", "b", "a", "c", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = a/b", "B", "b", "a", "c", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = b/c", "A", "b", "c", "a", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = c/b", "B", "b", "c", "a", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = c/a", "A", "c", "a", "b", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = a/c", "B", "c", "a", "b", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = c/b", "A", "c", "b", "a", "A", "B", "C"));
            listQuestions.Add(new Question("tan ? = b/c", "B", "c", "b", "a", "A", "B", "C"));

            if (index == -1)
                return listQuestions[Random.Range(0, listQuestions.Count - 1)];
            else
                return listQuestions[index];
        }

        public void CreateQuestion(GameObject triangle, List<GameObject> ctrls, float sr = 1.0f, float radius = 3.0f)
        {
            listControllers = ctrls;
            speedRatio = sr;
            triangleContainer = triangle;
            //triangleContainer.transform.position = Quaternion.AngleAxis(Random.Range(0, 359), Vector3.forward) * Vector3.right * radius;
            var angle = Random.Range(0, 359);
            triangleContainer.transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 1.5f, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);

            var newRotation = Quaternion.LookRotation(new Vector3(0, 0, 0) - triangleContainer.transform.position);
            newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y - 180, newRotation.eulerAngles.z);
            triangleContainer.transform.rotation = newRotation;

            var triController = triangleContainer.GetComponent<FreeformTriangleController>();
            question = GetQuestion();
            triController.SetQuestion(question, Random.Range(2, 12), Random.Range(2, 12), Random.Range(2, 12), Random.Range(0, 359));

            //foreach(var gController in listControllers)
            //{
            //    var controller = gController.GetComponent<ActionBasedController>();
            //    controller.selectAction.action.performed += SelectAction_Performed;
            //}

            var controller = listControllers[0].GetComponent<ActionBasedController>();
            controller.selectAction.action.performed += LeftSelectAction_Performed;

            controller = listControllers[1].GetComponent<ActionBasedController>();
            controller.selectAction.action.performed += RightSelectAction_Performed;
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

            var triXY = triangleContainer.transform.Find("Triangle Rotatable XY").gameObject;
            var tri = triXY.transform.Find("Triangle Rotatable Z").gameObject;
            //var gLineA = tri.transform.Find("Line A").gameObject;
            //var gLineB = tri.transform.Find("Line B").gameObject;
            //var gLineC = tri.transform.Find("Line C").gameObject;

            var gLabelLineA = tri.transform.Find("Label Line A").gameObject;
            var gLabelLineB = tri.transform.Find("Label Line B").gameObject;
            var gLabelLineC = tri.transform.Find("Label Line C").gameObject;
            var gLabelAngleA = tri.transform.Find("Label Angle A").gameObject;
            var gLabelAngleB = tri.transform.Find("Label Angle B").gameObject;
            var gLabelAngleC = tri.transform.Find("Label Angle C").gameObject;
            //List<GameObject> listLines = new List<GameObject>();
            //listLines.Add(gLineA);
            //listLines.Add(gLineB);
            //listLines.Add(gLineC);
            List<GameObject> listLabels = new List<GameObject>();
            listLabels.Add(gLabelLineA);
            listLabels.Add(gLabelLineB);
            listLabels.Add(gLabelLineC);
            listLabels.Add(gLabelAngleA);
            listLabels.Add(gLabelAngleB);
            listLabels.Add(gLabelAngleC);

            GameObject nearestLabel = null;
            float? nearestDistance = null;

            for (int i = 0; i < listLabels.Count; i++)
            {
                var posDiff = controller.transform.position - listLabels[i].transform.position;
                var txt = listLabels[i].GetComponent<TextMeshPro>();
                if ((nearestDistance == null && posDiff.magnitude < 0.075f) || (nearestDistance != null && posDiff.magnitude < nearestDistance))
                {
                    nearestLabel = listLabels[i];

                }
            }
            if(nearestLabel != null)
            {
                var txtNearest = nearestLabel.GetComponent<TextMeshPro>();
                if (question.Answer == txtNearest.text)
                {
                    isWon = true;
                    isLost = false;
                }
                else
                {
                    isWon = false;
                    isLost = true;
                }
            }
        }

        // Use this for initialization
        void Start()
        {
            //controller = GetComponent<ActionBasedController>();
        }

        // Update is called once per frame
        void Update()
        {
            triangleContainer.transform.position = new Vector3(triangleContainer.transform.position.x, triangleContainer.transform.position.y - 0.0005f * speedRatio, triangleContainer.transform.position.z);

            if(triangleContainer.transform.position.y < 0)
            {
                isWon = false;
                isLost = true;
            }

            var triXY = triangleContainer.transform.Find("Triangle Rotatable XY").gameObject;
            var tri = triXY.transform.Find("Triangle Rotatable Z").gameObject;
            //var gLineA = tri.transform.Find("Line A").gameObject;
            //var gLineB = tri.transform.Find("Line B").gameObject;
            //var gLineC = tri.transform.Find("Line C").gameObject;

            var gLabelLineA = tri.transform.Find("Label Line A").gameObject;
            var gLabelLineB = tri.transform.Find("Label Line B").gameObject;
            var gLabelLineC = tri.transform.Find("Label Line C").gameObject;
            var gLabelAngleA = tri.transform.Find("Label Angle A").gameObject;
            var gLabelAngleB = tri.transform.Find("Label Angle B").gameObject;
            var gLabelAngleC = tri.transform.Find("Label Angle C").gameObject;
            //List<GameObject> listLines = new List<GameObject>();
            //listLines.Add(gLineA);
            //listLines.Add(gLineB);
            //listLines.Add(gLineC);
            List<GameObject> listLabels = new List<GameObject>();
            listLabels.Add(gLabelLineA);
            listLabels.Add(gLabelLineB);
            listLabels.Add(gLabelLineC);
            listLabels.Add(gLabelAngleA);
            listLabels.Add(gLabelAngleB);
            listLabels.Add(gLabelAngleC);

            GameObject nearestLabel = null;
            float? nearestDistance = null;

            foreach (var controller in listControllers)
            {
                foreach(var label in listLabels)
                {
                    var txt = label.GetComponent<TextMeshPro>();
                    txt.color = Color.white;
                    //var renderer = line.GetComponent<LineRenderer>();
                    //var posDiff = controller.transform.position - line.transform.position;
                    var posDiff = controller.transform.position - label.transform.position;
                    //Debug.Log(renderer.GetPosition(0));
                    if ((nearestDistance == null && posDiff.magnitude < 0.075f) || (nearestDistance != null && posDiff.magnitude < nearestDistance))
                    {
                        //Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                        //renderer.material = mat;
                        nearestDistance = posDiff.magnitude;
                        nearestLabel = label;
                    }
                }

                if(nearestLabel != null)
                {
                    var txtNearest = nearestLabel.GetComponent<TextMeshPro>();
                    txtNearest.color = Color.blue;
                }
            }
        }
    }

    public class Question
    {
        public string Expression { get; set; }
        public string Answer { get; set; }
        public string LineA { get; set; } = "a";
        public string LineB { get; set; } = "b";
        public string LineC { get; set; } = "c";
        public string AngleA { get; set; } = "θ";
        public string AngleB { get; set; } = "θ";
        public string AngleC { get; set; } = "90°";

        public Question(string exp, string ans, string a, string b, string c, string A, string B, string C)
        {
            Expression = exp; Answer = ans; LineA = a; LineB = b; LineC = c; AngleA = A; AngleB = B; AngleC = C;
        }
    }
}