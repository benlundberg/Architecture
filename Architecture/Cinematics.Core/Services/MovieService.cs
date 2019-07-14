using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Architecture.Core;

namespace Cinematics.Core
{
	public class MovieService : BaseService
	{
		public async Task<IList<Movie>> GetDiscoverMoviesAsync(int page = 1)
		{
			try
			{
				var res = await MakeRequestAsync<RootObject>(string.Format(ServiceConfig.DISCOVER_URL, page), HttpMethod.Get);

				return res?.Results?.ToList() ?? new List<Movie>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<MovieDetail> GetMovieDetailAsync(int id)
		{
			try
			{
				var res = await MakeRequestAsync<MovieDetail>(string.Format(ServiceConfig.GET_MOVIE_DETAIL, id), HttpMethod.Get);

				return res;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<IList<Movie>> GetSimiliarMoviesAsync(int id)
		{
			try
			{
				var res = await MakeRequestAsync<RootObject>(string.Format(ServiceConfig.GET_SIMILAR_MOVIES, id), HttpMethod.Get);

				return res?.Results?.ToList() ?? new List<Movie>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<IList<Movie>> GetTopRatedMoviesAsync(int page = 1)
		{
			try
			{
				var res = await MakeRequestAsync<RootObject>(string.Format(ServiceConfig.TOP_RATED, page), HttpMethod.Get);

				return res?.Results?.ToList() ?? new List<Movie>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<IList<Movie>> SearchMoviesAsync(string query)
		{
			try
			{
				var res = await MakeRequestAsync<RootObject>(string.Format(ServiceConfig.SEARCH_MOVIE, query), HttpMethod.Get);

				return res?.Results?.ToList() ?? new List<Movie>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
