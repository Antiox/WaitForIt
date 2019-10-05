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
using WaitForIt.Models;

namespace WaitForIt.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        EditText _usernameEditText;
        EditText _passwordEditText;
        Button _loginButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.act_logInActivity);

            _usernameEditText = FindViewById<EditText>(Resource.Id.loginUsernameEditText);
            _passwordEditText = FindViewById<EditText>(Resource.Id.loginPasswordEditText);
            _loginButton = FindViewById<Button>(Resource.Id.loginButton);

            _usernameEditText.RequestFocus();
            Window.SetSoftInputMode(SoftInput.StateVisible);

            _loginButton.Click += LoginValidationButton_Click;
        }

        private void LoginValidationButton_Click(object sender, EventArgs e)
        {
            User user = WaitForItBLL.Singleton.ConnectUser(_usernameEditText.Text, _passwordEditText.Text);
            SharedComponents.CurrentUser = user;

            if (SharedComponents.CurrentUser.IsConnected)
            {
                SharedComponents.MainActivity.InvalidateOptionsMenu();
                Toast.MakeText(this, Resources.GetString(Resource.String.Welcome, user.Username) , ToastLength.Long).Show();
                Finish();
            }
            else
                Toast.MakeText(this, Resources.GetString(Resource.String.WrongCombination), ToastLength.Short).Show();
        }
    }
}