using Architecture.Demos.UI.Login;
using Architecture.Demos.UI.Register;

namespace Architecture.Demos
{
    public class Bootstrapper_Demo
    {
        public static void Init()
        {
            ViewContainer.Current.Register<LoginViewModel, LoginPage>();
            ViewContainer.Current.Register<RegisterViewModel, RegisterPage>();
        }
    }
}
