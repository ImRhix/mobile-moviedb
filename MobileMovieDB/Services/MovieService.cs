using System;
using System.Net.Http;
using System.Threading.Tasks;
using MobileMovieDB.Models;
using Newtonsoft.Json;
using System.Net;

namespace MobileMovieDB.Services
{
    public class MovieService
    {
        // NOTE: you shouldn't be exposing direct access keys in your code, but this is a simple way to get the job done.
        //       If you want to try this code i also advice going to www.themoviedb.org > create an account / login > settings > API and create an access key for yourself.
        // API Key(v3 auth)
        // 0f1d0b73d25595e9806aede52220a269

        public static readonly int MinSearchLength = 3;

        private const string apiKey = "0f1d0b73d25595e9806aede52220a269";
        private string movieUrl = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}";
        private const string imageUrl = "https://image.tmdb.org/t/p/w500/";

        private HttpClient _client = new HttpClient();

        // example request
        // https://api.themoviedb.org/3/search/movie?api_key=0f1d0b73d25595e9806aede52220a269&query=fights



        /// <summary>
        /// Finds the movies that start with the same characters as the ones in the search box. Returns a RootObject.
        /// </summary>
        public async Task<RootObject> FindMoviesByTit(string searchWords){
            if (searchWords.Length < MinSearchLength)
                return null;

            var response = await _client.GetAsync($"{movieUrl}&query={searchWords}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            string content = await response.Content.ReadAsStringAsync();

            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(content);

            return rootObject;
        }



        /// <summary>
        /// Gets movies that have the exact same title as the one given as a parameter. Called upon tapping a row from the listview.
        /// Correct way would be to find the movie by ID. That implies creating a new model, since an api call based on movieID returns a different json result.
        /// </summary>
        public async Task<RootObject> GetMovie(string title)
        {
            var response = await _client.GetAsync($"{movieUrl}&query={title}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            string content = await response.Content.ReadAsStringAsync();

            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(content);

            return rootObject;
        }
    }
}
