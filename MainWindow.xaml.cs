using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CurrentPlayer = 0;

        public string[] Markers = { "X", "O" };

        public int TurnCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            UpdateStatusBarText();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.Content = Markers[CurrentPlayer];

            var (row, col) = GetButtonPosition(button);
            CheckTurn(row, col);
        }

        private void uxNewGame_Click(object sender, RoutedEventArgs e)
        {
            CurrentPlayer = 0;
            TurnCount = 0;
            UpdateStatusBarText();
            
            foreach (Button button in uxGrid.Children)
            {
                button.Content = "";
            }

            uxGrid.IsEnabled = true;
        }

        private void uxExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == 0 ? 1 : 0;
        }

        private void UpdateStatusBarText()
        {
            uxTurn.Text = $"{Markers[CurrentPlayer]}'s turn.";
        }

        private Button GetGridButton(int row, int column)
        {
            var buttons = uxGrid.Children;
            var result = buttons.Cast<Button>().Where(b => (string)b.Tag == $"{row},{column}").FirstOrDefault();
            return result;
        }

        private (int, int) GetButtonPosition(Button button)
        {
            var position = (string)button.Tag;
            var coord = position.Split(",");
            var row = Int32.Parse(coord[0]);
            var col = Int32.Parse(coord[1]);
            return (row, col);
        }

        private void CheckTurn(int r, int c)
        {
            if (CheckRows(r) || CheckColumns(c) || CheckDiagonals())
            {
                uxTurn.Text = $"{Markers[CurrentPlayer]} wins!";
                uxGrid.IsEnabled = false;
            }

            else
            {
                TurnCount++;
                if (TurnCount == 9)
                {
                    uxTurn.Text = $"Its a tie!";
                    uxGrid.IsEnabled = false;
                }
                
                else
                {
                    SwitchPlayer();
                    UpdateStatusBarText();
                }
            }
        }

        private bool CheckRows(int r)
        {
            var checkMarker = GetGridButton(r, 0);

            if ((string)checkMarker.Content != "")
            {
                for (var col = 1; col < 3; col++)
                {
                    var marker = GetGridButton(r, col);
                    if ((string)checkMarker.Content != (string)marker.Content)
                    {
                        return false;
                    }
                }
            }

            else return false;

            return true;
        }

        private bool CheckColumns(int c)
        {
            var checkMarker = GetGridButton(0, c);

            if ((string)checkMarker.Content != "")
            {
                for (var row = 1; row < 3; row++)
                {
                    var marker = GetGridButton(row, c);
                    if ((string)checkMarker.Content != (string)marker.Content)
                    {
                        return false;
                    }
                }
            }

            else return false;

            return true;
        }

        private bool CheckDiagonals()
        {
            // 0,0 : 1,1 : 2,2 \
            // 2,0 : 1,1 : 0,2 /

            var checkMarker = (string)GetGridButton(1, 1).Content;

            if (checkMarker != "")
            {
                var leftDiag = ((string)GetGridButton(0, 0).Content == checkMarker) && (checkMarker == (string)GetGridButton(2, 2).Content);
                var rightDiag = ((string)GetGridButton(2, 0).Content == checkMarker) && (checkMarker == (string)GetGridButton(0,2).Content);
                return leftDiag || rightDiag;
            }

            else return false;
        }
    }
}
