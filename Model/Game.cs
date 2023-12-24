using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicTacToeWPF_MVVM.Model
{
    public class Game
    {
        public int xCount = 0;
        public int yCount = 0;
        public List<Player> Players { get; set; } = new List<Player>();
        public string CurrentMark { get; set; }
        private List<string> MarksToAssign { get; set; } = new List<string>();
        public string Winner { get; set; }
        public bool IsFinished { get; set; }

        public int XCount
        {
            get { return xCount; }
            set
            {
                if (xCount != value)
                {
                    xCount = value;
                    OnPropertyChanged(nameof(XCount));
                }
            }
        }

        public int YCount
        {
            get { return yCount; }
            set
            {
                if (yCount != value)
                {
                    yCount = value;
                    OnPropertyChanged(nameof(YCount));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Game(Player p1, Player p2)
        {
            // установка значений по умолчанию
            IsFinished = false;

            // добавление игроков в список
            Players.Add(p1);
            Players.Add(p2);

            // присвоение отметок списку, а затем распределение их по игрокам
            MarksToAssign.Add("X");
            MarksToAssign.Add("O");
            AssignMark();

            // распечатать информацию об игроках
            foreach (var player in Players)
            {
                Console.WriteLine($"PlayerInfo: {player.Name}, {player.Mark}, {player.IsTurn}");
            }

            // назначение хода игроку с помощью 'X'
            AssignTurn();

            foreach (var player in Players)
            {
                Console.WriteLine($"Информация об игроке после назначения хода: {player.Name}, {player.Mark}, {player.IsTurn}");
            }
        }

        // покажите, чей это раунд
        public string WhoseRound()
        {
            var player = Players.First(x => x.IsTurn == false);

            return $"Ход игрока: {player.Name}, он играет {player.Mark}";
        }

        // изменить поворот на противоположное состояние при вызове
        private void ChangeTurn(Player player)
        {
            player.IsTurn = !player.IsTurn;      
        }
        

        // способ управления игрой, переключения игрока, проверки выигрыша и т.д.
        public void ChangeTurns(string[] field)
        {
            foreach (var player in Players)
            {
                if (CheckWin(field) == true)
                {
                    var winner = Players.FirstOrDefault(x => x.IsWinner == true);
                    MessageBox.Show($"Выиграл игрок {winner.Name}, он играл {winner.Mark}");
                    if(winner.Mark == "X")
                    {
                        xCount++;
                    }

                    if (winner.Mark == "O")
                    {
                        yCount++;
                    }
                    IsFinished = true;
                    break;
                }
                // или ничья?
                else if (IsTie(field))
                {
                    MessageBox.Show("Ничья!");
                    IsFinished = true;
                    break;
                }

                if (player.IsTurn)
                {
                    CurrentMark = player.Mark;
                }

                // измените поворот в конце
                ChangeTurn(player);
            }
        }

        // назначьте первый раунд парню с отметкой "X"
        private void AssignTurn()
        {
            var player = Players.FirstOrDefault(x => x.Mark == "X");
            player.IsTurn = true;
        }

        private bool CheckWin(string[] table)
        {
            var conditionToWin1 = CheckWinHorizontal(table);
            var conditionToWin2 = CheckWinVertical(table);
            var conditionToWin3 = CheckWinDiagonal(table);

            return conditionToWin1 || conditionToWin2 || conditionToWin3;
        }

        // проверьте выигрыш по диагонали
        private bool CheckWinDiagonal(string[] table)
        {
            // логика здесь такова: если 3 отметки совпадают, то победителем становится игрок с такой же отметкой
            if ((table[0] + table[4] + table[8]) == "XXX")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "X");
                winner.IsWinner = true;

                return true;
            }

            if ((table[0] + table[4] + table[8]) == "OOO")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "O");
                winner.IsWinner = true;

                return true;
            }

            if ((table[2] + table[4] + table[6]) == "XXX")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "X");
                winner.IsWinner = true;

                return true;
            }

            if ((table[2] + table[4] + table[6]) == "OOO")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "O");
                winner.IsWinner = true;

                return true;
            }

            return false;
        }

        // проверьте выигрыш по вертикали
        private bool CheckWinVertical(string[] table)
        {
            for (int i = 0; i < 3; i++)
            {
                string txt = table[i] + table[i + 3] + table[i + 6];
                if (txt == "XXX")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "X");
                    winner.IsWinner = true;
                    
                    return true;
                }

                if (txt == "OOO")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "O");
                    winner.IsWinner = true;

                    return true;
                }
                    
            }

            return false;
        }

        // проверьте выигрыш по горизонтали
        private bool CheckWinHorizontal(string[] table)
        {
            for (int i = 0; i < 9; i += 3)
            {
                string txt = table[i] + table[i + 1] + table[i + 2];
                if (txt == "XXX")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "X");
                    winner.IsWinner = true;

                    return true;
                }

                if (txt == "OOO")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "O");
                    winner.IsWinner = true;

                    return true;
                }
            }

            return false;
        }

        private bool IsTie(string[] table)
        {
            var txt = "";

            foreach(var element in table)
            {
                txt += element;
            }

            if (txt.Length == 9 && !String.IsNullOrWhiteSpace(txt))
                return true;
            
            return false;
        }

        // случайным образом присвойте игрокам одну из двух отметок
        // метод, вызываемый в самом начале
        private void AssignMark()
        {
            var random = new Random();

            foreach (var player in Players)
            {
                int index = random.Next(MarksToAssign.Count);
                player.Mark = MarksToAssign.ElementAt(index);
                MarksToAssign.RemoveAt(index);
            }
        }
    }
}
