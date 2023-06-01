using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AsyncPortScanner
{
    public partial class AsyncPortScanner : Form
    {
        public AsyncPortScanner()
        {
            InitializeComponent();
        }

        private async void ScanButton_Click(object sender, EventArgs e)
        {
            string ipAddress = ipAddressTextBox.Text;
            int startPort = Convert.ToInt32(startPortTextBox.Text);
            int endPort = Convert.ToInt32(endPortTextBox.Text);

            progressBar.Minimum = startPort;
            progressBar.Maximum = endPort;
            progressBar.Value = startPort;
            progressBar.Visible = true;

            await ScanOpenPortsAsync(ipAddress, startPort, endPort);


        }

        private async Task ScanOpenPortsAsync(string ipAddress, int startPort, int endPort)
        {
            List<int> openPorts = new List<int>();

            for (int port = startPort; port <= endPort; port++)
            {
                TcpClient tcpClient = new TcpClient();

                try
                {
                    await tcpClient.ConnectAsync(ipAddress, port);
                    openPorts.Add(port);
                    tcpClient.Close();
                }
                catch (SocketException)
                {

                }

                progressBar.Value = port;
            }

            UpdateListBox(openPorts);
        }

        private void UpdateListBox(List<int> openPorts)
        {
            if (openPorts.Count > 0)
            {
                resultListBox.Items.Clear();
                foreach (int port in openPorts)
                {
                    resultListBox.Items.Add(port.ToString());
                }
            }
            else
            {
                resultListBox.Items.Clear();
                resultListBox.Items.Add("No open ports found.");
            }
        }

        private void AsyncPortScanner_Load(object sender, EventArgs e)
        {
            
                this.AutoSize = true;
                this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }
}
