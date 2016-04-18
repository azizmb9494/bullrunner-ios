using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreLocation;
using CoreImage;
using System.Runtime;

namespace BullRunner
{
	public class StopCell : UITableViewCell  {

		UILabel NameLbl;
		UILabel[] PredsLbl;

		public string Route;
		public List<Prediction> Predictions = new List<Prediction> ();

		public StopCell (string routeName, List<Prediction> preds) : base (UITableViewCellStyle.Default, "stopCell")
		{
			NameLbl = new UILabel ();

			PredsLbl = new UILabel[2];
			for (int i = 0; i < PredsLbl.Length; i++) {
				PredsLbl [i] = new UILabel () { TextAlignment = UITextAlignment.Center, TextColor = UIColor.White };
			}
				
			ContentView.AddSubview (NameLbl);
			ContentView.AddSubviews (PredsLbl);
			this.UpdateCell (routeName, preds);
		}

		public void UpdateCell (string route, List<Prediction> preds)
		{
			Route = route;
			Predictions = preds;

			this.NameLbl.Text = route;
			for (int i = 0; i < PredsLbl.Length; i++) {
				PredsLbl [i].Text = "N/A";
				PredsLbl [i].Layer.MasksToBounds = true;
				PredsLbl [i].Layer.CornerRadius = 10f;
				PredsLbl [i].BackgroundColor = UIColor.LightGray;
			}

			for (int i = 0; i < Math.Min (preds.Count, PredsLbl.Length); i++) {
				PredsLbl [i].Text = Predictions[i].Minutes == 0 ? "NOW" : Predictions [i].Minutes + "m";
				PredsLbl [i].BackgroundColor = AppDelegate.PrimaryColor;
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			NameLbl.Frame = new CGRect (10, 5, 150, 56);
			for (int i = 0; i < PredsLbl.Length; i++) {
				PredsLbl [i].Frame = new CGRect (200 + (i * 80), 13, 75, 40); 
			}
		}
	}
}

