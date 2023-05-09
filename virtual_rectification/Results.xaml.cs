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

            //деления
            double[] dataX = new double[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            double[] dataY = new double[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            //тестовый график
            double[] X1 = new double[] {0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100 };
            double[] Y1 = new double[] {0, 75, 80, 82, 85, 86, 88, 90, 85, 83, 75, 72, 75, 75, 78, 81, 85, 87, 91, 95, 100};

            Chart.Plot.AddScatter(X1, Y1);

            Chart.Plot.AddLine(0, 0, 100, 100);

            Chart.Refresh();
        }

    }
}
