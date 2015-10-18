using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using ClickerHeroesControl.WindowsApi;
using System.Drawing.Imaging;

namespace ClickerHeroesControl
{
    public sealed class CHController
    {
        private const string ClickerHeroesWindowTitle = "Clicker Heroes";

        private readonly IntPtr targetHandle = IntPtr.Zero;
        private readonly int leftOffset;
        private readonly int topOffset;

        public CHController()
        {
            targetHandle = WinApi.FindWindow(null, ClickerHeroesWindowTitle);
            if (targetHandle == IntPtr.Zero)
            {
                throw new TargetWindowNotFoundException("Could not locate the Clicker Heroes (Steam) window.");
            }

            WinApi.RECT chRect;
            WinApi.GetWindowRect(targetHandle, out chRect);
            WinApi.POINT topLeft;
            WinApi.ClientToScreen(targetHandle, out topLeft);
            leftOffset = topLeft.X - chRect.Left;
            topOffset = topLeft.Y - chRect.Top;
        }

        public Rectangle CHClientRectangle
        {
            get
            {
                WinApi.RECT clientRect;
                WinApi.GetClientRect(targetHandle, out clientRect);
                WinApi.RECT windowRect;
                WinApi.GetWindowRect(targetHandle, out windowRect);

                Rectangle rect = new Rectangle(windowRect.X + leftOffset, windowRect.Y + topOffset, clientRect.Width, clientRect.Height);
                return rect;
            }
        }

        public async Task AutoFire(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                TargetClick(ClickerHeroesPositions.Fish1);
                TargetClick(ClickerHeroesPositions.Fish2);
                TargetClick(ClickerHeroesPositions.Fish3);
                TargetClick(ClickerHeroesPositions.Fish4);
                TargetClick(ClickerHeroesPositions.Fish5);

                try
                {
                    await Task.Delay(110, ct);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        private void TargetClick(IntPtr position)
        {
            WinApi.PostMessage(targetHandle, WinApiConstants.WM_LBUTTONDOWN, (IntPtr)1, position);
            WinApi.PostMessage(targetHandle, WinApiConstants.WM_LBUTTONUP, (IntPtr)0, position);
        }

        private void KeyPlusClick(uint keyCode, IntPtr position)
        {
            WinApi.PostMessage(targetHandle, WinApiConstants.WM_KEYDOWN, (IntPtr)keyCode, IntPtr.Zero);
            TargetClick(position);
            WinApi.PostMessage(targetHandle, WinApiConstants.WM_KEYUP, (IntPtr)keyCode, IntPtr.Zero);
        }

        public Point? FindClickable()
        {
            List<Point> clickablePositions = new List<Point>
            {
                new Point(753, 425),
                new Point(766, 373),
                new Point(1011, 446),
                new Point(531, 481),
                new Point(1059, 436),
                new Point(879, 505)
            };
            List<Color> clickableColors = new List<Color>
            {
                Color.FromArgb(0xda, 0x4a, 0x00),
                Color.FromArgb(0xd9, 0x41, 0x03),
                Color.FromArgb(0xdc, 0x46, 0x08),
                Color.FromArgb(0xdc, 0x43, 0x02),
                Color.FromArgb(0xd3, 0x3d, 0x01),
                Color.FromArgb(0xc8, 0x4b, 0x16)
            };

            using (Bitmap bmp = TakeScreenshot())
            {
                int tolerance = 40;
                for (int i = 0; i < clickablePositions.Count; i++)
                {
                    Point p = clickablePositions[i];
                    Color scannedColor = bmp.GetPixel(p.X, p.Y);
                    if (AreColorsSimilar(scannedColor, clickableColors[i], tolerance))
                    {
                        int x = p.X - leftOffset;
                        int y = p.Y - topOffset;
                        IntPtr pos = (IntPtr)((y << 16) | x);
                        TargetClick(pos);
                        return new Point(x, y);
                    }
                }
            }

            return null;
        }

        private Bitmap TakeScreenshot()
        {
            WinApi.RECT chRect;
            WinApi.GetWindowRect(targetHandle, out chRect);

            Bitmap bmp = new Bitmap(chRect.Width, chRect.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                IntPtr hDC = g.GetHdc();
                WinApi.PrintWindow(targetHandle, hDC, 0);
                g.ReleaseHdc();

                return bmp;
            }
        }

        private static bool AreColorsSimilar(Color c1, Color c2, int tolerance)
        {
            return Math.Abs(c1.R - c2.R) <= tolerance
            && Math.Abs(c1.G - c2.G) <= tolerance
            && Math.Abs(c1.B - c2.B) <= tolerance;
        }
    }
}