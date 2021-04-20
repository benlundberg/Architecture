using Xamarin.Forms;

namespace Architecture.Controls
{
    public class CustomFrame : Frame
	{
        public CustomFrame()
        {
            BorderColor = App.Current.GrayColor();
        }

        public static BindableProperty ElevationProperty = BindableProperty.Create(
            propertyName: nameof(Elevation), 
            returnType: typeof(float), 
            declaringType: typeof(CustomFrame), 
            defaultValue: 16.0f);

        public float Elevation
        {
            get
            {
                return (float)GetValue(ElevationProperty);
            }
            set
            {
                SetValue(ElevationProperty, value);
            }
        }
    }
}
