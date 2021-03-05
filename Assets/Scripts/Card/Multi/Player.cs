using System;
using UnityEngine;

namespace YWVR.Card.Multi
{
    public class Player
    {
        public int Color { get; set; }
        public int Score { get; set; }
        public int[] Cards { get; set; }
        public int? SelectedCard { get; set; }
        public string BodyPosition { get; set; }
        public string BodyRotation { get; set; }
        public string RHandPosition { get; set; }
        public string RHandRotation { get; set; }
        public string LHandPosition { get; set; }
        public string LHandRotation { get; set; }

        public Player()
        {
        }
    }

}
