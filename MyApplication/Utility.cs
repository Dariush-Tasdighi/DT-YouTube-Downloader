namespace MyApplication;

internal static class Utility : object
{
	internal static string GetNow()
	{
		var result =
			DateTime.Now.ToString
			(format: "yyyy/mm/dd - HH:mm:ss");

		return result;
	}
}
