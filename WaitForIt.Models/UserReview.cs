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
    public class UserReview
    {
        public int MovieId { get; set; }
        public int Status { get; set; }

        public UserReview(int movieId)
        {
            MovieId = movieId;
            Status = -1;
        }
        public UserReview(int movieId, int status)
        {
            MovieId = movieId;
            Status = status;
        }

        public void Dispose()
        {
            MovieId = 0;
            Status = -1;
        }
    }
}