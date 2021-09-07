using KPILibrary.Enums;
using KPILibrary.Logger;
using KPILibrary.Models;
using System.Collections.Generic;

namespace Practice
{
	class Program
	{
		static void Main(string[] args)
		{
			var kpiCollection = new KPICollection();
			var employeeCollection = new List<Employee>()
			{
				new Employee("John", 1),
				new Employee("Mary", 2),
				new Employee("Bob", 3)
			};

			var binaryKPI1 = new KPI("binaryKPI1")
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			binaryKPI1.SetAlgorithm(
				fact => fact < binaryKPI1.Plan ? 0f : 1f);

			var ladderKPI1 = new KPI("ladderKPI1")
				.SetLogic(Logic.Ladder)
				.SetPlan(200)
				.SetWeight(0.8f);
			ladderKPI1.SetAlgorithm(
				fact =>
				{
					if (fact < ladderKPI1.Plan * 0.8) return 0f;
					if (fact >= ladderKPI1.Plan * 0.8 && fact < ladderKPI1.Plan) return 0.8f;
					return 1f;					
				});

			if (kpiCollection.AddRange(binaryKPI1, ladderKPI1, binaryKPI1))
				Logger.Log($"{binaryKPI1} and {ladderKPI1} added to collection");
			else
				Logger.Log($"Not all KPIs was added to collection. Check logs");

			var john = employeeCollection[0];
			var mary = employeeCollection[1];

			john.ApplyKPI(binaryKPI1);
			john.ApplyKPI(ladderKPI1);
			john.ApplyKPI(ladderKPI1);
			mary.ApplyKPI(binaryKPI1);
			mary.ApplyKPI(binaryKPI1);

			john.AddFactValueToKPI(binaryKPI1, 95);
			john.AddFactValueToKPI(ladderKPI1, 210);

			System.Console.WriteLine($"Fact of KPI complete of {john}: {john.GetFactOfComplete()}");
		}
	}
}
