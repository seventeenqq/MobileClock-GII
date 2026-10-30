using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Clock
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window(new MainPage());
            window.Title = "Часы";
            return window;
        }
    }
}
