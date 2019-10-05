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

namespace WaitForIt.Models
{
    public class DetailedFilm : Film
    {
        public string Realisateur { get; set; }
        public string Acteurs { get; set; }
        public string Genres { get; set; }
        public decimal Note { get; set; }
        public int NoteCount { get; set; }
        public TimeSpan Duree { get; set; }


        public DetailedFilm(Film f) :base()
        {
            Id = f.Id;
            OriginalLanguage = f.OriginalLanguage;
            OriginalTitle = f.OriginalTitle;
            Overview = f.Overview;
            Popularity = f.Popularity;
            PosterImagePath = f.PosterImagePath;
            PosterBigImagePath = f.PosterBigImagePath;
            ReleaseDate = f.ReleaseDate;
            Title = f.Title;
            VoteAverage = f.VoteAverage;
            VoteCount = f.VoteCount;
            Reviews = f.Reviews;
            UserReview = f.UserReview;
        }
    }
}