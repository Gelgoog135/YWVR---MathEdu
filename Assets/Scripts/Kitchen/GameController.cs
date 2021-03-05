using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YWVR.RE
{
    public class GameController : MonoBehaviour
    {
        public TEXDraw3D txtEquation;
        public TEXDraw3D txtAnswers;
        private Question question;
        private QuestionController questionController;
        private string equation;
        private string answer;
        // Start is called before the first frame update
        void Start()
        {
            questionController = new QuestionController();

            question = questionController.GetRandomEquation(Global.Difficulty);
            equation = question.Equation;
            List<string> listAnswers = new List<string>();
            listAnswers.Add($"{question.CorrectAnswer} (密碼 \\ Password: {question.AnswerCode}) \\\\");

            for (int i = 0; i < question.Answers.Count; i++)
            {
                listAnswers.Add($"{question.Answers[i]} (密碼 \\ Password: {question.Codes[i]}) \\\\");
            }

            listAnswers.Shuffle();
            answer = string.Join(Environment.NewLine, listAnswers);

            txtEquation.text = equation;
            txtAnswers.text = answer;
        }

        
        public bool CheckPassword(string pw)
        {
            return (pw == question.AnswerCode);
        }
    }
}

