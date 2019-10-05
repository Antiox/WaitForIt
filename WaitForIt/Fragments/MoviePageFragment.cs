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
using Android.Graphics;
using WaitForIt.Models;
using Android.Graphics.Drawables;
using UrlImageViewHelper;
using System.Globalization;
using Android.Text;

namespace WaitForIt.Fragments
{
    public class MoviePageFragment : Fragment
    {
        TextView _movieTitleTextView;
        TextView _movieReleaseDateTextView;
        TextView _movieOverviewTextView;
        TextView _movieDirectorTextView;
        TextView _movieActorsTextView;
        TextView _movieNoteTextView;
        TextView _movieNoteCountTextView;
        TextView _movieRuntimeTextView;
        TextView _movieGenresTextView;
        TextView _reviewStatusTextView;
        TextView _positiveReviewsTextView;
        TextView _negativeReviewsTextView;
        TextView _percentageTextView;
        ProgressBar _percentageProgressBar;
        ImageView _moviePosterImageView;
        Button _yepButton;
        Button _nopButton;
        Dialog _reviewDialog;
        AlertDialog.Builder _reviewBuilderAlertDialog;
        UserReview _userReview;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitializeReviewAlertDialog();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.frg_moviePageFragment, container, false);
        }
        public override void OnStart()
        {
            Film f = SharedComponents.CurrentFilm;
            DetailedFilm df = WaitForItBLL.Singleton.GetMovie(f);

            _userReview = SharedComponents.CurrentUser.GetUserReview(f.Id);

            _movieTitleTextView = Activity.FindViewById<TextView>(Resource.Id.filmNameTextView);
            _movieReleaseDateTextView = Activity.FindViewById<TextView>(Resource.Id.filmRealeaseDateTextView);
            _movieOverviewTextView = Activity.FindViewById<TextView>(Resource.Id.filmOverviewTextView);
            _moviePosterImageView = Activity.FindViewById<ImageView>(Resource.Id.filmPosterImageView);
            _movieDirectorTextView = Activity.FindViewById<TextView>(Resource.Id.filmDirectorTextView);
            _movieActorsTextView = Activity.FindViewById<TextView>(Resource.Id.filmCastTextView);
            _movieNoteTextView = Activity.FindViewById<TextView>(Resource.Id.filmNotesTextView);
            _movieNoteCountTextView = Activity.FindViewById<TextView>(Resource.Id.filmNotesCountTextView);
            _movieRuntimeTextView = Activity.FindViewById<TextView>(Resource.Id.filmRuntimeTextView);
            _movieGenresTextView = Activity.FindViewById<TextView>(Resource.Id.filmGenresTextView);
            _yepButton = Activity.FindViewById<Button>(Resource.Id.yepButton);
            _nopButton = Activity.FindViewById<Button>(Resource.Id.nopButton);
            _reviewStatusTextView = Activity.FindViewById<TextView>(Resource.Id.reviewStatusTextView);
            _positiveReviewsTextView = Activity.FindViewById<TextView>(Resource.Id.positiveReviewsTextView);
            _negativeReviewsTextView = Activity.FindViewById<TextView>(Resource.Id.negativeReviewsTextView);
            _percentageTextView = Activity.FindViewById<TextView>(Resource.Id.percentageTextView);
            _percentageProgressBar = Activity.FindViewById<ProgressBar>(Resource.Id.percentageProgressBar);


            _movieTitleTextView.Text = df.Title;
            _movieReleaseDateTextView.Text = Resources.GetString(Resource.String.Released, df.ReleaseDate.ToLongDateString());
            _movieOverviewTextView.Text = (df.Overview == null || df.Overview == string.Empty) ? Resources.GetString(Resource.String.EmptyOverview) : df.Overview;
            _movieActorsTextView.Text = df.Acteurs;
            _movieDirectorTextView.Text = Resources.GetString(Resource.String.By, df.Realisateur);
            _movieNoteTextView.Text = $"{df.VoteAverage}/10";
            _movieNoteCountTextView.Text = Resources.GetQuantityString(Resource.Plurals.ReviewsCount, df.VoteCount, df.VoteCount);
            _movieRuntimeTextView.Text = df.Duree.ToString();
            _movieGenresTextView.Text = df.Genres;

            if (df.Duree == TimeSpan.Zero)
                _movieRuntimeTextView.Visibility = ViewStates.Gone;
            if(df.Realisateur == null)
                _movieDirectorTextView.Visibility = ViewStates.Gone;


            _moviePosterImageView.SetUrlDrawable(df.PosterBigImagePath, Android.Resource.Drawable.IcMenuGallery);
            _positiveReviewsTextView.Text = Resources.GetString(Resource.String.SaidYes, df.PositiveReviewsCount);
            _negativeReviewsTextView.Text = Resources.GetString(Resource.String.SaidNo, df.NegativeReviewsCount);
            _percentageTextView.Text = $"{f.YesPercentage}%";
            _percentageProgressBar.SetProgress(f.YesPercentage, false);
            _percentageProgressBar.ProgressDrawable.SetColorFilter(GetGradientColor(f.YesPercentage), PorterDuff.Mode.SrcIn);

            _yepButton.Click += _yepButton_Click;
            _nopButton.Click += _nopButton_Click;

            GestionAffichageUtilisateur();
            base.OnStart();
        }

        private void _yepButton_Click(object sender, EventArgs e)
        {
            if (SharedComponents.CurrentUser.IsConnected)
                _reviewDialog.Show();
            else
                Toast.MakeText(Activity, Resources.GetString(Resource.String.LoginRequired), ToastLength.Short).Show();
        }
        private void _nopButton_Click(object sender, EventArgs e)
        {
            if (SharedComponents.CurrentUser.IsConnected)
            {
                _userReview.Status = 3;
                WaitForItBLL.Singleton.SendReview(SharedComponents.CurrentUser.Token, SharedComponents.CurrentFilm.Id, _userReview.Status);
                SharedComponents.CurrentUser.Reviews.Add(_userReview);
                Toast.MakeText(Activity, Resources.GetString(Resource.String.ReviewSent), ToastLength.Short).Show();
                SetNegativeReviewFeedback();
            }
            else
                Toast.MakeText(Activity, Resources.GetString(Resource.String.LoginRequired), ToastLength.Short).Show();
        }
        private void OnAlertDialogOkButton_Click(object sender, DialogClickEventArgs e)
        {
            WaitForItBLL.Singleton.SendReview(SharedComponents.CurrentUser.Token, SharedComponents.CurrentFilm.Id, _userReview.Status);
            SharedComponents.CurrentUser.Reviews.Add(_userReview);
            Toast.MakeText(Activity, Resources.GetString(Resource.String.ReviewSent), ToastLength.Short).Show();
            SetPositiveReviewFeedback();
        }
        private void OnAlertDialogItem_Click(object sender, DialogClickEventArgs e)
        {
            _userReview.Status = e.Which;
        }

        private void InitializeReviewAlertDialog()
        {
            string[] reviewOptions = new[] 
            {
                Resources.GetString(Resource.String.DuringCredits),
                Resources.GetString(Resource.String.AfterCredits),
                Resources.GetString(Resource.String.DuringAfterCredits)
            };

            _reviewBuilderAlertDialog = new AlertDialog.Builder(Activity);
            _reviewBuilderAlertDialog.SetTitle(Resources.GetString(Resource.String.When));
            _reviewBuilderAlertDialog.SetSingleChoiceItems(reviewOptions, 0, OnAlertDialogItem_Click);
            _reviewBuilderAlertDialog.SetNeutralButton(Resources.GetString(Resource.String.Confirm), OnAlertDialogOkButton_Click);
            _reviewDialog = _reviewBuilderAlertDialog.Create();
        }
        public void GestionAffichageUtilisateur()
        {
            if (_userReview.Status == 3) SetNegativeReviewFeedback();
            else if (_userReview.Status > -1) SetPositiveReviewFeedback();
            else SetNeutralReviewFeedback();
        }
        private void SetNegativeReviewFeedback()
        {
            _nopButton.Background.SetColorFilter(Color.DarkRed, PorterDuff.Mode.Multiply);
            _yepButton.Background.ClearColorFilter();
            _reviewStatusTextView.Text = Resources.GetString(Resource.String.YouSaidNothing);
            _reviewStatusTextView.SetTextColor(Color.DarkRed);
        }
        private void SetPositiveReviewFeedback()
        {
            _yepButton.Background.SetColorFilter(Color.DarkGreen, PorterDuff.Mode.Multiply);
            _nopButton.Background.ClearColorFilter();
            _reviewStatusTextView.Text = Resources.GetString(Resource.String.YouSaidSomething);
            _reviewStatusTextView.SetTextColor(Color.DarkGreen);
        }
        private void SetNeutralReviewFeedback()
        {
            _reviewStatusTextView.Text = string.Empty;
            _yepButton.Background.ClearColorFilter();
            _nopButton.Background.ClearColorFilter();
        }

        private Color GetGradientColor(int progressBarValue)
        {
            float val = progressBarValue / 100f;
            return Color.HSVToColor(new [] { val * 120f, 1f, 1f });
        }
    }
}