using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMS
{
    public partial class REPORTS : Form
    {
        public REPORTS()
        {
            InitializeComponent();
        }

        private async void REPORTS_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async(null);
            string powerbi = "https://app.powerbi.com/view?r=eyJrIjoiNjA0NjQ4NDctMjI5Ny00NGNiLWFhMDQtNTU2NDhiOWFlZDVlIiwidCI6IjIyNjgyN2Q2LWE5ZDAtNDcwZC04YzE1LWIxNDZiMDE5MmQ1MSIsImMiOjh9";
            webView21.Source = new Uri(powerbi);

            

        }

        private void webView21_Click(object sender, EventArgs e)
        {
            
        }
    }
}
