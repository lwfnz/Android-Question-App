using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace Android_Question_App
{
    [Activity(Label = "SidebarActivity")]
    public class SidebarActivity : Activity
    {


        //async for better UX
        public async Task loadHtml(string url)
        {
            try
            {
                var httpClient = new HttpClient();

                Task<string> contentsTask = httpClient.GetStringAsync(url);

                string contents = await contentsTask;

                var webView = new WebView(this);
                AddContentView(webView, new ViewGroup.LayoutParams(800, (int)DeviceDisplay.MainDisplayInfo.Height));
                webView.LoadData(contents, "text/html", "utf-8");

            }
            catch
            {
                Toast.MakeText(this, "fetching web failure", ToastLength.Short).Show();
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var sidebarUrl = Intent.Extras.GetString("sidebarUrl");

            await loadHtml(sidebarUrl);

        }
    }
}