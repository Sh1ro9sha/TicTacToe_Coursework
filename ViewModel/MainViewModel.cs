using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF_MVVM.Model;

namespace TicTacToeWPF_MVVM.ViewModel
{
    using BaseClass;
    using System.Windows.Controls;
    using System.Windows.Input;
    public class MainViewModel : ViewModel
    {
        // инициализация игрового объекта
        private Game _ngame;

        // вспомогательный флаг для сброса состояния
        private bool _gameInProgress = false;

        // создание массива пустых строк
        // 1x9
        public string[] _table = new string[] { "", "", "", "", "", "", "", "", "" };

        public string[] Table
        {
            get { return _table; }
            set
            {
                _table = value;
                onPropertyChange(nameof(Table));
            }
        }

        // полная опора для отображения информации под доской
        private string _whoseRound;

        public string WhoseRound
        {
            get
            {
                return _whoseRound;
            }
            set
            {
                _whoseRound = value;
                onPropertyChange(nameof(WhoseRound));
            }
        }


        public MainViewModel()
        {
        }

        // поля для ввода имени первого игрока
        private string _playerName1;

        public string PlayerName1
        {
            get { return _playerName1; }
            set
            {
                _playerName1 = value;
                onPropertyChange(nameof(PlayerName1));
            }
        }

        // поле для ввода имени второго игрока
        private string _playerName2;

        public string PlayerName2
        {
            get { return _playerName2; }
            set
            {
                _playerName2 = value;
                onPropertyChange(nameof(PlayerName2));
            }
        }

        // если можно щелкнуть, то запускается функция OnClick
        private ICommand _onClick;

        public ICommand OnClick
        {
            get
            {
                return _onClick ?? (_onClick = new RelayCommand(OnClickFunc, CanOnClick));
            }
        }

        private void OnClickFunc(object obj)
        {
            // команда синтаксического анализа кнопки для преобразования в int
            int field = int.Parse((string)obj);

            Table[field] = _ngame.CurrentMark;

            // обновить таблицу
            Table = Table;

            // измените поворот после щелчка
            _ngame.ChangeTurns(Table);
            WhoseRound = _ngame.WhoseRound();

            // сбросить поле, если игра закончена (кто-то выиграл или сыграл вничью)
            if (_ngame.IsFinished)
            {
                Table = new string[] { "", "", "", "", "", "", "", "", "" };
                PlayerName1 = "";
                PlayerName2 = "";
                _gameInProgress = false;
                WhoseRound = "";
            }
        }

        // если на одной из маленьких кнопок есть что-то еще, кроме "", то не нажимайте на нее
        private bool CanOnClick(object param)
        {
            // преобразуйте команду кнопки в int, чтобы она действовала как индекс
            int field = int.Parse((string)param);

            // заблокируйте все поля, если игра не запущена
            if (!_gameInProgress)
            {
                return false;
            }

            for (int i = 0; i < Table.Length; i++)
            {
                if (Table[field] != "")
                    return false;
            }

            return true;
        }

        private ICommand _start;

        public ICommand Start
        {
            get
            {
                return _start ?? (_start = new RelayCommand(StartGame, CanStartGame));
            }
        }

        // создаем игроков и игру после указания имен и нажатия кнопки
        private void StartGame(object param)
        {
            var player1 = new Player(_playerName1);
            var player2 = new Player(_playerName2);
            _ngame = new Game(player1, player2);
            _gameInProgress = true;

            // выполнение поворота при старте, чтобы текущая отметка не была нулевой
            _ngame.ChangeTurns(Table);

         
            WhoseRound = _ngame.WhoseRound();
        }

        // если указаны имена, то игру можно начинать
        private bool CanStartGame(object param)
        {
            // если оба текстовых поля не пусты
            var textBoxEmpty = !(String.IsNullOrEmpty(_playerName1) || String.IsNullOrEmpty(_playerName2));

            // если оба они не пусты и игра не запущена, то вы можете нажать кнопку
            return (textBoxEmpty && !_gameInProgress);
        }
    }
}
