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

namespace ArduinoGUI
{
    public partial class Form1 : Form
    {
        // string to hold the __ intensity
        //private string intensity;
        // delegate: date bridge btwn GUI thread with serial thread
        public delegate void d1(string indata);
        // hold data
        //private static int counter;

        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1 = new SerialPort("COM11", 9600);
            /*
            try
            {
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open serial port: {ex.Message}");
            }*/
        }   

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open serial port: {ex.Message}");
                }
                finally
                {
                    // Ensure the serial port is closed after each iteration
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.Close();
                    }
                }
            }
            // storing serialPort1 data in indata
            string indata = serialPort1.ReadLine();
            d1 writeit = new d1(Write2Form);
            while (!this.IsHandleCreated)
                // Wait until the form's handle is created to avoid InvalidOperationException
                System.Threading.Thread.Sleep(100);
            // marshal the call to the GUI thread
            BeginInvoke(writeit, indata);

            /*textBox1.Text = indata; (RM'ed. write inside Write2Form to fix the cross-thread operation error) */
        }

        public void Write2Form(string indata) 
        {
            // this txn handles data  sent form the Arduino
            char firstchar;
            Single numdata;
            Single volts;
            firstchar = indata[0];
            textBox1.Text = indata;
            switch (firstchar)
            {
                /*case 'p':   // button count
                    counter++;
                    textBox1.Text =  Convert.ToString(counter); // counter: int -> str
                    break;*/

                case 'v':   //potentiometer
                    numdata = Convert.ToSingle(indata.Substring(1));
                    volts = numdata / 1024 * 5;
                    textBox2.Text = string.Format("{0:0.00}", volts);   // format voltage as str with 2dp
                    progressBar1.Value = Convert.ToInt16(indata.Substring(1));
                    break;

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
