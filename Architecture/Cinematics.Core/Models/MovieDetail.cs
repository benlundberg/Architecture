using System.Linq;

namespace Cinematics.Core
{
    public class MovieDetail
    {
        public bool Adult { get; set; }
        public string Backdrop_path { get; set; }
        public object Belongs_to_collection { get; set; }
        public int Budget { get; set; }
        public Genre[] Genres { get; set; }
        public object Homepage { get; set; }
        public int Id { get; set; }
        public string Imdb_id { get; set; }
        public string Original_language { get; set; }
        public string Original_title { get; set; }
        public string Overview { get; set; }
        public float Popularity { get; set; }
        public string Poster_path { get; set; }
        public ProductionCompany[] Production_companies { get; set; }
        public ProductionCountry[] Production_countries { get; set; }
        public string Release_date { get; set; }
        public int Revenue { get; set; }
        public int Runtime { get; set; }
        public SpokenLanguage[] Spoken_languages { get; set; }
        public string Status { get; set; }
        public string Tagline { get; set; }
        public string Title { get; set; }
        public bool Video { get; set; }
        public float Vote_average { get; set; }
        public int Vote_count { get; set; }

		public string Country
		{
			get 
			{
				return Production_countries?.FirstOrDefault()?.Name;
			}
		}

        public string RuntimeDescription
        {
            get
            {
                return Runtime.ToString() + " min";
            }
        }

        public string GenresDescription
        {
            get
            {
                string genres = string.Empty;

                foreach (var genre in Genres)
                {
                    genres += genre.Name + " ";
                }

                return genres;
            }
        }

		public string Poster45 => ServiceConfig.GET_POSTER + "w45/" + Poster_path;
		public string Poster92 => ServiceConfig.GET_POSTER + "w92/" + Poster_path;
		public string Poster154 => ServiceConfig.GET_POSTER + "w154/" + Poster_path;
		public string Poster185 => ServiceConfig.GET_POSTER + "w185/" + Poster_path;
		public string Poster342 => ServiceConfig.GET_POSTER + "w342/" + Poster_path;
		public string Poster300 => ServiceConfig.GET_POSTER + "w300/" + Poster_path;
		public string Poster500 => ServiceConfig.GET_POSTER + "w500/" + Poster_path;
		public string Poster632 => ServiceConfig.GET_POSTER + "h632/" + Poster_path;
		public string Poster780 => ServiceConfig.GET_POSTER + "w780/" + Poster_path;
		public string Poster1280 => ServiceConfig.GET_POSTER + "w1280/" + Poster_path;
		public string PosterOriginal => ServiceConfig.GET_POSTER + "original/" + Poster_path;
	}
}
