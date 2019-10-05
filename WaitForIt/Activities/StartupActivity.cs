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
using WaitForIt.BLL;
using System.Threading.Tasks;
using Android.Webkit;
using System.IO;
using Android.Preferences;

namespace WaitForIt.Activities
{
    [Activity(Label = "WaitForIt", Icon = "@drawable/IconLauncher", MainLauncher = true)]
    public class StartupActivity : Activity
    {
        private Task _initTask;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.act_splashScreenActivity);
        }

        protected override void OnResume()
        {
            _initTask = new Task(InitializeApp);
            _initTask.ContinueWith(LaunchMainActivity, TaskScheduler.FromCurrentSynchronizationContext());
            _initTask.Start();

            base.OnStart();
        }


        private void InitializeApp()
        {
            SharedComponents.AutoConnect(this);
            SharedComponents.ShownMovies = WaitForItBLL.Singleton.GetPopularMoviesList();
        }
        private void LaunchMainActivity(Task t)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            Finish();
        }
    }
}