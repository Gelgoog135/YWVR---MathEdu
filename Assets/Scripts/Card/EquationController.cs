using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace YWVR.Card
{
    public class EquationController
    {
        List<Equation> listEquations = new List<Equation>();

        public EquationController()
        {
            TextAsset tempAsset = Resources.Load<TextAsset>("card_questions");
            string json = tempAsset.text;
            listEquations = JsonConvert.DeserializeObject<List<Equation>>(json);
        }

        public Equation GetRandomEquation(int idToIgnore = -1)
        {
            listEquations.Shuffle();
            if (idToIgnore == -1)
                return listEquations[0];
            else
                return listEquations.Where(x => x.ID != idToIgnore).FirstOrDefault();
        }

        public List<Equation> GetRandomEquations(int count, int idToIgnore)
        {
            listEquations.Shuffle();
            var tempList = listEquations.Where(x => x.ID != idToIgnore).FirstOrDefault();
            var result = new List<Equation>();
            for (int i = 0; i < count; i++)
            {
                result.Add(listEquations[i]);
            }
            return result;
        }
        public Equation GetRandomEquationByValue(int idToIgnore, string value)
        {
            listEquations.Shuffle();
            return listEquations.Where(x => x.ID != idToIgnore && x.Value == value).FirstOrDefault();
        }
        public Equation GetById(int id)
        {
            return listEquations.Where(x => x.ID == id).FirstOrDefault();
        }
    }

    public class Equation
    {
        public int ID { get; set; }
        public string Expression { get; set; }
        public string Value { get; set; }
    }
}