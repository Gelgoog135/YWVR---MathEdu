using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace YWVR.RE
{
    public class QuestionController
    {
        List<Question> listQuestions = new List<Question>();

        public QuestionController()
        {
            TextAsset tempAsset = Resources.Load<TextAsset>("Questions");
            string json = tempAsset.text;
            listQuestions = JsonConvert.DeserializeObject<List<Question>>(json);
            foreach (var question in listQuestions)
            {
                var len = question.Answers.Count;
                for (int i = 0; i < len; i++)
                    question.Codes.Add(RandomString(4));
                question.AnswerCode = RandomString(4);
            }
        }

        public Question GetRandomEquation(int level)
        {
            listQuestions.Shuffle();
            return listQuestions.Where(x => x.Level == level).FirstOrDefault();
        }
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[UnityEngine.Random.Range(0, s.Length)]).ToArray());
        }
    }
    public class Question
    {
        public string Equation { get; set; }
        public int Level { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Answers { get; set; }
        [JsonIgnore]
        public List<string> Codes = new List<string>();
        [JsonIgnore]
        public string AnswerCode { get; set; }
    }
}
