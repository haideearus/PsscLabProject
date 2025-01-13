using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Welcome_Click(object sender, EventArgs e)
        {

        }

        private void Inchide_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void Conectare_Click(object sender, EventArgs e)
        {
            string email = Email.Text;
            string parola = Password.Text;

        }
    }
}
