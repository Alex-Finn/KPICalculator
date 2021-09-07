using KPILibrary.Models;
using System.Collections.Generic;

namespace KPILibrary.Interfaces
{
	internal interface IKPIApplied
	{
		public Dictionary<KPI, int> KPIs { get; }
		public float GetFactOfComplete();
		public List<KPI> GetListOfKPI();
		public bool AddFactValueToKPI(KPI kpi, int fact);
	}
}