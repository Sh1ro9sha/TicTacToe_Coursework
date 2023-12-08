using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWPF_MVVM.Model
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsTurn { get; set; }
        public bool IsWinner { get; set; }
        public string Mark { get; set; }

        public Player(string name)
        {
            Name = name;
            IsWinner = false;
            IsTurn = false;
        }
    }
}
