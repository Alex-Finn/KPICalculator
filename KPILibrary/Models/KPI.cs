using KPILibrary.Enums;
using System;

namespace KPILibrary.Models
{
	public class KPI
	{
		private int _plan;
		private float _weight;
		private string _name;
		private Func<int, float> _algorithm;
		private Logic _logic;

		// must be unique
		public string Name
		{
			get => _name;
			private set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException($"Name must be not empty and unique. Actual:{value}");
				}
				_name = value;
			}
		}
		public int? Plan
		{
			get => _plan;
			private set
			{
				if (value < 0)
				{
					Logger.Logger.Log($"Plan must be positive value. Actual:{value}. Nothing changed");
					return;
				}
				_plan = value.Value;
			}
		}
		public float? Weight
		{
			get => _weight;
			private set
			{
				if (value < 0)
				{
					Logger.Logger.Log($"Weight must be positive value. Actual:{value}. Nothing changed");
					return;
				}
				_weight = value.Value;
			}
		}
		public Func<int, float> Algorithm
		{
			get => _algorithm;
			private set
			{
				if (value is null)
				{
					Logger.Logger.Log($"Algorithm must be not null. Nothing changed");
					return;
				}
				_algorithm = value;
			}
		}
		public Logic Logic
		{
			get => _logic;
			private set
			{
				if (value == Logic.Unknown)
				{
					Logger.Logger.Log($"Logic must be not {Logic.Unknown}. Nothing changed");
					return;
				}
				_logic = value;
			}
		}

		public KPI(string name)
		{
			Name = name;
			Logger.Logger.Log($"{this} was successfully created at {DateTime.Now}");
		}

		public void SetAlgorithm(Func<int, float> algorithm)
		{
			if (IsNull()) return;
			Algorithm = algorithm;
		}

		public KPI SetPlan(int plan)
		{
			Plan = plan;
			return IsNull() ? NullKPI.GetNullKPIObject() : this;
		}

		public KPI SetWeight(float weight)
		{
			Weight = weight;
			return IsNull() ? NullKPI.GetNullKPIObject() : this;
		}

		public KPI SetLogic(Logic logic)
		{
			Logic = logic;
			return IsNull() ? NullKPI.GetNullKPIObject() : this;
		}

		private bool ValidateKPI() =>
			!IsNull()
			&& Plan != null && Weight != null
			&& Logic != Logic.Unknown && Algorithm != null;

		public float GetKPIResult(int fact)
		{
			if (!ValidateKPI())
				throw new InvalidOperationException($"{this} validating error. Check properties.");

			return Algorithm.Invoke(fact) * Weight.Value;
		}

		public override bool Equals(object obj)
		{
			return !(obj is not KPI other) &&
				Name == other.Name &&
				Plan == other.Plan &&
				Weight == other.Weight &&
				Logic == other.Logic;
		}

		public override int GetHashCode()
		{
			// if Name unique this will work
			return Name.GetHashCode();
		}

		public override string ToString()
		{
			return $"KPI with name {Name} of logic {Logic}";
		}

		public virtual bool IsNull() => false;
	}

	public sealed class NullKPI : KPI
	{
		private static NullKPI _instance;
		private NullKPI() : base("Null KPI") { }
		public static NullKPI GetNullKPIObject() => _instance ??= new NullKPI();
		public override string ToString() => "Null KPI";
		public override bool Equals(object obj) => obj is NullKPI;
		public override int GetHashCode() => Name.GetHashCode();
		public override bool IsNull() => true;
	}
}