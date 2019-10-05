using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Views;
using WaitForIt.Fragments;
using Android.Runtime;
using Android.Content.PM;
using WaitForIt.BLL;
using WaitForIt.Models;

namespace WaitForIt.Activities
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity
    {
        SearchView _searchView;
        ListMoviesFragment filmsFragment;
        MoviePageFragment pageFragment;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.act_MainActivity);

            filmsFragment = new ListMoviesFragment();
            pageFragment = new MoviePageFragment();
            LoadAllFragments();

            SharedComponents.MainActivity = this;
        }
        private void SearchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            SharedComponents.ShownMovies = WaitForItBLL.Singleton.GetMoviesBySearch(e.Query);
            filmsFragment.FilmsListView.DataSource = SharedComponents.ShownMovies;
            LoadListFragment();
            Window.SetSoftInputMode(SoftInput.StateHidden);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_login: StartActivity(typeof(LoginActivity)); break;
                case Resource.Id.action_signin: StartActivity(typeof(SignUpActivity)); break;
                case Resource.Id.action_disconnect: DisconnectUser(); break;
                default: return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (SharedComponents.CurrentUser.IsConnected)
                MenuInflater.Inflate(Resource.Menu.disconnectMenu, menu);
            else
                MenuInflater.Inflate(Resource.Menu.mainMenu, menu);

            _searchView = SetActionMenuView<SearchView>(Resource.Id.action_search, menu);
            _searchView.QueryTextSubmit += SearchView_QueryTextSubmit;

            base.OnCreateOptionsMenu(menu);

            return true;
        }
       

        public void LoadMovieFragment()
        {
            FragmentTransaction fragmentTx = FragmentManager.BeginTransaction();
            fragmentTx.Detach(filmsFragment);
            fragmentTx.Attach(pageFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }
        public void LoadListFragment()
        {
            FragmentTransaction fragmentTx = FragmentManager.BeginTransaction();
            fragmentTx.Detach(pageFragment);
            fragmentTx.Attach(filmsFragment);
            fragmentTx.Commit();
        }
        private void LoadAllFragments()
        {
            FragmentTransaction fragmentTx = FragmentManager.BeginTransaction();
            fragmentTx.Add(Resource.Id.fragmentFrameLayout, filmsFragment);
            fragmentTx.Add(Resource.Id.fragmentFrameLayout, pageFragment);
            fragmentTx.Detach(pageFragment);
            fragmentTx.Commit();
        }
        private T SetActionMenuView<T>(int viewId, IMenu menu) where T : View
        {
            var item = menu.FindItem(viewId);
            var itemCompat = MenuItemCompat.GetActionView(item);
            return (T)itemCompat;
        }
        private void DisconnectUser()
        {
            SharedComponents.CurrentUser.Dispose();
            SharedComponents.CurrentUser = new User();
            WaitForItBLL.Singleton.DisconnectUser();
            Toast.MakeText(this, Resources.GetString(Resource.String.Disconnected), ToastLength.Short).Show();
            InvalidateOptionsMenu();
            pageFragment.GestionAffichageUtilisateur();
        }
    }
}