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

namespace PaginaWeb
{
    public partial class Form1 : Form
    {
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

        private void Form_Resize(object sender, EventArgs e)
        {
            webView21.Size = this.ClientSize - new System.Drawing.Size(webView21.Location);
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
           
        }
    }
}
