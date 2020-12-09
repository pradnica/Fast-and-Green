using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fast_and_Green
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight;
        int speed = 5;
        int score = 0;
        int missed = 0;

        Random randX = new Random();
        Random randY = new Random();

        PictureBox trash_hit = new PictureBox();

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainGame(object sender, EventArgs e) //event do Timera 
        {
            txtScore.Text = "Score: " + score; //wyswietlanie zmiennej score w labelu
            txtMissed.Text = "Missed: " + missed; //wyswietlanie zmiennej missed w labelu 

            //przesuwanie gracza w zaleznosci od 
            //player.Left pozwala na okreslenie odleglosci danego elementu od krawedzi kontenerwa, w ktorym element sie znajduje 
            if (goLeft == true && player.Left > 1)
            {
                player.Left -= 35; //przesuwanie gracza o 30 px w lewo z kazdym kliknieciem strzalki
                player.Image = Properties.Resources.smietnik_lewo; //zmiana obrazu gracza na "lewy"
            }


            if (goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += 35; //szybkosc, z jaka gracz moze przesuwac obiektem w prawo
                player.Image = Properties.Resources.smietnik_prawo; //zmiana obrazu gracza na "prawy"
            }


            //obsluga spadania smieci z gory 
            foreach (Control x in this.Controls)
            {
                //jesli kontrolka x jest typu PictureBox i ma tag "smieci"
                if (x is PictureBox && (string)x.Tag == "trash")
                {
                    x.Top += speed; //Top - pobiera odleglosc miedzy gornba krawedzia kontrolki a gorna krawedzia kontenera



                    if (x.Top + x.Height > this.ClientSize.Height)
                    {
                        trash_hit.Image = Properties.Resources.smiec_zbity;

                        //okreslanie parametrow PictureBoxa odpowiadajacemu pominietemu smieciowi 
                        trash_hit.Location = x.Location;
                        trash_hit.Height = 60;
                        trash_hit.Width = 60;
                        trash_hit.BackColor = Color.Transparent;

                        this.Controls.Add(trash_hit); //w miejscu w ktorym wystapi pomioniety smiec pojawi sie symbolizujacy to obrazek

                        //losowanie polozenia spadjaych jaj wzgledem gornej i lewej krawedzi kontenera 
                        x.Top = randY.Next(50, 100) * -1;
                        x.Left = randX.Next(10, this.ClientSize.Width - x.Width);

                        missed += 1;
                        player.Image = Properties.Resources.smietnik_ominal;
                    }

                    //co sie stanie gdy gracz dotkanie krawedzi kontrolki, ktora spada 
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        x.Top = randY.Next(50, 100) * -1;
                        x.Left = randX.Next(10, this.ClientSize.Width - x.Width);
                        score += 1;
                    }

                }

            }

            //przyspieszanie predkosci spadajacych elementow jesli zdobedzie sie 10 punktow 
            if (score > 10)
            {
                speed = 12;
            }

            //koniec gry i wyswietlenie powiadomienia jesli nie zbierze sie 5 smieci 
            if (missed > 5)
            {
                GameTimer.Stop();
                MessageBox.Show("Game over!" + Environment.NewLine + "You scored " + score);
                RestartGame();
            }
            
        


    }

        //funkcja sprawdzajaca, czy wcisnietete sa przyciski odpowiednio strzalki w lewo i strzalki w prawo

        //jesli jest wcisniety dany przycisk to mozna poruszyc sie w dana strone 
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }

        }

        //funkcja sprawdzajaca, czy przyciski strzalki w lewo/prawo zostały zwolnone 
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }

        }

        private void RestartGame()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "trash")
                {
                    x.Top = randY.Next(50, 100) * -1;
                    x.Left = randX.Next(10, this.ClientSize.Width - x.Width);
                }
            }

            player.Left = this.ClientSize.Width / 2; //gracz jest na srodku kontenera 
            player.Image = Properties.Resources.smietnik_prawo;

            score = 0;
            missed = 0;
            speed = 8;

            goLeft = false;
            goRight = false;

            GameTimer.Start();

        }
    }
}
