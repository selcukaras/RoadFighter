using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace roadfight
{
    public partial class Form1 : Form
    {
        private PictureBox[] cizgiler;
        private PictureBox[] canlar;
        private int hiz, sag_sol, tick, can, yakit, yakit_tick, konum, konum_tick, hiz_tick;
        private bool yanma;

        public Form1()
        {
            InitializeComponent();
            hiz = 5;
            can = 3;
            yakit = 100; konum = 0;
            timer1.Enabled = false;
            yanma = false;
            sag_sol = 0;
            cizgiler = new PictureBox[5];
            cizgiler[0] = cizgi1;
            cizgiler[1] = cizgi2;
            cizgiler[2] = cizgi3;
            cizgiler[3] = cizgi4;
            cizgiler[4] = cizgi5;
            canlar = new PictureBox[3];
            canlar[0] = can1;
            canlar[1] = can2;
            canlar[2] = can3;
        }

        private void Yanma()
        {
            Rectangle rect_arabam = new Rectangle(arabam.Location, arabam.Size);
            Rectangle rect_araba1 = new Rectangle(araba1.Location, araba1.Size);
            Rectangle rect_araba2 = new Rectangle(araba2.Location, araba2.Size);
            bool carpma = (arabam.Left < yol.Left || arabam.Right > yol.Right || 
                rect_arabam.IntersectsWith(rect_araba1) || rect_arabam.IntersectsWith(rect_araba2)) && !yanma;
            if (carpma)
            {
                yanma = true; can--;
                if (can < 0)
                {
                    label7.Visible = true;
                    timer1.Enabled = false;
                }
                arabam.Left = 232;
                int i = 2;
                while (i >= 0)
                {
                    canlar[i].Visible = false;
                    if (i == can)
                    {
                        break;
                    }
                    i--;
                }

            }
        }

        private void Arabalar()
        {
            Random rnd = new Random();
            int r = rnd.Next(50);
            if (araba1.Top > 600 && (r == 10 || r == 15))
            {
                araba1.Top = -200;
                araba1.Left = rnd.Next(188, 284);
            }
            if (araba2.Top > 600 && (r == 20 || r == 25))
            {
                araba2.Top = -400;
                araba2.Left = rnd.Next(188, 284);
            }

            araba1.Top += hiz - 2;
            araba2.Top += hiz - 2;
        }

        private void Hareket()
        {
            if (yanma)
            {
                tick++;
                arabam.Visible = !arabam.Visible;
                if (tick > 80)
                {
                    yanma = false; arabam.Visible = true;
                    tick = 0;
                }
            }
            if (sag_sol == 1)
            {
                arabam.Left += 3;
            }
            else if (sag_sol == 2)
            {
                arabam.Left -= 3;
            }
            Arabalar();
            Yanma();
            foreach (PictureBox cizgi in cizgiler)
            {
                cizgi.Top += hiz;
                if (cizgi.Top > 480)
                {
                    cizgi.Top = -120;
                }
            }
            sag1.Top += hiz;
            sag2.Top += hiz;
            sol1.Top += hiz;
            sol2.Top += hiz;
            if (sag2.Top > 0)          {
                sag1.Top = sag2.Top - 385;
            }
            if (sag1.Top > 0)
            {
                sag2.Top = sag1.Top - 385;
            }
            if (sol2.Top > 0)
            {
                sol1.Top = sol2.Top - 385;
            }
            if (sol1.Top > 0)
            {
                sol2.Top = sol1.Top - 385;
            }
            Gosterge();
            yakit_tick++; konum_tick++; hiz_tick++;
            if (yakit_tick > 150) { yakit_tick = 0; yakit--; if (yakit < 0) { yakit = 0; label7.Visible = true; timer1.Enabled = false; } }
            if (konum_tick > 200) { konum_tick = 0; konum += hiz; }
            if (hiz_tick > 3000) { hiz_tick = 0; hiz++; if (hiz > 10) hiz = 10; }
        }

        private void Gosterge()
        {
            label2.Text = (hiz * 10).ToString("000");
            progressBar1.Value = yakit;
            label1.Text = "% " + yakit.ToString();
            araba_konum.Top = 340 - konum;
            if (araba_konum.Top < label4.Bottom + 5)
            {
                label6.Visible = true;
                label6.Text = "FINISH";
                timer1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Hareket();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                sag_sol = 2;
            }
            else if (e.KeyCode == Keys.Right)
            {
                sag_sol = 1;
            }
            else if (e.KeyCode == Keys.Up && can >= 0)
            {
                timer1.Enabled = true;
                label6.Text = "START";
                label6.Visible = false;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            sag_sol = 0;
        }

        private void durdurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            label6.Text = "DURDURULDU";
            label6.Visible = true;
        }

        private void devamEtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (can < 0) return;
            timer1.Enabled = true;
            label6.Text = "START";
            label6.Visible = false;
        }

        private void yeniOyunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hiz = 5;
            can = 3;
            yakit = 100; konum = 0;
            timer1.Enabled = false;
            yanma = false;
            sag_sol = 0;
            can1.Visible = true; can2.Visible = true; can3.Visible = true;
            label6.Visible = true;
            label7.Visible = false;
            label6.Text = "START";
            araba1.Location = new Point(198, 220);
            araba2.Location = new Point(288, 33);
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
