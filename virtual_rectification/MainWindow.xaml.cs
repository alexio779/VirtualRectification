using System.Windows;


namespace virtual_rectification
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //переход ко второму окну

            MainCalc2 mc2 = new MainCalc2();
            mc2.Show();
            this.Close();
        }
    }
}
