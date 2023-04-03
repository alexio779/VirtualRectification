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
                //--------------------------------------------------------------

            }
        }

        /*Ниже представлен вариант заполнения и опустошения перегонного куба через Rectangle и изменение его Height через раймер,
         этот вариант рабочий, но я от него отказался, так как он достаточно ресурсоёмкий и "костыльный". Я взглянул в сторону 
         метода .GotoFrame(int) у ImageBehavior, он позволяет отобразить конкретный кадр гиф-файла, там самыс можно просто выставлять
         определённый кадр "количества" смеси, взависимости от температуры и испарения, используя гиф-файл своего рода "контейнером" кадров*/

        /*

    //Функция обрабатывает нажатие кнопки "Залить смесь"


    private void braga_Click(object sender, RoutedEventArgs e)
    {
        //создадим ещё один таймер
        DispatcherTimer dt2 = new DispatcherTimer();
        dt2.Tick += new EventHandler(dt_Tick2);
        dt2.Interval = new TimeSpan(0, 0, 0, 0, 75);
        dt2.Start();

    }


    private void dt_Tick2(object sender, EventArgs e)
    {
        //я не знаю как сделать иначе, оно не работает с for и while ┐(￣～￣)┌
        if (braga_line_0.Height != 111)
        {
            braga_line_0.Height++;

            if (braga_line_0.Height == 111)
            {
                MessageBox.Show("Бак полон", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //фрагмен ниже должен находитьтя в функции испарения браги

        if (braga_line_0.Height != 1 && tmp_slider_main.Value == 70)
        {
            braga_line_0.Height--;

            if (braga_line_0.Height == 1)
            {
                MessageBox.Show("Бак опустошён", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }

       */

        //Новая кнопка "Залить смесь"
        private void Braga_btn_Click(object sender, RoutedEventArgs e)
        {
            var controller1 = ImageBehavior.GetAnimationController(braga);
            controller1.Play();
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
            /*Что тут происходит, присваиваем каждой гифке контроллер для того, чтобы можно было ими управлять,
             т.е. запускать, останавливать или проверять на завершение
            далее запускаем асинхронный медод отображения с помощью async await с созданием задержки для того, 
            чтобы программа ждала пока завершится гифка и можно было проверить условие завершения для запуска следующей*/
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

        //Функция выключения воды в дефлегматоре и холодильнике
        void DeflegmatorStopWater()
        {
            //Дефлегматор
            var controller1 = ImageBehavior.GetAnimationController(water_def);
            controller1.Pause();
            controller1.GotoFrame(0);
            var controller2 = ImageBehavior.GetAnimationController(water_def2);
            controller2.Pause();
            controller2.GotoFrame(0);
            var controller3 = ImageBehavior.GetAnimationController(water_def3);
            controller3.Pause();
            controller3.GotoFrame(0);
            var controller4 = ImageBehavior.GetAnimationController(water_def4);
            controller4.Pause();
            controller4.GotoFrame(0);
            var controller5 = ImageBehavior.GetAnimationController(water_def5);
            controller5.Pause();
            controller5.GotoFrame(0);
            water_def7.Visibility = Visibility.Hidden;
            var controller6 = ImageBehavior.GetAnimationController(water_def6);
            controller6.Pause();
            controller6.GotoFrame(0);
            var controller7 = ImageBehavior.GetAnimationController(water_def7);
            controller7.Pause();
            controller7.GotoFrame(0);

            //Холодильник
            var controller8 = ImageBehavior.GetAnimationController(cold_water_fr);
            controller8.Pause();
            controller8.GotoFrame(0);
            var controller9 = ImageBehavior.GetAnimationController(water_cold_up);
            controller9.Pause();
            controller9.GotoFrame(0);
            var controller10 = ImageBehavior.GetAnimationController(water_cold_up2);
            controller10.Pause();
            controller10.GotoFrame(0);
            var controller11 = ImageBehavior.GetAnimationController(warm_water_fr);
            controller11.Pause();
            controller11.GotoFrame(0);
            warm_water_fr.Visibility = Visibility.Hidden;
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

        //Отслеживание значения слайдера температуры нагрева и спавним пар
        private void tmp_slider_main_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Создаём кисть красного цвета
            SolidColorBrush brush1 = new SolidColorBrush(Colors.Red);

            double tmp_vl = Math.Round(tmp_slider_main.Value)/100;

            brush1.Opacity = tmp_vl;

            rec1.Fill = brush1;
            rec2.Fill = brush1;
            rec3.Fill = brush1;

            //Отвечает за пар
            var controller = ImageBehavior.GetAnimationController(braga);

            double vapour_opacity = Math.Round(tmp_slider_main.Value)/100;

            if (controller.IsComplete==true && tmp_slider_main.Value >= 50)
            {
                vapour_blue1.Opacity = vapour_opacity;
                vapour_blue2.Opacity = vapour_opacity;
                vapour_gray4.Opacity = vapour_opacity;
            }

            if (controller.IsComplete == true && tmp_slider_main.Value < 50)
            {
                vapour_blue1.Opacity = 0;
                vapour_blue2.Opacity = 0;
                vapour_gray4.Opacity = 0;
            }

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

        //Кнопка выхода
        private void Exit_btn_Click(object sender, RoutedEventArgs e)
        {
            //здесь мы должны закрыть окно 2 и вызвать окно с отчётом
            if (MessageBox.Show("Вы уверены, что хотите завершить лабораторную работу?", "Завершение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //Выбор "НЕТ"
                //ничего
            }
            else
            {
                //Выбор "ДА"            
                Results results = new Results();
                results.Show();
                this.Close();
            }
        }

        //Кнопка сброса прогреса
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите сбросить все действия?", "Сброс", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                //Выбор "НЕТ"
                //ничего
            }
            else
            {
                //Выбор "ДА"
                HotFinish();
                DeflegmatorStopWater();
                sw.Restart();
            }
        }

    }
}
