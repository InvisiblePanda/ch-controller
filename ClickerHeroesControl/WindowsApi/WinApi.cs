using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ClickerHeroesControl.WindowsApi
{
    public static class WinApi
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out POINT lpPoint);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public POINT(Point pt)
                : this(pt.X, pt.Y)
            {
            }

            public static implicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }

            public static implicit operator POINT(Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int left;
            private int top;
            private int right;
            private int bottom;

            public RECT(RECT rect)
                : this(rect.Left, rect.Top, rect.Right, rect.Bottom)
            {
            }

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public int X
            {
                get { return left; }
                set { left = value; }
            }

            public int Y
            {
                get { return top; }
                set { top = value; }
            }

            public int Left
            {
                get { return left; }
                set { left = value; }
            }

            public int Top
            {
                get { return top; }
                set { top = value; }
            }

            public int Right
            {
                get { return right; }
                set { right = value; }
            }

            public int Bottom
            {
                get { return bottom; }
                set { bottom = value; }
            }

            public int Height
            {
                get { return bottom - top; }
                set { bottom = value + top; }
            }

            public int Width
            {
                get { return right - left; }
                set { right = value + left; }
            }

            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    left = value.X;
                    top = value.Y;
                }
            }

            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    right = value.Width + left;
                    bottom = value.Height + top;
                }
            }

            public static implicit operator Rectangle(RECT rect)
            {
                return new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return rect1.Equals(rect2);
            }

            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !rect1.Equals(rect2);
            }

            public override string ToString()
            {
                return "{Left: " + left + "; " + "Top: " + top + "; Right: " + right + "; Bottom: " + bottom + "}";
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public bool Equals(RECT other)
            {
                return other.Left == left && other.Top == top && other.Right == right && other.Bottom == bottom;
            }

            public override bool Equals(object other)
            {
                if (other is RECT)
                {
                    return Equals((RECT)other);
                }
                else if (other is Rectangle)
                {
                    return Equals(new RECT((Rectangle)other));
                }

                return false;
            }
        }
    }
}