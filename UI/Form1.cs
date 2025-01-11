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

        private void usersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.usersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.pssC_DataBaseDataSet1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pssC_DataBaseDataSet1.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.pssC_DataBaseDataSet1.Users);

        }
    }
}
