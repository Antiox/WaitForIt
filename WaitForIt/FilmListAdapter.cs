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
using System.Threading.Tasks;
using Android.Graphics;
using System.Threading;
using WaitForIt.Views;
using UrlImageViewHelper;

namespace WaitForIt
{
    class FilmListAdapter : BaseAdapter<Film>
    {
        Context _context;
        List<Film> _films;
        TextView _filmTitleTextView;
        TextView _filmReviewPositivePercentageTextView;
        TextView _filmReviewNegativePercentageTextView;
        ImageView _filmPosterImageView;


        public override Film this[int position]
        {
            get { return _films[position]; }
        }
        public override int Count
        {
            get { return _films.Count; }
        }


        public FilmListAdapter(Context context, List<Film> items)
        {
            _context = context;
            _films = items;
        }


        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Film item = _films[position];
            View view = convertView;

            view = view == null ? ((Activity)_context).LayoutInflater.Inflate(Resource.Layout.filmListViewItem, null) : view;

            _filmTitleTextView = view.FindViewById<TextView>(Resource.Id.listItemFilmTitleTextView);
            _filmReviewNegativePercentageTextView = view.FindViewById<TextView>(Resource.Id.reviewPercentageNoItemTextView);
            _filmReviewPositivePercentageTextView = view.FindViewById<TextView>(Resource.Id.reviewPercentageYesItemTextView);
            _filmPosterImageView = view.FindViewById<ImageView>(Resource.Id.filmPosterItemImageView);

            _filmPosterImageView.SetUrlDrawable(item.PosterImagePath, Android.Resource.Drawable.IcMenuGallery);
            _filmTitleTextView.Text = item.Title;
            _filmReviewPositivePercentageTextView.Text = _context.Resources.GetString(Resource.String.YesListItem, item.YesPercentage);
            _filmReviewNegativePercentageTextView.Text = _context.Resources.GetString(Resource.String.NoListItem, item.NoPercentage);
            return view;
        }
    }
}