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
using WaitForIt.BLL;

namespace WaitForIt
{
    static public class SharedComponents
    {
        static public Film CurrentFilm { get; set; }
        static public List<Film> ShownMovies { get; set; }
        static public User CurrentUser { get; set; }
        static public Activity MainActivity { get; set; }


        static public void AutoConnect(Context context)
        {
            WaitForItBLL.Singleton.SetAppContext(context);
            CurrentUser = WaitForItBLL.Singleton.AutoConnect();

            if (CurrentUser.IsConnected)
                CurrentUser.Reviews = WaitForItBLL.Singleton.GetReviewsByUser(CurrentUser.Token);
        }
    }
}