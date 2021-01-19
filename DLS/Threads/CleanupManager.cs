using DLS.Utils;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DLS.Threads
{
    class CleanupManager
    {
        static uint lastProcessTime = Game.GameTime;
        static int timeBetweenChecks = 10000;
        static int yieldAfterChecks = 10;
        public static void Process()
        {
            while (true)
            {
                int checksDone = 0;
                List<uint> UsedHashes = new List<uint>();

                foreach (ActiveVehicle activeVeh in Entrypoint.activeVehicles.ToList())
                {
                    if (!activeVeh.Vehicle)
                        Entrypoint.activeVehicles.Remove(activeVeh);
                    else
                        UsedHashes.Add(activeVeh.CurrentHash);

                    checksDone++;
                    if (checksDone % yieldAfterChecks == 0)
                        GameFiber.Yield();
                }

                foreach (uint hash in Entrypoint.UsedPool.Keys.ToList())
                {
                    if (!UsedHashes.Contains(hash))
                    {
#if DEBUG
                        ("Moving " + hash + " to Available Pool").ToLog();
#endif
                        Entrypoint.AvailablePool.Add(Entrypoint.UsedPool[hash]);
                        Entrypoint.UsedPool.Remove(hash);
                    }
                }
                GameFiber.Sleep((int)Math.Max(timeBetweenChecks, Game.GameTime - lastProcessTime));
                lastProcessTime = Game.GameTime;
            }
        }
    }
}
