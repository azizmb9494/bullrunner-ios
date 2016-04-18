using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;

namespace BullRunner
{
	public class StopsSource : UITableViewSource
	{
		public Stop[] Stops;

		public int[] Keys;
		public Dictionary<int, Dictionary<int, List<Prediction>>> Predictions;

		public StopsSource (List<Stop> stops, Dictionary<int, Dictionary<int, List<Prediction>>> preds)
		{
			
			Stops = stops.OrderBy (x => x.StopId).ToArray ();
			Keys = preds.Keys.ToArray();
			Predictions = preds;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return this.Keys.Length;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Predictions [Keys [(int)section]].Keys.Count ();
		}

		public override string TitleForFooter (UITableView tableView, nint section)
		{
			return Stops.First (x => x.StopId == Keys [(int)section]).StopName;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("StopCell");
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Value2, "StopCell");
			}

			cell.TextLabel.Text = "Hello";
			return cell;
		}
	}
}

