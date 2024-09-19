using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DifferentialEquationSolver
{
    public partial class Form1 : Form
    {
        const int tMax = 10;
        const double stepDE = 0.01;
        const int numberPoints = (int)(tMax / stepDE) + 1;
        

        const double g = 9.8;

        double[] dx = new double[numberPoints];
        double[] dy = new double[numberPoints];
        double[] x = new double[numberPoints];
        double[] y = new double[numberPoints];
        double[] t = new double[numberPoints];

        int[] xRange = new int[2] { 0, tMax };
        int[] yRange = new int[2] { -7, 7 };

        int[] xRangeTr = new int[2] { -4, 4 };
        int[] yRangeTr = new int[2] { -4, 4 };

        int markerSizeForPlot = 2;
        int markerSizeForAnimation = 10;

        int stepPointForTimer;

        ChartArea c;

        Random r = new Random(1);

        double[][] randomNoise = new double[10][];
        double[][] xNoise = new double[5][];
        double[][] yNoise = new double[5][];
        double[] xMean = new double[numberPoints];
        double[] yMean = new double[numberPoints];

        double[] bigDeltaX = new double[numberPoints];
        double[] bigDeltaY = new double[numberPoints];

        int numberNameSeries = 0;

        SeriesChartType standartPlotType = SeriesChartType.Line;

        public Form1()
        {
            InitializeComponent();

            lbNumPoints.Text = "Points: " + numberPoints.ToString();
            lbTime.Text = "Time: " + tMax.ToString();
            lbStep.Text = "Step: " + stepDE.ToString();

            stepPointForTimer = (int)(numberPoints * (timer1.Interval / 1000.0) / tMax);

            progressBar1.Maximum = numberPoints / stepPointForTimer;

            for (int i = 0; i < randomNoise.Length; i++)
            {
                randomNoise[i] = new double[numberPoints];                
            }

            for (int i = 0; i < xNoise.Length; i++)
            {
                xNoise[i] = new double[numberPoints];
                yNoise[i] = new double[numberPoints];
            }
        }
        
        public void InitPlot(int[] xRange, int[] yRange)
        {
            chart1.Series.Clear();
            c = chart1.ChartAreas[0];
            c.AxisX.Minimum = xRange[0];
            c.AxisX.Maximum = xRange[1];

            c.AxisY.Minimum = yRange[0];
            c.AxisY.Maximum = yRange[1];

            Grid gridX = new Grid();
            gridX.Enabled = true;
            c.AxisX.MajorGrid = gridX;

            Grid gridY = new Grid();
            gridY.Enabled = true;
            c.AxisY.MajorGrid = gridY;
            
            c.AxisX.Interval = (xRange[1] - xRange[0]) / 8;
            c.AxisY.Interval = (yRange[1] - yRange[0]) / 8;
        }

        public void Plot(double[] x, double[] y, int markerSize, Color col, SeriesChartType type)
        {
            Series series;
            series = chart1.Series.Add("Point" + numberNameSeries++.ToString());
            series.ChartType = type; // SeriesChartType.Point;
            series.IsVisibleInLegend = false;
            series.Color = col;
            series.MarkerSize = markerSize;
            series.MarkerStyle = MarkerStyle.Circle;

            if (x.Length != y.Length)
            {
                return;
            }            
            
            for (int i = 0; i < x.Length; i++)
            {
                series.Points.AddXY(x[i], y[i]);
            }
        }

        void SolveEquations()
        {
            if(rbEuler.Checked)
            {
                SolveEquationEuler();
            }
            else if(rbHeun.Checked)
            {
                SolveEquationHeun();
            }
            else if(rbRK4.Checked)
            {
                SolveEquationRK4();
            }
            else
            {
                return;
            }            
        }

        void SolveEquationEuler()
        {
            
        }
        
        void SolveEquationHeun()
        {
            
        }

        void SolveEquationRK4()
        {
            
        }
                
        double fdX(double dx_, double dy_, double x_, double y_, Vector4d bias)
        {
            double ret = 0;

            dx_ += bias[0];
            dy_ += bias[1];
            x_ += bias[2];
            y_ += bias[3];

            ret = -x_ * (dx_ * dx_ + dy_ * dy_ - g * y_) / (x_ * x_ + y_ * y_);

            return ret;
        }

        double fdY(double dx_, double dy_, double x_, double y_, Vector4d bias)
        {
            double ret = 0;

            dx_ += bias[0];
            dy_ += bias[1];
            x_ += bias[2];
            y_ += bias[3];

            ret = -g - y_ * (dx_ * dx_ + dy_ * dy_ - g * y_) / (x_ * x_ + y_ * y_);

            return ret;
        }
        double fX(double dx_, double dy_, double x_, double y_, Vector4d bias)
        {
            double ret = 0;

            dx_ += bias[0];
            dy_ += bias[1];
            x_ += bias[2];
            y_ += bias[3];
            
            ret = dx_;

            return ret;
        }

        double fY(double dx_, double dy_, double x_, double y_, Vector4d bias)
        {
            double ret = 0;

            dx_ += bias[0];
            dy_ += bias[1];
            x_ += bias[2];
            y_ += bias[3];

            ret = dy_;

            return ret;
        }


        private void btnDx_Click(object sender, EventArgs e)
        {
            SolveEquations();
            InitPlot(xRange, yRange);
            Plot(t, dx, markerSizeForPlot, Color.Red, standartPlotType);
        }
        private void btnDy_Click(object sender, EventArgs e)
        {
            SolveEquations();
            InitPlot(xRange, yRange);
            Plot(t, dy, markerSizeForPlot, Color.Red, standartPlotType);
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            SolveEquations();
            InitPlot(xRange, yRange);
            Plot(t, x, markerSizeForPlot, Color.Red, standartPlotType);
        }

        private void btnY_Click(object sender, EventArgs e)
        {
            SolveEquations();
            InitPlot(xRange, yRange);
            Plot(t, y, markerSizeForPlot, Color.Red, standartPlotType);
        }
        private void btnTrajectory_Click(object sender, EventArgs e)
        {
            // animation for trajectory
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // timer for animation
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            // to get xNoise and yNoise
        }

        private void btnXNoise_Click(object sender, EventArgs e)
        {
            Color[] colArr = {Color.Red, Color.Blue, Color.Green, Color.Aqua, Color.Gold};

            for (int i = 0; i < xNoise.Length; i++)
            {
                Plot(t, xNoise[i], markerSizeForPlot, colArr[i], standartPlotType);
            }
        }

        private void btnYNoise_Click(object sender, EventArgs e)
        {
            Color[] colArr = { Color.Red, Color.Blue, Color.Green, Color.Aqua, Color.Gold };

            for (int i = 0; i < xNoise.Length; i++)
            {
                Plot(t, yNoise[i], markerSizeForPlot, colArr[i], standartPlotType);
            }
        }

        private void btnMeanX_Click(object sender, EventArgs e)
        {
            // calculate xMean[]

            Plot(t, xMean, markerSizeForPlot, Color.Black, standartPlotType);
        }

        private void btnMeanY_Click(object sender, EventArgs e)
        {
            // calculate xMean[]

            Plot(t, yMean, markerSizeForPlot, Color.Black, standartPlotType);
        }

        private void btnConfIntX_Click(object sender, EventArgs e)
        {
            // calculate confident interval for x
        }

        private void btnConfIntY_Click(object sender, EventArgs e)
        {
            // calculate confident interval for y
        }

        private void btnClearPlot_Click(object sender, EventArgs e)
        {
            InitPlot(xRange, yRange);
        }
    }
}
