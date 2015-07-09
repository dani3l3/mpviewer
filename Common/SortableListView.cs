using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Common
{
    internal class SortableListView : ListView
    {
        ListViewColumnSorter m_columnSorter;

        //---------------------------------------------------------------------
        internal SortableListView()
        {
            m_columnSorter          = new ListViewColumnSorter();
            ListViewItemSorter      = m_columnSorter;
            FullRowSelect           = true;
            ColumnClick             += new ColumnClickEventHandler(PropertiesListView_ColumnClick);
            View                    = System.Windows.Forms.View.Details;
        }

        //---------------------------------------------------------------------
        void PropertiesListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == m_columnSorter.SortColumn)
            {
                if (m_columnSorter.Order == SortOrder.Ascending)
                {
                    m_columnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    m_columnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                m_columnSorter.SortColumn = e.Column;
                m_columnSorter.Order = SortOrder.Ascending;
            }

            Sort();
        }

        //---------------------------------------------------------------------
        internal void SortByColumn(int columnIndex)
        {
            if (m_columnSorter.SortColumn == columnIndex)
            {
                Sort();
            }
            else
            {
                PropertiesListView_ColumnClick(this, new ColumnClickEventArgs(columnIndex));
            }
        }

        //---------------------------------------------------------------------
        internal void AdjustColumnSizes()
        {
            foreach (ColumnHeader column in Columns)
            {
                column.Width = -1;
            }

            foreach (ColumnHeader column in Columns)
            {
                column.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }
    }

    //---------------------------------------------------------------------
    public class ListViewColumnSorter : IComparer
    {
        private int                     ColumnToSort;
        private SortOrder               OrderOfSort;
        private CaseInsensitiveComparer ObjectCompare;

        //---------------------------------------------------------------------
        public ListViewColumnSorter()
        {

            ColumnToSort = 0;
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
            Order = SortOrder.Ascending;
        }

        //---------------------------------------------------------------------
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            if (OrderOfSort == SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }

        //---------------------------------------------------------------------
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        //---------------------------------------------------------------------
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
