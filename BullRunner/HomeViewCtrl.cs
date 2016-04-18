using System;
using System.Collections.Generic;
using CoreLocation;
using UIKit;

namespace BullRunner
{
	public partial class HomeViewCtrl : UITableViewController
	{
		private CLLocationManager _Manager = new CLLocationManager ();
		public List<Stop> Stops = new List<Stop>();
		public Dictionary<int, Dictionary<int, List<Prediction>>> Predictions;

		public HomeViewCtrl (IntPtr handle) : base (handle)
		{
		}

		async public override void ViewDidLoad ()
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
		}

		void LocationsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var l = e.Locations [e.Locations.Length - 1];

			InvokeInBackground (async delegate {
				try {
					Stops = await Provider.GetNearbyStopsAsync(l.Coordinate.Latitude, l.Coordinate.Longitude);
					Predictions = new Dictionary<int, Dictionary<int, List<Prediction>>>();
					foreach (var s in Stops) {
						try {
							var predictions = await Provider.GetPredictionsAsync(s.RouteId, s.StopId);
							if (predictions.Count > 0) {
								if (Predictions.ContainsKey(s.StopId)) {
									Predictions[s.StopId].Add(s.RouteId, predictions);
								} else {
									var d = new Dictionary<int, List<Prediction>>();
									d.Add(s.RouteId, predictions);
									Predictions.Add(s.StopId, d);
								}
							}
						} catch (Exception ex) {
							Console.WriteLine(ex);
						}
					}
				} catch (Exception ex) {
					Console.WriteLine(ex);
				}
			});
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
