using KPILibrary.Enums;
using KPILibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KPITest.LogicTests
{
	[TestClass]
	public class KPILogicTests
	{
		private const string ValidName1 = "Unique name1";
		private const int ValidPlan = 25;
		private const float ValidWeight = 0.5f;
		private const string ValidKPIName1 = "KPI 1";
		private const string ValidKPIName2 = "KPI 2";

		[TestMethod]
		public void KPI_GetKPIResult_Throws_If_Not_Validated()
		{
			const int ValidFactValue = 100;
			var kpi = new KPI(ValidName1);
			Func<int, float> algorithm = (fact) => 1f;
			float ExpectedResult = algorithm.Invoke(100) * ValidWeight;

			Assert.ThrowsException<InvalidOperationException>(() => kpi.GetKPIResult(ValidFactValue),
				$"KPI GetResult() not throws if model not completed");

			kpi = kpi.SetPlan(ValidPlan);
			Assert.ThrowsException<InvalidOperationException>(() => kpi.GetKPIResult(ValidFactValue),
				$"KPI GetResult() not throws if model not completed");

			kpi = kpi.SetWeight(ValidWeight);
			Assert.ThrowsException<InvalidOperationException>(() => kpi.GetKPIResult(ValidFactValue),
				$"KPI GetResult() not throws if model not completed");

			kpi = kpi.SetLogic(Logic.Binary);
			Assert.ThrowsException<InvalidOperationException>(() => kpi.GetKPIResult(ValidFactValue),
				$"KPI GetResult() not throws if model not completed");

			kpi.SetAlgorithm(algorithm);
			Assert.AreEqual(ExpectedResult, kpi.GetKPIResult(ValidFactValue),
				$"KPI GetResult() throws after KPI was correct setted");
		}

		[TestMethod]
		public void KPI_Result_Returns_Valid_Value()
		{
			var kpi1 = new KPI(ValidKPIName1)
				 .SetLogic(Logic.Binary)
				 .SetPlan(100);
			kpi1.SetAlgorithm(fact => fact < kpi1.Plan ? 0f : 2f);
			var kpi2 = new KPI(ValidKPIName2)
				.SetLogic(Logic.Ladder)
				.SetPlan(200);
			kpi2.SetAlgorithm(
				fact =>
				{
					if (fact < kpi2.Plan * 0.8) return 0f;
					if (fact >= kpi2.Plan * 0.8 && fact < kpi2.Plan) return 0.8f;
					return 1f;
				});

			kpi1 = kpi1.SetWeight(ValidWeight);
			Assert.AreEqual(0f, kpi1.GetKPIResult(90), "Incorrect result value for fact that below plan");
			Assert.AreEqual(1f, kpi1.GetKPIResult(100), "Incorrect result value for fact that equals plan");
			Assert.AreEqual(1f, kpi1.GetKPIResult(200), "Incorrect result value for fact that above plan");

			kpi2 = kpi2.SetWeight(0.8f);
			Assert.AreEqual(0f, kpi2.GetKPIResult(Convert.ToInt32(200 * 0.8 - 1)),
				"Incorrect result value for fact that below lower bound");
			Assert.AreEqual(0.8f * 0.8f, kpi2.GetKPIResult(Convert.ToInt32(200 * 0.8 + 1)),
				"Incorrect result value for fact that above lower bound but below plan");
			Assert.AreEqual(0.8f, kpi2.GetKPIResult(200),
				"Incorrect result value for fact that equals plan");
			Assert.AreEqual(0.8f, kpi2.GetKPIResult(1000),
				"Incorrect result value for fact that above plan");
		}
	}
}
