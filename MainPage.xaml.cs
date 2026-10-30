using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace Clock
{
    public partial class MainPage : ContentPage
    {
        private ClockView localClock;
        private ClockView tokyoClock;
        private ClockView stokClock;
        private IDispatcherTimer timer;

        public MainPage()
        {
            InitializeComponent();

            // Находим созданные в XAML часы по их x:Name.
            localClock = this.FindByName<ClockView>("LocalClock");
            tokyoClock = this.FindByName<ClockView>("TokyoClock");
            stokClock = this.FindByName<ClockView>("StokClock");

            timer = Dispatcher.CreateTimer();
            // Проверяем время чаще секунды, чтобы смена цифр не запаздывала.
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += OnTimerTick;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateClocks();
            timer.Start();
        }

        protected override void OnDisappearing()
        {
            timer.Stop();
            base.OnDisappearing();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            UpdateClocks();
        }

        private void UpdateClocks()
        {
            // Оба времени считаем от одного момента, чтобы секунды совпадали.
            DateTime utc = DateTime.UtcNow;
            localClock.SetTime(utc.ToLocalTime());

            // В Токио UTC+9 круглый год, перевода на летнее время нет.
            tokyoClock.SetTime(utc.AddHours(9));

            stokClock.SetTime(utc.AddHours(1));
        }
    }
}
