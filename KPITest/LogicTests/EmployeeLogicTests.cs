using KPILibrary.Enums;
using KPILibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KPITest.LogicTests
{
	[TestClass]
	public class EmployeeLogicTests
	{
		private const string ValidEmployeeName1 = "Employee 1";
		private const string ValidKPIName1 = "KPI 1";
		private const string ValidKPIName2 = "KPI 2";
		private const string ValidKPIName3 = "KPI 3";
		private const string ValidKPIName4 = "KPI 4";
		private const int ValidId1 = 123;
		private const int ValidFactValue1 = 95;
		private const int ValidFactValue2 = 210;

		[TestMethod]
		public void Employee_Check_KPI_Count()
		{
			var emp = new Employee(ValidEmployeeName1, ValidId1);
			Assert.AreEqual(0, emp.GetListOfKPI().Count, $"{emp} was created with not empty list of KPI");

			var kpi1 = new KPI(ValidKPIName1)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			kpi1.SetAlgorithm(fact => fact < kpi1.Plan ? 0f : 1f);
			var kpi2 = new KPI(ValidKPIName2)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			kpi2.SetAlgorithm(fact => fact < kpi2.Plan ? 0f : 1f);

			Assert.IsTrue(emp.ApplyKPI(kpi1), $"{kpi1} was not applied to {emp}");
			Assert.AreEqual(1, emp.GetListOfKPI().Count, $"Count of KPI of {emp} was not increased after add new one");
			Assert.IsTrue(emp.ApplyKPI(kpi2), $"{kpi2} was not applied to {emp}");
			Assert.AreEqual(2, emp.GetListOfKPI().Count, $"Count of KPI of {emp} was not increased after add another one");
		}

		[TestMethod]
		public void Employee_Cannot_Apply_Same_KPI()
		{
			var emp = new Employee(ValidEmployeeName1, ValidId1);
			var kpi1 = new KPI(ValidKPIName1)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			kpi1.SetAlgorithm(fact => fact < kpi1.Plan ? 0f : 1f);
			var kpi2 = new KPI(ValidKPIName1)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			kpi2.SetAlgorithm(fact => fact < kpi2.Plan ? 0f : 1f);

			Assert.IsTrue(emp.ApplyKPI(kpi1), $"{kpi1} was not applied to {emp}");
			Assert.IsFalse(emp.ApplyKPI(kpi2), $"{kpi2} was applied to {emp} but same KPI (KPI name) already exists");
		}

		[TestMethod]
		public void Employee_Cannot_Apply_KPIs_With_Weight_More_Than_1()
		{
			var emp = new Employee(ValidEmployeeName1, ValidId1);
			var kpi1 = new KPI(ValidKPIName1)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			kpi1.SetAlgorithm(fact => fact < kpi1.Plan ? 0f : 1f);
			var kpi2 = new KPI(ValidKPIName2)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.3f);
			kpi2.SetAlgorithm(fact => fact < kpi2.Plan ? 0f : 1f);
			var kpi3 = new KPI(ValidKPIName3)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.5f);
			kpi3.SetAlgorithm(fact => fact < kpi3.Plan ? 0f : 1f);
			var kpi4 = new KPI(ValidKPIName4)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.1f);
			kpi4.SetAlgorithm(fact => fact < kpi4.Plan ? 0f : 1f);

			Assert.IsTrue(emp.ApplyKPI(kpi1), $"{kpi1} was not applied to {emp}. Total weight is {kpi1.Weight}");
			Assert.IsTrue(emp.ApplyKPI(kpi2), $"{kpi2} was not applied to {emp}. Total weight is {kpi1.Weight + kpi2.Weight}");
			Assert.IsTrue(emp.ApplyKPI(kpi3), $"{kpi3} was not applied to {emp}. Total weight is {kpi1.Weight + kpi2.Weight + kpi3.Weight}");
			Assert.IsFalse(emp.ApplyKPI(kpi4), $"{kpi4} was applied to {emp}. But total weight is {kpi1.Weight + kpi2.Weight + kpi3.Weight + kpi4.Weight}");
		}

		[TestMethod]
		public void Employee_Cannot_Add_Fact_Value_To_Not_Applied_KPI()
		{
			var emp = new Employee(ValidEmployeeName1, ValidId1);
			var kpi1 = new KPI(ValidKPIName1)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			kpi1.SetAlgorithm(fact => fact < kpi1.Plan ? 0f : 1f);
			var kpi2 = new KPI(ValidKPIName2)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.3f);
			kpi2.SetAlgorithm(fact => fact < kpi2.Plan ? 0f : 1f);

			Assert.IsTrue(emp.ApplyKPI(kpi1), $"{kpi1} was not applied to {emp}");

			Assert.IsTrue(emp.AddFactValueToKPI(kpi1, ValidFactValue1), $"Failed to add fact value to {kpi1} that applied to {emp}");
			Assert.IsFalse(emp.AddFactValueToKPI(kpi2, ValidFactValue2), $"Fact value was added to {kpi2}, but this not applied to {emp}");

			Assert.IsTrue(emp.ApplyKPI(kpi2), $"{kpi2} was not applied to {emp}");
			Assert.IsTrue(emp.AddFactValueToKPI(kpi2, ValidFactValue2), $"Failed to add fact value to {kpi2}, but this already applied");
		}

		[TestMethod]
		public void Employee_GetFactOfComplete_Works_Fine()
		{
			const float ZeroKPIResult = 0f;
			const float CorrectKPIResult = 0.8f;
			var emp = new Employee(ValidEmployeeName1, ValidId1);
			var kpi1 = new KPI(ValidKPIName1)
				.SetLogic(Logic.Binary)
				.SetPlan(100)
				.SetWeight(0.2f);
			kpi1.SetAlgorithm(fact => fact < kpi1.Plan ? 0f : 1f);
			var kpi2 = new KPI(ValidKPIName2)
				.SetLogic(Logic.Ladder)
				.SetPlan(200)
				.SetWeight(0.8f);
			kpi2.SetAlgorithm(
				fact =>
				{
					if (fact < kpi2.Plan * 0.8) return 0f;
					if (fact >= kpi2.Plan * 0.8 && fact < kpi2.Plan) return 0.8f;
					return 1f;
				});

			Assert.IsTrue(emp.ApplyKPI(kpi1), $"{kpi1} was not applied to {emp}");
			Assert.IsTrue(emp.ApplyKPI(kpi2), $"{kpi2} was not applied to {emp}");

			Assert.AreEqual(ZeroKPIResult, emp.GetFactOfComplete(), $"Fact of KPI complete not zero without fact values.");

			Assert.IsTrue(emp.AddFactValueToKPI(kpi1, ValidFactValue1), $"Failed to add fact value to {kpi1} that applied to {emp}");
			Assert.IsTrue(emp.AddFactValueToKPI(kpi2, ValidFactValue2), $"Failed to add fact value to {kpi2} that applied to {emp}");

			Assert.AreEqual(CorrectKPIResult, emp.GetFactOfComplete(), $"Fact of KPI complete of {emp} not as expected");
		}
	}
}