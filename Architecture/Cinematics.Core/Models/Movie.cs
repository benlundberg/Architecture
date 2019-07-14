namespace Cinematics.Core
{
    public class Movie
    {
        public int Vote_count { get; set; }
        public int Id { get; set; }
        public bool Video { get; set; }
        public float Vote_average { get; set; }
        public string Title { get; set; }
        public float Popularity { get; set; }
        public string Poster_path { get; set; }
        public string Original_language { get; set; }
        public string Original_title { get; set; }
        public int[] Genre_ids { get; set; }
        public string Backdrop_path { get; set; }
        public bool Adult { get; set; }
        public string Overview { get; set; }
        public string Release_date { get; set; }

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

        /*    "backdrop_sizes": [
          "w300",
          "w780",
          "w1280",
          "original"
        ],
        "logo_sizes": [
          "w45",
          "w92",
          "w154",
          "w185",
          "w300",
          "w500",
          "original"
        ],
        "poster_sizes": [
          "w92",
          "w154",
          "w185",
          "w342",
          "w500",
          "w780",
          "original"
        ],
        "profile_sizes": [
          "w45",
          "w185",
          "h632",
          "original"
        ],
        "still_sizes": [
          "w92",
          "w185",
          "w300",
          "original"
        ]*/

    }
}
