using KPILibrary.Enums;
using KPILibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KPITest.ModelsTests
{
	[TestClass]
	public class KPIModelTests
	{
		private const string ValidName1 = "Unique name1";
		private const int ValidPlan = 25;
		private const float ValidWeight = 0.5f;
		private const int InvalidPlan = -1;
		private const float InvalidWeight = -0.5f;

		[TestMethod]
		public void KPI_Name_Must_Be_Not_Null()
		{
			Assert.ThrowsException<ArgumentException>(() => new KPI(null),
				$"KPI's name must be not null");
		}

		[TestMethod]
		public void KPI_Name_Must_Be_Not_Empty()
		{
			Assert.ThrowsException<ArgumentException>(() => new KPI(""),
				$"KPI's name must be not empty");
		}

		[TestMethod]
		public void KPI_Name_Must_Be_Not_Only_Whitespace()
		{
			Assert.ThrowsException<ArgumentException>(() => new KPI(" "),
				$"KPI's name must contain not only whitespace");
		}

		[TestMethod]
		public void KPI_Name_Must_Be_Correct()
		{
			Assert.AreEqual(ValidName1, new KPI(ValidName1).Name,
				$"KPI should be created with correct name");
		}

		[TestMethod]
		public void KPI_Plan_Must_Be_Not_Lesser_Than_Zero()
		{
			var kpi = new KPI(ValidName1).SetPlan(ValidPlan);
			Assert.AreEqual(ValidPlan, kpi.Plan, $"KPI's plan is not correct");
			kpi.SetPlan(InvalidPlan);
			Assert.AreNotEqual(InvalidPlan, kpi.Plan, $"KPI's plan was set to negative value");
			Assert.AreEqual(ValidPlan, kpi.Plan, $"KPI's plan was changed after set to incorrect value");
		}

		[TestMethod]
		public void KPI_Weight_Must_Be_Not_Lesser_Than_Zero()
		{
			var kpi = new KPI(ValidName1).SetWeight(ValidWeight);
			Assert.AreEqual(ValidWeight, kpi.Weight, $"KPI's weight is not correct");
			kpi.SetWeight(InvalidWeight);
			Assert.AreNotEqual(InvalidWeight, kpi.Weight, $"KPI's weight was set to negative value");
			Assert.AreEqual(ValidWeight, kpi.Weight, $"KPI's weight was changed after set to incorrect value");
		}

		[TestMethod]
		public void KPI_Logic_Must_Be_Correct()
		{
			var kpi = new KPI(ValidName1);
			const Logic unknown = Logic.Unknown;
			const Logic binary = Logic.Binary;
			const Logic ladder = Logic.Ladder;
			Assert.AreNotEqual(ladder, kpi.Logic, $"KPI's logic initialize fail");
			Assert.AreNotEqual(binary, kpi.Logic, $"KPI's logic initialize fail");
			Assert.AreEqual(ladder, kpi.SetLogic(ladder).Logic, $"KPI's logic must be correct");
			Assert.AreNotEqual(unknown, kpi.SetLogic(unknown).Logic, $"KPI's logic was set to {unknown}");
			Assert.AreEqual(ladder, kpi.SetLogic(ladder).Logic, $"KPI's logic was changed after set to incorrect value");
			Assert.AreEqual(binary, kpi.SetLogic(binary).Logic, $"KPI's logic not changed after set to correct value");
		}

		[TestMethod]
		public void KPI_Algorithm_Must_Be_Correct()
		{
			var kpi = new KPI(ValidName1);
			Func<int, float> algorithm = (fact) => 1f;

			Assert.IsNull(kpi.Algorithm, $"KPI's algorithm must be null after initialization");
			kpi.SetAlgorithm(algorithm);
			Assert.IsNotNull(kpi.Algorithm, "KPI's algorithm still null after set correct value");
			kpi.SetAlgorithm(null);
			Assert.IsNotNull(kpi.Algorithm, "KPI's algorithm was changed to null");
			Assert.AreEqual(algorithm, kpi.Algorithm, "KPI's algorithm was changed after set incorrect value");
		}
	}
}