using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App
{
    public partial class MainPage : ContentPage
    {
        Dictionary<Button, Position> ButtonPosition = new Dictionary<Button, Position>();
        char[,] state = new char[3,3];
        public MainPage()
        {
            InitializeComponent();

        }

        private void bStart_Clicked(object sender, EventArgs e)
        {
            bStart.IsVisible = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button button = new Button();
                    button.IsVisible = true;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    button.BackgroundColor = Color.Black;
                    button.TextColor = Color.White;
                    button.FontSize = 64;
                    button.Clicked += Button_Clicked;
                    g.Children.Add(button);
                    ButtonPosition.Add(button, new Position() { X = j, Y = i }) ;
                    state[j, i] = ' ';
                }
            }
        }
        private void Clear()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    state[j, i] = ' ';
                }
            }
            foreach(var kvp in ButtonPosition)
            {
                kvp.Key.Text = " ";
            }
        }

        private async Task<bool> Check()
        {
            for (int i = 0; i < 3; i++)
            {
                bool row = true;
                for(int j = 0;j < 3; j++)
                {
                    if (!(state[j, i] == 'X'))
                        row = false;
                }
                if (row)
                {
                    await DisplayAlert("Game Over", "You Won", "Close");
                    Clear();
                    return true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                bool column = true;
                for(int j = 0; j < 3; j++)
                {
                    if (!(state[i, j] == 'X'))
                        column = false;
                }
                if (column)
                {
                    await DisplayAlert("Game Over", "You Won", "Close");
                    Clear();
                    return true;
                }
            }
            if (state[0,0] == 'X' && state[1,1] == 'X' && state[2,2] == 'X')
            {
                await DisplayAlert("Game Over", "You Won", "Close");
                Clear(); 
                return true;
            }
            if (state[2, 0] == 'X' && state[1, 1] == 'X' && state[0, 2] == 'X')
            {
                await DisplayAlert("Game Over", "You Won", "Close");
                Clear();
                return true;
            }
            if (await DrawCheck())
            {
                await DisplayAlert("Game Over", "Draw", "Close");
                Clear();
                return true;
            }
            return false;
        }

        private async Task<bool> DrawCheck()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (state[j, i] == ' ')
                        return false;
                }
            }

            return true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var position = ButtonPosition[btn];
            state[position.X, position.Y] = 'X';
            btn.Text = "X";
            if(!await Check())
            {
                eturn();
                await Check();
            }
        }
        
        private void eturn()
        {
            Random rand = new Random();
            int x = (int)rand.Next(3);
            int y = (int)rand.Next(3);
            while(true)
            {
                if (state[x, y] == ' ')
                {
                    state[x, y] = 'O';
                    GetButtonByPosition(x, y).Text = "O";
                    break;
                }
                else
                {
                    x = (int)rand.Next(3);
                    y = (int)rand.Next(3);
                }
            }
            
        }
        private Button GetButtonByPosition(int x, int y)
        {
            foreach (var kvp in ButtonPosition)
            {
                if((x == kvp.Value.X) && (y == kvp.Value.Y))
                {
                    return kvp.Key;
                }
            }
            return null;
        }
    }
}
