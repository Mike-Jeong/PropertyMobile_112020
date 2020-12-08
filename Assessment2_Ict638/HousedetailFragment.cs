﻿using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Assessment2_Ict638
{/// <summary>
/// //
/// </summary>
    public class HousedetailFragment : Fragment, IOnMapReadyCallback
    {
        GoogleMap gMap;
        LatLng curLocation;
        int photonum;
        int id;
        string h;
        string nr;
        string nt;
        string r;
        string l;
        string an;
        string d;


        public HousedetailFragment(string heading, string numberofroom, string numberoftoilet, string rentfee, string location, string agencyname, string description) 
        {
            h = heading; nr = numberofroom; nt = numberoftoilet; r = rentfee; l = location; an = agencyname; d = description;

        }


        public void getph(int n) 
        {
            photonum = n;
           
            switch (photonum)
            {
                case 1:
                    id = Resource.Drawable.P1;

                    break;

                case 2:
                    id = Resource.Drawable.P2;

                    break;

                case 3:
                    id = Resource.Drawable.P3;

                    break;

                case 4:
                    id = Resource.Drawable.P4;

                    break;

                case 5:
                    id = Resource.Drawable.P5;

                    break;

                case 6:
                    id = Resource.Drawable.P6;

                    break;

                case 7:
                    id = Resource.Drawable.P7;

                    break;

                case 8:
                    id = Resource.Drawable.P8;

                    break;

                case 9:
                    id = Resource.Drawable.P9;

                    break;

                case 10:
                    id = Resource.Drawable.P10;

                    break;

                default:
                    break;
            }

        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

          
        


            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View v = inflater.Inflate(Resource.Layout.fragment_housedetail, container, false);

            TextView Heading = v.FindViewById<EditText>(Resource.Id.HtvHeading);
            TextView Numberofroom = v.FindViewById<EditText>(Resource.Id.HtvNumberofroom);
            TextView Numberoftoilet = v.FindViewById<EditText>(Resource.Id.HtvNumberoftoilet);
            TextView Rentfee = v.FindViewById<EditText>(Resource.Id.HtvRentfee);
            TextView nAgency = v.FindViewById<EditText>(Resource.Id.HtvAgency);
            TextView ALocation = v.FindViewById<EditText>(Resource.Id.HtvLocation);
            TextView Description = v.FindViewById<EditText>(Resource.Id.HtvDescription);

            Heading.Text = h;
            Numberofroom.Text = nr;
            Numberoftoilet.Text = nt;
            Rentfee.Text = r;
            nAgency.Text = an;
            ALocation.Text = l;
            Description.Text = d;



            ImageView imgMapFrag = v.FindViewById<ImageView>(Resource.Id.imgHMapFrag);
            imgMapFrag.SetImageResource(id);

            var mapFrag = MapFragment.NewInstance();
            ChildFragmentManager.BeginTransaction()
                .Add(Resource.Id.mapFrgContainer, mapFrag, "map")
                .Commit();

            mapFrag.GetMapAsync(this);

           
            Button btnShare = v.FindViewById<Button>(Resource.Id.btnShare);
            Button btnSMS = v.FindViewById<Button>(Resource.Id.btnSMS);

            btnShare.Click += BtnShare_Click;
            btnSMS.Click += BtnSMS_Click;


            return v;
        }

        private async void BtnSMS_Click(object sender, EventArgs e)
        {
            //To use this, delete async
            //throw new NotImplementedException();

            try
            {
                string text = " ";
                string recipient = "0211231234";
                var message = new SmsMessage(text, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (Exception exp)
            {
                Toast.MakeText(Activity, "Exception Found", ToastLength.Long).Show();
            }
        }
        private async void BtnShare_Click(object sender, EventArgs e)
        {
            string locDetails = "";
            locDetails += "Latitude: " + curLocation.Latitude + "\nLongtitude" + curLocation.Longitude;
            await ShareText(locDetails);
        }

        public async Task ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Location Share"
            }
            );
        }


        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.MapType = GoogleMap.MapTypeNormal;
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;

            LatLng loc = new LatLng(-36.84966, 174.76526);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(loc);
            builder.Zoom(20);
            builder.Tilt(65);

            CameraPosition cPos = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cPos);
            googleMap.MoveCamera(cameraUpdate);

            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(loc);
            markerOptions.SetTitle("NZSE City Campus");

            googleMap.AddMarker(markerOptions);
        }

        public async void getLastLocation(GoogleMap googleMap)
        {
            Console.WriteLine("Test - LastLoc");
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Console.WriteLine($"Last Loc - Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    MarkerOptions curLoc = new MarkerOptions();
                    curLoc.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    var address = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = address?.FirstOrDefault();
                    var geocodeAddress = "";
                    if (placemark != null)
                    {
                        geocodeAddress =
                        $"AdminArea: {placemark.AdminArea}\n" +
                        $"CountryCode: {placemark.CountryCode}\n" +
                        $"CountryName: {placemark.CountryName}\n" +
                        $"FeatureName: {placemark.FeatureName}\n" +
                        $"Locality: {placemark.Locality}\n" +
                        $"PostalCode: {placemark.PostalCode}\n" +
                        $"SubAdminArea: {placemark.SubAdminArea}\n" +
                        $"SubLocality: {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare: {placemark.Thoroughfare}\n";

                    }
                    curLoc.SetTitle("You were here" + geocodeAddress);
                    curLoc.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure));
                    googleMap.AddMarker(curLoc);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Toast.MakeText(Activity, "Feature Not Supported", ToastLength.Short);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Toast.MakeText(Activity, "Feature Not Enabled", ToastLength.Short);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Toast.MakeText(Activity, "Needs more permission", ToastLength.Short);
            }
            catch (Exception ex)
            {
                // Unable to get location
                Toast.MakeText(Activity, "Unable to get location", ToastLength.Short);
            }
        }
    
        public async void getCurrentLoc(GoogleMap googleMap)
        {
            Console.WriteLine("Test - CurrentLoc");
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"current Loc - Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    MarkerOptions curLoc = new MarkerOptions();
                    curLoc.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    var address = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = address?.FirstOrDefault();
                    var geocodeAddress = "";
                    if (placemark != null)
                    {
                        geocodeAddress =
                        $"AdminArea: {placemark.AdminArea}\n" +
                        $"CountryCode: {placemark.CountryCode}\n" +
                        $"CountryName: {placemark.CountryName}\n" +
                        $"FeatureName: {placemark.FeatureName}\n" +
                        $"Locality: {placemark.Locality}\n" +
                        $"PostalCode: {placemark.PostalCode}\n" +
                        $"SubAdminArea: {placemark.SubAdminArea}\n" +
                        $"SubLocality: {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare: {placemark.Thoroughfare}\n";

                    }
                    curLoc.SetTitle("You are here" + geocodeAddress);
                    curLoc.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure));

                    googleMap.AddMarker(curLoc);

                }
                else
                {
                    getLastLocation(googleMap);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Toast.MakeText(Activity, "Feature Not Supported", ToastLength.Short);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Toast.MakeText(Activity, "Feature Not Enabled", ToastLength.Short);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Toast.MakeText(Activity, "Needs more permission", ToastLength.Short);
            }
            catch (Exception ex)
            {
                getLastLocation(googleMap);
            }
        }


    }
}