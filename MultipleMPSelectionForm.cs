using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Microsoft.EnterpriseManagement.Configuration;

namespace MPViewer
{
    public partial class MultipleMPSelectionForm : Form
    {
        public ManagementPack ChosenMP;
        private IList<ManagementPack> MPList;

        public MultipleMPSelectionForm(IList<ManagementPack> MPListInput)
        {

            this.MPList = MPListInput;

            this.MPSortableList = new SortableListView();
            this.MPSortableList.Items.Clear();
            this.MPSortableList.Columns.Clear();

            this.MPSortableList.Columns.Add("Management Pack");
            this.MPSortableList.Columns.Add("Version");

            foreach (ManagementPack mp in MPListInput)
            {
                ListViewItem item = new ListViewItem();
                item.Text = mp.Name;
                item.SubItems.Add(mp.Version.ToString());

                this.MPSortableList.Items.Add(item);
            }

            this.MPSortableList.AdjustColumnSizes();

            InitializeComponent();
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            //since the form has multi-select disabled, the selected item is always index 0
            IEnumerable<ManagementPack> ChosenMPCollection = MPList.Where(mp => mp.Name.Equals(this.MPSortableList.SelectedItems[0].Text));
            ChosenMP = ChosenMPCollection.ToList()[0];
            this.Close();
            return;
        }

        private void MPSortableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MPSortableList.SelectedItems.Count == 0)
            {
                this.buttonOK.Enabled = false;
            }
            else
            {
                this.buttonOK.Enabled = true;
            }

            return;
        }

    }
}
