using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.EnterpriseManagement.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Common;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement.Administration;
using Microsoft.EnterpriseManagement.Configuration.IO;
using Microsoft.EnterpriseManagement.Packaging;
using Microsoft.EnterpriseManagement.Common;


namespace MPViewer
{
    public partial class MPViewer : Form
    {
        ManagementPack          m_managementPack;
        ManagementPackBundle    m_bundle;
        TabPage                 m_alertDescriptionTabPage;
        WebBrowser              m_alertDescriptionTextBox;
        TabPage                 m_ScriptResourceTabPage;
        RichTextBox             m_ScriptResourceTextBox;
        TabPage                 m_QueryResourceTabPage;
        RichTextBox             m_QueryResourceTextBox;
        TabPage                 m_ImageResourceTabPage;
        PictureBox              m_ImageResourcePictureBox;
        DataSet                 m_dataset;
        OpenFileDialog          m_openFileDialog;

        private ProgressDialog  m_progressDialog;
        internal delegate void MPLoadingProgressDelegate(int percentage, string status);
        internal event MPLoadingProgressDelegate MPLoadingProgress;



        static string xslt = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xsl:stylesheet 
	version=""1.0""
	xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
	xmlns:fo=""http://www.w3.org/1999/XSL/Format""
	xmlns:maml=""http://schemas.microsoft.com/maml/2004/10""
>
<xsl:param name=""sectionName""/>
  <xsl:template match=""/"">
  <xsl:choose>
  <xsl:when test=""$sectionName=''"">
<html>
	<head>
		<title>Knowledge Article</title>
		<style>
body {
	margin: 15px 30px 0px 15px;
	}

h1	{
	font-family: Arial, Helvetica, sans-serif;
	font-size: 130%;
	font-weight: normal;
	margin: 12px 0px 0px 0px;
	color: #000000;
	}

h2.subtitle	{
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 95%;
	font-weight: normal;
	margin: 2px 0px 0em 0px;
	padding: 0px;
	}

h2	{
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 95%;
	font-weight: bold;
	margin: 0px 0px 0px 0px;
	padding: 8px 0px 4px 0px;
	}

h3	{
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 80%;
	font-weight: bold;
	margin: 8px 0px 0px 0px;
	padding-bottom: 4px;
	}
	
p	{
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 70%;
	line-height: 140%;
	padding: 0px 0px 1em 0px;
	margin: 0px;
	}
	
a	{
	color: #0033CC;
	}

a:link	{
	color: #0033CC;
	}

a:visited	{
	color: #800080;
	}

a:hover	{
	color: #FF6600;
	}

a:active	{
	color: #800080;
	}	
	
.listBullet
{
	color: #A6A6A6;
	font-size: 120%;
}

.listItem
{
	padding: 0em 0em 0em 0.5em;
}	

.dataTable	{
	border-bottom: solid 1px #CCCCCC;
	}
	
.dataTable td	{
	padding: 5px;
	}

.stdHeader	{
	background: #CCCCCC;
	color: #000000;
	}

.stdHeader td	{
	font-weight: bold;
	border-top: solid 1px #CCCCCC;
	border-left: solid 1px #CCCCCC;
	}
	
.record td	{
	border-top: solid 1px #CCCCCC;
	border-left: solid 1px #CCCCCC;
	}
	
.record td td 
{
	border-width: 0px;
}

.evenRecord	{
	background: #E9E9E6;
	}

.evenRecord td	{
	border-top: solid 1px #CCCCCC;
	border-left: solid 1px #CCCCCC;
	}
	
.evenRecord td td
{
	border-width: 0px;
}

p.lastInCell	{
	padding-bottom: 0px;
	}		
		</style>
	</head>
	<body>
		<xsl:apply-templates/>
	</body>
</html>
  </xsl:when>
    <xsl:otherwise>
      <!-- the html node is added so there are no ns declarations added to the other nodes -->
      <html>
        <!-- locate the section and skip the title node -->
        <xsl:apply-templates select=""//maml:section[maml:title=$sectionName]/child::node()[position()>1]""/>
      </html> 
    </xsl:otherwise>
  </xsl:choose>    
</xsl:template>

<xsl:template match=""maml:title"">
	<h2><xsl:value-of select="".""/></h2>
</xsl:template>

<xsl:template match=""maml:subTitle"">
	<h2 class=""subtitle""><xsl:value-of select="".""/></h2>
</xsl:template>

<xsl:template match=""maml:para"">
<xsl:choose>
	<xsl:when test=""position() = last()"">
		<p class=""lastInCell""><xsl:apply-templates/></p>
	</xsl:when>
	<xsl:otherwise>
		<p><xsl:apply-templates/></p>
	</xsl:otherwise>
</xsl:choose>
</xsl:template>

<!-- List processing -->
<xsl:template match=""maml:list"">
	<table cellspacing=""0"" cellpadding=""0"" border=""0"">
		<xsl:apply-templates/>
	</table>
</xsl:template>

<xsl:template match=""maml:listItem"">
	<tr><td class=""listBullet"" valign=""top"">•</td><td class=""listItem"">
		<xsl:apply-templates/>
	</td></tr>
</xsl:template>
<!-- End of List processing -->

