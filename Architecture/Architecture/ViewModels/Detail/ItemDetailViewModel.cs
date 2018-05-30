using Cinematics;

namespace Architecture
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemDetailViewModel(MovieDetail movieDetail)
        {
            Movie = movieDetail;
        }

        public MovieDetail Movie { get; private set; }
    }
}
