namespace PipeModel.Forms
{
    partial class SingleProfileForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart_SingleProfile = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.一键纠偏补正ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart_SingleProfile)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart_SingleProfile
            // 
            this.chart_SingleProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            chartArea4.BackColor = System.Drawing.Color.Gainsboro;
            chartArea4.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.BottomLeft;
            chartArea4.BackImageTransparentColor = System.Drawing.Color.White;
            chartArea4.BorderColor = System.Drawing.Color.Transparent;
            chartArea4.Name = "C_SingleProfile";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 90F;
            chartArea4.Position.Width = 95F;
            chartArea4.Position.Y = 10F;
            this.chart_SingleProfile.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart_SingleProfile.Legends.Add(legend4);
            this.chart_SingleProfile.Location = new System.Drawing.Point(11, 61);
            this.chart_SingleProfile.Margin = new System.Windows.Forms.Padding(2);
            this.chart_SingleProfile.Name = "chart_SingleProfile";
            series4.ChartArea = "C_SingleProfile";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Legend = "Legend1";
            series4.Name = "S_SingleProfile";
            this.chart_SingleProfile.Series.Add(series4);
            this.chart_SingleProfile.Size = new System.Drawing.Size(714, 385);
            this.chart_SingleProfile.TabIndex = 9;
            this.chart_SingleProfile.Text = "chart";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.一键纠偏补正ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(745, 25);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 一键纠偏补正ToolStripMenuItem
            // 
            this.一键纠偏补正ToolStripMenuItem.Name = "一键纠偏补正ToolStripMenuItem";
            this.一键纠偏补正ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.一键纠偏补正ToolStripMenuItem.Text = "一键纠偏补正";
            // 
            // SingleProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 457);
            this.Controls.Add(this.chart_SingleProfile);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SingleProfileForm";
            this.Text = "SingleProfileForm";
            ((System.ComponentModel.ISupportInitialize)(this.chart_SingleProfile)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_SingleProfile;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 一键纠偏补正ToolStripMenuItem;
    }
}