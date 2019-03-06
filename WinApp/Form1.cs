using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp
{
    
    public partial class Form1 : Form
    {

        public int CountButton = 30; // нужно сделать чтобы это вводил игрок
        private int BoombsSet = 0; // текущее количество бомб!!!
        private int widht;
        private int height;
        private int DistanceButtonWidht;
        private int DistanceButtonHeight;
        private bool FlagEndGame = false;
        ButtonExtended[,] AllButtons;

        public Form1()
        {
            Data.BoomCount = Properties.Settings.Default.Booms;
            CountButton = Properties.Settings.Default.Count;
            //CountButton = 10;
            widht = this.ClientSize.Width * 2;
            height = this.ClientSize.Height * 2;
            //widht = this.Size.Width * 2-11;
            //height = this.Size.Height * 2-73;
            //MessageBox.Show(this.ClientSize.ToString());
            DistanceButtonWidht = widht / CountButton;
            DistanceButtonHeight = height / CountButton;
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AllButtons = new ButtonExtended[widht, height];
            Random rand = new Random();
            for (int y = 0; y < CountButton; y++)
            {
                for (int x = 0; x < CountButton; x++)
                {
                    ButtonExtended button = new ButtonExtended();
                    
                    button.TabStop = false;
                    if (rand.Next(1, 100) < 20)
                    {
                        
                        BoombsSet++;
                        button.Isbomb = true;
                    }
                    button.Location = new Point(10 + x * DistanceButtonWidht, 37 + y * DistanceButtonHeight); // поле постоянно смещенно на 15 !!!!
                    button.Size = new Size(DistanceButtonWidht, DistanceButtonHeight);
                    Controls.Add(button);
                    button.MouseDown += new MouseEventHandler(this.Button_RightClick);
                    button.Click += new EventHandler(ClickEvent);
                    AllButtons[x, y] = button;
                    button.xb = x;
                    button.yb = y;
                }
            }

        }
        void Button_RightClick(object sender, MouseEventArgs e)
        {
            ButtonExtended button = (ButtonExtended)sender;
            if (e.Button == MouseButtons.Right && CheckBut(button))
            {
               
                
                if (button.BackColor == Color.Yellow)
                {
                    button.BackColor = default(Color);
                    button.Text = "";
                }
                else
                {
                    //MessageBox.Show((button.BackColor.ToString()));
                    button.Text = "L";
                    button.BackColor = Color.Yellow;// Нужно подумать над проверкой флашкоф!!!!!!!!
                    //MessageBox.Show("Left tButton");
                }
                
               
            }
        }

        int CheckDigit(int xb,int yb)
        {
            int CountBomb = 0;
            for (int y = yb-1; y <= yb+1; y++)
            {
                for (int x = xb - 1; x<= xb + 1; x++)
                {
                    if(0<=x && x<CountButton && 0<=y && y<CountButton && AllButtons[x,y].Isbomb )
                    {
                        CountBomb++;
                    }
                }
            }
            return CountBomb;
        }

        bool CheckBut(ButtonExtended button)
        {
            for(int j = 0;j<10;j++)
            {
                if (button.Text.Equals(j.ToString())) return false;
            }
            return true;
        }

        void Algoritm(int xb,int yb,int digit, EventArgs e)
        {
            int digitTemp = 0;
            for (int y = yb - 1; y <= yb + 1; y++)
            {
                for (int x = xb - 1; x <= xb + 1; x++)
                {
                    if (0 <= x && x < CountButton && 0 <= y && y < CountButton && AllButtons[x, y].Text == "L") digitTemp++;
                }
            }
            if(digitTemp == digit)
            {
                AllButtons[xb, yb].tempstate = true;
                for (int y = yb - 1; y <= yb + 1; y++)
                {
                    for (int x = xb - 1; x <= xb + 1; x++)
                    {
                        if (0 <= x && x < CountButton && 0 <= y && y < CountButton)
                        {
                            //if (x == xb && y == yb || AllButtons[x, y].tempstate==true) continue;
                            if (AllButtons[x, y].tempstate == true) continue;
                            ClickEvent(AllButtons[x, y], e);
                        }
                        
                    }
                }
            }
        }
        bool CheckWin()
        {
            for (int y = 0; y < CountButton; y++)
            {
                for (int x = 0; x < CountButton; x++)
                {
                    if (AllButtons[x, y].Isbomb && AllButtons[x, y].BackColor == Color.Yellow) continue;
                    if (AllButtons[x, y].Isbomb == false && CheckBut(AllButtons[x, y]) == false) continue;
                    return false;
                }
            }
            return true;
        }
        void ClickEvent(object sender, EventArgs e)
        {
            if (CheckWin())
            {
                DialogResult result = MessageBox.Show(
                        "Вы выйграли",
                        "Сообщение",
                        MessageBoxButtons.RetryCancel
                        );
                if (DialogResult.Retry == result)
                {
                    Application.Exit();
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                }
                else
                {
                    this.Close();
                }
            }
            var senderB = (ButtonExtended)sender;
            if (CheckBut(senderB) == false) Algoritm(senderB.xb,senderB.yb, int.Parse(senderB.Text),e); // самая главная функция по красивой игре!!!!
            if (senderB.BackColor != Color.Yellow)
            {
                if (senderB.Isbomb == true && FlagEndGame != true)
                {
                    FlagEndGame = true;
                    for (int y = 0; y < CountButton; y++)
                    {
                        for (int x = 0; x < CountButton; x++)
                        {
                            if (AllButtons[x, y].Isbomb)
                            {
                                AllButtons[x, y].Text = "*";
                                AllButtons[x, y].BackColor = Color.Red;
                            }
                            else
                            {
                                AllButtons[x, y].Text = CheckDigit(x, y).ToString();
                                
                            }
                        }
                    }

                    DialogResult result = MessageBox.Show(
                        "Вы проиграли",
                        "Сообщение",
                        MessageBoxButtons.RetryCancel
                        );
                    if (DialogResult.Retry == result)
                    {
                        Properties.Settings.Default.Count = CountButton;
                        Properties.Settings.Default.Save();
                        Application.Exit();
                        System.Diagnostics.Process.Start(Application.ExecutablePath);
                    }


                }
                else if (senderB.Isbomb == false)
                {
                    senderB.Text = CheckDigit(senderB.xb, senderB.yb).ToString();
                    if (CheckDigit(senderB.xb, senderB.yb) == 0) senderB.BackColor = Color.White;
                }
            }
        }

        private void выбратьРазмерПоляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(CountButton);
            this.Hide();
            //Register form closed event
            form2.FormClosed += new FormClosedEventHandler(form2_FormClosed);

            Visible = false;

            form2.ShowDialog();
            
        }

        void form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            CountButton = Data.DataButton;
            Properties.Settings.Default.Count = CountButton;
            Properties.Settings.Default.Save();
            Application.Exit();
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            //Application.Run(new Form1());

            //this.Show();
            //this.Visible = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void оПриложенииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            //this.Hide();
            //Register form closed event
            //form2.FormClosed += new FormClosedEventHandler(form2_FormClosed);

           

            form3.ShowDialog();
        }
    }

    class ButtonExtended : Button
    {
        public int yb;
        public int xb;
        public bool Isbomb;
        public bool tempstate;

    }
}
