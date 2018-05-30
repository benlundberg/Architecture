using Acr.UserDialogs;
using Architecture.Core;
using Cinematics;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Architecture
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel()
        {
            movieService = new MovieService();
        }

        public override void Appearing()
        {
            LoadDiscoverMovies();
            LoadTopRatedMovies();
            LoadWatchlist();
        }

        private void LoadDiscoverMovies()
        {
            Task.Run(async () =>
            {
                try
                {
                    var discoverMovies = await movieService.GetDiscoverMoviesAsync();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DiscoverMovies = new ObservableCollection<Movie>(discoverMovies.Take(6));
                    });
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
            });
        }

        private void LoadTopRatedMovies()
        {
            Task.Run(async () =>
            {
                try
                {
                    var topRatedMovies = await movieService.GetTopRatedMoviesAsync();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        TopRatedMovies = new ObservableCollection<Movie>(topRatedMovies.Take(6));
                    });
                }
                catch (Exception ex)
                {
                    ex.Print();
                }
            });
        }

        private void LoadWatchlist()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        private async Task ItemSelected(Movie item)
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading();

                var movie = await new MovieService().GetMovieAsync(item.Id);

                if (movie == null)
                {
                    UserDialogs.Instance.Alert("Could not find movie");
                }

                await Navigation.PushAsync(ViewContainer.Current.CreatePage(new ItemDetailViewModel(movie)));
            }
            catch (Exception ex)
            {
                ex.Print();
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }

        private ICommand itemClickedCommand;
        public ICommand ItemClickedCommand => itemClickedCommand ?? (itemClickedCommand = new Command(async (param) =>
        {
            if (param as Movie == null)
            {
                return;
            }

            await ItemSelected(param as Movie);
        }));

        private Movie selectedItem;
        public Movie SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; if (selectedItem != null) { ItemSelected(selectedItem); selectedItem = null; } }
        }

        public ObservableCollection<Movie> DiscoverMovies { get; private set; }
        public ObservableCollection<Movie> TopRatedMovies { get; private set; }
        public ObservableCollection<Movie> Watchlist { get; private set; }

        public bool IsDiscoverMoviesVisible { get { return DiscoverMovies?.Any() == true; } }
        public bool IsTopRatedMoviesVisible { get { return TopRatedMovies?.Any() == true; } }
        public bool IsWatchlistVisible { get { return Watchlist?.Any() == true; } }

        private MovieService movieService;
    }
}
