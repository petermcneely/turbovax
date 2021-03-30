using CommandLine;
using System;

namespace TurboVax
{
    public class TurboVaxOptions
    {
        [Option('o', "find-and-quit", Required = false, Default = false, HelpText = "After finding a set of sites, will quit")]
        public bool FindAndQuit { get; set; }

        [Option('b', "find-and-wait", Required = false, HelpText = "After finding a set of sites, will wait the specified amount of time before polling for sites again")]
        public TimeSpan FindAndWait { get; set; }

        [Option('p', "portal-names", Required = false, Default = null, HelpText = "Will only search for sites that match the names of these portals (pipe delimited)")]
        public string PortalNames { get; set; }
    }
}
