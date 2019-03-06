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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            richTextBox1.Text = "Это прога была написана Волковым Дмитрием.\n По всем вопросам и пожеланиям пишите на почту" +
                " hot-gun@mail.ru \n" +
                "Just for fun))))";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
