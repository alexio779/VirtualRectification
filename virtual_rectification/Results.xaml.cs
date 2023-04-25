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
            double[] dataX = new double[] { 1, 3, 6, 10, 30 };
            double[] dataY = new double[] { 1, 4, 9, 16, 35 };
            double[] dataX2 = new double[] { 1, 5, 11, 18, 30 };
            double[] dataY2 = new double[] { 1, 6, 10, 15, 40 };
            Chart.Plot.AddScatter(dataX, dataY);
            Chart.Plot.AddScatter(dataX2, dataY2);
            Chart.Refresh();
        }

    }
}
