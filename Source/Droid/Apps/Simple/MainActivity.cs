using System;
using Android.App;
using Android.Widget;
using Android.OS;

using ManagedIrbis;

namespace Simple
{
    [Activity(Label = "Simple", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        //int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {


            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //// Get our button from the layout resource,
            //// and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.myButton);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            TextView textView = FindViewById<TextView>(Resource.Id.textView1);

            try
            {
                using (IrbisConnection connection = new IrbisConnection())
                {
                    connection.ParseConnectionString(
                        "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;");
                    connection.Connect();
                    int maxMfn = connection.GetMaxMfn();
                    textView.Text = string.Format
                        (
                            "MAX MFN={0}",
                            maxMfn
                        );
                }
            }
            catch (Exception exception)
            {
                textView.Text = exception.ToString();
            }
        }
    }
}

