using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Reflection;
using Rage;
using Rage.Native;


namespace DLS.Utils
{

    internal static class ExtrasExtensions
    {
        public static bool HasExtra(this Vehicle vehicle, int extra) => NativeFunction.Natives.DoesExtraExist<bool>(vehicle, extra);
        public static bool IsExtraEnabled(this Vehicle vehicle, int extra) => NativeFunction.Natives.IsVehicleExtraTurnedOn<bool>(vehicle, extra);
        public static void SetExtra(this Vehicle vehicle, int extra, bool enabled) => NativeFunction.Natives.SetVehicleExtra(vehicle, extra, !enabled);
    }

    internal static unsafe class ExtraRepairPatch
    {
        // AdvancedHookV.dll must be present, it's included with ELS
        [DllImport("AdvancedHookV.dll", EntryPoint = "?CreateAdvHookService@@YAPEAVIAdvancedHookV@@XZ")]
        private static extern IAdvancedHookV* CreateAdvHookService();

        private delegate bool InitializeAdvancedHookServiceDelegate(IAdvancedHookV* @this, string token);
        private delegate bool SetVehicleRepairStateDelegate(IAdvancedHookV* @this, bool enable);
        
        private struct IAdvancedHookV
        {
            public IAdvancedHookVVTable* VTable;
        }

        private struct IAdvancedHookVVTable
        {
            public IntPtr InitializePtr;
            public IntPtr HasInitializedPtr;
            public IntPtr DrawCoronaPtr;
            public IntPtr SetVehicleRepairStatePtr;
        }

        // default to null, set to false if patch fails, true if successful
        private static bool? hasPatched = null;

        public static bool DisableExtraRepair()
        {
            // Only run once per session
            if (hasPatched.HasValue) return hasPatched.Value;

            IAdvancedHookV* advancedHookService;
            try
            {
                advancedHookService = CreateAdvHookService();
            } catch (DllNotFoundException)
            {
                "ERROR: AdvancedHookV.dll is missing or could not be loaded, unable to patch extra repair".ToLog();
                hasPatched = false;
                return hasPatched.Value;
            }
             
            if (!hasPatched.HasValue && advancedHookService != null)
            {
                var vTable = advancedHookService->VTable;
                var initializeFuncPtr = vTable->InitializePtr;
                var setVehicleRepairStateFuncPtr = vTable->SetVehicleRepairStatePtr;

                InitializeAdvancedHookServiceDelegate initializeFunc = Marshal.GetDelegateForFunctionPointer<InitializeAdvancedHookServiceDelegate>(initializeFuncPtr);
                SetVehicleRepairStateDelegate setRepairStateFunc = Marshal.GetDelegateForFunctionPointer<SetVehicleRepairStateDelegate>(setVehicleRepairStateFuncPtr);

                string token = Assembly.GetExecutingAssembly().GetName().Name;
                if (initializeFunc(advancedHookService, token))
                {
                    hasPatched = setRepairStateFunc(advancedHookService, false);
                }
            }

            // Default to false if it doesn't work
            hasPatched = hasPatched ?? false;
            
            // Return status
            return hasPatched.Value;
        }
    }
}
