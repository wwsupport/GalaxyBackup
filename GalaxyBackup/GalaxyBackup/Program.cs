using System;
using System.Diagnostics;
using ArchestrA.GRAccess;

namespace GalaxyBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Failure, missing arguments.");
                Console.WriteLine("galaxyBackup.exe <galaxyname> <path>");
                return;
            }

            string nodeName = Environment.MachineName;

            GRAccessApp grAccess = new GRAccessAppClass();

            IGalaxies gals = grAccess.QueryGalaxies(nodeName);

            if (gals == null || grAccess.CommandResult.Successful == false)
            {
                Console.WriteLine(grAccess.CommandResult.CustomMessage + grAccess.CommandResult.Text);
                return;
            }

            IGalaxy galaxy = gals[args[0]];

            if (galaxy == null)
            {
                Console.WriteLine("Failure, galaxy '{0}' does not exist", args[0]);
                return;
            }

            Process currentProc = Process.GetCurrentProcess();

            galaxy.Backup(currentProc.Id, args[1], nodeName, args[0]);
        }
    }
}
