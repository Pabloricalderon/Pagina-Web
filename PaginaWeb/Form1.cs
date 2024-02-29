using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using System.IO;

namespace PaginaWeb
{
    public partial class Form1 : Form
    {
        //Innicializar lista de urls
        List<Navegador> urls = new List<Navegador>();

        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            webView21.NavigationStarting += EnsureHttps;
            InitializeAsync();
        }
        async void InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.WebMessageReceived += UpdateAddressBar;

            await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }
        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            comboBox1.Text = uri;
            webView21.CoreWebView2.PostWebMessageAsString(uri);
        }
        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                webView21.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }
        private void Guardar(string fileName, string texto)
        {
            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);

            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine(texto);
            writer.Close();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView21.Size = this.ClientSize - new System.Drawing.Size(webView21.Location);
        }

        private void Grabar(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);

            foreach(var url in urls)
            {
                writer.WriteLine(url.Pagina);
                writer.WriteLine(url.VecesIngreso);
                writer.WriteLine(url.FechaIngreso);

            }

            writer.Close ();
        }

        private void btnIr_Click(object sender, EventArgs e)
        {
            //if (webView21 != null && webView21.CoreWebView2 != null)
            //{
            //    webView21.CoreWebView2.Navigate(comboBox1.Text);
            //}
           

            if (comboBox1.Text.Contains("https://www."))
            {
                webView21.CoreWebView2.Navigate(comboBox1.SelectedItem.ToString());
            }
            else if(comboBox1.Text.Contains(".com"))
            {
                string busqueda = comboBox1.Text;
                comboBox1.Text = "https://www." + busqueda;
                webView21.CoreWebView2.Navigate(comboBox1.Text.ToString());
            }
            else
            {
                string palabra = comboBox1.Text;
                comboBox1.Text = "https://www.google.com/search?q=" + palabra;
                webView21.CoreWebView2.Navigate(comboBox1.Text.ToString());
            }


            //Guardar("archivo.txt", comboBox1.Text);
            ////comboBox1.Items.Clear();
            //MessageBox.Show("Historial guardado");



            string urlIngresada = comboBox1.Text;

            Navegador urlExiste = urls.Find(u => u.Pagina == urlIngresada);

            if(urlExiste == null)
            {
                Navegador urlNueva = new Navegador();
                urlNueva.Pagina = urlIngresada;
                urlNueva.VecesIngreso = 1;
                urlNueva.FechaIngreso = DateTime.Now;
                urls.Add(urlNueva);
                Grabar("historial.txt");
                webView21.CoreWebView2.Navigate(urlIngresada);

            }
            else
            {
                urlExiste.VecesIngreso++;
                urlExiste.FechaIngreso = DateTime.Now;
                webView21.CoreWebView2.Navigate(urlExiste.Pagina);
                Grabar("historial.txt");
            }

            //Guardamos en una variable el nombre del archivo que abrimos
            string fileName = @"C:\Users\edgar\Documents\Programacion III\PaginaWeb\PaginaWeb\bin\Debug\historial.txt";

            //Abrimos el archivo, en este caso lo abrimos para lectura
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            //Un ciclo para leer el archivo hasta el final del archivo
            //Lo leído se va guardando en un control richTextBox
            while (reader.Peek() > -1)
            //Esta linea envía el texto leído a un control richTextBox, se puede cambiar para que
            //lo muestre en otro control por ejemplo un combobox
            {
                Navegador url = new Navegador();
                url.Pagina = reader.ReadLine();
                url.VecesIngreso = int.Parse(reader.ReadLine());
                url.FechaIngreso = Convert.ToDateTime(reader.ReadLine());

                urls.Add(url);
                //comboBox1.Items.Add(reader.ReadLine());
            }
            //Cerrar el archivo, esta linea es importante porque sino despues de correr varias veces el programa daría error de que el archivo quedó abierto muchas veces. Entonces es necesario cerrarlo despues de terminar de leerlo.
            reader.Close();

            comboBox1.DisplayMember = "Pagina";
            comboBox1.DataSource = urls;
            comboBox1.Refresh();

        }

        

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView21.CoreWebView2.Navigate("https://www.bing.com");
        }

        private void haciaAtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView21.CoreWebView2.GoBack();
        }

        private void haciaAdelanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView21.CoreWebView2.GoForward();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            //Guardamos en una variable el nombre del archivo que abrimos
            string fileName = @"C:\Users\edgar\Documents\Programacion III\PaginaWeb\PaginaWeb\bin\Debug\archivo.txt";

            //Abrimos el archivo, en este caso lo abrimos para lectura
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            //Un ciclo para leer el archivo hasta el final del archivo
            //Lo leído se va guardando en un control richTextBox
            while (reader.Peek() > -1)
            //Esta linea envía el texto leído a un control richTextBox, se puede cambiar para que
            //lo muestre en otro control por ejemplo un combobox
            {
                comboBox1.Items.Add(reader.ReadLine());
            }
            //Cerrar el archivo, esta linea es importante porque sino despues de correr varias veces el programa daría error de que el archivo quedó abierto muchas veces. Entonces es necesario cerrarlo despues de terminar de leerlo.
            reader.Close();

        }
    }
}
