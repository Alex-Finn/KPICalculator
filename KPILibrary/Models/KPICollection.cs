using System.Collections.Generic;
using System.Linq;

namespace KPILibrary.Models
{
	public class KPICollection
	{
		private List<KPI> _kpiCollection;

		public KPICollection()
		{
			_kpiCollection = new List<KPI>();
		}

		public KPI GetKPI(string name)
		{
			return _kpiCollection.FirstOrDefault(k => k.Name == name) 
				?? NullKPI.GetNullKPIObject();
		}

		public bool IsKPIInCollection(KPI kpi)
		{
			return _kpiCollection.Contains(kpi);
		}

		public bool IsKPIInCollection(string name)
		{
			return _kpiCollection.Select(k => k.Name).Contains(name);
		}

		public bool Add(KPI kpi)
		{
			if (IsKPIInCollection(kpi))
			{
				Logger.Logger.Log($"{kpi} already exist in database");
				return false;
			}

			_kpiCollection.Add(kpi);
			return true;
		}

		public bool AddRange(params KPI[] kpis)
		{
			return kpis.ToList().Count(k => Add(k) == false) == 0;
		}
	}
}