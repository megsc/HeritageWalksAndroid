using Android.App;
using Android.Views;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Content;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using HeritageWalks.Fragments;

namespace HeritageWalks.Activities
{
    [Activity(Label = "Heritage Walks", Theme = "@style/Theme.HeritageWalks")]
    public class DetailStopActivity : AppCompatActivity
    {
        private DrawerLayout _drawerLayout;
        private string trailId;
        private string stopId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            trailId = Intent.GetStringExtra("Trail ID");
            stopId = Intent.GetStringExtra("Stop ID");

            SetContentView(Resource.Layout.DetailStop);

            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);


            SupportActionBar ab = SupportActionBar;
            ab.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_small);
            ab.SetDisplayHomeAsUpEnabled(true);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }

            TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            SetUpViewPager(viewPager);

            tabs.SetupWithViewPager(viewPager);
        }

        public void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);
            adapter.AddFragment(new DetailStopFragment(), "Details");

            Bundle args = new Bundle();
            args.PutString("Trail ID", trailId);
            args.PutString("Stop ID", stopId);
            var stopFragment = adapter.GetItem(0);
            stopFragment.Arguments = args;

            viewPager.Adapter = adapter;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _drawerLayout.OpenDrawer((int)GravityFlags.Left);
                    return true;

                case Resource.Id.nav_home:
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    return true;

                case Resource.Id.nav_help:
                    var intent2 = new Intent(this, typeof(HelpActivity));
                    StartActivity(intent2);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public void SetUpDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                OnOptionsItemSelected(e.MenuItem);
                //e.MenuItem.SetChecked(true);
                _drawerLayout.CloseDrawers();
            };

        }

        public class TabAdapter : FragmentPagerAdapter
        {
            public List<SupportFragment> Fragments { get; set; }
            public List<string> FragmentNames { get; set; }

            public TabAdapter(SupportFragmentManager sfm) : base(sfm)
            {
                Fragments = new List<SupportFragment>();
                FragmentNames = new List<string>();
            }

            public void AddFragment(SupportFragment fragment, string name)
            {
                Fragments.Add(fragment);
                FragmentNames.Add(name);
            }

            public override int Count
            {
                get
                {
                    return Fragments.Count;
                }
            }

            public override SupportFragment GetItem(int position)
            {
                return Fragments[position];
            }

            public override ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(FragmentNames[position]);
            }
        }
    }
}
    
