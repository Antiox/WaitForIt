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
using TMDbLib.Objects.General;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using System.Globalization;
using TMDbLib.Objects.Search;
using WFIWLib.Models;
using WFIWLib;
using WaitForIt.Models;
using Android.Preferences;

namespace WaitForIt.DAL
{
    public class WaitForItDAL
    {
        private static WaitForItDAL _instance;
        public static WaitForItDAL Singleton
        {
            get
            {
                if (_instance == null) _instance = new WaitForItDAL();
                return _instance;
            }
        }
        private TMDbClient _tmdbClient;
        private WFIClient _wfiClient;
        private Context _appContext;

        private WaitForItDAL()
        {
            string systemLanguage = RegionInfo.CurrentRegion.Name.ToLower();

            _tmdbClient = new TMDbClient("SECRET");
            _tmdbClient.GetConfig();
            _tmdbClient.DefaultLanguage = (systemLanguage == "en" ? "" : systemLanguage);

            //_wfiClient = new WFIClient("waitforit.ovh");
            _wfiClient = new WFIClient("192.168.1.22");
        }


        public SearchContainer<MovieResult> GetMovies(int page)
        {
            return _tmdbClient.GetMovieList(MovieListType.NowPlaying, page);
        }

        public SearchContainer<SearchMovie> GetMoviesBySearch(string query, int page)
        {
            return _tmdbClient.SearchMovie(query, page);
        }

        public Movie GetMovieFullDetails(int movieId)
        {
            return _tmdbClient.GetMovie(movieId, MovieMethods.Credits);
        }

        public ApiResult ConnectUser(string username, string password)
        {
            ApiResult result = _wfiClient.LoginUser(username, password);
            PutUserInCache(username, result.Token);
            return result;
        }

        public ApiResult Subscribe(string username, string password, string mail)
        {
            ApiResult result = _wfiClient.CreateUser(username, password, mail);
            PutUserInCache(username, result.Token);
            return result;
        }

        public string GetMovieImageURL(string baseUrl, ImageSizeEnum size)
        {
            if (baseUrl == null) return string.Empty;
            return _tmdbClient.GetImageUrl("w" + (int)size, baseUrl).AbsoluteUri;
        }

        public ApiResult SendReview(string token, int movieId, int reviewStatus)
        {
            return _wfiClient.SendReview(token, movieId, reviewStatus);
        }

        public ApiResult GetMovieReviews(int movieId)
        {
            return _wfiClient.GetReviewsByMovie(movieId);
        }

        public ApiResult GetMovieReviewByUser(string token, int movieId)
        {
            return _wfiClient.GetUserReview(token, movieId);
        }

        public ApiResult GetReviewsByUser(string token)
        {
            return _wfiClient.GetReviewsByUser(token);
        }

        public void SetAppContext(Context context)
        {
            _appContext = context;
        }

        public void RemoveUserFromCache()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(_appContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove("userName");
            editor.Remove("token");
            editor.Apply();
        }

        private void PutUserInCache(string username, string token)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(_appContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("userName", username);
            editor.PutString("token", token);
            editor.Apply();
        }

        public User GetUserInCache()
        {
            User user = new User();

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(_appContext);
            user.Username = prefs.GetString("userName", string.Empty);
            user.Token = prefs.GetString("token", string.Empty);

            return user;
        }
    }
}