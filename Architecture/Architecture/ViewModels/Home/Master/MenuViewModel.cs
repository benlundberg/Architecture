using Xamarin.Forms;

namespace Architecture
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public Page Page { get; set; }
    }
}