using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    class Program
    {
        static void Main(string[] args)
        {
            FluentCommandLineParser<ApplicationArguments> p = new FluentCommandLineParser<ApplicationArguments>();
            p.Setup(arg => arg.Host).As('h', "host").SetDefault("localhost");
            p.Setup(arg => arg.Port).As('p', "port").SetDefault(6379);
            p.Setup(arg => arg.Password).As('w', "password").SetDefault("");
            p.Setup(arg => arg.DbID).As('d', "db").Required();
            p.Setup(arg => arg.Instance).As('i', "instance").Required();

            var result = p.Parse(args);

            if (result.HasErrors == true)
            {
                Console.WriteLine("Imports redis data into a real database =D");
                Console.WriteLine("");
                Console.WriteLine("\t--host\t\tSets redis host.");
                Console.WriteLine("\t--port\t\tSets redis port.");
                Console.WriteLine("\t--password\t\tSets redis password.");
                Console.WriteLine("\t--db\t\tSets redis DB ID");
                Console.WriteLine("\t--instancet\tEpoch server instance");
                Console.WriteLine("");
                Environment.Exit(0);
            }
            ApplicationArguments options = p.Object as ApplicationArguments;
            Console.WriteLine(options.Instance);
            Importer.Run(options);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
