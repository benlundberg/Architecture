using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Architecture.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlideView : ContentView
    {
        public SlideView()
        {
            InitializeComponent();
            HiddenView.PropertyChanged += HiddenView_PropertyChanged;
        }

        private void HiddenView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (HiddenContent != null && e.PropertyName == "Height" && HiddenView.Height != -1)
            {
                hiddenContentHeight = HiddenView.Height;
                HiddenView.HeightRequest = 0;
                HiddenView.PropertyChanged -= HiddenView_PropertyChanged;
            }
        }

        private ICommand tappedCommand;
        public ICommand TappedCommand => tappedCommand ?? (tappedCommand = new Command(() =>
        {
            if (isAnimating)
            {
                return;
            }

            if (HiddenView.HeightRequest <= 0)
            {
                isAnimating = true;

                HiddenView.Animate("show", (height) =>
                {
                    HiddenView.HeightRequest = height;
                },
                start: 0, end: hiddenContentHeight, easing: Easing.CubicIn, finished: (d, b) =>
                {
                    isAnimating = false;
                });

                IsOpen = true;
            }
            else
            {
                if (HiddenView.HeightRequest > 0)
                {
                    isAnimating = true;

                    HiddenView.Animate("hide", (height) =>
                    {
                        HiddenView.HeightRequest = height;
                    },
                    start: HiddenView.HeightRequest, end: 0, easing: Easing.CubicOut, finished: (d, b) =>
                    {
                        isAnimating = false;
                    });
                }

                IsOpen = false;
            }

        }));

        public static readonly BindableProperty IsOpenProperty =
            BindableProperty.Create(
                propertyName: "IsOpen",
                returnType: typeof(bool),
                declaringType: typeof(SlideView),
                defaultValue: false,
                propertyChanged: IsOpenPropertyChanged);

        private static void IsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is SlideView view))
            {
                return;
            }

            view.TappedCommand?.Execute(null);
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public View HeaderContent { get; set; }
        public View HiddenContent { get; set; }

        private bool isAnimating;
        private double hiddenContentHeight;
    }
}