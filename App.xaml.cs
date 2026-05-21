using Microsoft.Extensions.DependencyInjection;

namespace S6dchinchin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new view.vEstudiante());
        }
    }
}