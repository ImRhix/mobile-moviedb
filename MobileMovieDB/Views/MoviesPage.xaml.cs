using MobileMovieDB.Models;
using MobileMovieDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileMovieDB.Views 
{
    public partial class MoviesPage : ContentPage 
    {
        private MovieService _service = new MovieService();

        private BindableProperty IsSearchingProperty = BindableProperty.Create("IsSearching", typeof(bool), typeof(MoviesPage), false);
        public bool IsSearching {
            get { return (bool)GetValue(IsSearchingProperty); }
            set { SetValue(IsSearchingProperty, value); }
        }

        public MoviesPage() {
            BindingContext = this;
            InitializeComponent();
        }



        // While Typing on the Search bar
        async void OnTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e) {
            if (e.NewTextValue == null || e.NewTextValue.Length < MovieService.MinSearchLength)
                return;

            await FindMovies(tit: e.NewTextValue);
        }



        // Clicking on a movie from the table view. Creates a new page with that movie info
        async void OnMovieSelected(object sender, SelectedItemChangedEventArgs e) {
            if (e.SelectedItem == null)
                return;

            var movieObject = e.SelectedItem as Movie;
            string titleToPass = movieObject.Title;

            moviesListView.SelectedItem = null;

            await Navigation.PushAsync(new MoviesDetailPage(titleToPass));
        }

        public async Task FindMovies(string tit) {
            try {
                IsSearching = true;

                var movies = await _service.FindMoviesByTit(tit);

                moviesListView.ItemsSource = movies.Results;        // binds the items source to the json array that has the info we want (Results)
                moviesListView.IsVisible = movies.Results.Any();
                notFound.IsVisible = !moviesListView.IsVisible;

                //foreach (var item in rootObject.Results)
                //{
                //    System.Diagnostics.Debug.WriteLine($"\nTitle: {item.Title} // Id: {item.Id}");
                //}            
            }
            catch (Exception ex) { await DisplayAlert("Error MoviesPage", ex.Message, "OK"); }
            finally { IsSearching = false; }
        }
    }
}
