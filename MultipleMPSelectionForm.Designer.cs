namespace MPViewer
{
    partial class MultipleMPSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(412, 191);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // MPSortableList
            // 
            this.MPSortableList.FullRowSelect = true;
            this.MPSortableList.Location = new System.Drawing.Point(13, 13);
            this.MPSortableList.Name = "MPSortableList";
            this.MPSortableList.Size = new System.Drawing.Size(474, 169);
            this.MPSortableList.TabIndex = 1;
            this.MPSortableList.UseCompatibleStateImageBehavior = false;
            this.MPSortableList.View = System.Windows.Forms.View.Details;
            this.MPSortableList.SelectedIndexChanged += new System.EventHandler(this.MPSortableList_SelectedIndexChanged);
            // 
            // MultipleMPSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 226);
            this.Controls.Add(this.MPSortableList);
            this.Controls.Add(this.buttonOK);
            this.Name = "MultipleMPSelectionForm";
            this.Text = "Choose one MP contained in this bundle";
            this.AcceptButton = buttonOK;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private Common.SortableListView MPSortableList;
    }
}