using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private MapViewModel viewModel;

        public MapPage()
        {
            InitializeComponent();

            void OptionsMenuHeightChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
            {
                if (OptionsMenu != null && args.PropertyName == "Height" && OptionsMenu.Height != -1)
                {
                    optionsMenuHeight = OptionsMenu.Height * 2;
                    OptionsMenu.HeightRequest = 0;
                    OptionsMenu.PropertyChanged -= OptionsMenuHeightChanged;
                }
            };

            OptionsMenu.PropertyChanged += OptionsMenuHeightChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.Initialize(MapContent, null);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            viewModel = BindingContext as MapViewModel;
            viewModel.AnimateMenu += MapPage_AnimateMenu;
        }

        private void MapPage_AnimateMenu(object sender, System.EventArgs e)
        {
            if (isAnimating)
            {
                return;
            }

            isAnimating = true;

            if (OptionsMenu.HeightRequest > 0)
            {
                OptionsMenu.Animate("invis", (height) =>
                {
                    OptionsMenu.HeightRequest = height;
                },
                start: OptionsMenu.HeightRequest, end: 0, easing: Easing.CubicInOut, finished: (d, b) =>
                {
                    isAnimating = false;
                });
            }
            else
            {
                OptionsMenu.Animate("invis", (height) =>
                {
                    OptionsMenu.HeightRequest = height;
                },
                start: 0, end: optionsMenuHeight, easing: Easing.CubicInOut, finished: (d, b) =>
                {
                    isAnimating = false;
                });
            }
        }

        private double optionsMenuHeight = -1;
        private bool isAnimating;
    }
}