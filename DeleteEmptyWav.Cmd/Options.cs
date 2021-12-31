using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeleteEmptyWav.Cmd
{
    class Options
    {
        [Option('p', "path", Required = false, HelpText = "Path to folder to scan.", Default = ".\\")]
        public string Path { get; set; }

        [Option('s', "silent", Required = false, HelpText = "Show only empty files.", Default = false)]
        public bool Silent { get; set; }

        [Option('m', "measure", Required = false, HelpText = "Measure the emptyness as a percentange of the file.", Default = true)]
        public bool Measure { get; set; }

        [Option('t', "threshold", Required = false, HelpText = "Noise floor threshold (between 0 and 1).", Default = 0.001F)]
        public float Threshold { get; set; }
    }
}
