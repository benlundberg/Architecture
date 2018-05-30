namespace Cinematics
{
    public class ServiceConfig
    {
        private static readonly string API_KEY = "50a24e67902b9a999d442a7f641a0a5c";

        private static readonly string BASE_URL = "https://api.themoviedb.org/3";

        public static string GET_POSTER = "https://image.tmdb.org/t/p/";

        public static string DISCOVER_URL = BASE_URL + "/discover/movie?api_key=" + API_KEY + "&page={0}";

        public static string GET_MOVIE_DETAIL = BASE_URL + "/movie/{0}?api_key=" + API_KEY;

        public static string SEARCH_MOVIE = BASE_URL + "/search/movie?query={0}&api_key=" + API_KEY;

        public static string TOP_RATED = BASE_URL + "/movie/top_rated?api_key=" + API_KEY + "&page={0}";

        public static string GET_SIMILAR_MOVIES = BASE_URL + "/movie/{0}/similar?api_key=" + API_KEY + "&language=en-US&page=1";
    }
}
