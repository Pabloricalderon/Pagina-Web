using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaginaWeb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnIr_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text.Contains("https://www"+ ".com"))
            {
                webBrowser1.Navigate(new Uri(comboBox1.SelectedItem.ToString()));
            }
            else if(comboBox1.Text.Contains(".com"))
            {
                string busqueda = comboBox1.Text;
                comboBox1.Text = "https://www." + busqueda;
                webBrowser1.Navigate(new Uri(comboBox1.Text.ToString()));
            }
            else
            {
                string palabra = comboBox1.Text;
                comboBox1.Text = "https://www.google.com/search?q=" + palabra;
                webBrowser1.Navigate(new Uri(comboBox1.Text.ToString()));
            }


          

        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        private void haciaAtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void haciaAdelanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            webBrowser1.GoHome();
        }
    }
}
