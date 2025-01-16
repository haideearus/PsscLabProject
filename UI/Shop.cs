using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UI
{
    public partial class Shop : Form
    {
        public string Names { get; set; }
        //private ShoppingCart _shoppingCart = new ShoppingCart();

        public Shop(string name)
        {
            Names = name;
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Shop_Load(object sender, EventArgs e)
        {
           //MessageBox.Show("Numele primit este: " + Names);
            ShowProduct();
        }
        private async Task<bool> ShowProduct()
        {
            // Aici adăugați logica pentru a verifica utilizatorul
            // folosind un serviciu API sau o bază de date locală.

            // Exemplu de apel API:
            var client = new HttpClient();
            try
            {
                Console.WriteLine("Aici");
                var response = await client.GetAsync("https://localhost:7195/api/products");
                Console.WriteLine("Treci");
                Debug.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {

                    Debug.WriteLine("Mesaj de debug");
                    var result = await response.Content.ReadAsAsync<dynamic>();
                    dataGridViewProducts.Rows.Clear();
                    dataGridViewProducts.Columns.Add("Name", "Nume Produs");
                    dataGridViewProducts.Columns.Add("Price", "Preț");
                    dataGridViewProducts.Columns.Add("Buton", " ");
                    foreach (var item in result)
                    {
                        int rowIndex = dataGridViewProducts.Rows.Add(item.Name, item.Price, "Adaugă în coș");
                        DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridViewProducts.Rows[rowIndex].Cells[2];
                        buttonCell.Value = "Adaugă în coș";
                    }
                    return result == null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }


            return false;
        }
    }
}
