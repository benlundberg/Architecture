namespace Architecture.UIKit.ViewModels
{
    public class MessageItemViewModel : BaseItemViewModel
    {
        public string Message { get; set; }
        public string From { get; set; }
        public int UnseenMessages { get; set; }
        public string Time { get; set; }
        public string ProfilePicture { get; set; }
        public bool Online { get; set; }
    }
}
