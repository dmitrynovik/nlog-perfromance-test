using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace NlogPerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int tasks = ParseCommandLine(args, "--tasks") ?? 10000;
            int delay = ParseCommandLine(args, "--delay") ?? 0;

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Start ...");

            RunTasks(logger, tasks, delay).GetAwaiter().GetResult();

            logger.Info("... End");
        }

        private static async Task RunTasks(ILogger logger, int tasks, int delay)
        {
            for (int i = 1; i <= tasks; ++i)
            {
                await Task.Run(() => logger.Info($"\tTask {i}", TaskCreationOptions.LongRunning));
                await Task.Delay(delay);
            }
        }

        private static int? ParseCommandLine(string[] args, string name)
        {
            var arg = args.FirstOrDefault(x => x.StartsWith(name));
            if (arg != null && int.TryParse(arg.Substring(name.Length), out var value))
            {
                return value;
            }
            return null;
        }
    }
}
