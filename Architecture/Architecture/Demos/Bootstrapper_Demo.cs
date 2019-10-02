using Architecture.Demos.UI.Login;
using Architecture.Demos.UI.ForgotPassword;
using Architecture.Demos.UI.Register;
using Architecture.Demos.UI.Details;

namespace Architecture.Demos
{
    public class Bootstrapper_Demo
    {
        public static void Init()
        {
            ViewContainer.Current.Register<LoginViewModel, LoginPage>();
			ViewContainer.Current.Register<ForgotPasswordViewModel, ForgotPasswordPage>();
			ViewContainer.Current.Register<RegisterViewModel, RegisterPage>();
			ViewContainer.Current.Register<DetailsViewModel, DetailsPage>();
        }
	}
}
