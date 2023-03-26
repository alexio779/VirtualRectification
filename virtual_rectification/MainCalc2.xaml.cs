using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WpfAnimatedGif;
using System.Diagnostics;


namespace virtual_rectification
{
    public partial class MainCalc2 : Window
    {
        //1 -й кусок кода, отвечающий за таймер
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;
        //-------------------------------------

        public MainCalc2()
        {
            InitializeComponent();

            //2-й кусок кода, отвечающий за таймер
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            sw.Start();
            dt.Start();
            //------------------------------------
        }

        //3-й кусок кода, отвечающий за таймер (событие "тика")
        void dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
                time_label.Content = currentTime;
                
            }
        }

        //Функция обрабатывает нажатие кнопки "Залить смесь"
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var controller1 = ImageBehavior.GetAnimationController(braga);
            controller1.Play();

            //засунуть в другую кнопку(-ки)

            /* Ниже мусор, который НАДО разобрать!
            if (controller1.IsComplete)

            {
                var controller2 = ImageBehavior.GetAnimationController(flegma_s);
                controller2.Play();

                vapour_blue1.Visibility = Visibility.Visible;
                vapour_blue2.Visibility = Visibility.Visible;

                vapour_gray1.Visibility = Visibility.Visible;
                vapour_gray2.Visibility = Visibility.Visible;
                vapour_gray3.Visibility = Visibility.Visible;
                vapour_gray4.Visibility = Visibility.Visible;

                distildone.Visibility = Visibility.Visible;

                drops1.Visibility = Visibility.Visible;
                drops2.Visibility = Visibility.Visible;
                drops3.Visibility = Visibility.Visible;
                drops4.Visibility = Visibility.Visible;
                drops5.Visibility = Visibility.Visible;
                drops6.Visibility = Visibility.Visible;

                var controller3 = ImageBehavior.GetAnimationController(distil_g);
                controller3.Play();
            }
            */


        }

        //функция обрабатывает нажатие на кнопку "Подать воду"
        private void Voda_Click(object sender, RoutedEventArgs e)
        {
            HolodilnikWater();
            DeflegmatorWater();
        }

        //Функция отвечает за воду в холодильнике
        async void HolodilnikWater()
        {
            var controller3 = ImageBehavior.GetAnimationController(cold_water_fr);
            cold_water_fr.Visibility = Visibility.Visible;
            controller3.Play();
            await Task.Delay(2600);
            if (controller3.IsComplete)
            {
                var controller5 = ImageBehavior.GetAnimationController(water_cold_up);
                var controller6 = ImageBehavior.GetAnimationController(water_cold_up2);
                controller5.Play();
                controller6.Play();
                water_cold_up.Visibility = Visibility.Visible;
                water_cold_up2.Visibility = Visibility.Visible;
                await Task.Delay(3000);

                if (water_cold_up.IsVisible && water_cold_up2.IsVisible)
                {
                    var controller4 = ImageBehavior.GetAnimationController(warm_water_fr);
                    warm_water_fr.Visibility = Visibility.Visible;
                    controller4.Play();
                }
            }
        }

        //Функция отвечает за всё, что происходит в дефлегматоре
        async void DeflegmatorWater()
        {
            /*Что тут происходит. Присваиваем каждой гифке контроллер для того, чтобы можно было ими управлять,
             т.е. запускать, останавливать или проверять на завершение
            далее запускаем асинхронный медод отображения с помощью async await с созданием задержки для того, 
            чтобы программа ждала пока завершится гифка и можно было проверить условие завршения для запуска следующей*/
            var controller1 = ImageBehavior.GetAnimationController(water_def);
            var controller2 = ImageBehavior.GetAnimationController(water_def2);
            var controller3 = ImageBehavior.GetAnimationController(water_def3);
            var controller4 = ImageBehavior.GetAnimationController(water_def4);
            var controller5 = ImageBehavior.GetAnimationController(water_def5);
            var controller6 = ImageBehavior.GetAnimationController(water_def6);
            var controller7 = ImageBehavior.GetAnimationController(water_def7);
            controller1.Play();
            await Task.Delay(3250);

            if (controller1.IsComplete)
            {
                controller2.Play();
                await Task.Delay(3350);
            }
            
            if (controller2.IsComplete)
            {
                controller3.Play();
                await Task.Delay(3250);
            }

            if (controller3.IsComplete)
            {
                controller5.Play();
                await Task.Delay(3500);
            }

            if (controller5.IsComplete)
            {
                water_def7.Visibility = Visibility.Visible;
                controller7.Play();
                await Task.Delay(3250);
            }

            if (controller7.IsComplete)
            {
                controller6.Play();
                await Task.Delay(3400);
            }

            if (controller6.IsComplete)
            {
                controller4.Play();
            }
        }

        //Глобальная переменная отслеживающая состояние нагрева (включено или выключено)
        bool hot_is_on = false;

        //Функция, обрабатывающая нажатие кнопки
        private void Hot_Click(object sender, RoutedEventArgs e)
        {
            if (hot_is_on == false)
            {
                HotStart();
            }
            else
            {
                HotFinish();
            }
        }

        //Функция отвечает за запуск нагрева
        void HotStart()
        {
            hot_is_on = true;

            //делаем слайдер активным
            tmp_slider.IsEnabled = true;

            hot.Content = "Выключить нагрев";
        }

        //функция отвечает за выключение нагрева
        void HotFinish()
        {
            tmp_slider.IsEnabled = false;

            //меняем надпись на кнопке
            hot.Content = "Включить нагрев";

            //меняем проверку на нагрев
            hot_is_on = false;

            //выставляем значение слайдера на 0
            tmp_slider_main.Value = 0;
        }

        //Отслеживание значения слайдера температуры нагрева
        private void tmp_slider_main_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Создаём кисть красного цвета
            SolidColorBrush brush1 = new SolidColorBrush(Colors.Red);

            double tmp_vl = Math.Round(tmp_slider_main.Value)/100;

            brush1.Opacity = tmp_vl;

            rec1.Fill = brush1;
            rec2.Fill = brush1;
            rec3.Fill = brush1;

            /*
            //ниже приведёт ещё один вариант реализации "нагрева", но я от него отказался, так как он муторный и менее наглядный
            //MessageBox.Show("DEBUG#01");

            
            if (tmp_slider_main.Value >= 1 && tmp_slider_main.Value < 10)
            {
                //MessageBox.Show("DEBUG#011");

                //Color color = (Color)ColorConverter.ConvertFromString("#BF9B9B");
                //var brush1 = (SolidColorBrush)new BrushConverter().ConvertFromString("#BF9B9B");

                //Меняем прозрачность цвета, в зависимости от выставленной температуры

                //brush1.Opacity = tmp_slider_main.Value;

                //SolidColorBrush MyBrush1 = brush;

                rec1.Fill = brush1;
                rec2.Fill = brush1;
                rec3.Fill = brush1;
            }

            if(tmp_slider_main.Value >= 25 && tmp_slider_main.Value < 50)
            {
                MessageBox.Show("DEBUG#02");
                //rec1.Fill = Brushes.;
                rec2.Fill = Brushes.RosyBrown;
                rec3.Fill = Brushes.RosyBrown;
            }
            */
        }




    }
}

