using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YWVR.Triangle
{
    public class GameController : MonoBehaviour
    {
        public GameObject trianglePrefab;
        public Text txtScore;
        public Text txtLives;
        public float speedRatio = 1.0f;
        public float radius = 2.0f;
        private float elapsed = 0f;
        public float spawnRate = 10.0f;
        public float minSpawnRate = 5.0f;

        public int lives = 3;

        [ReadOnly] public int score = 0;

        private List<QuestionController> listQuestionController = new List<QuestionController>();
        public List<GameObject> listControllers = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            //InvokeRepeating("CreateNewQuestion", 1f, 5f);  //1s delay, repeat every 5s
            CreateNewQuestion();
            txtLives.text = lives.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < listQuestionController.Count; i++)
            {
                if (listQuestionController[i].isLost)
                {
                    score--;
                    lives--;
                    txtLives.text = lives.ToString();

                    if (lives <= 0)
                    {
                        var sceneController = new SceneController(SceneController.Game.Triangle);
                        sceneController.ChangeToGameLobby($"Score: {score}");
                    }
                }
                if (listQuestionController[i].isWon) score++;

                if (listQuestionController[i].isLost || listQuestionController[i].isWon)
                {
                    txtScore.text = score.ToString();
                    Destroy(listQuestionController[i].triangleContainer);
                    Destroy(listQuestionController[i]);
                    listQuestionController.RemoveAt(i);
                    i--;
                }
            }

            speedRatio += 0.0002f;
            if (spawnRate > minSpawnRate)
            {
                spawnRate -= 0.0005f;
            }

            elapsed += Time.deltaTime;
            if (elapsed >= spawnRate)
            {
                elapsed = 0;
                CreateNewQuestion();
            }
        }

        void CreateNewQuestion()
        {
            var tri = Instantiate(trianglePrefab);
            //var controller = tri.GetComponent<FreeformTriangleController>();
            var questionController = gameObject.AddComponent<QuestionController>();
            questionController.CreateQuestion(tri, listControllers, speedRatio, radius);
            listQuestionController.Add(questionController);
        }
    }
}