 <!-- Table processing -->
  <xsl:template match=""maml:table"">
    <xsl:apply-templates select=""maml:title"" />
    <table cellspacing=""0"" cellpadding=""0"" class=""dataTable"">
      <xsl:apply-templates  select=""maml:summary"" />
      <xsl:apply-templates  select=""maml:tableHeader"" />
      <xsl:apply-templates  select=""maml:tableFooter"" />
      <tbody>
        <xsl:apply-templates  select=""maml:row"" />
      </tbody>
    </table>
  </xsl:template>
  
  <xsl:template match=""maml:table/maml:title"">
	<h3><xsl:value-of select="".""/></h3>
</xsl:template>

  <xsl:template match=""maml:table/maml:summary"">
    <xsl:attribute name=""summary""><xsl:apply-templates /></xsl:attribute>
  </xsl:template>

  <xsl:template match=""maml:tableHeader"">
    <thead><xsl:apply-templates mode=""header"" select=""maml:row"" /></thead>
  </xsl:template>

  <xsl:template match=""maml:tableFooter"">
    <tfoot><xsl:apply-templates  select=""maml:row"" /></tfoot>
  </xsl:template>

  <xsl:template match=""maml:row"">
  <xsl:choose>					
   <xsl:when test=""position() mod 2 = 1"">
    <tr valign=""top"" class=""record""><xsl:apply-templates/></tr>
    </xsl:when>
    <xsl:otherwise>
    <tr valign=""top"" class=""evenRecord""><xsl:apply-templates/></tr>
    </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match=""maml:row"" mode=""header"">
    <tr  valign=""top"" class=""stdHeader""><xsl:apply-templates/></tr>
  </xsl:template>

  <xsl:template match=""maml:row/maml:headerEntry | maml:row/maml:entry | maml:row/maml:footerEntry"">
    <xsl:if test=""@rowSpan""><xsl:attribute name=""rowspan""><xsl:value-of select=""@rowSpan""/></xsl:attribute></xsl:if>
    <xsl:if test=""@columnSpan""><xsl:attribute name=""colspan""><xsl:value-of select=""@columnSpan""/></xsl:attribute></xsl:if>
    <xsl:choose>
	<xsl:when test=""position() = last()"">
   		 <td style=""border-right: solid 1px #CCCCCC""><xsl:apply-templates /></td>
	</xsl:when>
	<xsl:otherwise>
   		 <td><xsl:apply-templates /></td>
	</xsl:otherwise>
     </xsl:choose>
  </xsl:template>
  <!-- End of Table processing -->

<!-- Link prrocessing -->
<xsl:template match=""maml:navigationLink"">
    <xsl:element name=""a"">
    <xsl:attribute name=""href"">
                <xsl:value-of select=""maml:uri/@href""/>
    </xsl:attribute>
    <xsl:value-of select=""maml:linkText""/>
  </xsl:element>
</xsl:template>

<xsl:template match=""maml:uri"">
</xsl:template>

<xsl:template match=""maml:ui"">
	<b>
		<xsl:value-of select="".""/>
	</b>
</xsl:template>	
	
</xsl:stylesheet>";
        
        //---------------------------------------------------------------------
        public MPViewer()
        {
            InitializeComponent();

            FormClosed += new FormClosedEventHandler(MPViewer_FormClosed);
            Shown += new EventHandler(MPViewer_Shown);
        }

