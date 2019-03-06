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
    
    public partial class Form2 : Form
    {
        private int CountButton; // количество кнопок
        public Form2(int CountButton)
        {
            this.CountButton = CountButton;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            InitValue();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void InitValue()
        {
            if (CountButton == 10)
            {
                radioButton1.Checked = true;
                label1.Text = "10";
                label5.Text = "10";
                label6.Text = "хз"; // на первое время 15 мин пусть будет!!!
            }  
            if (CountButton == 20)
            {
                radioButton3.Checked = true;
                label1.Text = "20";
                label5.Text = "20";
                label6.Text = "хз"; // нужно поменять 
            }
            if (CountButton == 30)
            {
                radioButton2.Checked = true;
                label1.Text = "30";
                label5.Text = "30";
                label6.Text = "хз"; // нужно поменять
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            ChangeValue(radioButton1);
            ChangeValue(radioButton2);
            ChangeValue(radioButton3);
        }
        private void ChangeValue(RadioButton radio)
        {
            if(radio.Checked)
            {
                if(radio.Text == "Новичёк")
                {
                    Data.DataButton = 10;
                    Data.BoomCount = 15;
                    CountButton = 10;
                    InitValue();
                }
                if (radio.Text == "Профи")
                {
                    Data.DataButton = 20;
                    Data.BoomCount = 15; // нужно будет поменять!!!
                    CountButton = 20;
                    InitValue();
                }
                if (radio.Text == "Эксперт")
                {
                    Data.DataButton = 30;
                    Data.BoomCount = 15; // нужно будет поменять!!!
                    CountButton = 30;
                    InitValue();
                }
            }
        }


    }
}
