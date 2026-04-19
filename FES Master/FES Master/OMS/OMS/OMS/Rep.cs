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
    public partial class Rep : Form
    {
        public Rep()
        {
            InitializeComponent();
        }

        private async void Rep_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async(null);
            string powerBIlink = "https://app.powerbi.com/view?r=eyJrIjoiMWIzOTI1ZDQtZTczYS00MzNhLTlmNjItMDJiZWE5NjViYzI3IiwidCI6IjIyNjgyN2Q2LWE5ZDAtNDcwZC04YzE1LWIxNDZiMDE5MmQ1MSIsImMiOjh9";
            webView21.Source = new Uri(powerBIlink);
        }
    }
}