        //---------------------------------------------------------------------
        void MPViewer_Shown(object sender, EventArgs e)
        {
            loadManagementPackToolStripMenuItem_Click(this, null); 
        }

        //---------------------------------------------------------------------
        void MPViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(File.Exists(GetTempFileName()))
            {
                File.Delete(GetTempFileName());
            }
        }

        //---------------------------------------------------------------------
        private void unsealManagementPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Choose a folder where the unsealed/unpacked XML and resources will be dumped. In the case of an MPB, a subdirectory will be created.";

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                if (System.IO.Path.GetExtension(m_openFileDialog.FileName).Equals(".mpb", StringComparison.InvariantCultureIgnoreCase))
                { 
                    // unpack the bundle. since many files might be there, let's create a folder
                    string file = System.IO.Path.GetFileNameWithoutExtension(m_openFileDialog.FileName);
                    string newfolder = dlg.SelectedPath + "\\" + file;
                    try
                    {
                        System.IO.Directory.CreateDirectory(newfolder);

                        ManagementPackXmlWriter mpWriter = new Microsoft.EnterpriseManagement.Configuration.IO.ManagementPackXmlWriter(newfolder);

                        // if a bundle, have multiple of those mps in the bundle nd want to unpack them all
                        foreach (ManagementPack mp in m_bundle.ManagementPacks) 
                        {
                            mpWriter.WriteManagementPack(mp);
                        }


                        // get a dictionary of ALL the streams from ALL the MPs and dump the streams for each MP
                        foreach (ManagementPack mp in m_bundle.ManagementPacks)
                        {
                            IDictionary<string, Stream> streams = m_bundle.GetStreams(mp);

                            foreach (KeyValuePair<string, Stream> stream in streams)
                            {
                                // safest assumption is that we already have the file name as stream.Key
                                string streamFileName = newfolder + "\\" + stream.Key;

                                foreach (ManagementPackResource resource in mp.GetResources<ManagementPackResource>())
                                {
                                    ManagementPackAssemblyResource assemblyResource = resource as ManagementPackAssemblyResource;
                                    if (assemblyResource != null &&
                                        assemblyResource.HasNullStream)
                                    {
                                        continue;
                                    }

                                    if (resource.Name.Equals(stream.Key, StringComparison.CurrentCulture))
                                    {
                                        if (!String.IsNullOrEmpty(resource.FileName))
                                        {
                                            // this filename might be preferred, if it exists
                                            streamFileName = newfolder + "\\" + resource.FileName;
                                        }
                                    }
                                }

                                // make sure we have the path (sometimes resources have nested paths like /Silverlight/ or other ones)
                                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(streamFileName));


                                // buffer stuff to copy from one stream to another
                                int length = 256;
                                int bytesRead = 0;
                                Byte[] buffer = new Byte[length];

                                // write the required bytes
                                using (FileStream fs = new FileStream(streamFileName, FileMode.Create))
                                {
                                    do
                                    {
                                        bytesRead = stream.Value.Read(buffer, 0, length);
                                        fs.Write(buffer, 0, bytesRead);
                                    }
                                    while (bytesRead == length);

                                    fs.Close();
                                }
                            }
                        }
                        MessageBox.Show(String.Format("Done extracting bundle to {0}", newfolder));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Dumping MP bundle!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (System.IO.Path.GetExtension(m_openFileDialog.FileName).Equals(".mp", StringComparison.InvariantCultureIgnoreCase))
                {
                    // no need to reopen the file, since we have the MP already
                    ManagementPackXmlWriter mpWriter = new Microsoft.EnterpriseManagement.Configuration.IO.ManagementPackXmlWriter(dlg.SelectedPath);
                    mpWriter.WriteManagementPack(m_managementPack);

                    MessageBox.Show("Unsealing done!");
                }
            }
            return;
        }

        //---------------------------------------------------------------------
        private void loadManagementPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_openFileDialog = new OpenFileDialog();

            m_openFileDialog.AddExtension = true;
            m_openFileDialog.CheckPathExists = true;
            m_openFileDialog.DefaultExt = "mp";
            m_openFileDialog.Filter = "Sealed MP files (*.mp)|*.mp|Sealed MP bundles (*.mpb)|*.mpb|Unsealed MP files (*.xml)|*.xml";

            m_openFileDialog.InitialDirectory = (string)Application.UserAppDataRegistry.GetValue("MPFolder", (object)"C:\\");

            if (m_openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            ManagementPackFileStore store = Utilities.GetManagementPackStoreFromPath(m_openFileDialog.FileName);


            if (System.IO.Path.GetExtension(m_openFileDialog.FileName).Equals(".mpb", StringComparison.InvariantCultureIgnoreCase))
            {
                ManagementPackBundleReader reader = ManagementPackBundleFactory.CreateBundleReader();
                m_bundle = reader.Read(m_openFileDialog.FileName, store);

                // 1 at the time is ok
                if (m_bundle.ManagementPacks.Count == 1)
                {
                    m_managementPack = m_bundle.ManagementPacks[0];
                }
                else
                {
                    // multiple MPs contained in this MPB! - can only open one at the time! Need to ask the user which one.
                    MultipleMPSelectionForm MPListForm = new MultipleMPSelectionForm(m_bundle.ManagementPacks);
                    MPListForm.ShowDialog();

                    //selected in the form
                    m_managementPack = MPListForm.ChosenMP;
                }
            }
            else // we are dealing with an MP or XML - the old stuff works as it did for 2007
            {
                m_managementPack = new ManagementPack(m_openFileDialog.FileName, store);
            }


            ClearViews();

            PopulateObjectTypeTree();


            Application.UserAppDataRegistry.SetValue("MPFolder",
                                                        Path.GetDirectoryName(m_openFileDialog.FileName));

            if (m_managementPack.KeyToken != null)
            {
                Text = string.Format("Management Pack Viewer 2012 - {0} {1} {2}",
                                        Utilities.GetBestManagementPackName(m_managementPack),
                                        m_managementPack.Version.ToString(),
                                        m_managementPack.KeyToken.ToString());
            }
            else
            {     
                //TODO - Unsealed don't have keytoken!
                Text = string.Format("Management Pack Viewer 2012 - {0} {1} Unsealed",
                                        Utilities.GetBestManagementPackName(m_managementPack),
                                        m_managementPack.Version.ToString());
            }


            //loading dataset is memory expensive and can take time... so moved to a worker thread
            this.m_progressDialog = new ProgressDialog();
            this.m_progressDialog.Show(this);
            this.backgroundWorker.RunWorkerAsync();



            Cursor = Cursors.Default;
            
            // now that an MP is loaded, enable the menus
            saveToHTMLToolStripMenuItem.Enabled = true;
            saveToExcelToolStripMenuItem.Enabled = true;

            //this might make sense or not, if we have loaded XML...
            if (System.IO.Path.GetExtension(m_openFileDialog.FileName).Equals(".mpb", StringComparison.InvariantCultureIgnoreCase) ||
                System.IO.Path.GetExtension(m_openFileDialog.FileName).Equals(".mp", StringComparison.InvariantCultureIgnoreCase))
            {
                unsealManagementPackToolStripMenuItem.Enabled = true;
            }
        }

        //---------------------------------------------------------------------
        private void PopulateObjectTypeTree()
        {
            objectTypeTree.Nodes.Add("Monitors - Aggregate");
            objectTypeTree.Nodes.Add("Monitors - Unit");
            objectTypeTree.Nodes.Add("Monitors - Dependency");
            objectTypeTree.Nodes.Add("Rules");
            objectTypeTree.Nodes.Add("Views");
            objectTypeTree.Nodes.Add("Tasks");
            objectTypeTree.Nodes.Add("Console Tasks");
            objectTypeTree.Nodes.Add("Reports");
            objectTypeTree.Nodes.Add("Linked Reports");
            objectTypeTree.Nodes.Add("Discoveries");
            objectTypeTree.Nodes.Add("Classes");
            objectTypeTree.Nodes.Add("Relationships");
            objectTypeTree.Nodes.Add("Diagnostics");
            objectTypeTree.Nodes.Add("Recoveries");
            objectTypeTree.Nodes.Add("Dependencies");
            objectTypeTree.Nodes.Add("Overrides");
            objectTypeTree.Nodes.Add("Groups");
            objectTypeTree.Nodes.Add("Resources");
            objectTypeTree.Nodes.Add("Dashboards and Widgets");

            objectTypeTree.Sort();
        }

        //---------------------------------------------------------------------
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //---------------------------------------------------------------------
        private void MPViewer_Load(object sender, EventArgs e)
        {
            ClearDetailsViews();      
            
            // initially, these two menu actions don't make sense - we'll re-enable them once an MP is loaded
            unsealManagementPackToolStripMenuItem.Enabled = false;
            saveToHTMLToolStripMenuItem.Enabled = false;
            saveToExcelToolStripMenuItem.Enabled = false;
        }

        //---------------------------------------------------------------------
        private void objectTypeTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            ClearDetailsViews();

            mpElementListView.BeginUpdate();

            
            if (e.Node.Text == "Monitors - Aggregate" || e.Node.Text == "Monitors - Unit" || e.Node.Text == "Monitors - Dependency")
            {
                ClearDetailsViews();
                PopulateListView(e.Node.Text);
                CreateAdditionalAlertTab();
            }
            else if (e.Node.Text == "Resources")
            {
                ClearDetailsViews();
                PopulateListView(e.Node.Text);
                CreateAdditionalScriptResourceTab();
                CreateAdditionalImageResourceTab();
            }
            else if (e.Node.Text == "Rules")
            {
                ClearDetailsViews();
                PopulateListView(e.Node.Text);
                CreateAdditionalQueryResourceTab();
            }
            else
            {
                ClearDetailsViews();
                PopulateListView(e.Node.Text);
            }

            mpElementListView.EndUpdate();

            mpElementListView.AdjustColumnSizes();

            mpElementListView.BeginUpdate();
            
            mpElementListView.SortByColumn(0);

            mpElementListView.EndUpdate();

            Cursor = Cursors.Default;
        }

        //---------------------------------------------------------------------
        private void PopulateListView(string contentsType)
        {
            DataTable table = m_dataset.Tables[contentsType];

            mpElementListView.Items.Clear();
            mpElementListView.Columns.Clear();


            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName != "ObjectRef")
                {
                    mpElementListView.Columns.Add(column.ColumnName);
                }
            }

            foreach (DataRow row in table.Rows)
            {
                ListViewItem item = new ListViewItem();

                item.Text = row[0].ToString();

                for (int i = 1; i < table.Columns.Count - 1; i++)
                {
                    string value = row[i].ToString();
                                                     
                    item.SubItems.Add(value);
                }

                PopulateObjectReference(item, row["ObjectRef"].ToString(), contentsType);

                mpElementListView.Items.Add(item);
            }
        }

        //---------------------------------------------------------------------
        private void PopulateObjectReference(
            ListViewItem        item,
            string              objectName,
            string              objectType
            )
        {
            if (objectType == "Monitors - Aggregate" || objectType == "Monitors - Unit" || objectType == "Monitors - Dependency")
            {
                item.Tag = m_managementPack.GetMonitor(objectName);
            }
            else if (objectType == "Rules")
            {
                item.Tag = m_managementPack.GetRule(objectName);
            }
            else if (objectType == "Views")
            {
                item.Tag = m_managementPack.GetView(objectName);
            }
            else if (objectType == "Discoveries")
            {
                item.Tag = m_managementPack.GetDiscovery(objectName);
            }
            else if (objectType == "Reports")
            {
                item.Tag = m_managementPack.GetReport(objectName);
            }
            else if (objectType == "Classes")
            {
                item.Tag = m_managementPack.GetClass(objectName);
            }
            else if (objectType == "Relationships")
            {
                item.Tag = m_managementPack.GetRelationship(objectName);
            }
            else if (objectType == "Tasks")
            {
                item.Tag = m_managementPack.GetTask(objectName);
            }
            else if (objectType == "Console Tasks")
            {
                item.Tag = m_managementPack.GetConsoleTask(objectName);
            }
            else if (objectType == "Linked Reports")
            {
                item.Tag = m_managementPack.GetLinkedReport(objectName);
            }
            else if (objectType == "Dependencies")
            {
                item.Tag = m_managementPack.References[objectName];
            }
            else if (objectType == "Recoveries")
            {
                item.Tag = m_managementPack.GetRecovery(objectName);
            }
            else if (objectType == "Diagnostics")
            {
                item.Tag = m_managementPack.GetDiagnostic(objectName);
            }
            else if (objectType == "Overrides")
            {
                item.Tag = m_managementPack.GetOverride(objectName);
            }
            else if (objectType == "Groups")
            {
                item.Tag = m_managementPack.GetClass(objectName);
            }
            else if (objectType == "Resources")
            {
                item.Tag = m_managementPack.GetResource<ManagementPackResource>(objectName);
            }
            else if (objectType == "Dashboards and Widgets")
            {
                item.Tag = m_managementPack.GetComponentType(objectName);
            }
        }

        //---------------------------------------------------------------------
        private void CreateAdditionalAlertTab()
        {
            m_alertDescriptionTabPage           = new TabPage("Alert Description");
            m_alertDescriptionTextBox           = new WebBrowser();
            m_alertDescriptionTextBox.Dock      = DockStyle.Fill;

            detailsTabControl.TabPages.Add(m_alertDescriptionTabPage);

            m_alertDescriptionTabPage.Controls.Add(m_alertDescriptionTextBox);
        }


        //---------------------------------------------------------------------
        private void CreateAdditionalImageResourceTab()
        {
            m_ImageResourceTabPage = new TabPage("Image");
            m_ImageResourcePictureBox = new PictureBox();
            m_ImageResourcePictureBox.Dock = DockStyle.Fill;

            detailsTabControl.TabPages.Add(m_ImageResourceTabPage);

            m_ImageResourceTabPage.Controls.Add(m_ImageResourcePictureBox);
        }

        //---------------------------------------------------------------------
        private void CreateAdditionalScriptResourceTab()
        {
            m_ScriptResourceTabPage = new TabPage("Script");
            m_ScriptResourceTextBox = new RichTextBox();
            m_ScriptResourceTextBox.Dock = DockStyle.Fill;

            detailsTabControl.TabPages.Add(m_ScriptResourceTabPage);

            m_ScriptResourceTabPage.Controls.Add(m_ScriptResourceTextBox);
        }



        //---------------------------------------------------------------------
        private void CreateAdditionalQueryResourceTab()
        {
            m_QueryResourceTabPage = new TabPage("Query");
            m_QueryResourceTextBox = new RichTextBox();
            m_QueryResourceTextBox.Dock = DockStyle.Fill;

            detailsTabControl.TabPages.Add(m_QueryResourceTabPage);

            m_QueryResourceTabPage.Controls.Add(m_QueryResourceTextBox);
        }


        //---------------------------------------------------------------------
        private void ShowAdditionalTab(bool show)
        {
            if (show)
            {
                if (!detailsTabControl.TabPages.Contains(m_alertDescriptionTabPage))
                {
                    detailsTabControl.TabPages.Add(m_alertDescriptionTabPage);
                }
                else if (!detailsTabControl.TabPages.Contains(m_ScriptResourceTabPage))
                {
                    detailsTabControl.TabPages.Add(m_ScriptResourceTabPage);
                }
                else if (!detailsTabControl.TabPages.Contains(m_ImageResourceTabPage))
                {
                    detailsTabControl.TabPages.Add(m_ImageResourceTabPage);
                }
            }
            else
            {
                if (m_alertDescriptionTabPage != null && detailsTabControl.TabPages.Contains(m_alertDescriptionTabPage))
                {
                    detailsTabControl.TabPages.Remove(m_alertDescriptionTabPage);
                }
                else if (m_ScriptResourceTabPage != null && detailsTabControl.TabPages.Contains(m_ScriptResourceTabPage))
                {
                    detailsTabControl.TabPages.Remove(m_ScriptResourceTabPage);
                }
                else if (m_ImageResourceTabPage != null && detailsTabControl.TabPages.Contains(m_ImageResourceTabPage))
                {
                    detailsTabControl.TabPages.Remove(m_ImageResourceTabPage);
                }
            }
        }


        //---------------------------------------------------------------------
        private void mpElementListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mpElementListView.SelectedItems.Count != 1)
            {
                return;
            }

            if (mpElementListView.SelectedItems[0].Tag is ManagementPackReference)
            {
                return;
            }

            ManagementPackElement element;
            
            element = (ManagementPackElement)mpElementListView.SelectedItems[0].Tag;


            DisplayRawXml(element);

            DisplayKnowledgeArticle(element);


            //Additional Tabs
            DisplayAlertDescriptionIfApplicable(element);
            DisplayAdvisorQueryIfApplicable(element);
            DisplayScriptIfApplicable(element);
            DisplayImageIfApplicable(element);

        }



        //---------------------------------------------------------------------
        private void DisplayImageIfApplicable(ManagementPackElement element)
        {
            if (!(element is ManagementPackResource))
            {
                return;
            }

            ManagementPackResource resource = (ManagementPackResource)element;


            if (resource.FileName != null)
            {
                if (resource.XmlTag.Equals("Image", StringComparison.InvariantCultureIgnoreCase))
                {
                    IDictionary<string, Stream> streams = m_bundle.GetStreams(m_managementPack);
                    foreach (var stream in streams)
                    {
                        if (stream.Key.Equals(resource.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            Image pic = Image.FromStream(stream.Value);
                            m_ImageResourcePictureBox.BackColor = Color.White;
                            m_ImageResourcePictureBox.Image = pic;
                            m_ImageResourcePictureBox.Visible = true;
                        }
                    }
                }
                else 
                {
                    m_ImageResourcePictureBox.Visible = false;
                }
            }
        }



        //---------------------------------------------------------------------
        private void DisplayAdvisorQueryIfApplicable(ManagementPackElement element)
        {
            if (!(element is ManagementPackRule))
            {
                return;
            }

            ManagementPackRule rule = (ManagementPackRule)element;


            DataTable t = m_dataset.Tables["Rules"];



            m_QueryResourceTextBox.Text = String.Empty;

        }



        //---------------------------------------------------------------------
        private void DisplayScriptIfApplicable(ManagementPackElement element)
        {
            if (!(element is ManagementPackResource))
            {
                return;
            }

            ManagementPackResource resource = (ManagementPackResource)element;

            m_ScriptResourceTextBox.Text = String.Empty;


            if (resource.FileName != null)
            {
                // works for now... maybe with "config" files (or "deployableresource"s) we might want to show in browser control since it is XML...
                if (resource.FileName.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase) || 
                    resource.FileName.EndsWith(".ps1", StringComparison.InvariantCultureIgnoreCase) ||
                    resource.FileName.EndsWith(".config", StringComparison.InvariantCultureIgnoreCase))
                {
                    string ScriptBody = String.Empty;

                    IDictionary<string, Stream> streams = m_bundle.GetStreams(m_managementPack);
                    foreach (var stream in streams)
                    {
                        if (stream.Key.Equals(resource.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            StreamReader reader = new StreamReader(stream.Value);
                            ScriptBody = reader.ReadToEnd();
                        }
                    }


                    if (String.IsNullOrEmpty(ScriptBody))
                    {
                        return;
                    }

                    m_ScriptResourceTextBox.Text = ScriptBody;
                }
            }
        }





        //---------------------------------------------------------------------
        private void DisplayAlertDescriptionIfApplicable(ManagementPackElement element)
        {
            if (!(element is ManagementPackMonitor))
            {
                return;
            }

            ManagementPackMonitor monitor = (ManagementPackMonitor)element;
            
            m_alertDescriptionTextBox.DocumentText = @"<HTML></HTML>";
            
            if (monitor.AlertSettings != null && monitor.AlertSettings.AlertMessage != null)
            {
                string alertDescription;

                alertDescription = m_managementPack.GetStringResource(monitor.AlertSettings.AlertMessage.Name).Description;

                if (alertDescription == null)
                {
                    return;
                }

                if (alertDescription.IndexOf("\r\n") == 0)
                {
                    alertDescription = alertDescription.Remove(0, 2);
                    alertDescription = alertDescription.TrimStart(new char[] { ' ' });
                }

                m_alertDescriptionTextBox.DocumentText = string.Format(@"<HTML><BODY>{0}</BODY></HTML>",alertDescription);
            }
        }

        //---------------------------------------------------------------------
        private void DisplayRawXml(ManagementPackElement element)
        {
            StringWriter    stringWriter    = new StringWriter();
            XmlTextWriter   xmlTextWriter   = new System.Xml.XmlTextWriter(stringWriter);
            string          tempFileName    = GetTempFileName();
            StreamWriter    writer          = File.CreateText(tempFileName);
            
            mpElementXml.DocumentText = string.Empty;

            element.WriteXml(xmlTextWriter);            
            writer.Write(stringWriter.ToString());

            writer.Flush();
            writer.Close();

            mpElementXml.Url = new Uri(tempFileName);
        }

        //---------------------------------------------------------------------
        private string GetTempFileName()
        {
            string tempFileName = Application.ExecutablePath;

            tempFileName = Path.GetDirectoryName(tempFileName);

            tempFileName += "\\mpelementdefinition.xml";

            return (tempFileName);
        }

        //---------------------------------------------------------------------
        private void DisplayKnowledgeArticle(ManagementPackElement element)
        {
            knowledgeBrowser.DocumentText = string.Empty;

            ManagementPackKnowledgeArticle article = element.GetKnowledgeArticle("ENU");

            if (article == null)
            {
                return;
            }

            if (article.HtmlContent != null && article.HtmlContent.Length > 0)
            {
                knowledgeBrowser.DocumentText = article.HtmlContent;
            }
            else if (article.MamlContent != null && article.MamlContent.Length > 0)
            {
                DisplayMamlKnowledgeArticle(article);
            }
        }

        //---------------------------------------------------------------------
        private void DisplayMamlKnowledgeArticle(ManagementPackKnowledgeArticle article)
        {
            try
            {
                NameTable               nt                  = new NameTable();
                XmlNamespaceManager     nsmgr               = new XmlNamespaceManager(nt);
                XslCompiledTransform    transform           = new XslCompiledTransform();
                XmlDocument             xsltDocument        = new XmlDocument();
                XmlReaderSettings       settings            = new XmlReaderSettings();
                TextWriter              articleTextWriter   = new StringWriter();

                nsmgr.AddNamespace("maml", "http://schemas.microsoft.com/maml/2004/10");

                XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
                
                settings.ConformanceLevel = ConformanceLevel.Fragment;

                XmlReader reader = XmlReader.Create(new StringReader(article.MamlContent), settings, context);

                xsltDocument.LoadXml(xslt);
                                
                XmlTextWriter articleWriter = new XmlTextWriter(articleTextWriter);

                transform.Load(xsltDocument);

                transform.Transform(reader, articleWriter);

                knowledgeBrowser.DocumentText = articleTextWriter.ToString();
            }
            catch (Exception)
            {
            }
        }

        //---------------------------------------------------------------------
        private void ClearViews()
        {
            ClearDetailsViews();
            ClearTreeView();            
        }

        //---------------------------------------------------------------------
        private void ClearDetailsViews()
        {
            mpElementListView.Items.Clear();
            mpElementListView.Columns.Clear();

            knowledgeBrowser.DocumentText   = string.Empty;
            mpElementXml.DocumentText       = string.Empty;

            ShowAdditionalTab(false);
        }

        //---------------------------------------------------------------------
        private void ClearTreeView()
        {
            objectTypeTree.Nodes.Clear();
        }




        // -------------------------------------------------------------------------------------

        public void MPViewer_MPLoadingProgress(int percentage, string status)
        {
            backgroundWorker.ReportProgress(percentage, status);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadData();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            m_progressDialog.UpdateProgress(new ProgressInfo(e.UserState.ToString(), e.ProgressPercentage));
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_progressDialog.Close();
        
        }


        private void LoadData()
        {
            MPLoadingProgress += new MPViewer.MPLoadingProgressDelegate(MPViewer_MPLoadingProgress);

            //load the dataset in this thread
            m_dataset = new DatasetCreator(m_managementPack, MPLoadingProgress).Dataset;
        }









        //---------------------------------------------------------------------
        private void saveToHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.AddExtension    = true;
            dlg.CheckPathExists = true;
            dlg.DefaultExt      = "html";
            dlg.Filter          = "HTML files (*.html)|*.html";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ReportGenerator reportGenerator = new ReportGenerator(m_dataset,m_managementPack);

                reportGenerator.GenerateHTMLReport(dlg.FileName,false);
            }
        }


        //---------------------------------------------------------------------
        private void saveToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            dlg.DefaultExt = "xml";
            dlg.Filter = "Excel XML files (*.xml)|*.xml";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportToExcel(dlg.FileName);
            }
        }
        
        //this version to be used for GUI export - there is another version of this for batch export in ReportGenerator.cs
        private void ExportToExcel(string filename)
        {
            ExcelWriter writer = new ExcelWriter();

            foreach (DataTable table in m_dataset.Tables)
            {
                writer.WriteDataTable(table);
            }

            writer.SaveToFile(filename);
        }
        
    }
}