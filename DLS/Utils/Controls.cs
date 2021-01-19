using Rage;
using Rage.Native;

namespace DLS.Utils
{
    internal class Controls
    {
        public static bool IsDisabledControlJustReleased(int padIndex, GameControl control)
        {
            return NativeFunction.Natives.IsDisabledControlJustReleased<bool>(padIndex, (int)control);
        }

        public static bool IsDisabledControlPressed(int padIndex, GameControl control)
        {
            return NativeFunction.Natives.IsDisabledControlPressed<bool>(padIndex, (int)control);
        }
    }
}