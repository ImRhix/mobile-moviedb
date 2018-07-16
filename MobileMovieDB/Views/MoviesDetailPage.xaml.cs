using MobileMovieDB.Models;
using MobileMovieDB.Services;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace MobileMovieDB.Views
{
    public partial class MoviesDetailPage : ContentPage
    {
        private MovieService _movieService = new MovieService();

        private Movie _movie = new Movie();

        public MoviesDetailPage(string titlePassed){
            if (titlePassed == null)
                throw new ArgumentNullException(nameof(titlePassed));

            _movie.Title = titlePassed;

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var uniqueMovie = await _movieService.GetMovie(_movie.Title);

            foreach (var item in uniqueMovie.Results)
                Debug.WriteLine($"Title:  {item.Title} // Id: {item.Id}");

            BindingContext = uniqueMovie.Results.FirstOrDefault();

            base.OnAppearing();
        }
    }
}
