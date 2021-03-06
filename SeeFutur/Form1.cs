using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace SeeFutur
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "ULTIMATE_PREDICTOR";
        private readonly string PREDICTION_CONFIG_PATH = $"{Environment.CurrentDirectory}\\predictionConfig.json";
        private string[] _predictions;
        private Random _random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            buttonPredict.Enabled = false;
            await Task.Run(async () =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    this.Invoke(new Action(() =>
                    {

                        UpdateProgressBar(i);
                        this.Text = $"{i} %";

                    }));
                    Thread.Sleep(20);
                }
            });

            var index = _random.Next(_predictions.Length);
            
            var prediction = _predictions[index];

            MessageBox.Show($"{prediction}!");

            progressBar1.Value = 0;
            this.Text = APP_NAME;
            buttonPredict.Enabled = true;
        }

        private void UpdateProgressBar(int i)
        {
            if (i == progressBar1.Maximum)
            {
                progressBar1.Maximum = i + 1;
                progressBar1.Value = i + 1;
                progressBar1.Maximum = i;
            }
            else
            {
                progressBar1.Value = i + 1;
            }
            progressBar1.Value = i;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = APP_NAME;

            try
            {
                var data = File.ReadAllText(PREDICTION_CONFIG_PATH);
                _predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_predictions == null)
                {
                    Close();
                }
                else if (_predictions.Length == 0)
                {
                    MessageBox.Show("Предсказания закончились, кина не будет! =)");
                    Close();
                }
            }
        }
    }
}
