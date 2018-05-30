using System.Threading.Tasks;
using Xamarin.Forms;

namespace Architecture.Controls
{
	public partial class FloatingEntry : ContentView
	{
		public FloatingEntry()
		{
			InitializeComponent();

			EntryField.BindingContext = this;

			EntryField.TextColor = Color.White;
			EntryField.PlaceholderColor = Color.White;

			EntryField.Focused += async (s, a) =>
			{
				HiddenBottomBorder.BackgroundColor = HintColor;
				HiddenLabel.TextColor = HintColor;
				HiddenLabel.IsVisible = true;

				if (string.IsNullOrEmpty(EntryField.Text))
				{
					double y = 0;

					if (Device.RuntimePlatform == Device.Android)
					{
						y = ((EntryField.Height / 2) - 4);
					}
					else if (Device.RuntimePlatform == Device.iOS) 
					{
						y = EntryField.Height + 4;
					}

					// animate both at the same time
					await Task.WhenAll(
						HiddenBottomBorder.LayoutTo(new Rectangle(BottomBorder.X, BottomBorder.Y, BottomBorder.Width, BottomBorder.Height), 200),
						HiddenLabel.TranslateTo(HiddenLabel.TranslationX, EntryField.Y - y, 200, Easing.SinIn)
					 );
					EntryField.Placeholder = null;
				}
				else
				{
					await HiddenBottomBorder.LayoutTo(new Rectangle(BottomBorder.X, BottomBorder.Y, BottomBorder.Width, BottomBorder.Height), 200);
				}
			};

			EntryField.Unfocused += async (s, a) =>
			{
				HiddenLabel.TextColor = DefaultColor;
				HiddenLabel.IsVisible = false;
				if (string.IsNullOrEmpty(EntryField.Text))
				{
					// animate both at the same time
					await Task.WhenAll(
						HiddenBottomBorder.LayoutTo(new Rectangle(BottomBorder.X, BottomBorder.Y, 0, BottomBorder.Height), 200),
						HiddenLabel.TranslateTo(HiddenLabel.TranslationX, EntryField.Y, 200, Easing.SinOut)
					 );
					EntryField.Placeholder = Placeholder;
				}
				else
				{
					await HiddenBottomBorder.LayoutTo(new Rectangle(BottomBorder.X, BottomBorder.Y, 0, BottomBorder.Height), 200);
				}
			};
		}

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create("Text", typeof(string), typeof(FloatingEntry), string.Empty);

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create("Placeholder", typeof(string), typeof(FloatingEntry), string.Empty, propertyChanged: (bindable, oldValue, newValue) => 
		{
			var entry = (FloatingEntry)bindable;
			entry.EntryField.Placeholder = (string)newValue;
			entry.HiddenLabel.Text = (string)newValue;
		});

		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set { SetValue(PlaceholderProperty, value); }
		}

		public static readonly BindableProperty IsPasswordProperty =
			BindableProperty.Create("IsPassword", typeof(bool), typeof(FloatingEntry), false, propertyChanged: (bindable, oldValue, newValue) => 
		{
			var entry = (FloatingEntry)bindable;
			entry.EntryField.IsPassword = (bool)newValue;
		});

		public bool IsPassword
		{
			get { return (bool)GetValue(IsPasswordProperty); }
			set { SetValue(IsPasswordProperty, value); }
		}

		public Color TextColor
		{
			get;
			set;
		}

		public Color DefaultColor
		{
			get;
			set;
		}

		public Color HintColor
		{
			get;
			set;
		}
	}
}
