using CommandLine;
using System;
using System.IO;
using System.Linq;

namespace DeleteEmptyWav.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("--- Empty WAV Finder ---");
            Console.WriteLine();

            try
            {
                Parser.Default
                    .ParseArguments<Options>(args)
                    .WithParsed(c =>
                    {
                        if (!c.Silent) ShowWelcome();
                        DoSomething(c);
                    });
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void ShowWelcome()
        {
            Console.WriteLine("Scanning for empty WAV files ...");
        }

        static void DoSomething(Options options)
        {
            DirectoryInfo dir = new DirectoryInfo(options.Path);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            dir.EnumerateFiles("*.wav").ToList().ForEach(c => 
            {
                var t = options.Threshold;
                var d = new EmptyWavDetector(c.FullName, t);

                if (!options.Silent && options.Measure && d.PercentEmpty == 0)
                {
                    Console.WriteLine($"File {d.FileName} is empty!");
                }
                else if (!options.Silent && options.Measure)
                {
                    Console.WriteLine($"File {d.FileName} is {d.PercentEmpty}% empty.");
                }
                else if (d.IsEmpty)
                {
                    Console.WriteLine($"File {d.FileName} is empty!");
                }
                else if (!options.Silent)
                {
                    Console.WriteLine($"File {d.FileName} is NOT empty!");
                }
            });

            watch.Stop();

            Console.WriteLine($"{Environment.NewLine}Scan complete in {watch.ElapsedMilliseconds}msec{Environment.NewLine}");
        }
    }
}
