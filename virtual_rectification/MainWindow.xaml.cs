using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            int control_summ = 8;
            if (String.IsNullOrEmpty(textBox_RP.Text))
            {
                textBox_RP.BorderBrush = Brushes.Red;
            }   
            else
            {
                control_summ++;
                textBox_RP.BorderBrush = Brushes.MediumSpringGreen;
            }

            if (String.IsNullOrEmpty(textBox_RD.Text))
            {
                textBox_RD.BorderBrush = Brushes.Red;
            }
            else
            {
                control_summ++;
                textBox_RD.BorderBrush = Brushes.MediumSpringGreen;
            }
            if (String.IsNullOrEmpty(textBox_SP.Text))
            {
                textBox_SP.BorderBrush = Brushes.Red;
            }
            else
            {
                control_summ++;
                textBox_SP.BorderBrush = Brushes.MediumSpringGreen;
            }
            if (String.IsNullOrEmpty(textBox_SD.Text))
            {
                textBox_SD.BorderBrush = Brushes.Red;
            }
            else
            {
                control_summ++;
                textBox_SD.BorderBrush = Brushes.MediumSpringGreen;
            }
            if (String.IsNullOrEmpty(textBox_Fch.Text))
            {
                textBox_Fch.BorderBrush = Brushes.Red;
            }
            else
            {
                control_summ++;
                textBox_Fch.BorderBrush = Brushes.MediumSpringGreen;
            }
            if (String.IsNullOrEmpty(textBox_ChT.Text))
            {
                textBox_ChT.BorderBrush = Brushes.Red;
            }
            else
            {
                control_summ++;
                textBox_ChT.BorderBrush = Brushes.MediumSpringGreen;
            }
            if (String.IsNullOrEmpty(textBox_NTP.Text))
            {
                textBox_NTP.BorderBrush = Brushes.Red;

            }
            else
            {
                control_summ++;
                textBox_NTP.BorderBrush = Brushes.MediumSpringGreen;
            }
            if (String.IsNullOrEmpty(textBox_KPD.Text))
            {
                textBox_KPD.BorderBrush = Brushes.Red;

            }
            else
            {
                control_summ++;
                textBox_KPD.BorderBrush = Brushes.MediumSpringGreen;
            }

            if(control_summ == 8)
            {
                //check step
                //MessageBox.Show("ok");

                //Переход к форме 2
                //MainCalc mc = new MainCalc();
                //mc.Show();
                MainCalc2 mc2 = new MainCalc2();
                mc2.Show();
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены!");
            }
        }
    }
}
