using Microsoft.Extensions.Configuration;

#if RELEASE_WIN
using Microsoft.Win32;
#endif

namespace IO.TBRT.FreeZVT
{
    class Program
    {
        

        public static void Main(string[] args)
        {
            DataTypes.Options options;
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                //Set current executable path in registry when running on windows
#if RELEASE_WIN
#pragma warning disable CA1416
                Registry.SetValue(config["Registry:DefaultKey"] ?? string.Empty, "START",
                    System.Reflection.Assembly.GetExecutingAssembly().Location);
#pragma warning restore CA1416
#endif

                var regDefaultKey = config["Registry:DefaultKey"];
                if (string.IsNullOrEmpty(regDefaultKey))
                    throw new DataTypes.ParsingException("Default regex path cannot be empty.");
                options = args.Any() ? Parser.ParseCommandLineArgs(args) : Parser.ParseRegistryArgs(regDefaultKey);
                
                if (!string.IsNullOrEmpty(options.Eingabedatei))
                    options = Parser.ParseCommandLineArgs(File.ReadAllText(options.Eingabedatei).Split());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
                //TODO: cleanup, log file and report error to application
            }
        }
    }
}

