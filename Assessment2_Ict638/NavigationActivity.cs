﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assessment2_Ict638
{
    class Agency
    {
        public string agencyname { get; set; }
        public string agencyemail { get; set; }
        public string agencyphonenumber { get; set; }
        public string agencylocation { get; set; }
     
    }




    [Activity(Label = "NavigationActivity")]
    public class NavigationActivity : Activity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        [System.Obsolete]

        List<Data> hList = new List<Data>();

        string heading;
        string numberofroom;
        string numberoftoilet;
        string rentfee;
        string location;
        string agencyname;
        string description;


        public bool OnNavigationItemSelected(IMenuItem item)
        {
            FrameLayout navFragContainer = FindViewById<FrameLayout>(Resource.Id.navFragContainer);
            FragmentTransaction transaction;
            Bundle data = Intent.GetBundleExtra("data");

            switch (item.ItemId)
            {
                case Resource.Id.menu1:

                    navFragContainer.RemoveAllViewsInLayout();
                    HousedetailFragment sFrag = new HousedetailFragment(heading, numberofroom, numberoftoilet, rentfee, location, agencyname, description);
                    
                    transaction = FragmentManager.BeginTransaction();
                    transaction.Replace(Resource.Id.navFragContainer, sFrag);
                    transaction.Commit();

                    return true;

                case Resource.Id.menu2:
                    
                    bool staus = false;
                    string url = "https://10.0.2.2:5001/api/Users";
                    string response = APIConnect.Get(url);
                    List<Agency> agencies = JsonConvert.DeserializeObject<List<Agency>>(response);


                    foreach (Agency agency in agencies)
                    {
                        if (agency.agencyname == data.GetString("agencyname"))
                        {
                            navFragContainer.RemoveAllViewsInLayout();
                            AgencydetailFragment aFrag = new AgencydetailFragment(agency.agencyname, agency.agencyphonenumber, agency.agencyemail, agency.agencylocation);

                            transaction = FragmentManager.BeginTransaction();
                            transaction.Replace(Resource.Id.navFragContainer, aFrag);
                            transaction.Commit();

                            break;
                        }
                    }
                    



                    return true;
               
            }
            return false;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_navigation);

            Bundle data = Intent.GetBundleExtra("data");

            BottomNavigationView navigationView = FindViewById<BottomNavigationView>(Resource.Id.TopNavBar);
            navigationView.SetOnNavigationItemSelectedListener(this);

            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            // sFrag. PutExtra("data", data);
            heading = "House name : " + data.GetString("heading");
            numberofroom = data.GetString("numberofroom");
            numberoftoilet = data.GetString("numberoftoilet");
            rentfee = data.GetString("rentfee");
            location = data.GetString("location");
            agencyname = "Agency name : " + data.GetString("agencyname");
            description = "Description " + data.GetString("description");

            HousedetailFragment sFrag = new HousedetailFragment(heading, numberofroom, numberoftoilet, rentfee, location, agencyname, description);
            sFrag.getph(data.GetInt("photoid"));
          
            

            

            navigationView.SelectedItemId = Resource.Id.menu1;
            
    

        }
        

    }
}