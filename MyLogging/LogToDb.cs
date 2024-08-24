namespace CollegeApp.MyLogging;

public class LogToDb : IMyLogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine("Log to Db");
    }
}