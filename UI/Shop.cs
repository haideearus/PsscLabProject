﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UI
{
    public partial class Shop : Form
    {
        public string Name { get; set; }
        public Shop()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Numele primit este: " + Name);
        }
    }
}
