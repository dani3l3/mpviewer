using Common;
namespace MPViewer
{
    partial class MPViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MPViewer));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.objectTypeTree = new System.Windows.Forms.TreeView();
            this.detailsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.detailsTabControl = new System.Windows.Forms.TabControl();
            this.knowledgeTab = new System.Windows.Forms.TabPage();
            this.knowledgeBrowser = new System.Windows.Forms.WebBrowser();
            this.mpElementXmlTab = new System.Windows.Forms.TabPage();
            this.mpElementXml = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadManagementPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unsealManagementPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToHTMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.detailsSplitContainer.Panel1.SuspendLayout();
            this.detailsSplitContainer.Panel2.SuspendLayout();
            this.detailsSplitContainer.SuspendLayout();
            this.detailsTabControl.SuspendLayout();
            this.knowledgeTab.SuspendLayout();
            this.mpElementXmlTab.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.objectTypeTree);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.detailsSplitContainer);
            this.splitContainer.Size = new System.Drawing.Size(722, 652);
            this.splitContainer.SplitterDistance = 146;
            this.splitContainer.TabIndex = 0;
            // 
            // objectTypeTree
            // 
            this.objectTypeTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTypeTree.HideSelection = false;
            this.objectTypeTree.Location = new System.Drawing.Point(0, 0);
            this.objectTypeTree.Name = "objectTypeTree";
            this.objectTypeTree.Size = new System.Drawing.Size(146, 652);
            this.objectTypeTree.TabIndex = 0;
            this.objectTypeTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectTypeTree_AfterSelect);
            // 
            // detailsSplitContainer
            // 
            this.detailsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.detailsSplitContainer.Name = "detailsSplitContainer";
            this.detailsSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // detailsSplitContainer.Panel1
            // 

            mpElementListView = new SortableListView();
            this.detailsSplitContainer.Panel1.Controls.Add(this.mpElementListView);
            // 
            // detailsSplitContainer.Panel2
            // 
            this.detailsSplitContainer.Panel2.Controls.Add(this.detailsTabControl);
            this.detailsSplitContainer.Size = new System.Drawing.Size(572, 652);
            this.detailsSplitContainer.SplitterDistance = 350;
            this.detailsSplitContainer.TabIndex = 0;
            // 
            // mpElementListView
            // 
            this.mpElementListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpElementListView.FullRowSelect = true;
            this.mpElementListView.HideSelection = false;
            this.mpElementListView.Location = new System.Drawing.Point(0, 0);
            this.mpElementListView.MultiSelect = false;
            this.mpElementListView.Name = "mpElementListView";
            this.mpElementListView.Size = new System.Drawing.Size(572, 350);
            this.mpElementListView.TabIndex = 0;
            this.mpElementListView.UseCompatibleStateImageBehavior = false;
            this.mpElementListView.View = System.Windows.Forms.View.Details;
            this.mpElementListView.SelectedIndexChanged += new System.EventHandler(this.mpElementListView_SelectedIndexChanged);
            // 
            // detailsTabControl
            // 
            this.detailsTabControl.Controls.Add(this.knowledgeTab);
            this.detailsTabControl.Controls.Add(this.mpElementXmlTab);
            this.detailsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsTabControl.Location = new System.Drawing.Point(0, 0);
            this.detailsTabControl.Name = "detailsTabControl";
            this.detailsTabControl.SelectedIndex = 0;
            this.detailsTabControl.Size = new System.Drawing.Size(572, 298);
            this.detailsTabControl.TabIndex = 0;
            // 
            // knowledgeTab
            // 
            this.knowledgeTab.Controls.Add(this.knowledgeBrowser);
            this.knowledgeTab.Location = new System.Drawing.Point(4, 22);
            this.knowledgeTab.Name = "knowledgeTab";
            this.knowledgeTab.Padding = new System.Windows.Forms.Padding(3);
            this.knowledgeTab.Size = new System.Drawing.Size(564, 272);
            this.knowledgeTab.TabIndex = 0;
            this.knowledgeTab.Text = "Knowledge";
            this.knowledgeTab.UseVisualStyleBackColor = true;
            // 
            // knowledgeBrowser
            // 
            this.knowledgeBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.knowledgeBrowser.IsWebBrowserContextMenuEnabled = false;
            this.knowledgeBrowser.Location = new System.Drawing.Point(3, 3);
            this.knowledgeBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.knowledgeBrowser.Name = "knowledgeBrowser";
            this.knowledgeBrowser.Size = new System.Drawing.Size(558, 266);
            this.knowledgeBrowser.TabIndex = 0;
            // 
            // mpElementXmlTab
            // 
            this.mpElementXmlTab.Controls.Add(this.mpElementXml);
            this.mpElementXmlTab.Location = new System.Drawing.Point(4, 22);
            this.mpElementXmlTab.Name = "mpElementXmlTab";
            this.mpElementXmlTab.Size = new System.Drawing.Size(564, 272);
            this.mpElementXmlTab.TabIndex = 1;
            this.mpElementXmlTab.Text = "Raw XML";
            this.mpElementXmlTab.UseVisualStyleBackColor = true;
            // 
            // mpElementXml
            // 
            this.mpElementXml.AllowWebBrowserDrop = false;
            this.mpElementXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpElementXml.IsWebBrowserContextMenuEnabled = false;
            this.mpElementXml.Location = new System.Drawing.Point(0, 0);
            this.mpElementXml.MinimumSize = new System.Drawing.Size(20, 20);
            this.mpElementXml.Name = "mpElementXml";
            this.mpElementXml.Size = new System.Drawing.Size(564, 272);
            this.mpElementXml.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(722, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadManagementPackToolStripMenuItem,
            this.unsealManagementPackToolStripMenuItem,
            this.saveToExcelToolStripMenuItem,
            this.saveToHTMLToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadManagementPackToolStripMenuItem
            // 
            this.loadManagementPackToolStripMenuItem.Name = "loadManagementPackToolStripMenuItem";
            this.loadManagementPackToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.loadManagementPackToolStripMenuItem.Text = "Load Management Pack";
            this.loadManagementPackToolStripMenuItem.Click += new System.EventHandler(this.loadManagementPackToolStripMenuItem_Click);
            // 
            // unsealManagementPackToolStripMenuItem
            // 
            this.unsealManagementPackToolStripMenuItem.Name = "unsealManagementPackToolStripMenuItem";
            this.unsealManagementPackToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.unsealManagementPackToolStripMenuItem.Text = "Unseal / Unpack Management Pack";
            this.unsealManagementPackToolStripMenuItem.Click += new System.EventHandler(this.unsealManagementPackToolStripMenuItem_Click);
            // 
            // saveToHTMLToolStripMenuItem
            // 
            this.saveToHTMLToolStripMenuItem.Name = "saveToHTMLToolStripMenuItem";
            this.saveToHTMLToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.saveToHTMLToolStripMenuItem.Text = "Save to HTML";
            this.saveToHTMLToolStripMenuItem.Click += new System.EventHandler(this.saveToHTMLToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // saveToExcelToolStripMenuItem
            // 
            this.saveToExcelToolStripMenuItem.Name = "saveToExcelToolStripMenuItem";
            this.saveToExcelToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.saveToExcelToolStripMenuItem.Text = "Save to Excel";
            this.saveToExcelToolStripMenuItem.Click += new System.EventHandler(this.saveToExcelToolStripMenuItem_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // MPViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 676);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MPViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Management Pack Viewer 2012";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MPViewer_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.detailsSplitContainer.Panel1.ResumeLayout(false);
            this.detailsSplitContainer.Panel2.ResumeLayout(false);
            this.detailsSplitContainer.ResumeLayout(false);
            this.detailsTabControl.ResumeLayout(false);
            this.knowledgeTab.ResumeLayout(false);
            this.mpElementXmlTab.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadManagementPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unsealManagementPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TreeView objectTypeTree;
        private SortableListView mpElementListView;
        private System.Windows.Forms.SplitContainer detailsSplitContainer;
        private System.Windows.Forms.TabControl detailsTabControl;
        private System.Windows.Forms.TabPage knowledgeTab;
        private System.Windows.Forms.WebBrowser knowledgeBrowser;
        private System.Windows.Forms.TabPage mpElementXmlTab;
        private System.Windows.Forms.WebBrowser mpElementXml;
        private System.Windows.Forms.ToolStripMenuItem saveToHTMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToExcelToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

