using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Views.InputMethods;


namespace Android_Question_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            Button searchButton = FindViewById<Button>(Resource.Id.search_button);
            searchButton.Click += SearchButton_Click;
        }

        //async for better UX
        public async Task DownloadJsonFile(string url)
        {
            try
            {
                var httpClient = new HttpClient();

                Task<string> contentsTask = httpClient.GetStringAsync(url);

                string contents = await contentsTask;

                var subreddits = JsonConvert.DeserializeObject<JObject>(contents);

                var txtList = FindViewById<LinearLayout>(Resource.Id.subreddit__list);

                //clean all items first
                txtList.RemoveAllViewsInLayout();

                foreach (var subreddit in subreddits["data"]["children"] as JArray)
                {
                    var name = subreddit["data"]["display_name_prefixed"].ToString();

                    var newListItem = new TextView(this);
                    newListItem.Text = name;
                    newListItem.Click += NewListItem_Click;

                    txtList.AddView(newListItem);
                }

            }
            catch
            {
                Toast.MakeText(this, "searching failure", ToastLength.Short).Show();
            }

        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            var txt = FindViewById<TextInputEditText>(Resource.Id.textInput1).Text;
            var jsonUrl = "http://www.reddit.com/subreddits/search.json?q=" + txt;

            await DownloadJsonFile(jsonUrl);

        }


        private void NewListItem_Click(object sender, EventArgs e)
        {
            var listItem = (TextView)sender;
            var subredditName = listItem.Text;

            if (subredditName.Trim() != "")
            {
                var sidebarUrl = "http://www.reddit.com/" + subredditName + "/about/sidebar";

                var intent = new Intent(this, typeof(SidebarActivity));
                intent.PutExtra("sidebarUrl", sidebarUrl);
                StartActivity(intent);
            }


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
