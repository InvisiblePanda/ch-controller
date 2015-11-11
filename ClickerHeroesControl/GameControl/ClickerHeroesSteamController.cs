using ClickerHeroesControl.Exceptions;
using ClickerHeroesControl.WindowsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickerHeroesControl.GameControl
{
    internal sealed class ClickerHeroesSteamController
    {
        private const string WindowTitle = "Clicker Heroes";

        private readonly IntPtr windowHandle = IntPtr.Zero;
        private readonly int leftBorderWidth;
        private readonly int topBorderHeight;

        public ClickerHeroesSteamController()
        {
            windowHandle = WinApi.FindWindow(null, WindowTitle);
            if (windowHandle == IntPtr.Zero)
            {
                throw new TargetWindowNotFoundException();
            }

            WinApi.RECT windowRect;
            WinApi.GetWindowRect(windowHandle, out windowRect);
            WinApi.POINT topLeft;
            WinApi.ClientToScreen(windowHandle, out topLeft);
            leftBorderWidth = topLeft.X - windowRect.Left;
            topBorderHeight = topLeft.Y - windowRect.Top;
        }


    }
}