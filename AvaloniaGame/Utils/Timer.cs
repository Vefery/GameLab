//using Avalonia.Controls;
//using Avalonia.Threading;
//using System;
//using System.Timers;

//namespace TimerApp
//{
//    public partial class MainWindow : Window
//    {
//        private Timer _timer;
//        private double _timeRemaining = 60; // Время в секундах

//        public MainWindow()
//        {
//            InitializeComponent();
//            StartTimer();
//        }

//        private void StartTimer()
//        {
//            _timer = new Timer(1000); // Устанавливаем интервал в 1 секунду
//            _timer.Elapsed += OnTimerElapsed;
//            _timer.Start();
//        }

//        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
//        {
//            if (_timeRemaining > 0)
//            {
//                _timeRemaining--;
//                // Обновляем UI в главном потоке
//                Dispatcher.UIThread.InvokeAsync(UpdateTimerDisplay);
//            }
//            else
//            {
//                _timer.Stop();
//                Dispatcher.UIThread.InvokeAsync(TimerExpired);
//            }
//        }

//        private void UpdateTimerDisplay()
//        {
//            TimerText.Text = $"Осталось времени: {_timeRemaining} секунд";
//        }

//        private void TimerExpired()
//        {
//            TimerText.Text = "Таймер истек!";
//            // Здесь можно добавить дополнительную логику по истечении таймера
//        }
//    }
//}
