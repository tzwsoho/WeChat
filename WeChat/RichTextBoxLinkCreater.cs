using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WeChat
{
    static public class RichTextBoxLinkCreater
    {
        private const int CFE_LINK = 0x20;
        private const int CFM_LINK = 0x20;
        private const int EM_SETCHARFORMAT = (WM_USER + 68);
        private const int SCF_SELECTION = 0x1;
        private const int WM_USER = 0x400;

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2
        {
            internal int cbSize;
            internal uint dwMask;
            internal uint dwEffects;
            internal int yHeight;
            internal int yOffset;
            internal int crTextColor;
            internal byte bCharSet;
            internal byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal char[] szFaceName;
            internal short wWeight;
            internal short sSpacing;
            internal int crBackColor;
            internal int LCID;
            internal uint dwReserved;
            internal short sStyle;
            internal short wKerning;
            internal byte bUnderlineType;
            internal byte bAnimation;
            internal byte bRevAuthor;
            internal byte bReserved1;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageCHARFORMAT(IntPtr hWnd, uint Msg, int wParam, ref CHARFORMAT2 lParam);

        public static void CreateLink(RichTextBox rtb, int n_begin, int n_length)
        {
            rtb.Select(n_begin, n_length);

            CHARFORMAT2 cf2 = new CHARFORMAT2();
            cf2.cbSize = Marshal.SizeOf(typeof(CHARFORMAT2));
            cf2.dwMask = CFM_LINK;
            cf2.dwEffects = CFE_LINK;
            SendMessageCHARFORMAT(rtb.Handle, EM_SETCHARFORMAT, SCF_SELECTION, ref cf2);

            rtb.Select(rtb.TextLength, 0);
        }
    }
}
