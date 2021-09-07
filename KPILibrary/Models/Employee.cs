using KPILibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KPILibrary.Models
{
	public class Employee : IEmployee, IKPIApplied
	{
		private string _name;
		private int _id;

		// Unique
		// this things must be in db-side. there some simplicity
		public int Id
		{
			get => _id;
			private set
			{
				if (value < 0)
				{
					throw new ArgumentException($"Id must be positive and unique. Actual:{value}");
				}
				_id = value;
			}
		}
		public string Name
		{
			get => _name;
			private set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException($"Name must be not empty. Actual:{value}");
				}
				_name = value;
			}
		}
		public Dictionary<KPI, int> KPIs { get; private set; }

		public Employee(string name, int id)
		{
			Name = name;
			Id = id;
			KPIs = new Dictionary<KPI, int>();
			Logger.Logger.Log($"{this} was created at {DateTime.Now}");
		}

		public float GetFactOfComplete() => KPIs.Select(kpi => kpi.Key.GetKPIResult(kpi.Value)).Sum();
		public List<KPI> GetListOfKPI() => KPIs.Select(kpi => kpi.Key).ToList();
		private float GetWeightOfAllKPIs() => KPIs.Select(kpi => kpi.Key.Weight.Value).Sum();

		public bool ApplyKPI(KPI kpi)
		{
			var weight = GetWeightOfAllKPIs();

			if (weight < 1f)
			{
				if (kpi.Weight + weight <= 1f)
				{
					if (KPIs.TryAdd(kpi, 0))
					{
						Logger.Logger.Log($"{kpi} applied to {this}");
						return true;
					}
					Logger.Logger.Log($"Cannot apply {kpi} to {this}. KPI with same properties already applied.");
					return false;
				}
				Logger.Logger.Log($"Cannot apply {kpi} to {this}. New KPI weight too high.");
				return false;
			}
			Logger.Logger.Log($"Cannot apply {kpi} to {this}. Total weight of already applied is {weight}");
			return false;
		}

		public bool AddFactValueToKPI(KPI kpi, int fact)
		{
			if (!KPIs.ContainsKey(kpi))
			{
				Logger.Logger.Log($"Cannot add {fact} to {kpi}. This KPI not applied to {this}");
				return false;
			}

			KPIs[kpi] = fact;
			return true;
		}

		public override bool Equals(object obj)
		{
			return !(obj is not Employee other) &&
				Name == other.Name &&
				Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return $"Employee {Name} with ID {Id}";
		}
	}
}