namespace Architecture.UIKit.ViewModels
{
    public class ChatItemViewModel : BaseItemViewModel
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public bool IsMe { get; set; }
    }
}
