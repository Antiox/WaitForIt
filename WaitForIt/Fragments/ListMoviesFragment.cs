using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using WaitForIt.BLL;
using WaitForIt.Activities;
using WaitForIt.Models;
using WaitForIt;
using WaitForIt.Views;
using Java.Lang;
using System.Threading.Tasks;

namespace WaitForIt.Fragments
{
    public class ListMoviesFragment : Fragment
    {
        public FilmListView FilmsListView { get; set; }
        private bool _loadingMoreData = false;
        private int _index = 0;
        private View _footer;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.frg_listMovieFragment, container, false);
        }
        public override void OnStart()
        {
            _footer = Activity.LayoutInflater.Inflate(Resource.Layout.filmListViewFooter, null, false);

            FilmsListView = View.FindViewById<FilmListView>(Resource.Id.filmsListView);
            if(FilmsListView.FooterViewsCount == 0 && !WaitForItBLL.Singleton.NoMoreMoviesToLoad) FilmsListView.AddFooterView(_footer, null, false);
            FilmsListView.DataSource = SharedComponents.ShownMovies;
            FilmsListView.ItemClick += FilmsListView_ItemClick;
            FilmsListView.Scroll += FilmsListView_Scroll;
            FilmsListView.SetSelection(_index);

            base.OnStart();
        }

        private void FilmsListView_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            bool scrollLimit = e.FirstVisibleItem + e.VisibleItemCount >= e.TotalItemCount - 10 && e.TotalItemCount != 0;

            if (scrollLimit && !_loadingMoreData && !WaitForItBLL.Singleton.NoMoreMoviesToLoad)
            {
                _loadingMoreData = true;
                new Thread(LoadAdditionnalMovies).Start();
            }
        }

        private void FilmsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            _index = FilmsListView.FirstVisiblePosition;
            SharedComponents.CurrentFilm = FilmsListView.SelectedBoundItem;
            ((MainActivity)Activity).LoadMovieFragment();
        }

        private void LoadAdditionnalMovies()
        {
            List<Film> additionalRecords = new List<Film>();
            lock(SharedComponents.ShownMovies) additionalRecords = WaitForItBLL.Singleton.GetNextMovies();
            Activity.RunOnUiThread((() => FilmsListView.AddRecordToDataSource(additionalRecords)));
            if(WaitForItBLL.Singleton.NoMoreMoviesToLoad) Activity.RunOnUiThread((() => FilmsListView.RemoveFooterView(_footer)));
            _loadingMoreData = false;
        }
    }
}