using Android.Graphics;
using System;
using System.Collections.Generic;
using WFIWLib.Models;

namespace WaitForIt.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string OriginalLanguage { get; set; }
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public string PosterPath { get; set; }
        public string PosterImagePath { get; set; }
        public string PosterBigImagePath { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Title { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public List<int> Reviews { get; set; }
        public int UserReview { get; set; }
        public int YesPercentage { get { return GetPercentage(PositiveReviewsCount); } }
        public int NoPercentage { get { return GetPercentage(NegativeReviewsCount); } }


        public int PositiveReviewsCount { get { return Reviews.FindAll(x => x != 3).Count; } }
        public int NegativeReviewsCount { get { return Reviews.FindAll(x => x == 3).Count; } }
        private int TotalReviewsCount { get { return Reviews.Count == 0 ? 1 : Reviews.Count; } }



        public Film()
        {
            UserReview = -1;
        }



        public override string ToString()
        {
            return Title;
        }


        private int GetPercentage(int reviewsCount)
        {
            float quotient = (float)reviewsCount / (float)TotalReviewsCount;
            return (int)(quotient * 100);
        }
    }
}