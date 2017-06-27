using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenHapticsWrapper;
using System.Runtime.InteropServices;

namespace HapticMouse
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("User32.dll", EntryPoint = "mouse_event", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        static OpenHaptics openHaptics = new OpenHaptics(false, false);
        static bool prevButton1 = false;
        static bool currButton1 = false;
        static bool prevButton2 = false;
        static bool currButton2 = false;

        static int cursorX;
        static int cursorY;

        static void GetData()
        {
            // Convert degree to position
            cursorX = (int) (-openHaptics.outRotX / Math.PI * 1920 * 2) + 960;
            cursorY = (int) (openHaptics.outRotY / Math.PI * 1080 * 2) + 540;

            // Move mouse
            SetCursorPos(cursorX, cursorY);

            // Capture clicks
            currButton1 = openHaptics.bButton1State;
            currButton2 = openHaptics.bButton2State;

            if(currButton1 && !prevButton1)
            {
                LeftMouseDown();
            }
            if(!currButton1 && prevButton1)
            {
                LeftMouseUp();
            }
            if(currButton2 && !prevButton2)
            {
                RightMouseDown();
            }
            if(!currButton2 && prevButton2)
            {
                RightMouseUp();
            }

            prevButton1 = currButton1;
            prevButton2 = currButton2;
        }

        static void LeftMouseDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, cursorX, cursorY, 0, 0);
        }

        static void LeftMouseUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, cursorX, cursorY, 0, 0);
        }

        static void RightMouseDown()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, cursorX, cursorY, 0, 0);
        }

        static void RightMouseUp()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, cursorX, cursorY, 0, 0);
        }

        static void Main(string[] args)
        {
            openHaptics.InitDevice();
            openHaptics.userFunctionHandler = GetData;
            openHaptics.Run();
            Console.Read();
        }
    }
}
