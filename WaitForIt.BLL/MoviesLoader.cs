using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using WaitForIt.DAL;
using WaitForIt.Models;
using WFIWLib.Models;


namespace WaitForIt.BLL
{
    class MoviesLoader
    {
        public string Query { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalResults { get; set; }
        public bool NoMoreMoviesToLoad { get { return TotalPage == CurrentPage; } }

        private MovieListType _listType;


        public MoviesLoader(MovieListType type)
        {
            _listType = type;
            CurrentPage = 0;
        }
        public MoviesLoader(MovieListType type, string query) : this(type)
        {
            Query = query;
        }


        public List<Film> GetNextMovies() 
        {
            if (_listType == MovieListType.PopularMovies)
                return GetNextPagePopularMoviesList();
            else
                return GetNextPageMoviesBySearch();
        }


        private Film ConvertMovie(object movie)
        {
            Film film = new Film();

            List<PropertyInfo> movieProperties = new List<PropertyInfo>(movie.GetType().GetProperties());
            List<PropertyInfo> filmProperties = new List<PropertyInfo>(film.GetType().GetProperties());

            for (int i = 0; i < movieProperties.Count; i++)
            {
                PropertyInfo pMovie = movieProperties[i];
                PropertyInfo pFilm = filmProperties.Find(p => p.Name == pMovie.Name);

                if(pFilm != null)
                    pFilm.SetValue(film, pMovie.GetValue(movie));
            }
            film.PosterImagePath = GetMovieImageURL(film.PosterPath, ImageSizeEnum.xSmall);
            film.PosterBigImagePath = GetMovieImageURL(film.PosterPath, ImageSizeEnum.Medium);
            film.Reviews = GetMovieReviews(film.Id);

            return film;
        }
        private List<int> GetMovieReviews(int movieId)
        {
            ApiResult result = WaitForItDAL.Singleton.GetMovieReviews(movieId);
            List<int> reviews = new List<int>();

            foreach (Review review in result.Reviews)
                reviews.Add(review.Status);

            return reviews;
        }
        private string GetMovieImageURL(string path, ImageSizeEnum size)
        {
            return WaitForItDAL.Singleton.GetMovieImageURL(path, size);
        }
        private List<Film> GetNextPageMoviesBySearch()
        {
            SearchContainer<SearchMovie> movieResults = WaitForItDAL.Singleton.GetMoviesBySearch(Query, ++CurrentPage);
            List<Film> films = movieResults.Results.Select(mr => ConvertMovie(mr)).ToList();
            TotalResults = movieResults.TotalResults;
            TotalPage = movieResults.TotalPages;

            return films;
        }
        private List<Film> GetNextPagePopularMoviesList()
        {
            SearchContainer<MovieResult> movieResults = WaitForItDAL.Singleton.GetMovies(++CurrentPage);
            List<Film> films = movieResults.Results.Select(mr => ConvertMovie(mr)).ToList();
            TotalResults = movieResults.TotalResults;
            TotalPage = movieResults.TotalPages;

            return films;
        }
    }
}