using System;
using System.Linq;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace BullRunner
{
	public class StopsSource : UITableViewSource
	{
		public int[] Indexes;
		public Dictionary<int, Stop> Stops;
		public Dictionary<int, string> Routes;
		public Dictionary<int, Dictionary<int, List<Prediction>>> Map;

		public StopsSource (Dictionary<Stop, List<Prediction>> map)
		{
			Stops = new Dictionary<int, Stop> (map.Keys.Count);
			Map = new Dictionary<int, Dictionary<int, List<Prediction>>> (map.Keys.Count);
			Routes = new Dictionary<int, string> (map.Keys.Count * 3);

			foreach (var s in map.Keys) {
				if (!Stops.ContainsKey (s.StopId)) {
					Stops.Add (s.StopId, s);
				}

				if (!Routes.ContainsKey (s.RouteId)) {
					Routes.Add (s.RouteId, s.RouteName);
				}
					
				if (Map.ContainsKey (s.StopId)) {
					if (Map [s.StopId].ContainsKey(s.RouteId)) {
						Map [s.StopId] [s.RouteId].AddRange (map [s]);
					} else {
						Map [s.StopId].Add (s.RouteId, map [s]);
					}
				} else {
					var d = new Dictionary<int, List<Prediction>> (map [s].Count);
					d.Add (s.RouteId, map [s]);
					Map.Add (s.StopId, d);
				}
			}

			Indexes = Stops.Keys.ToArray ();
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return Indexes.Length;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Map [Indexes [(int)section]].Keys.Count;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return Stops [Indexes [(int)section]].StopName;
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			return 42f;
		}

		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			UILabel stopLbl = new UILabel (new CGRect (15, 0, tableView.Frame.Width-115, 42));
			stopLbl.BackgroundColor = UIColor.Clear;
			stopLbl.Text = Stops [Indexes [(int)section]].StopName;
			stopLbl.Font = UIFont.BoldSystemFontOfSize (18f);

			UILabel distanceLbl = new UILabel (new CGRect (tableView.Frame.Width-110, 0, 100, 42));
			distanceLbl.BackgroundColor = UIColor.Clear;
			distanceLbl.TextAlignment = UITextAlignment.Right;
			distanceLbl.Font = UIFont.SystemFontOfSize (18f);
			distanceLbl.Text = Stops [Indexes [(int)section]].Distance.ToString ("0.00") + " mi";



			UIView view = new UIView (new CGRect (0, 0, tableView.Frame.Width, 42));
			view.Add (stopLbl);
			view.Add (distanceLbl);
			view.BackgroundColor = UIColor.FromRGB (247, 247, 247);
			return view;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("StopCell");
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Value2, "StopCell");
			}


			var m = Map [Indexes [indexPath.Section]];
			var route = Routes [m.Keys.ToArray () [indexPath.Row]];
			cell.TextLabel.Text = route;
			return cell;
		}
	}
}

