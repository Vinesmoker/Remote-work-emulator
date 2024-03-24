using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MouseCursorMover
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(int vKey);
        const int MOUSE_MOVE_DELAY = 2000;
        const int ESCAPE_KEY = 0x1B;
        static void Main()
        {
            SetThreadExecutionState(0x80000002);
            while (true)
            {
                int newX = new Random().Next(Screen.PrimaryScreen.Bounds.Width);
                int newY = new Random().Next(Screen.PrimaryScreen.Bounds.Height);
                SmoothlyMoveCursor(newX, newY);
                Thread.Sleep(MOUSE_MOVE_DELAY);
                if (GetAsyncKeyState(ESCAPE_KEY))
                    break;
            }
            SetThreadExecutionState(0x00000000);
        }
        private static void SmoothlyMoveCursor(int targetX, int targetY)
        {
            const int STEPS = 50;
            int currentX = Cursor.Position.X;
            int currentY = Cursor.Position.Y;
            double deltaX = targetX - currentX;
            double deltaY = targetY - currentY;

            for (int i = 1; i <= STEPS; i++)
            {
                int newX = currentX + (int)(deltaX * i / STEPS);
                int newY = currentY + (int)(deltaY * i / STEPS);
                SetCursorPos(newX, newY);
                Thread.Sleep(MOUSE_MOVE_DELAY / STEPS);
            }
            SetCursorPos(targetX, targetY);
        }
    }
}
