using System;
using System.Collections.Generic;
using CoreLocation;
using UIKit;
using Foundation;

namespace BullRunner
{
	public partial class HomeViewCtrl : UITableViewController
	{
		private CLLocationManager _Manager = new CLLocationManager ();
		public List<Stop> Stops = new List<Stop>();
		public Dictionary<Stop, List<Prediction>> Predictions = new Dictionary<Stop, List<Prediction>>();

		public HomeViewCtrl (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			if (!CLLocationManager.LocationServicesEnabled) {
				new UIAlertView ("Location Services", "Location Services is currently disabled. Please enable it in your Settings -> Privacy -> Location Services.", null, "OK").Show ();
			} else if (UIDevice.CurrentDevice.CheckSystemVersion(8,0)) {
				_Manager.RequestWhenInUseAuthorization ();
				_Manager.RequestAlwaysAuthorization ();
				_Manager.StartUpdatingLocation ();
			} else {
				_Manager.StartUpdatingLocation ();
			}

			_Manager.DesiredAccuracy = 20f;
			_Manager.DistanceFilter = 100f;

			_Manager.LocationsUpdated += LocationsUpdated;
			this.RefreshControl.ValueChanged += (sender, e) => RefreshData (_Manager.Location);
			this.NavigationItem.RightBarButtonItem.Clicked += (sender, e) => RefreshData(_Manager.Location);

			NSTimer.CreateRepeatingScheduledTimer (60, delegate {
				Console.WriteLine("Updating");
				RefreshData(_Manager.Location);
			});
		}

		void LocationsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var l = _Manager.Location;
			RefreshData (l);
		}

		void RefreshData(CLLocation l)
		{
			if (l == null) {
				this.RefreshControl.EndRefreshing ();
				return;
			}
				
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			this.RefreshControl.BeginRefreshing ();

			InvokeInBackground (async delegate {
				try {
					Stops = await Provider.GetNearbyStopsAsync(l.Coordinate.Latitude, l.Coordinate.Longitude);
					Predictions = new Dictionary<Stop, List<Prediction>>();

					foreach (var s in Stops) {
						try {
							var predictions = await Provider.GetPredictionsAsync(s.RouteId, s.StopId);
							if (predictions.Count > 0) { 
								Predictions.Add(s, predictions);
								InvokeOnMainThread(delegate {
									this.TableView.Source = new StopsSource(this.Predictions);
									this.TableView.ReloadData();
								});
							}
						} catch (Exception ex) {
							Console.WriteLine(ex);
						}
					}
				} catch (Exception ex) {
					Console.WriteLine(ex);
				}

				InvokeOnMainThread(delegate {
					this.RefreshControl.EndRefreshing();
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				});
			});
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
