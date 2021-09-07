namespace KPILibrary.Logger
{
	public static class Logger
	{
		static Logger() { }
		public static void Log(string message) => System.Console.WriteLine(message);
	}
}
