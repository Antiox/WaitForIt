using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WaitForIt.Models;
using WaitForIt.DAL;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using Android.Graphics;
using System.Net;
using TMDbLib.Objects.Search;
using WFIWLib.Models;
using System.Threading.Tasks;

namespace WaitForIt.BLL
{
    public class WaitForItBLL
    {
        private MoviesLoader _loader;
        private static WaitForItBLL _instance;
        public static WaitForItBLL Singleton
        {
            get
            {
                if (_instance == null) _instance = new WaitForItBLL();
                return _instance;
            }
        }
        public bool NoMoreMoviesToLoad { get { return _loader.NoMoreMoviesToLoad; } }




        private WaitForItBLL()
        {
        }



        public List<Film> GetPopularMoviesList()
        {
            _loader = new MoviesLoader(MovieListType.PopularMovies);
            return GetNextMovies();
        }

        public List<Film> GetMoviesBySearch(string query)
        {
            _loader = new MoviesLoader(MovieListType.SearchMovies, query);
            return GetNextMovies();
        }

        public List<Film> GetNextMovies()
        {
            return _loader.GetNextMovies();
        }

        public User ConnectUser(string username, string password)
        {
            ApiResult result = WaitForItDAL.Singleton.ConnectUser(username, password);
            User user = new User();

            if (!result.HasError)
            {
                user.Token = result.Token;
                user.Username = username;
                user.Reviews = GetReviewsByUser(result.Token);
            }

            return user;
        }

        public User Subscribe(string username, string password, string mail)
        {
            ApiResult result = WaitForItDAL.Singleton.Subscribe(username, password, mail);
            User user = new User();

            if (!result.HasError)
            {
                user.Token = result.Token;
                user.Username = username;
            }

            return user;
        }

        public bool SendReview(string token, int movieId, int reviewStatus)
        {
            ApiResult result = WaitForItDAL.Singleton.SendReview(token, movieId, reviewStatus);
            return result.Success;
        }

        public List<UserReview> GetReviewsByUser(string token)
        {
            ApiResult result = WaitForItDAL.Singleton.GetReviewsByUser(token);
            List<UserReview> reviews = new List<UserReview>();

            foreach (Review resultReview in result.Reviews)
                reviews.Add(new UserReview(resultReview.MovieID, resultReview.Status));

            return reviews;
        }

        public int GetMovieReviewByUser(string token, int movieId)
        {
            ApiResult result = WaitForItDAL.Singleton.GetMovieReviewByUser(token, movieId);

            if (result.Reviews.Count == 0) return -1;
            else return result.Reviews[0].Status;
        }

        public void SetAppContext(Context context)
        {
            WaitForItDAL.Singleton.SetAppContext(context);
        }

        public void DisconnectUser()
        {
            WaitForItDAL.Singleton.RemoveUserFromCache();
        }

        public User AutoConnect()
        {
            return WaitForItDAL.Singleton.GetUserInCache();
        }

        public DetailedFilm GetMovie(Film film)
        {
            DetailedFilm detailedFilm = new DetailedFilm(film);
            TMDbLib.Objects.Movies.Movie movie = WaitForItDAL.Singleton.GetMovieFullDetails(film.Id);

            detailedFilm.Duree = TimeSpan.FromMinutes(movie.Runtime ?? 0);
            detailedFilm.Genres = string.Join(" / ", movie.Genres.Select(i => i.Name).ToArray());
            detailedFilm.Realisateur = movie.Credits.Crew.Find(i => i.Job == "Director")?.Name;
            detailedFilm.Acteurs = string.Join(System.Environment.NewLine, movie.Credits.Cast.Select(i => i.Name).Take(4).ToArray());
            detailedFilm.VoteAverage = movie.VoteAverage;
            detailedFilm.VoteCount = movie.VoteCount;


            return detailedFilm;
        }
    }
}
