namespace MyApplication;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();
		Application.Run(mainForm: new MainForm());
	}
}
