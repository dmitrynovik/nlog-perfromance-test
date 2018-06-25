using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace NlogPerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int tasks = ParseCommandLine(args, "--tasks") ?? 1000;
            int delay = ParseCommandLine(args, "--delay") ?? 1;

            var logger = LogManager.GetCurrentClassLogger();

            logger.Info("Start");
            RunTasks(logger, tasks, delay).GetAwaiter().GetResult();
            logger.Info("End");
        }

        private static async Task RunTasks(Logger logger, int tasks, int delay)
        {
            for (int i = 0; i < tasks; ++i)
            {
                Task.Run(() => logger.Info($"Task {i}"));
                await Task.Delay(delay);
            }
        }

        private static int? ParseCommandLine(string[] args, string name)
        {
            var arg = args.FirstOrDefault(x => x.StartsWith(name));
            if (arg == null)
                return null;

            if (int.TryParse(arg.Substring(name.Length), out var value))
            {
                return value;
            }
            return null;
        }
    }
}
