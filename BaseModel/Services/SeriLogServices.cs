using Serilog;

namespace BaseModel.Services
{
    public class SeriLogServices
    {
        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

        }
        public void InfoLog(string message)
        {
            Log.Information(message);
        }

        public void ErrorLog(string message)
        {
            Log.Error(message);
        }

        public void WarningLog(string message)
        {
            Log.Warning(message);
        }
    }
}
