
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Graphics;

namespace Clock
{
    public partial class MainPage : ContentPage
    {
        private ClockView localClock;
        private ClockView tokyoClock;
        private ClockView stokClock;
        private IDispatcherTimer timer;
        private TimeZoneInfo tokyoZone;
        private TimeZoneInfo stokZone;

        private List<TimeZoneInfo> cityZones = new List<TimeZoneInfo>();

        private List<ClockView> addedClocks = new List<ClockView>();

        private List<TimeZoneInfo> addedZones = new List<TimeZoneInfo>();

        public MainPage()
        {
            InitializeComponent();

            localClock = this.FindByName<ClockView>("LocalClock");
            tokyoClock = this.FindByName<ClockView>("TokyoClock");
            stokClock = this.FindByName<ClockView>("StokClock");

            tokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
            stokZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm");

            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
            {
                if (!zone.Id.Contains("/"))
                {
                    continue;
                }

                if (zone.Id.StartsWith("Etc/"))
                {
                    continue;
                }

                string city = zone.Id.Substring(
                    zone.Id.LastIndexOf('/') + 1
                );

                city = city.Replace('_', ' ');

                CityPicker.Items.Add(city + " (" + zone.Id + ")");

                cityZones.Add(zone);
            }

            timer = Dispatcher.CreateTimer();
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
            DateTimeOffset utc = DateTimeOffset.UtcNow;

            localClock.SetTime(utc.ToLocalTime().DateTime);

            DateTimeOffset tokyoTime = TimeZoneInfo.ConvertTime(
                utc, tokyoZone
            );

            tokyoClock.SetTime(tokyoTime.DateTime);

            DateTimeOffset stokTime = TimeZoneInfo.ConvertTime(
                utc, stokZone
            );

            stokClock.SetTime(stokTime.DateTime);

            for (int i = 0; i < addedClocks.Count; i++)
            {
                DateTimeOffset cityTime = TimeZoneInfo.ConvertTime(
                    utc, addedZones[i]
                );

                addedClocks[i].SetTime(cityTime.DateTime);
            }
        }

        private void OnAddClockClicked(object? sender, EventArgs e)
        {
            if (CityPicker.SelectedIndex == -1)
            {
                return;
            }

            int index = CityPicker.SelectedIndex;

            string city = CityPicker.Items[index];

            TimeZoneInfo zone = cityZones[index];

            Label label = new Label();

            label.Text = city;
            label.TextColor = Colors.Black;
            label.FontSize = 20;
            label.Margin = new Thickness(0, 24, 0, 0);
            label.HorizontalTextAlignment = TextAlignment.Center;

            ClockView clock = new ClockView();

            clock.HeightRequest = 100;

            AddedClocks.Children.Add(label);
            AddedClocks.Children.Add(clock);

            addedClocks.Add(clock);
            addedZones.Add(zone);

            UpdateClocks();
        }
    }
}
