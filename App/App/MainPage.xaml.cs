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
        string[,] state = new string[3,3];
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
                    state[j, i] = " ";
                }
            }
        }
        private void Clear()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    state[j, i] = " ";
                }
            }
            foreach(var kvp in ButtonPosition)
            {
                kvp.Key.Text = " ";
            }
        }

        private async Task ScaleSelect(string x)
        {
            foreach (var kvp in ButtonPosition)
            {
                if(kvp.Key.Text == x)
                {
                    await kvp.Key.ScaleTo(1.5);
                    await kvp.Key.ScaleTo(1);
                }
            }
        }

        private async Task ScaleAll()
        {
            await g.ScaleTo(1.5);
            await g.ScaleTo(1);
        }

        private async Task<bool> Check(string x, string s)
        {
            for (int i = 0; i < 3; i++)
            {
                bool row = true;
                for(int j = 0;j < 3; j++)
                {
                    if (!(state[j, i] == x))
                        row = false;
                }
                if (row)
                {
                    await ScaleSelect(x);
                    await DisplayAlert("Game Over", s, "Close");
                    Clear();
                    return true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                bool column = true;
                for(int j = 0; j < 3; j++)
                {
                    if (!(state[i, j] == x))
                        column = false;
                }
                if (column)
                {
                    await ScaleSelect(x);
                    await DisplayAlert("Game Over", s, "Close");
                    Clear();
                    return true;
                }
            }
            if (state[0,0] == x && state[1,1] == x && state[2,2] == x)
            {
                await ScaleSelect(x);
                await DisplayAlert("Game Over", s, "Close");
                Clear(); 
                return true;
            }
            if (state[2, 0] == x && state[1, 1] == x && state[0, 2] == x)
            {
                await ScaleSelect(x);
                await DisplayAlert("Game Over", s, "Close");
                Clear();
                return true;
            }
            for (int i = 0; i < 3; i++)
            {
                bool row = true;
                for (int j = 0; j < 3; j++)
                {
                    if (!(state[j, i] == x))
                        row = false;
                }
                if (row)
                {
                    await ScaleSelect(x);
                    await DisplayAlert("Game Over", s, "Close");
                    Clear();
                    return true;
                }
            }
            if (await DrawCheck())
            {
                await ScaleAll();
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
                    if (state[j, i] == " ")
                        return false;
                }
            }

            return true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var position = ButtonPosition[btn];
            if(state[position.X, position.Y] == " ")
            {
                state[position.X, position.Y] = "X";
                btn.Text = "X";
                await btn.FadeTo(0);
                await btn.FadeTo(1);
            }
            else
            {
                await DisplayAlert("Misinput", "Don't try to overwrite positions", "Sorry >~<");
            }
            if (!await Check("X", "You won"))
            {
                await eturn();
                await Check("O", "You Lost");
            }
        }
        
        private async Task eturn()
        {
            Random rand = new Random();
            int x = (int)rand.Next(3);
            int y = (int)rand.Next(3);
            while(true)
            {
                
                if (state[x, y] == " ")
                {
                    Button btn = await GetButtonByPosition(x, y);
                    state[x, y] = "O";
                    btn.Text = "O";
                    await btn.FadeTo(0);
                    await btn.FadeTo(1);
                    break;
                }
                else
                {
                    x = (int)rand.Next(3);
                    y = (int)rand.Next(3);
                }
            }
        }
        private async Task<Button> GetButtonByPosition(int x, int y)
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
