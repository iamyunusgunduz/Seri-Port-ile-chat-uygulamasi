using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seriPortChatApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var seriport in SerialPort.GetPortNames())
            {
                comboBoxPorts.Items.Add(seriport);
            }
            comboBoxPorts.SelectedIndex = 0;
            buttonDisconnect.Enabled = false;
            buttonSend.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBoxPorts.Text;
            serialPort1.BaudRate = 9600;
            serialPort1.Parity = Parity.Even;
            serialPort1.StopBits = StopBits.One;
            serialPort1.DataBits = 8;
            try
            {
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Seri portu bulamadım\n Hata :{ex.Message}", "Porblem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
                buttonSend.Enabled = true;
            }

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
                buttonSend.Enabled = false;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(textBoxSend.Text + "\n");
                textBoxSend.Clear();
            }
        }

        public delegate void veriGoster(string s);
        public void textBoxYaz(string s)
        {
            textBoxReceived.Text += s;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String gelenVeri = serialPort1.ReadExisting();
            textBoxReceived.Invoke(new veriGoster(textBoxYaz), gelenVeri);
        }
    }
}
