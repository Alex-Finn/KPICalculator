using KPILibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KPITest.ModelsTests
{
	[TestClass]
	public class EmployeeModelTests
	{
		private const string ValidName = "Employee1";
		private const int ValidId1 = 123;
		private const int InvalidId = -345;

		[TestMethod]
		public void Employee_Name_Must_Be_Not_Null()
		{
			Assert.ThrowsException<ArgumentException>(() => new Employee(null, ValidId1),
				$"Employee's name must be not null");
		}

		[TestMethod]
		public void Employee_Name_Must_Be_Not_Empty()
		{
			Assert.ThrowsException<ArgumentException>(() => new Employee("" , ValidId1),
				$"Employee's name must be not empty");
		}

		[TestMethod]
		public void Employee_Name_Must_Be_Not_Only_Whitespace()
		{
			Assert.ThrowsException<ArgumentException>(() => new Employee(" ", ValidId1),
				$"Employee's name must contain not only whitespace");
		}

		[TestMethod]
		public void Employee_Id_Must_Be_Positive()
		{
			Assert.ThrowsException<ArgumentException>(() => new Employee(ValidName, InvalidId),
				$"Employee's id must be positive");
		}

		[TestMethod]
		public void Employee_Name_Must_Be_Correct()
		{
			Assert.AreEqual(ValidName, new Employee(ValidName, ValidId1).Name,
				$"Employee must be created with correct name");
		}

		[TestMethod]
		public void Employee_Id_Must_Be_Correct()
		{
			Assert.AreEqual(ValidId1, new Employee(ValidName, ValidId1).Id,
				$"Employee must be created with correct id");
		}
	}
}
