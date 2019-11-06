using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PipeModel.Forms
{
    public partial class SingleProfileForm : Form
    {
        public SingleProfileForm()
        {

            InitializeComponent();

            InitChart_Profile();
        }
        

        private void InitChart_Profile()
        {
            chart_SingleProfile.ChartAreas.Clear();
            ChartArea chartArea_SingleProfile = new ChartArea("C_SingleProfile");
            chartArea_SingleProfile.Position.Height = Define.CSPH;
            chartArea_SingleProfile.Position.Width = Define.CSPW;
            chartArea_SingleProfile.Position.X = Define.CSPX;
            chartArea_SingleProfile.Position.Y = Define.CSPY;
            chart_SingleProfile.ChartAreas.Add(chartArea_SingleProfile);


            chart_SingleProfile.Series.Clear();
            Series series_SingleProfile = new Series("S_SingleProfile");
            series_SingleProfile.ChartArea = "C_SingleProfile";

            chart_SingleProfile.Series.Add(series_SingleProfile);
            chart_SingleProfile.Series[0].Color = Color.Blue;
            chart_SingleProfile.Series[0].ChartType = SeriesChartType.FastLine;

            //设置最大最小值
            chart_SingleProfile.ChartAreas[0].AxisY.Minimum = Define.PROFILE_MIN_Y;
            chart_SingleProfile.ChartAreas[0].AxisY.Maximum = Define.PROFILE_MAX_Y;
            //设置刻度
            chart_SingleProfile.ChartAreas[0].AxisY.Interval = 100;
            chart_SingleProfile.ChartAreas[0].AxisX.Interval = 100;
           

            chart_SingleProfile.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chart_SingleProfile.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;


            chart_SingleProfile.Titles.Clear();
            chart_SingleProfile.Titles.Add("S01:X轴方向");
            //chart_Profile.Titles[0].Text = "探头A";
            chart_SingleProfile.Titles[0].Position.X = Define.CTPX;
            chart_SingleProfile.Titles[0].Position.Y = Define.CTPY;
            chart_SingleProfile.Titles[0].ForeColor = Color.Black;
            chart_SingleProfile.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            
        }
    }
}
