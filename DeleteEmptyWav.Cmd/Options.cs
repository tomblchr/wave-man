using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeleteEmptyWav.Cmd
{
    class Options
    {
        [Option('p', "path", Required = false, HelpText = "Path to folder to scan.")]
        public string Path { get; set; }

        [Option('s', "silent", Required = false, HelpText = "Show only empty files.", Default = false)]
        public bool Silent { get; set; }
    }
}
