
namespace Restaurants
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

#if WINDOWS10_0_17763_0_OR_GREATER
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 1200;
            const int newHeight = 900;

            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }
#endif

    }
}
