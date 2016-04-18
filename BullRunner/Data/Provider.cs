using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BullRunner
{
	public static class Provider
	{
		private static HttpClient _Client { get; set; }
		private const string BASE_URL = "http://usfbullrunner.com/";
		private const int TIMEOUT = 5;

		static Provider()
		{
			_Client = new HttpClient () { BaseAddress = new Uri (BASE_URL), Timeout = TimeSpan.FromSeconds (TIMEOUT) };
		}

		/// <summary>
		/// Gets the nearest stops.
		/// </summary>
		/// <returns>List of Stops</returns>
		/// <param name="lat">Latitude.</param>
		/// <param name="lng">Longitude.</param>
		async public static Task<List<Stop>> GetNearbyStopsAsync(double lat, double lng)
		{
			try {
				var resp = await _Client.GetAsync(String.Format("api/nearbystops/{0}/{1}?format=json", lat, lng));
				return JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), typeof(List<Stop>)) as List<Stop>;
			} catch (Exception e) {
				Console.WriteLine (e);
				throw e;
			}
		}

		/// <summary>
		/// Gets arrivals of route & stop.
		/// </summary>
		/// <returns>Arrival w/ Predictions</returns>
		/// <param name="routeId">Route identifier.</param>
		/// <param name="stopId">Stop identifier.</param>
		async public static Task<List<Prediction>> GetPredictionsAsync(int routeId, int stopId)
		{
			try {
				var resp = await _Client.GetAsync(String.Format("Route/{0}/Stop/{1}/Arrivals", routeId, stopId));
				var a = JsonConvert.DeserializeObject (await resp.Content.ReadAsStringAsync (), typeof(Arrival)) as Arrival;
				return a.Predictions;
			} catch (Exception e) {
				Console.WriteLine (e);
				throw e;
			}
		}
	}
}

