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
        bool water_is_on = false;
        //bool hol_water_is_on = false;
        bool _isTempered = false;
        bool _TankIsFull = false;
        bool fl_In_Stock = true;
        bool tmp_maxed = false;
        int power_lvl = 0;
        double wsv = 0, wmv = 0;
        double distil_kol_res = 0;
        
        //int seconds_now = 0;

        //1 -й кусок кода, отвечающий за таймер
        DispatcherTimer dt = new DispatcherTimer();

        Stopwatch sw = new Stopwatch();

        string currentTime = string.Empty;

        DispatcherTimer dt2 = new DispatcherTimer();

        Stopwatch sw2 = new Stopwatch();

        string currentTime2 = string.Empty;
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
            dt2.Tick += new EventHandler(dt2_TickAsync);
            dt2.Interval = new TimeSpan(0, 0, 0, 1, 0);
            sw2.Start();
            dt2.Start();
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

                if (fl_slider.Value == 0 || power_lvl < 1)
                {
                    DropsDefStop();

                    fl_slider.Value = 0;

                    fl_dock.IsEnabled = false;
                }

            }

            if(_TankIsFull == true && _isTempered == true && power_lvl > 0)
            {
                fl_dock.IsEnabled = true;
            }

            //--------------------------------------------------------------------------------

            if(temperature_progress.Value < 20 && tmp_maxed == true)
            {
                def_water_out_sensor.Text = "15 °C";
                vapour_tmp_0.Text = "0 °C";
                vapour_tmp_1.Text = "0 °C";
                vapour_tmp_2.Text = "0 °C";

            }

            //----------------------------------------------------------------------------------

        }

        async void dt2_TickAsync(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
                time_label.Content = currentTime;

                //--------------------------------------------------------------
                //Убывание исходной смеси и появление дистиллята

                if(temperature_progress.Value > 60 && _TankIsFull && power_lvl >= 2 && braga_slider.Value != 0)
                {
                    double rashod_smesi = 0.1;
                    
                    var controller1 = ImageBehavior.GetAnimationController(distil_g);

                    double distil_done = rashod_smesi * 0.1 * 400;

                    distil_kol_res = distil_kol_res + distil_done / 400;

                    distil_kol_res = Math.Round(distil_kol_res, 2);

                    distil_kol.Text = distil_kol_res.ToString();

                    distil_progress.Value = distil_progress.Value + distil_done;

                    if (controller1.CurrentFrame < 24)
                    {
                        controller1.GotoFrame((int)distil_progress.Value / 16);
                    }

                    braga_slider.Value = braga_slider.Value - rashod_smesi;
                }
                //--------------------------------------------------------------
                //Мерник воды и заполнение водосборника
                var controller2 = ImageBehavior.GetAnimationController(water_def6);
                var controller3 = ImageBehavior.GetAnimationController(water_def4);
                
                if (controller2.IsComplete == true)
                {
                    wsv = water_slider.Value; //получаем значение слайдера water slider value

                    wmv = wmv + wsv / 60; //присваиваем water meter value значение wsv делённое на 60 (в сек)

                    //System.Console.WriteLine("Tick");

                    wmv = Math.Round(wmv, 3); //округляем

                    string value = wmv.ToString();
                    water_meter_value.Text = value;
                    ImageBehavior.SetAnimationSpeedRatio(water_def4, 0.02);
                    controller3.Play();
                }
                //--------------------------------------------------------------
                //Отключение при окончании смеси
                if (braga_slider.Value == 0 && distil_progress.Value > 0)
                {
                    HotFinish();
                }
            }
        }

        //Слайдер количества исходной смеси
        private void Braga_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int frameOfBraga = 0;
            frameOfBraga = (int)braga_slider.Value; //получаем значение слайдера
            frameOfBraga = frameOfBraga * 3;

            var controller1 = ImageBehavior.GetAnimationController(braga);
            controller1.Pause();
            controller1.GotoFrame(frameOfBraga);

            if(frameOfBraga > 0)
            {
                _TankIsFull = true;
            }
            
        }

        //функция обрабатывает нажатие на кнопку "Подать воду на дефлегматор"
        private void Water_Click(object sender, RoutedEventArgs e)
        {
            
            if (water_is_on == false)
            {
                WaterStart();
            }
            else
            {
                WaterFinish();
            }

        }

        //Подача воды в дефлегматор
        void WaterStart()
        {
            water_is_on = true;
            //hol_water_is_on = true;

            water_dock.IsEnabled = true;

            water_pusk.Content = "Закрыть воду";
        }

        //Остановка воды в дефлегматоре
        void WaterFinish()
        {
            water_is_on = false;
            //hol_water_is_on = false;

            water_dock.IsEnabled = false;

            water_slider.Value = 0;

            HolodilnikStopWater();
            DeflegmatorStopWater();

            water_pusk.Content = "Подать воду";
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
            /*Присваиваем каждой гифке контроллер для того, чтобы можно было ими управлять,
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
        private void Water_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DeflegmatorWater();
            HolodilnikWater();

            if (water_slider.Value > 0)
            {
                hot.IsEnabled = true;
            }
            else
            {
                hot.IsEnabled = false;
            }

            if (water_slider.Value == 0)
            {
                HolodilnikStopWater();
                DeflegmatorStopWater();
            }
        }

        //Функция отвечает за выключение воды в холодильнике
        void HolodilnikStopWater()
        {
            //hol_water_is_on = false;
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
            if (power_lvl > 0)
            {
                flegma_r.Visibility = Visibility.Visible;
                drops1.Visibility = Visibility.Visible;
                drops2.Visibility = Visibility.Visible;
                drops3.Visibility = Visibility.Visible;

                var controller1 = ImageBehavior.GetAnimationController(flegma_r);
                var controller2 = ImageBehavior.GetAnimationController(drops1);
                var controller3 = ImageBehavior.GetAnimationController(drops2);
                var controller4 = ImageBehavior.GetAnimationController(drops3);

                controller1.Play();
                controller2.Play();
                controller3.Play();
                controller4.Play();
                
            }
        }

        //Сброс капель в колонне
        void DropsDefStop()
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

            flegma_r.Visibility = Visibility.Hidden;
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

            braga_slider.Value = 0;
            braga_dock.IsEnabled = true;

            _TankIsFull = false;
        }
        
        //Функция, обрабатывающая нажатие кнопки
        private void Hot_Click(object sender, RoutedEventArgs e)
        {

            if (hot_is_on == false)
            {
                HotStart();
                braga_dock.IsEnabled = false;
            }
            else
            {
                HotFinish();
                braga_dock.IsEnabled = true;
            }
        }

        //Функция отвечает за запуск нагрева
        void HotStart()
        {
            hot_is_on = true;

            water_pusk.IsEnabled = false;

            //делаем слайдер активным
            tmp_slider.IsEnabled = true;

            hot.Content = "Выключить ЛАТР";
        }

        //функция отвечает за выключение нагрева
        void HotFinish()
        {
            tmp_slider.IsEnabled = false;

            water_pusk.IsEnabled = true;

            //меняем надпись на кнопке
            hot.Content = "Включить ЛАТР";

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
            vapour_blue0.Opacity = cold_op;
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
            vapour_blue0.Opacity = cold_op;
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
            //определение уровня мощности

            int pwrValue = (int)tmp_slider_main.Value;

            switch (pwrValue)
            {
                case 1:
                    power_lvl = 1;
                    break;
                case 2:
                    power_lvl = 2;
                    break;
                case 3:
                    power_lvl = 3;
                    break;
                case 4:
                    power_lvl = 4;
                    break;
                default:
                    power_lvl = 0;
                    break;
            }

            //привязка нагрева к прогресс-бару
            ProgressTemper();

        }

        //Функция заполнения прогресс-бара
        async void ProgressTemper()
        {
            if(power_lvl == 0)
            {
                for (int i = (int)temperature_progress.Value; i > 0; i--)
                {
                    temperature_progress.Value = i;
                    await Task.Delay(200);
                    if (i < 70)
                    {
                        fl_slider.Value = 0;
                    }
                }
            }

            if(power_lvl == 2)
            {
                for (int i = (int)temperature_progress.Value; i <= 85; i++)
                {
                    temperature_progress.Value = i;
                    await Task.Delay(200);
                    if (i > 70)
                    {
                        fl_slider.Value = 1;
                    }
                }
            }
        }

        //Отслеживание изменения прогресс-бара
        private void Temperature_progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Создаём кисть красного цвета
            SolidColorBrush brush1 = new SolidColorBrush(Colors.Red);

            double tmp_vl = Math.Round(temperature_progress.Value) / 100;

            brush1.Opacity = tmp_vl;

            rec1.Fill = brush1;
            rec2.Fill = brush1;
            rec3.Fill = brush1;

            //Отвечает за пар

            double vapour_opacity = 0;

            if (_TankIsFull == true && temperature_progress.Value == 85)
            {
                vapour_opacity = 0.7;
                vapour_blue0.Opacity = vapour_opacity;
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

                tmp_maxed = true;

                BubblesAndFlegma();

                def_water_out_sensor.Text = "35 °C";
                vapour_tmp_0.Text = "82 °C";
                vapour_tmp_1.Text = "79 °C";
                vapour_tmp_2.Text = "76 °C";
                flegma_tmp.Text = "55 °C";
            }

            if (_TankIsFull == true && temperature_progress.Value > 60)
            {
                vapour_opacity = 0.4;
                vapour_blue0.Opacity = vapour_opacity;
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

                drops4.Visibility = Visibility.Visible;
                var controller3 = ImageBehavior.GetAnimationController(drops4);
                controller3.Play();

                drops5.Visibility = Visibility.Visible;
                var controller4 = ImageBehavior.GetAnimationController(drops5);
                controller4.Play();

                BubblesAndFlegma();
            }

            if (temperature_progress.Value < 10 && _isTempered == true && tmp_maxed == true)
            {
                vapour_opacity = 0.3;
                vapour_blue0.Opacity = vapour_opacity;
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

                drops4.Visibility = Visibility.Hidden;
                var controller4 = ImageBehavior.GetAnimationController(drops4);
                controller4.Pause();

                drops5.Visibility = Visibility.Hidden;
                var controller5 = ImageBehavior.GetAnimationController(drops5);
                controller5.Pause();

                tmp_maxed = false;

                vapor_finish_smooth();
            }
        }

        //Анимация деталей
        void BubblesAndFlegma()
        {
            
            var controller2 = ImageBehavior.GetAnimationController(bubbles1);
            var controller3 = ImageBehavior.GetAnimationController(bubbles2);

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

        //Сброс всех капель
        void DropsReset()
        {
            //капли в колонне
            DropsDefStop();
            
            //капли в дефлегматоре
            var controller4 = ImageBehavior.GetAnimationController(drops4);
            controller4.Pause();
            controller4.GotoFrame(0);
            drops4.Visibility = Visibility.Hidden;
            var controller5 = ImageBehavior.GetAnimationController(drops5);
            controller5.Pause();
            controller5.GotoFrame(0);
            drops5.Visibility = Visibility.Hidden;

            //капли в ёмкость
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

            flegma_tmp.Text = "0 °C";
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
                //Results results = new Results();
                //results.Show();
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
                MainCalc2 mc2 = new MainCalc2();
                mc2.Show();
                this.Close();

            }
        }

    }
}
