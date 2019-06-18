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

        public int CountButton = 30; // how many will be bombs
        private int BoombsSet = 0; // текущее количество бомб!!!
        private int widht;
        private int height;
        private int DistanceButtonWidht;
        private int DistanceButtonHeight;
        private bool FlagEndGame = false; // flag end game
        private bool StartGame = true; // flag start game
        ButtonExtended[,] AllButtons;

        /// <summary>
        /// Init buttons size and init form1
        /// </summary>
        public Form1()
        {
            Data.BoomCount = Properties.Settings.Default.Booms; // 
            CountButton = Properties.Settings.Default.Count;
            widht = this.ClientSize.Width * 2;
            height = this.ClientSize.Height * 2;
            DistanceButtonWidht = widht / CountButton;
            DistanceButtonHeight = height / CountButton;
            InitializeComponent();
            
        }

        /// <summary>
        /// Extended for Button
        /// </summary>
        class ButtonExtended : Button
        {
            public int yb;
            public int xb;
            public bool Isbomb;
            public bool tempstate;

        }

        /// <summary>
        /// Add button to form and add them to array. Also to some buttons add bombs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            AllButtons = new ButtonExtended[widht, height];
            Random rand = new Random();
            for (int y = 0; y < CountButton; y++)
            {
                for (int x = 0; x < CountButton; x++)
                {
                    ButtonExtended button = new ButtonExtended
                    {
                        TabStop = false
                    };
                    if (rand.Next(1, 100) < 20) // 20% that button will with bomb
                    {
                        BoombsSet++;
                        button.Isbomb = true;
                    }
                    button.Location = new Point(10 + x * DistanceButtonWidht, 37 + y * DistanceButtonHeight); // it's a constant to place the button(this's not correct, but without it looks ugly)
                    button.Size = new Size(DistanceButtonWidht, DistanceButtonHeight);
                    Controls.Add(button);
                    button.MouseDown += new MouseEventHandler(this.Button_RightClick); // subscribe to event(right and left click)
                    button.Click += new EventHandler(ClickEvent);
                    AllButtons[x, y] = button;
                    button.xb = x;
                    button.yb = y;
                }
            }

        }

        /// <summary>
        /// Event if have clicked right button.(change color (it's flag))
        /// </summary>  
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    button.Text = "L";
                    button.BackColor = Color.Yellow;
                }
                
               
            }
        }
        /// <summary>
        /// count bombs around button
        /// </summary>
        /// <param name="xb"></param>
        /// <param name="yb"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns>return false if it's not number</returns>
        bool CheckBut(ButtonExtended button)
        {
            for(int j = 0;j<10;j++)
            {
                if (button.Text.Equals(j.ToString())) return false;
            }
            return true;
        }

        void Algorithm(int xb,int yb,int digit, EventArgs e)
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
                            if (AllButtons[x, y].tempstate == true) continue;
                            ClickEvent(AllButtons[x, y], e);
                        }
                        
                    }
                }
            }
        }
        /// <summary>
        /// Check all button in array.
        /// </summary>
        /// <returns></returns>
        bool CheckWin()
        {
            for (int y = 0; y < CountButton; y++)
            {
                for (int x = 0; x < CountButton; x++)
                {
                    if (AllButtons[x, y].Isbomb && AllButtons[x, y].BackColor == Color.Yellow) continue;// if all buttons with bombs with flags
                    if (AllButtons[x, y].Isbomb == false && CheckBut(AllButtons[x, y]) == false) continue;// and other buttons without bombs open
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// if we click left button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClickEvent(object sender, EventArgs e)
        {
            if (CheckWin()) //if we win show dialog
            {
                DialogResult result = MessageBox.Show(
                        "you win",
                        "Window",
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

            var senderB = (ButtonExtended)sender;// temp var

            Attention
            //if (CheckBut(senderB) == false) Algorithm(senderB.xb,senderB.yb, int.Parse(senderB.Text),e); // This's an algorithm when you press a button with numbers already opened. (It's not necessary)
            Attention
            if (senderB.BackColor != Color.Yellow)
            {
                if (senderB.Isbomb == true && FlagEndGame != true && StartGame != true) // if't a bomb, and not the first click(if the buttons didn't open earlier)
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
                        "you lost",
                        "Message",
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
                else if (senderB.Isbomb == false) // if it's not bomb then open it
                {
                    StartGame = false;
                    senderB.Text = CheckDigit(senderB.xb, senderB.yb).ToString();
                    if (CheckDigit(senderB.xb, senderB.yb) == 0) senderB.BackColor = Color.White;
                }
                else if (senderB.Isbomb == true && FlagEndGame != true && StartGame == true) // if it's first click then change button on common button(without bomb)
                {
                    senderB.Isbomb = false;
                    StartGame = false;
                    ClickEvent(sender, e);
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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void оПриложенииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }
    }
    
}
