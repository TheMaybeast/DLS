using Rage;

namespace DLS.Utils
{
    internal static class UIHelper
    {
        public static bool IsUIAbleToDisplay { get; set; } = false;
        public static DLSModel dlsModel { get; set; } = null;
        public static ActiveVehicle activeVeh { get; set; } = null;
        public static Ped player { get; set; } = null;
        public static bool IsInAnyVehicle { get; set; } = false;
        public static Vehicle currentVehicle { get; set; }
        public static bool IsPlayerDriver { get; set; } = false;
        public static bool IsVehicleValid { get; set; } = false;
        public static SirenStatus[] IsSirenOn { get; set; } = new SirenStatus[20];

        private static void Process()
        {
            while (true)
            {
                GameFiber.Yield();
                if (!Game.IsPaused && !Game.IsLoading && !Game.IsScreenFadedOut)
                    IsUIAbleToDisplay = true;
                else
                    IsUIAbleToDisplay = false;
                player = Game.LocalPlayer.Character;
                IsInAnyVehicle = player.IsInAnyVehicle(false);
                currentVehicle = player.CurrentVehicle;
                if (currentVehicle)
                {
                    IsVehicleValid = true;
                    if (currentVehicle.IsEngineOn && currentVehicle.HasSiren)
                        IsVehicleValid = true;
                    if (currentVehicle.Driver == player)
                        IsPlayerDriver = true;
                    else
                        IsPlayerDriver = false;
                    dlsModel = currentVehicle.GetDLS();
                    activeVeh = currentVehicle.GetActiveVehicle();
                    if (IsPlayerDriver && activeVeh != null && dlsModel != null && currentVehicle.HasSiren)
                    {
                        for (int i = 1; i <= 20; i++)
                        {
                            IsSirenOn[i - 1] = Lights.GetSirenStatus(activeVeh, i);
                        }
                    }
                }
                else
                    IsVehicleValid = false;
            }
        }

        static UIHelper()
        {
            GameFiber.StartNew(Process, "DLS - UI Helper");
        }
    }
}
