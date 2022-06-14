using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Empacadora
{
    public partial class FrmEmpacadora : Form
    {

        private string datoSerial = "";
        private int dato = 0;
        public FrmEmpacadora()
        {
            InitializeComponent();
            string[] puertos = SerialPort.GetPortNames();
            foreach (string puerto in puertos)
            {
                comboBox1.Items.Add(puerto);
            }
        }

        private void FrmEmpacadora_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                try
                {
                    if (!serialPort1.IsOpen)
                    {
                        serialPort1.Open();
                        checkBox1.Text = "Puerto abierto";
                        checkBox1.BackColor = Color.LightGreen;
                        numericUpDown1.Enabled = true;
                    }
                }
                catch (System.IO.IOException error)
                {
                    MessageBox.Show(error.Message);
                    checkBox1.Checked = false;
                }
            }
            else
            {
                try
                {
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.Close();
                        checkBox1.Text = "Puerto cerrado";
                        checkBox1.BackColor = Color.LightYellow;
                        numericUpDown1.Enabled = false;
                    }
                }
                catch (System.IO.IOException error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                serialPort1.ReadTimeout = 200;
                datoSerial = serialPort1.ReadTo("\r\n");
                Invoke(new EventHandler(RecibirTexto));
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                serialPort1.DiscardInBuffer();
            }
        }

        private void RecibirTexto(object sender, EventArgs e)
        {
            textBox1.Text = datoSerial;
            AnalizarTexto(datoSerial);
        }

        private void AnalizarTexto(string cadena)
        {
            if (cadena.Contains(":"))
            {
                dato = Convert.ToInt32(cadena.Substring(40));
            }
            if (dato % numericUpDown1.Value == 0)
            {
                label4.ForeColor = Color.White;
                label4.Text = "Caja llena";
                label4.BackColor = Color.Red;
            }
            else
            {
                label4.ForeColor = Color.Black;
                label4.Text = "Recibiendo cosas";
                label4.BackColor = Color.Yellow;
            }
        }
    }
}
