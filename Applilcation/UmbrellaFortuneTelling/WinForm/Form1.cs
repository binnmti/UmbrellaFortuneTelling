using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenWeatherMap;

namespace UmbrellaFortuneTelling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.BeginUpdate();
            comboBox1.Items.AddRange(OpenWeatherMapCityUtil.GetCountrys().ToArray());
            comboBox1.Text = OpenWeatherMapCityUtil.GetCurrentCountry();
            comboBox1.EndUpdate();
            labelCountryCount.Text = comboBox1.Items.Count.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.BeginUpdate();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(OpenWeatherMapCityUtil.GetCitys(comboBox1.Text).ToArray());
            comboBox2.Text = comboBox2.Items[0].ToString();
            comboBox2.EndUpdate();
            labelCityCount.Text = comboBox2.Items.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var report = new OpenWeatherMapWeatherReport();
            report.Update(comboBox2.Text);
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (var data in report.TodayWeatherDatas())
            {
                var item = listView1.Items.Add(data.Key.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(data.Value.Weather[0]);
            }
            listView1.EndUpdate();
            var um = report.GetUmbrella(DateTime.Now);
            label3.Text = um.Is ? "傘" : "×";
            labelRainPercent.Text = um.Percent + @"%";
        }
    }
}
