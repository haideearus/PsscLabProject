using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
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
            var userExists = await VerifyUserAsync(email, parola);

            if (userExists)
            {
                Shop shop = new Shop(email);
                shop.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("A apărut o eroare. Vă rugăm să încercați din nou.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<bool> VerifyUserAsync(string email, string password)
        {
            // Aici adăugați logica pentru a verifica utilizatorul
            // folosind un serviciu API sau o bază de date locală.

            // Exemplu de apel API:
            var client = new HttpClient();
            try
            {
                Console.WriteLine("Aici");
                var response = await client.GetAsync("https://localhost:7195/api/users");
                Console.WriteLine("Treci");
                if (response.IsSuccessStatusCode)
                {

                    Debug.WriteLine("Mesaj de debug");
                    var result = await response.Content.ReadAsAsync<dynamic>();
                    
                    foreach(var item in result)
                    {
                        Console.WriteLine(item);
                        if (item["email"]?.ToString() == email)
                        {
                            if(item["password"]?.ToString() == password)
                            {
                                return true;
                            }
                        }
                    }
                    return result==null;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            

            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

