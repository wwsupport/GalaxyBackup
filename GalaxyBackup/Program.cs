using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchestrA.GRAccess;
using SideSoftware.Wonderware.Logger;

namespace GalaxyBackup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WwLog.LogSetIdentityName("GalaxyBackup");

            string nodeName = Environment.MachineName;
            string galaxyName = args[0];
            string path = args[1];

            GRAccessApp gracc = new GRAccessAppClass();

            try
            {
                if (args.Length < 2)
                {
                    // Failure - needs 2 args
                    WwLog.LogInfo("Failure, missing arguments.");
                    WwLog.LogInfo("galaxyBackup <galaxyName> <path to save>");
                    return;
                }

                

                WwLog.LogInfo("Reading galaxies...");

                IGalaxies galaxies = gracc.QueryGalaxies(nodeName);

                if (galaxies == null)
                {
                    WwLog.LogWarning("Sorry, it looks like you're running this on a machine with no galaxies");
                    return;
                }

                IGalaxy galaxy = galaxies[galaxyName];

                if (galaxy == null)
                {
                    WwLog.LogWarning("Galaxy " + galaxyName + " cannot be found on this machine, please confirm the galaxy exists");
                    return;
                }

                try
                {
                    using (FileStream fs = File.Create(path + Path.GetRandomFileName(), 1, FileOptions.DeleteOnClose)) { };
                    Process proc = Process.GetCurrentProcess();

                    galaxy.Backup(proc.Id, path + galaxyName + ".cab", nodeName, galaxyName);
                }
                catch
                {
                    WwLog.LogError("Sorry, the path " + path + " is not writeable, please correct the permissions or choose a different path.");
                    return;
                }
            }
            catch (Exception ex)
            {
                WwLog.LogError("There was an unexpected failure, please refer the following to support");
                WwLog.LogError(ex.ToString());
            }
        }
    }
}
