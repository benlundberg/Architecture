using Architecture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinematics
{
    public class MovieService : BaseService
    {
        public async Task<List<Movie>> GetDiscoverMoviesAsync(int page = 1)
        {
            try
            {
                var res = await GetFromService<RootObject>(String.Format(ServiceConfig.DISCOVER_URL, page), ParseType.JSON);

                return res?.Results?.ToList() ?? new List<Movie>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Movie>> GetTopRatedMoviesAsync(int page = 1)
        {
            try
            {
                var res = await GetFromService<RootObject>(String.Format(ServiceConfig.TOP_RATED, page), ParseType.JSON);

                return res?.Results?.ToList() ?? new List<Movie>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MovieDetail> GetMovieAsync(int id)
        {
            try
            {
                var res = await GetFromService<MovieDetail>(String.Format(ServiceConfig.GET_MOVIE_DETAIL, id), ParseType.JSON);

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
