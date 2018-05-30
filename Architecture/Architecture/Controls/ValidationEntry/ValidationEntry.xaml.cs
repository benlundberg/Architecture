using Xamarin.Forms;

namespace Architecture.Controls
{
	public partial class ValidationEntry : ContentView
	{
		public ValidationEntry()
		{
			InitializeComponent();

			entryField.BindingContext = this;

			entryField.Unfocused += (sender, e) => 
			{
				if (string.IsNullOrEmpty(ValidationText))
				{
					return;
				}

				if (string.IsNullOrEmpty(Text))
				{
					validationLabel.Text = ValidationText;
					validationLabel.IsVisible = true;
				}
				else
				{
					validationLabel.Text = string.Empty;
					validationLabel.IsVisible = false;
				}
			};

			entryField.TextColor = Color.Black;
			entryField.PlaceholderColor = Color.Gray;
			entryField.Keyboard = Keyboard;
		}

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create("Text", typeof(string), typeof(ValidationEntry), defaultBindingMode: BindingMode.TwoWay, defaultValue: string.Empty);

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create("Placeholder", typeof(string), typeof(ValidationEntry), string.Empty, propertyChanged: (bindable, oldValue, newValue) =>
			{
				var entry = (ValidationEntry)bindable;
				entry.entryField.Placeholder = (string)newValue;
			});

		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set { SetValue(PlaceholderProperty, value); }
		}

		public static readonly BindableProperty IconSourceProperty =
			BindableProperty.Create("IconSource", typeof(string), typeof(ValidationEntry), string.Empty, propertyChanged: (bindable, oldValue, newValue) =>
			{
				var entry = (ValidationEntry)bindable;
				entry.imgIcon.Source = (string)newValue;
				entry.imgIcon.IsVisible = true;
			});

		public string IconSource
		{
			get { return (string)GetValue(IconSourceProperty); }
			set { SetValue(IconSourceProperty, value); }
		}

		public static readonly BindableProperty ValidationTextProperty =
			BindableProperty.Create("ValidationText", typeof(string), typeof(ValidationEntry), string.Empty, propertyChanged: (bindable, oldValue, newValue) =>
			{
				var entry = (ValidationEntry)bindable;
				entry.validationLabel.Text = (string)newValue;
			});

		public string ValidationText
		{
			get { return (string)GetValue(ValidationTextProperty); }
			set { SetValue(ValidationTextProperty, value); }
		}

		public static readonly BindableProperty IsPasswordProperty =
			BindableProperty.Create("IsPassword", typeof(bool), typeof(ValidationEntry), false, propertyChanged: (bindable, oldValue, newValue) =>
			{
				var entry = (ValidationEntry)bindable;
				entry.entryField.IsPassword = (bool)newValue;
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

		public Color HintColor
		{
			get;
			set;
		}

		public Keyboard Keyboard
		{
			get;
			set;
		}
	}
}
