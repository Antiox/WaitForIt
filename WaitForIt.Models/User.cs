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
using WFIWLib.Models;

namespace WaitForIt.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public List<UserReview> Reviews { get; set; }


        public User()
        {
            Username = string.Empty;
            Token = string.Empty;
        }

        public bool IsConnected
        {
            get
            {
                return Token != string.Empty;
            }
        }


        public UserReview GetUserReview(int movieId)
        {
            UserReview ur;
            UserReview defaultReview = new UserReview(movieId, -1);

            if (IsConnected)
            {
                ur = Reviews.Find(r => r.MovieId == movieId);
                if (ur == null) ur = defaultReview;
            }
            else ur = defaultReview;

            return ur;
        }

        public void Dispose()
        {
            for (int i = 0; i < Reviews.Count; i++)
                Reviews[i].Dispose();

            Reviews.Clear();
            Reviews = null;
        }
    }
}