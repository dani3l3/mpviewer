using System;
using System.Windows.Forms;

namespace MPViewer
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void ProgressDialog_Load(object sender, EventArgs e)
        {
            this.progressBar.Maximum = 100;
            this.progressBar.Minimum = 0;
            this.progressBar.Value = 0;
        }

        internal void UpdateProgress(ProgressInfo progressInfo)
        {
            this.lblCurrentOperation.Text = progressInfo.Status;
            this.progressBar.Value = progressInfo.PercentageComplete;
        }

    }
}
