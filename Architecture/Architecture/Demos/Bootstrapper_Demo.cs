namespace Architecture.Demos
{
    public class Bootstrapper_Demo
    {
        public static void Init()
        {
            ViewContainer.Current.Register<LoginViewModel, LoginPage>();
			ViewContainer.Current.Register<SignUpViewModel, SignUpPage>();
            ViewContainer.Current.Register<ForgotPasswordViewModel, ForgotPasswordPage>();
			ViewContainer.Current.Register<DetailsViewModel, DetailsPage>();
            ViewContainer.Current.Register<ListViewModel, ListPage>();
            ViewContainer.Current.Register<ItemViewModel, ItemPage>();
            ViewContainer.Current.Register<ListCardViewModel, ListCardViewPage>();
            ViewContainer.Current.Register<ControlsViewModel, ControlsPage>();
        }
	}
}
