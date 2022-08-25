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

                var regDefaultKey = config["Registry:DefaultKey"];
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

