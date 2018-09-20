using System;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WeChat
{
    public class ListViewSorter
    {
        #region 枚举

        [StructLayout(LayoutKind.Sequential)]
        public struct LVCOLUMN
        {
            public Int32 mask;
            public Int32 cx;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public Int32 cchTextMax;
            public Int32 fmt;
            public Int32 iSubItem;
            public Int32 iImage;
            public Int32 iOrder;
        }

        const Int32 HDI_WIDTH = 0x0001;
        const Int32 HDI_HEIGHT = HDI_WIDTH;
        const Int32 HDI_TEXT = 0x0002;
        const Int32 HDI_FORMAT = 0x0004;
        const Int32 HDI_LPARAM = 0x0008;
        const Int32 HDI_BITMAP = 0x0010;
        const Int32 HDI_IMAGE = 0x0020;
        const Int32 HDI_DI_SETITEM = 0x0040;
        const Int32 HDI_ORDER = 0x0080;
        const Int32 HDI_FILTER = 0x0100;

        const Int32 HDF_LEFT = 0x0000;
        const Int32 HDF_RIGHT = 0x0001;
        const Int32 HDF_CENTER = 0x0002;
        const Int32 HDF_JUSTIFYMASK = 0x0003;
        const Int32 HDF_RTLREADING = 0x0004;
        const Int32 HDF_OWNERDRAW = 0x8000;
        const Int32 HDF_STRING = 0x4000;
        const Int32 HDF_BITMAP = 0x2000;
        const Int32 HDF_BITMAP_ON_RIGHT = 0x1000;
        const Int32 HDF_IMAGE = 0x0800;
        const Int32 HDF_SORTUP = 0x0400;
        const Int32 HDF_SORTDOWN = 0x0200;

        const Int32 LVM_FIRST = 0x1000;         // List messages
        const Int32 LVM_GETHEADER = LVM_FIRST + 31;
        const Int32 HDM_FIRST = 0x1200;         // Header messages
        const Int32 HDM_SETIMAGELIST = HDM_FIRST + 8;
        const Int32 HDM_GETIMAGELIST = HDM_FIRST + 9;
        const Int32 HDM_GETITEM = HDM_FIRST + 11;
        const Int32 HDM_SETITEM = HDM_FIRST + 12;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, Int32 Msg, IntPtr wParam, ref LVCOLUMN lPLVCOLUMN);

        #endregion

        #region 属性

        //private enDataType m_nDataType = enDataType.enDataType_None;
        //public enDataType DataType
        //{
        //    get
        //    {
        //        return m_nDataType;
        //    }

        //    set
        //    {
        //        m_nDataType = value;
        //    }
        //}

        private int m_nSortColumn = -1;
        public int SortColumn
        {
            get
            {
                return m_nSortColumn;
            }

            set
            {
                m_nSortColumn = value;
            }
        }

        private ListView m_ListView = null;
        public ListView SortListView
        {
            get
            {
                return m_ListView;
            }

            set
            {
                m_ListView = value;
            }
        }

        #endregion

        #region 方法

        public void SetColumnSortIcon(ListView listView, int columnIndex, SortOrder order)
        {
            listView.Visible = false;
            IntPtr columnHeader = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
            {
                IntPtr columnPtr = new IntPtr(columnNumber);
                LVCOLUMN lvColumn = new LVCOLUMN();
                lvColumn.mask = HDI_FORMAT;

                SendMessageLVCOLUMN(columnHeader, HDM_GETITEM, columnPtr, ref lvColumn);

                if (!(order == SortOrder.None) && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case System.Windows.Forms.SortOrder.Ascending:
                            lvColumn.fmt &= ~HDF_SORTUP;
                            lvColumn.fmt |= HDF_SORTDOWN;
                            break;

                        case System.Windows.Forms.SortOrder.Descending:
                            lvColumn.fmt &= ~HDF_SORTDOWN;
                            lvColumn.fmt |= HDF_SORTUP;
                            break;
                    }

                    lvColumn.fmt |= (HDF_LEFT | HDF_BITMAP_ON_RIGHT);
                }
                else
                {
                    lvColumn.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP & ~HDF_BITMAP_ON_RIGHT;
                }

                SendMessageLVCOLUMN(columnHeader, HDM_SETITEM, columnPtr, ref lvColumn);
            }

            listView.Visible = true;
        }

        public ListViewSorter(ListView lvw, int nColumn, ListViewItem[] lvis = null)
        {
            m_ListView = lvw;
            m_nSortColumn = nColumn;

            if (SortOrder.Ascending == lvw.Sorting)
            {
                lvw.Sorting = SortOrder.Descending;
            }
            else
            {
                lvw.Sorting = SortOrder.Ascending;
            }

            if (null == lvis)
            {
                lvw.ListViewItemSorter = new ListViewItemsSorter(nColumn, lvw.Sorting);
                lvw.Sort();
            }
            else
            {
                Array.Sort(lvis, new ListViewItemsSorter(nColumn, lvw.Sorting));
            }

            SetColumnSortIcon(lvw, nColumn, lvw.Sorting);
        }

        #endregion
    }

    public class ListViewItemsSorter : IComparer
    {
        private int m_sort_column = -1;
        public int SortColumn
        {
            get { return m_sort_column; }
            set { m_sort_column = value; }
        }

        private SortOrder m_sort_order = SortOrder.None;
        public SortOrder SortOrder
        {
            get { return m_sort_order; }
            set { m_sort_order = value; }
        }

        public ListViewItemsSorter(int sort_column, SortOrder sort_order)
        {
            m_sort_column = sort_column;
            m_sort_order = sort_order;
        }

        public int Compare(object x, object y)
        {
            ListViewItem lviX = x as ListViewItem;
            ListViewItem lviY = y as ListViewItem;
            string strX = lviX.SubItems[m_sort_column].Text;
            string strY = lviY.SubItems[m_sort_column].Text;

            int nSign = (SortOrder.Descending == m_sort_order) ? 1 : -1;

            // Number
            decimal nX, nY;
            if (decimal.TryParse(strX, out nX) &&
                decimal.TryParse(strY, out nY))
            {
                return nSign * Math.Sign(nX - nY);
            }

            // Date
            DateTime dt_x, dt_y;
            if (DateTime.TryParse(strX, out dt_x) &&
                DateTime.TryParse(strY, out dt_y))
            {
                return nSign * Math.Sign(dt_x.Ticks - dt_y.Ticks);
            }

            return nSign * string.Compare(strX, strY);
        }
    }
}
