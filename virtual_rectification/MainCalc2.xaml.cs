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
        //Глобальные переменные отслеживающие состояния
        bool hot_is_on = false;
        bool def_water_is_on = false;
        bool hol_water_is_on = false;
        bool _isTempered = false;
        bool _TankIsFull = false;
        bool fl_In_Stock = true;
        int seconds_now = 0;
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
            dt.Interval = new TimeSpan(0, 0, 0, 1, 0);
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

                /*
                var controller1 = ImageBehavior.GetAnimationController(braga);
                int frame = controller1.FrameCount;
                if (_isTempered == true && ts.Seconds % 3 == 0)
                {
                    controller1.Pause();

                    controller1.GotoFrame();
                }
                */

            }

            if(_TankIsFull == true && fl_In_Stock==true && _isTempered == true)
            {
                fl_dock.IsEnabled = true;
            }
        }

        //Новая кнопка "Залить смесь"
        private void Braga_btn_Click(object sender, RoutedEventArgs e)
        {
            var controller1 = ImageBehavior.GetAnimationController(braga);
            controller1.Play();

            _TankIsFull = true;
        }

        //функция обрабатывает нажатие на кнопку "Подать воду на дефлегматор"
        private void Def_Voda_Click(object sender, RoutedEventArgs e)
        {
            
            if (def_water_is_on == false)
            {
                WaterDefStart();
            }
            else
            {
                WaterDefFinish();
            }

        }

        //Подача воды в дефлегматор
        void WaterDefStart()
        {
            def_water_is_on = true;

            water_def_dock.IsEnabled = true;

            def_voda_pusk.Content = "Закрыть воду на дефлегматор";
        }

        //Остановка воды в дефлегматоре
        void WaterDefFinish()
        {
            def_water_is_on = false;

            water_def_dock.IsEnabled = false;

            water_def_slider.Value = 0;

            DeflegmatorStopWater();

            def_voda_pusk.Content = "Подать воду на дефлегматор";
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

        //Обработчик нажатия кнопки подычи воды на холодильник
        private void Hol_voda_pusk_Click(object sender, RoutedEventArgs e)
        {
            if (hol_water_is_on == false)
            {
                HolWaterStart();
            }
            else
            {
                HolWaterFinish();
            }
        }
        
        //Подача воды в холодильник
        void HolWaterStart()
        {
            hol_water_is_on = true;

            water_hol_dock.IsEnabled = true;

            hol_voda_pusk.Content = "Закрыть воду на холодильник";
        }

        //Остановка воды в холодильнике
        void HolWaterFinish()
        {

            water_hol_slider.Value = 0;

            HolodilnikStopWater();

            hol_voda_pusk.Content = "Подать воду на холодильник";

            water_hol_dock.IsEnabled = false;

            hol_water_is_on = false;
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

        //Функция выключения воды в дефлегматоре
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

        }

        //Слайдер воды дефлегматора
        private void Water_def_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DeflegmatorWater();

            if(water_def_slider.Value == 0)
            {
                DeflegmatorStopWater();
            }
        }

        //Слайдер воды холодильника
        private void Hol_water_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HolodilnikWater();

            if (water_hol_slider.Value == 0)
            {
                HolodilnikStopWater();
            }
        }

        //Функция отвечает за выключение воды в холодильнике
        void HolodilnikStopWater()
        {
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

        //Слайдер подачи флегмы
        private void Fl_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (fl_In_Stock == true && tmp_slider_main.Value > 50)
            {
                flegma_r.Visibility = Visibility.Visible;
                drops1.Visibility = Visibility.Visible;
                drops2.Visibility = Visibility.Visible;
                drops3.Visibility = Visibility.Visible;

                var controller1 = ImageBehavior.GetAnimationController(flegma_r);
                var controller2 = ImageBehavior.GetAnimationController(drops1);
                var controller3 = ImageBehavior.GetAnimationController(drops2);
                var controller4 = ImageBehavior.GetAnimationController(drops3);
                var controller5 = ImageBehavior.GetAnimationController(drops4);
                var controller6 = ImageBehavior.GetAnimationController(drops5);

                controller1.Play();
                controller2.Play();
                controller3.Play();
                controller4.Play();
                
                if (fl_slider.Value == 0)
                {
                    controller1.Pause();
                    flegma_r.Visibility = Visibility.Hidden;
                    drops1.Visibility = Visibility.Visible;
                    drops2.Visibility = Visibility.Visible;
                    drops3.Visibility = Visibility.Visible;
                    drops4.Visibility = Visibility.Visible;
                    drops5.Visibility = Visibility.Visible;
                    controller2.Pause();
                    controller3.Pause();
                    controller4.Pause();
                    controller5.Pause();
                    controller6.Pause();
                }
                
            }
        }

        //Сброс дефлегматора
        void DeflegmatorReset()
        {
            flegma_r.Visibility = Visibility.Hidden;
            var controller1 = ImageBehavior.GetAnimationController(flegma_r);
            controller1.Pause();
            controller1.GotoFrame(0);
            flegma_s.Visibility = Visibility.Hidden;
            var controller2 = ImageBehavior.GetAnimationController(flegma_s);
            controller2.Pause();
            controller2.GotoFrame(0);

            fl_slider.Value = 0;

        }

        //Сброс куба
        void TankReset()
        {
            var controller1 = ImageBehavior.GetAnimationController(braga);
            controller1.Pause();
            controller1.GotoFrame(0);

            _TankIsFull = false;
        }
        
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

        //Это функция отвечет за исчезновение пара плавно
        async void vapor_finish_smooth()
        {
            await Task.Delay(2000);
            double cold_op = 0;
            vapour_blue1.Opacity = cold_op;
            vapour_blue2.Opacity = cold_op;
            vapour_gray4.Opacity = cold_op;
            vapour_gray_l.Opacity = cold_op;
            vapour_gray2.Opacity = cold_op;
            vapour_gray1.Opacity = cold_op;
            vapour_gray3.Opacity = cold_op;
            distildone.Opacity = cold_op;
            distil_line.Visibility = Visibility.Hidden;

            _isTempered = false;
            
        }

        //Исчезновение пара моментально
        void vapor_finish()
        {

            double cold_op = 0;
            vapour_blue1.Opacity = cold_op;
            vapour_blue2.Opacity = cold_op;
            vapour_gray4.Opacity = cold_op;
            vapour_gray_l.Opacity = cold_op;
            vapour_gray2.Opacity = cold_op;
            vapour_gray1.Opacity = cold_op;
            vapour_gray3.Opacity = cold_op;
            distildone.Opacity = cold_op;

            _isTempered = false;
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
            

            if (controller.IsComplete==true && tmp_slider_main.Value >= 60)
            {
                vapour_blue1.Opacity = vapour_opacity;
                vapour_blue2.Opacity = vapour_opacity;
                vapour_gray4.Opacity = vapour_opacity;
                vapour_gray_l.Opacity = vapour_opacity;
                vapour_gray2.Opacity = vapour_opacity;
                vapour_gray1.Opacity = vapour_opacity;
                vapour_gray3.Opacity = vapour_opacity;
                distildone.Opacity = vapour_opacity;
                distil_line.Visibility = Visibility.Visible;

                drops6.Visibility = Visibility.Visible;
                var controller1 = ImageBehavior.GetAnimationController(drops6);
                controller1.Play();

                drops4.Visibility = Visibility.Visible;
                var controller2 = ImageBehavior.GetAnimationController(drops4);
                controller2.Play();

                drops5.Visibility = Visibility.Visible;
                var controller3 = ImageBehavior.GetAnimationController(drops5);
                controller3.Play();

                BubblesAndFlegma();
            }

            if (controller.IsComplete == true && tmp_slider_main.Value < 50 && _isTempered == true)
            {
                vapour_opacity = 0.3;
                vapour_blue1.Opacity = vapour_opacity;
                vapour_blue2.Opacity = vapour_opacity;
                vapour_gray4.Opacity = vapour_opacity;
                vapour_gray_l.Opacity = vapour_opacity;
                vapour_gray2.Opacity = vapour_opacity;
                vapour_gray1.Opacity = vapour_opacity;
                vapour_gray3.Opacity = vapour_opacity;
                distildone.Opacity = vapour_opacity;

                drops6.Visibility = Visibility.Hidden;
                var controller2 = ImageBehavior.GetAnimationController(drops6);
                controller2.Pause();
                var controller3 = ImageBehavior.GetAnimationController(flegma_s);
                controller3.Pause();

                vapor_finish_smooth();
            }

            if(controller.IsComplete == true && tmp_slider_main.Value > 50 && tmp_slider_main.Value < 60)
            {
                vapour_opacity = 0.4;
                vapour_blue1.Opacity = vapour_opacity;
                vapour_blue2.Opacity = vapour_opacity;
                vapour_gray4.Opacity = vapour_opacity;
                vapour_gray_l.Opacity = vapour_opacity;
                vapour_gray2.Opacity = vapour_opacity;
                vapour_gray1.Opacity = vapour_opacity;
                vapour_gray3.Opacity = vapour_opacity;
                distildone.Opacity = vapour_opacity;

                drops6.Visibility = Visibility.Visible;
                var controller2 = ImageBehavior.GetAnimationController(drops6);
                controller2.Play();

                BubblesAndFlegma();
            }

        }

        //Анимация деталей
        void BubblesAndFlegma()
        {
            //временно
            var controller1 = ImageBehavior.GetAnimationController(distil_g);
            var controller2 = ImageBehavior.GetAnimationController(bubbles1);
            var controller3 = ImageBehavior.GetAnimationController(bubbles2);
            distil_g.Visibility = Visibility.Visible;
            ImageBehavior.SetAnimationSpeedRatio(distil_g, 0.1);
            controller1.Play();

            bubbles1.Visibility = Visibility.Visible;
            controller2.Play();

            bubbles2.Visibility = Visibility.Visible;
            controller3.Play();

            flegma_s.Visibility = Visibility.Visible;
            var controller4 = ImageBehavior.GetAnimationController(flegma_s);
            controller4.Play();
            ImageBehavior.SetAnimationSpeedRatio(flegma_s, 0.25);

            _isTempered = true;
        }

        //Сброс капель
        void DropsReset()
        {
            var controller1 = ImageBehavior.GetAnimationController(drops1);
            controller1.Pause();
            controller1.GotoFrame(0);
            drops1.Visibility = Visibility.Hidden;
            var controller2 = ImageBehavior.GetAnimationController(drops2);
            controller2.Pause();
            controller2.GotoFrame(0);
            drops2.Visibility = Visibility.Hidden;
            var controller3 = ImageBehavior.GetAnimationController(drops3);
            controller3.Pause();
            controller3.GotoFrame(0);
            drops3.Visibility = Visibility.Hidden;
            var controller4 = ImageBehavior.GetAnimationController(drops4);
            controller4.Pause();
            controller4.GotoFrame(0);
            drops4.Visibility = Visibility.Hidden;
            var controller5 = ImageBehavior.GetAnimationController(drops5);
            controller5.Pause();
            controller5.GotoFrame(0);
            drops5.Visibility = Visibility.Hidden;
            var controller6 = ImageBehavior.GetAnimationController(drops6);
            controller6.Pause();
            controller6.GotoFrame(0);
            drops6.Visibility = Visibility.Hidden;
        }

        //Сброс остальных деталей
        void DetailsReset()
        {
            distil_line.Visibility = Visibility.Hidden;
            var controller1 = ImageBehavior.GetAnimationController(bubbles1);
            controller1.Pause();
            controller1.GotoFrame(0);
            var controller2 = ImageBehavior.GetAnimationController(bubbles2);
            controller2.Pause();
            controller2.GotoFrame(0);

            var controller3 = ImageBehavior.GetAnimationController(distil_g);
            controller3.Pause();
            controller3.GotoFrame(0);
            distil_g.Visibility = Visibility.Hidden;
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
                WaterDefFinish();
                vapor_finish();
                HolWaterFinish();
                sw.Restart();
                DeflegmatorReset();
                TankReset();
                DropsReset();
                DetailsReset();
            }
        }
    }
}
