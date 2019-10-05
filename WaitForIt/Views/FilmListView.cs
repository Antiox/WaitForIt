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
using WaitForIt.Models;

namespace WaitForIt.Views
{
    public class FilmListView : ListView
    {
        private List<Film> _dataSource;
        public List<Film> DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                ResetAdapter();
            }
        }
        public Film SelectedBoundItem { get; set; }


        public FilmListView(Context context) 
            : base(context)
        {
            Initialize();
        }
        public FilmListView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }
        public FilmListView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize();
        }
        public FilmListView(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
            Initialize();
        }
        public FilmListView(Context context, IAttributeSet attrs, int defStyle, int defStyleRes)
            : base(context, attrs, defStyle, defStyleRes)
        {
            Initialize();
        }


        private void FilmListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedBoundItem = DataSource[e.Position];
        }


        private void Initialize()
        {
            ItemClick += FilmListView_ItemClick;
        }

        private void ResetAdapter()
        {
            Adapter = new FilmListAdapter(Context, _dataSource);
        }

        public void AddRecordToDataSource(List<Film> records)
        {
            _dataSource.AddRange(records);
            ((FilmListAdapter)((HeaderViewListAdapter)Adapter).WrappedAdapter).NotifyDataSetChanged();
        }
    }
}