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
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        private Button _validerInscriptionButton;
        private EditText _loginEditText;
        private EditText _mailAddressEditText;
        private EditText _passwordEditText;
        private EditText _confirmPasswordEditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.act_SignUpActivity);

            _validerInscriptionButton = FindViewById<Button>(Resource.Id.subscribeButton);
            _loginEditText = FindViewById<EditText>(Resource.Id.signupUsernameTextView);
            _mailAddressEditText = FindViewById<EditText>(Resource.Id.signupMailTextView);
            _passwordEditText = FindViewById<EditText>(Resource.Id.signupPasswordTextView);
            _confirmPasswordEditText = FindViewById<EditText>(Resource.Id.signupConfirmPasswordTextView);

            _loginEditText.RequestFocus();
            Window.SetSoftInputMode(SoftInput.StateVisible);

            _validerInscriptionButton.Click += ValiderInscriptionButton_Click;
        }


        private void ValiderInscriptionButton_Click(object sender, EventArgs e)
        {
            User user = new User();

            if (_passwordEditText.Text == _confirmPasswordEditText.Text)
                user = WaitForItBLL.Singleton.Subscribe(_loginEditText.Text, _passwordEditText.Text, _mailAddressEditText.Text);
            else
                Toast.MakeText(this, Resources.GetString(Resource.String.PasswordsDontMatch), ToastLength.Short).Show();

            if (user.IsConnected)
            {
                SharedComponents.MainActivity.InvalidateOptionsMenu();
                Toast.MakeText(this, Resources.GetString(Resource.String.SubscriptionSuccessful, user.Username), ToastLength.Long).Show();
                Finish();
            }
        }
    }
}