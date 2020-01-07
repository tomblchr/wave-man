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
            Console.WriteLine("Here we go...");

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
            Console.WriteLine("... Scanning for empty WAV files ...");
        }

        static void DoSomething(Options options)
        {
            DirectoryInfo dir = new DirectoryInfo(options.Path);

            dir.EnumerateFiles("*.wav").ToList().ForEach(c => 
            {
                var d = new EmptyWavDetector(c.FullName);

                if (d.IsEmpty)
                {
                    Console.WriteLine($"File {c.FullName} is empty!");
                }
                else
                {
                    if (!options.Silent) Console.WriteLine($"File {c.FullName} is not empty.");
                }
            });
        }
    }
}
