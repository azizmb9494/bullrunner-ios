using System;
using System.Collections.Generic;

namespace BullRunner
{
	public class Stop
	{
		public string RouteName { get; set; }
		public int RouteId { get; set; }
		public string StopName { get; set; }
		public int StopId { get; set; }
		public int Region { get; set; }
		public float Distance { get; set; }
		public bool ArrivalsEnabled { get; set; }
	}
		
	public class Prediction
	{
		public int RouteId { get; set; }
		public string RouteName { get; set; }
		public int StopId { get; set; }
		public string BusName { get; set; }
		public int Minutes { get; set; }
		public string ArrivalTime { get; set; }
		public int Direction { get; set; }
		public bool SchedulePrediction { get; set; }
		public int VehicleId { get; set; }
		public bool IsLayover { get; set; }
		public float SecondsToArrival { get; set; }
		public bool OnBreak { get; set; }
	}

	public class Arrival
	{
		public string PredictionTime { get; set; }
		public List<Prediction> Predictions { get; set; }
	}
}

