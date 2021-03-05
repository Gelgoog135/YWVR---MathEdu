using System;
using System.Collections.Generic;
using UnityEngine;

namespace YWVR.Card.Multi
{
    public class Game
    {
        public float MaxWaitingTime { get; set; }
        public float WaitingTime { get; set; }
        public int CurrentQuestion { get; set; }
        public int? ExpressionId { get; set; }
        public int? Host { get; set; }
        //public List<Player> Players { get; set; }

        public Game()
        {
        }
    }

}
