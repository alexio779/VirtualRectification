using System.Windows;


namespace virtual_rectification
{
    /// <summary>
    /// Логика взаимодействия для Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        public Results()
        {
            InitializeComponent();
            ChartBuilder();
        }

        void ChartBuilder()
        {
            Chart.Plot.XAxis.Label("Концентрация жидкости, % об.");
            Chart.Plot.YAxis.Label("Концентрация пара, % об.");

            double[] dataX = new double[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            double[] dataY = new double[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            Chart.Plot.AddLine(0,0, 100, 100);

            Chart.Refresh();
        }

    }
}
