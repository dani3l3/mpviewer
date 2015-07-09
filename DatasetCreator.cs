using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EnterpriseManagement.Configuration;
using System.Data;
using Common;
using System.Xml;
using System.Diagnostics;
using Microsoft.EnterpriseManagement.Configuration.IO;
using MPViewer;

namespace MPViewer
{
    class DatasetCreator
    {
        DataSet                 m_dataset;
        ManagementPack          m_managementPack;
        Delegate                m_mpLoadingProcess;

        internal delegate void StubDelegate();

        //---------------------------------------------------------------------
        // this overload is only used by the HTML batch export, which is horribly implemented anyways...
        internal DatasetCreator(
            ManagementPack managementPack
            )
        {
            m_dataset = new DataSet();
            m_managementPack = managementPack;

            CreateUnitMonitorsTable();
            CreateDependencyMonitorsTable();
            CreateAggregateMonitorsTable();
            CreateRulesTable();
            CreateDiscoveriesTable();
            CreateReferencesTable();
            CreateRelationshipsTable();
            CreateClassesTable();
            CreateRecoveriesTable();
            CreateDiagnosticsTable();
            CreateOverridesTable();
            CreateGroupsTable();
            CreateViewsTable();
            CreateConsoleTasksTable();
            CreateResourcesTable();
            CreateDashboardsTable();
            CreateGenericTable(m_managementPack.GetTasks(), "Tasks", true, true);
            CreateGenericTable(m_managementPack.GetLinkedReports(), "Linked Reports", false, true);
            CreateGenericTable(m_managementPack.GetReports(), "Reports", false, true);
        }
        


        //this overload is the one used by the UI
        internal DatasetCreator(
            ManagementPack managementPack, Delegate MPLoadingProcess
            )
        {
            m_dataset           = new DataSet();            
            m_managementPack    = managementPack;
            m_mpLoadingProcess  = MPLoadingProcess;


            m_mpLoadingProcess.DynamicInvoke(5, "Loading Unit Monitors");
            CreateUnitMonitorsTable();

            m_mpLoadingProcess.DynamicInvoke(10, "Loading Dependency Monitors");
            CreateDependencyMonitorsTable();
            
            m_mpLoadingProcess.DynamicInvoke(15, "Loading Aggregate Monitors");
            CreateAggregateMonitorsTable();
            
            m_mpLoadingProcess.DynamicInvoke(20, "Loading Rules");
            CreateRulesTable();
            
            m_mpLoadingProcess.DynamicInvoke(25, "Loading Discoveries");
            CreateDiscoveriesTable();
            
            m_mpLoadingProcess.DynamicInvoke(30, "Loading Dependencies");
            CreateReferencesTable();
            
            m_mpLoadingProcess.DynamicInvoke(35, "Loading Relationships");
            CreateRelationshipsTable();
            
            m_mpLoadingProcess.DynamicInvoke(40, "Loading Classes");
            CreateClassesTable();
            
            m_mpLoadingProcess.DynamicInvoke(45, "Loading Recoveries");
            CreateRecoveriesTable();
            
            m_mpLoadingProcess.DynamicInvoke(50, "Loading Diagnostics");
            CreateDiagnosticsTable();
            
            m_mpLoadingProcess.DynamicInvoke(55, "Loading Overrides");
            CreateOverridesTable();
            
            m_mpLoadingProcess.DynamicInvoke(60, "Loading Groups");
            CreateGroupsTable();
            
            m_mpLoadingProcess.DynamicInvoke(65, "Loading Views");
            CreateViewsTable();
            
            m_mpLoadingProcess.DynamicInvoke(70, "Loading Console Tasks");
            CreateConsoleTasksTable();
            
            m_mpLoadingProcess.DynamicInvoke(75, "Loading Resources");
            CreateResourcesTable();
            
            m_mpLoadingProcess.DynamicInvoke(80, "Loading Dashboards");
            CreateDashboardsTable();
            
            m_mpLoadingProcess.DynamicInvoke(85, "Loading Agent Tasks");
            CreateGenericTable(m_managementPack.GetTasks(), "Tasks", true, true);
            
            m_mpLoadingProcess.DynamicInvoke(90, "Loading Linked Reports");
            CreateGenericTable(m_managementPack.GetLinkedReports(), "Linked Reports", false, true);
            
            m_mpLoadingProcess.DynamicInvoke(95, "Loading Reports");
            CreateGenericTable(m_managementPack.GetReports(), "Reports", false, true);
            
            m_mpLoadingProcess.DynamicInvoke(100, "Done!");
        }

        //---------------------------------------------------------------------
        private void CreateAggregateMonitorsTable()
        {
            DataTable table = new DataTable("Monitors - Aggregate");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Algorithm", Type.GetType("System.String"));
            table.Columns.Add("Category", Type.GetType("System.String"));
            table.Columns.Add("Enabled", Type.GetType("System.Boolean"));
            table.Columns.Add("Generate Alert", Type.GetType("System.Boolean"));
            table.Columns.Add("Alert Severity", Type.GetType("System.String"));
            table.Columns.Add("Alert Priority", Type.GetType("System.String"));
            table.Columns.Add("Auto Resolve", Type.GetType("System.String"));
            table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackMonitor monitor in m_managementPack.GetMonitors())
            {
                DataRow row = table.NewRow();

                if (!(monitor is ManagementPackAggregateMonitor))
                {
                    continue;
                }

                ManagementPackAggregateMonitor aggregateMonitor = (ManagementPackAggregateMonitor)monitor;

                PopulateGenericMonitorProperties(monitor, row);

                row["Algorithm"] = ((ManagementPackAggregateMonitor)monitor).Algorithm.ToString();                             

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void PopulateGenericMonitorProperties(
            ManagementPackMonitor       monitor,
            DataRow                     row
            )
        {
            row["Name"]     = Utilities.GetBestMPElementName(monitor);
            row["Category"] = monitor.Category.ToString();
            row["Enabled"]  = (monitor.Enabled != ManagementPackMonitoringLevel.@false);
            row["Target"]   = AttempToResolveName(monitor.Target);

            if (monitor.AlertSettings == null)
            {
                row["Generate Alert"]   = false;
                row["Alert Severity"]   = string.Empty;
                row["Auto Resolve"]     = string.Empty;
            }
            else
            {
                row["Generate Alert"]   = true;
                row["Alert Severity"]   = monitor.AlertSettings.AlertSeverity.ToString();
                row["Alert Priority"]   = monitor.AlertSettings.AlertPriority.ToString();
                row["Auto Resolve"]     = monitor.AlertSettings.AutoResolve.ToString();
            }

            row["Remotable"]        = monitor.Remotable;
            row["Description"]      = monitor.Description;
            row["ObjectRef"]        = monitor.Name;
            row["Accessibility"]    = monitor.Accessibility.ToString();
        }

        //---------------------------------------------------------------------
        private void CreateDependencyMonitorsTable()
        {
            DataTable table = new DataTable("Monitors - Dependency");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Algorithm", Type.GetType("System.String"));
            table.Columns.Add("Algorithm Parameter", Type.GetType("System.String"));
            table.Columns.Add("Source Monitor", Type.GetType("System.String"));
            table.Columns.Add("Relationship", Type.GetType("System.String"));
            table.Columns.Add("Category", Type.GetType("System.String"));
            table.Columns.Add("Enabled", Type.GetType("System.Boolean"));
            table.Columns.Add("Generate Alert", Type.GetType("System.Boolean"));
            table.Columns.Add("Alert Severity", Type.GetType("System.String"));
            table.Columns.Add("Alert Priority", Type.GetType("System.String"));
            table.Columns.Add("Auto Resolve", Type.GetType("System.String"));
            table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackMonitor monitor in m_managementPack.GetMonitors())
            {
                DataRow row = table.NewRow();

                if (!(monitor is ManagementPackDependencyMonitor))
                {
                    continue;
                }

                PopulateGenericMonitorProperties(monitor, row);

                row["Algorithm"]            = ((ManagementPackDependencyMonitor)monitor).Algorithm.ToString();
                row["Algorithm Parameter"]  = ((ManagementPackDependencyMonitor)monitor).AlgorithmParameter;
                row["Source Monitor"]       = AttempToResolveName(((ManagementPackDependencyMonitor)monitor).MemberMonitor);
                row["Relationship"]         = AttempToResolveName(((ManagementPackDependencyMonitor)monitor).RelationshipType);

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void CreateConsoleTasksTable()
        {
            DataTable table = new DataTable("Console Tasks");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Application", Type.GetType("System.String"));
            table.Columns.Add("WorkingDirectory", Type.GetType("System.String"));
            table.Columns.Add("Require Output", Type.GetType("System.Boolean"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackConsoleTask task in m_managementPack.GetConsoleTasks())
            {
                DataRow row = table.NewRow();

                row["Name"]             = Common.Utilities.GetBestMPElementName(task);
                row["Target"]           = AttempToResolveName(task.Target);
                row["Accessibility"]    = task.Accessibility;
                row["Application"]      = task.Application;
                row["WorkingDirectory"] = task.WorkingDirectory;
                row["Require Output"]   = task.RequireOutput;
                row["Description"]      = task.Description;
                row["ObjectRef"]        = task.Name;

                table.Rows.Add(row);
            }
        }


        //---------------------------------------------------------------------
        private void CreateResourcesTable()
        {
            DataTable table = new DataTable("Resources");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("FileName", Type.GetType("System.String"));
            table.Columns.Add("ResourceType", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackResource resource in m_managementPack.GetResources<ManagementPackResource>())
            {
                ManagementPackAssemblyResource assemblyResource = resource as ManagementPackAssemblyResource;
                if (assemblyResource != null &&
                    assemblyResource.HasNullStream)
                {
                    continue;
                }
          
                DataRow row = table.NewRow();

                row["Name"] = resource.Name;
                row["FileName"] = resource.FileName;
                row["ResourceType"] = resource.XmlTag;

                row["ObjectRef"] = resource.Name;

                table.Rows.Add(row);     
            }
        }

        //---------------------------------------------------------------------
        private void CreateDashboardsTable()
        {
            DataTable table = new DataTable("Dashboards and Widgets");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            //table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackComponentType componentType in m_managementPack.GetComponentTypes())
            {

                DataRow row = table.NewRow();

                row["Name"] = Common.Utilities.GetBestMPElementName(componentType);
                row["Accessibility"] = componentType.Accessibility;
                //row["Target"] = componentType.Target; // is this string here good enough? it uses the mpelement:// syntax...
                row["Description"] = componentType.Description;
                row["ObjectRef"] = componentType.Name;

                table.Rows.Add(row);
            }
        }

        
        //---------------------------------------------------------------------
        private void CreateViewsTable()
        {
            DataTable table = new DataTable("Views");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Type", Type.GetType("System.String"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            table.Columns.Add("Visible", Type.GetType("System.Boolean"));                        
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackView view in m_managementPack.GetViews())
            {
                DataRow row = table.NewRow();

                row["Name"]             = Common.Utilities.GetBestMPElementName(view);
                row["Target"]           = AttempToResolveName(view.Target);
                row["Accessibility"]    = view.Accessibility;
                row["Visible"]          = view.Visible;
                row["Description"]      = view.Description;

                if (view.TypeID.Name == "Microsoft.SystemCenter.AlertViewType")
                {
                    row["Type"] = "Alert View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.DashboardViewType")
                {
                    row["Type"] = "Dashboard View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.DiagramViewType")
                {
                    row["Type"] = "Diagram View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.EventViewType")
                {
                    row["Type"] = "Event View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.InventoryViewType")
                {
                    row["Type"] = "Inventory View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.PerformanceViewType")
                {
                    row["Type"] = "Performance View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.StateViewType")
                {
                    row["Type"] = "State View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.TaskStatusViewType")
                {
                    row["Type"] = "Task View";
                }
                else if (view.TypeID.Name == "Microsoft.SystemCenter.UrlViewType")
                {
                    row["Type"] = "Url View";
                }

                row["ObjectRef"]        = view.Name;

                table.Rows.Add(row);                
            }
        }

        //---------------------------------------------------------------------
        private void CreateGroupsTable()
        {
            DataTable table = new DataTable("Groups");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));            
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackClass mpClass in m_managementPack.GetClasses())
            {
                DataRow row = table.NewRow();

                if(mpClass.Base.Name == "System.Group" ||
                   mpClass.Base.Name == "Microsoft.SystemCenter.InstanceGroup" ||
                   mpClass.Base.Name == "Microsoft.SystemCenter.ComputerGroup")                   
                {
                    row["Name"]             = Common.Utilities.GetBestMPElementName(mpClass);
                    row["Description"]      = mpClass.Description;
                    row["Accessibility"]    = mpClass.Accessibility;
                    row["ObjectRef"]        = mpClass.Name;

                    table.Rows.Add(row);
                }
            }
        }

        //---------------------------------------------------------------------
        private void CreateOverridesTable()
        {
            DataTable table = new DataTable("Overrides");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Workflow", Type.GetType("System.String"));
            table.Columns.Add("Workflow Type", Type.GetType("System.String"));
            table.Columns.Add("Property", Type.GetType("System.String"));
            table.Columns.Add("Value", Type.GetType("System.String"));
            table.Columns.Add("Is Enforced", Type.GetType("System.Boolean"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackOverride mpOverride in m_managementPack.GetOverrides())
            {
                DataRow row = table.NewRow();

                row["Name"]         = mpOverride.Name;
                row["Value"]        = mpOverride.Value;
                row["Is Enforced"]  = mpOverride.Enforced;
                row["Name"]         = AttempToResolveName(mpOverride.Context);

                if (mpOverride is ManagementPackDiscoveryOverride)
                {
                    row["Workflow Type"] = "Discovery";
                    row["Workflow"] = AttempToResolveName(((ManagementPackDiscoveryOverride)mpOverride).Discovery);

                    if (mpOverride is ManagementPackDiscoveryConfigurationOverride)
                    {
                        row["Property"] = ((ManagementPackDiscoveryConfigurationOverride)mpOverride).Parameter;
                    }
                    else if (mpOverride is ManagementPackDiscoveryPropertyOverride)
                    {
                        row["Property"] = ((ManagementPackDiscoveryPropertyOverride)mpOverride).Property.ToString();
                    }
                }
                else if (mpOverride is ManagementPackRuleOverride)
                {
                    row["Workflow Type"] = "Rule";
                    row["Workflow"] = AttempToResolveName(((ManagementPackRuleOverride)mpOverride).Rule);

                    if (mpOverride is ManagementPackRuleConfigurationOverride)
                    {
                        row["Property"] = ((ManagementPackRuleConfigurationOverride)mpOverride).Parameter;
                    }
                    else if (mpOverride is ManagementPackRulePropertyOverride)
                    {
                        row["Property"] = ((ManagementPackRulePropertyOverride)mpOverride).Property.ToString();
                    }
                }
                else if (mpOverride is ManagementPackMonitorOverride)
                {
                    row["Workflow Type"] = "Monitor";
                    row["Workflow"] = AttempToResolveName(((ManagementPackMonitorOverride)mpOverride).Monitor);

                    if (mpOverride is ManagementPackMonitorConfigurationOverride)
                    {
                        row["Property"] = ((ManagementPackMonitorConfigurationOverride)mpOverride).Parameter;
                    }
                    else if (mpOverride is ManagementPackMonitorPropertyOverride)
                    {
                        row["Property"] = ((ManagementPackMonitorPropertyOverride)mpOverride).Property.ToString();
                    }
                }
                else if (mpOverride is ManagementPackDiagnosticOverride)
                {
                    row["Workflow Type"] = "Diagnostic";
                    row["Workflow"] = AttempToResolveName(((ManagementPackDiagnosticOverride)mpOverride).Diagnostic);

                    if (mpOverride is ManagementPackDiagnosticConfigurationOverride)
                    {
                        row["Property"] = ((ManagementPackDiagnosticConfigurationOverride)mpOverride).Parameter;
                    }
                    else if (mpOverride is ManagementPackDiagnosticPropertyOverride)
                    {
                        row["Property"] = ((ManagementPackDiagnosticPropertyOverride)mpOverride).Property.ToString();
                    }
                }
                else if (mpOverride is ManagementPackRecoveryOverride)
                {
                    row["Workflow Type"] = "Recovery";
                    row["Workflow"] = AttempToResolveName(((ManagementPackRecoveryOverride)mpOverride).Recovery);

                    if (mpOverride is ManagementPackRecoveryConfigurationOverride)
                    {
                        row["Property"] = ((ManagementPackRecoveryConfigurationOverride)mpOverride).Parameter;
                    }
                    else if (mpOverride is ManagementPackRecoveryPropertyOverride)
                    {
                        row["Property"] = ((ManagementPackRecoveryPropertyOverride)mpOverride).Property.ToString();
                    }
                }

                row["ObjectRef"] = mpOverride.Name;

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private string AttempToResolveName<MPElementType>(
            ManagementPackElementReference<MPElementType> managementPackElementReference
            ) where MPElementType : ManagementPackElement
        {
            string elementDisplayName = "";

            try
            {
                MPElementType element = managementPackElementReference.GetElement();
                elementDisplayName = Common.Utilities.GetBestMPElementName(element);
            }
            catch (Exception)
            {
                if (managementPackElementReference != null)
                {
                    elementDisplayName = managementPackElementReference.Name;
                }
            }

            return (elementDisplayName);
        }

        //---------------------------------------------------------------------
        private void CreateDiagnosticsTable()
        {
            DataTable table = new DataTable("Diagnostics");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Monitor Name", Type.GetType("System.String"));
            table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            table.Columns.Add("Timeout", Type.GetType("System.String"));
            table.Columns.Add("Execute On State", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackDiagnostic diagnostic in m_managementPack.GetDiagnostics())
            {
                DataRow row = table.NewRow();

                row["Name"]             = Utilities.GetBestMPElementName(diagnostic);
                row["Target"]           = AttempToResolveName(diagnostic.Target);
                row["Monitor Name"]     = diagnostic.Monitor.Name;
                row["Remotable"]        = diagnostic.Remotable;
                row["Timeout"]          = diagnostic.Timeout.ToString();
                row["Execute On State"] = diagnostic.ExecuteOnState.ToString();
                row["Description"]      = diagnostic.Description;
                row["ObjectRef"]        = diagnostic.Name;

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void CreateRecoveriesTable()
        {
            DataTable table = new DataTable("Recoveries");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Monitor Name", Type.GetType("System.String"));
            table.Columns.Add("Reset Monitor", Type.GetType("System.Boolean"));
            table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            table.Columns.Add("Timeout", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackRecovery recovery in m_managementPack.GetRecoveries())
            {
                DataRow row = table.NewRow();

                row["Name"]             = Utilities.GetBestMPElementName(recovery);
                row["Target"]           = AttempToResolveName(recovery.Target);
                row["Monitor Name"]     = recovery.Monitor.Name;
                row["Reset Monitor"]    = recovery.ResetMonitor;
                row["Remotable"]        = recovery.Remotable;
                row["Timeout"]          = recovery.Timeout.ToString();
                row["Description"]      = recovery.Description;
                row["ObjectRef"]        = recovery.Name;

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void CreateRelationshipsTable()
        {
            DataTable table = new DataTable("Relationships");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));            
            table.Columns.Add("Source", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Type", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackRelationship relationship in m_managementPack.GetRelationships())
            {
                DataRow row = table.NewRow();

                row["Name"]         = Utilities.GetBestMPElementName(relationship);
                row["Source"] = AttempToResolveName(relationship.Source.Type);
                row["Target"] = AttempToResolveName(relationship.Target.Type);
                row["Type"]         = relationship.Base.Name;
                row["Description"]  = relationship.Description;
                row["ObjectRef"]    = relationship.Name;
                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void CreateClassesTable()
        {
            DataTable table = new DataTable("Classes");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Base Class", Type.GetType("System.String"));
            table.Columns.Add("Is Abstract", Type.GetType("System.Boolean"));
            table.Columns.Add("Is Hosted", Type.GetType("System.Boolean"));
            table.Columns.Add("Is Singleton", Type.GetType("System.Boolean"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));            
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackClass mpClass in m_managementPack.GetClasses())
            {
                DataRow row = table.NewRow();

                row["Name"]             = Utilities.GetBestMPElementName(mpClass);
                row["Base Class"]       = AttempToResolveName(mpClass.Base);
                row["Is Abstract"]      = mpClass.Abstract;
                row["Is Hosted"]        = mpClass.Hosted;
                row["Is Singleton"]     = mpClass.Singleton;
                row["Accessibility"]    = mpClass.Accessibility.ToString();
                row["Description"]      = mpClass.Description;
                row["ObjectRef"]        = mpClass.Name;
                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void CreateReferencesTable()
        {
            DataTable table = new DataTable("Dependencies");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Version", Type.GetType("System.String"));
            table.Columns.Add("KeyToken", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (KeyValuePair<string, ManagementPackReference> reference in m_managementPack.References)
            {
                DataRow row = table.NewRow();

                row["Name"]         = reference.Value.Name;
                row["Version"]      = reference.Value.Version.ToString();
                row["KeyToken"]     = reference.Value.KeyToken.ToString();
                row["ObjectRef"]    = reference.Key;

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void CreateUnitMonitorsTable()
        {
            DataTable table = new DataTable("Monitors - Unit");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Category", Type.GetType("System.String"));
            table.Columns.Add("Enabled", Type.GetType("System.Boolean"));
            table.Columns.Add("Object Name", Type.GetType("System.String"));
            table.Columns.Add("Counter Name", Type.GetType("System.String"));
            table.Columns.Add("Frequency", Type.GetType("System.String"));
            table.Columns.Add("Generate Alert", Type.GetType("System.Boolean"));
            table.Columns.Add("Alert Severity", Type.GetType("System.String"));
            table.Columns.Add("Alert Priority", Type.GetType("System.String"));
            table.Columns.Add("Auto Resolve", Type.GetType("System.String"));
            table.Columns.Add("MonitorType", Type.GetType("System.String"));
            table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            table.Columns.Add("Accessibility", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackMonitor monitor in m_managementPack.GetMonitors())
            {
                DataRow row = table.NewRow();

                if (!(monitor is ManagementPackUnitMonitor))
                {
                    continue;
                }

                PopulateGenericMonitorProperties(monitor, row);

                ManagementPackUnitMonitor unitMonitor = (ManagementPackUnitMonitor)monitor;

                row["MonitorType"] = GetFriendlyMonitorTypeName(unitMonitor.TypeID.Name);
                
                if (IsPerformanceUnitMonitor(unitMonitor))
                {
                    string counterName;
                    string objectName;
                    string frequency;

                    ExtractCounterAndObjectNameFromConfig(unitMonitor.Configuration, out objectName, out counterName, out frequency);

                    row["Object Name"] = objectName;
                    row["Counter Name"] = counterName;
                    row["Frequency"] = frequency;
                }     

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private bool IsPerformanceUnitMonitor(ManagementPackUnitMonitor monitor)
        {
            if (monitor.TypeID.Name == "Microsoft.Windows.WmiBased.Performance.AverageThreshold" ||
               monitor.TypeID.Name == "Microsoft.Windows.WmiBased.Performance.ConsecutiveSamplesThreshold" ||
               monitor.TypeID.Name == "Microsoft.Windows.WmiBased.Performance.DeltaThreshold" ||
               monitor.TypeID.Name == "Microsoft.Windows.WmiBased.Performance.DoubleThreshold" ||
               monitor.TypeID.Name == "Microsoft.Windows.WmiBased.Performance.ThresholdMonitorType" ||
               monitor.TypeID.Name == "System.Performance.AverageThreshold" ||
               monitor.TypeID.Name == "System.Performance.ConsecutiveSamplesThreshold" ||
               monitor.TypeID.Name == "System.Performance.DeltaThreshold" ||
               monitor.TypeID.Name == "System.Performance.DoubleThreshold" ||
               monitor.TypeID.Name == "System.Performance.PerformanceDSBased.AverageDoubleThreshold" ||
               monitor.TypeID.Name == "System.Performance.PerformanceDSBased.Delta.ThreeStateBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.Performance.PerformanceDSBased.Delta.TwoStateAboveBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.Performance.PerformanceDSBased.Delta.TwoStateBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.Performance.PerformanceDSBased.Delta.TwoStateBelowBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.Performance.PerformanceDSBased.DeltaDoubleThreshold" ||
               monitor.TypeID.Name == "System.Performance.ThreeStateBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.Performance.ThresholdMonitorType" ||
               monitor.TypeID.Name == "SSystem.Performance.TwoStateAboveBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.Performance.TwoStateBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.Performance.TwoStateBelowBaseliningMonitorWithoutCompression" ||
               monitor.TypeID.Name == "System.NetworkManagement.ComputedLowThresholdMonitorType" ||
               monitor.TypeID.Name == "System.NetworkManagement.ComputedLowThresholdMonitorType.Division" ||
               monitor.TypeID.Name == "System.NetworkManagement.ComputedExcessiveFragmentationMonitorType" ||
               monitor.TypeID.Name == "System.NetworkManagement.ComputedMultiConditionLowThresholdMonitorType" ||
               monitor.TypeID.Name == "System.NetworkManagement.ComputedThresholdMonitorType" ||
               monitor.TypeID.Name == "System.NetworkManagement.ComputedThresholdMonitorType.Division" ||
               monitor.TypeID.Name == "System.NetworkManagement.LowThresholdMonitorType" ||
               monitor.TypeID.Name == "System.NetworkManagement.ThresholdMonitor" ||
               monitor.TypeID.Name == "System.NetworkManagement.ThresholdMonitor.ProcessorUtilization" ||
               monitor.TypeID.Name == "Microsoft.Unix.WSMan.Performance.Average.Threshold.MonitorType" ||
               monitor.TypeID.Name == "Microsoft.Unix.WSMan.Performance.Filtered.Average.Threshold.MonitorType")
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        //---------------------------------------------------------------------
        private void CreateRulesTable()
        {
            DataTable table = new DataTable("Rules");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Category", Type.GetType("System.String"));
            table.Columns.Add("Enabled", Type.GetType("System.Boolean"));
            table.Columns.Add("Object Name", Type.GetType("System.String"));
            table.Columns.Add("Counter Name", Type.GetType("System.String"));
            table.Columns.Add("Frequency", Type.GetType("System.String"));

            table.Columns.Add("Event ID", Type.GetType("System.String"));
            table.Columns.Add("Event Source", Type.GetType("System.String"));
            table.Columns.Add("Event Log", Type.GetType("System.String"));

            table.Columns.Add("Generate Alert", Type.GetType("System.Boolean"));
            table.Columns.Add("Alert Severity", Type.GetType("System.String"));
            table.Columns.Add("Alert Priority", Type.GetType("System.String"));
            table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            table.Columns.Add("Description", Type.GetType("System.String"));            
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackRule rule in m_managementPack.GetRules())
            {
                DataRow row = table.NewRow();

                row["Name"]         = Utilities.GetBestMPElementName(rule);
                row["Category"]     = rule.Category.ToString();
                row["Enabled"]      = (rule.Enabled != ManagementPackMonitoringLevel.@false);
                row["ObjectRef"]    = rule.Name;
                row["Target"]       = AttempToResolveName(rule.Target);

                ExtractAlertGenerationInfo(rule, row);
                ExtractPerfCounterInfo(rule, row);

                ExtractEventLogData(rule, row);

                row["Remotable"]        = rule.Remotable;
                row["Description"]      = rule.Description;

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void ExtractAlertGenerationInfo(ManagementPackRule rule, DataRow row)
        {
            try
            {
                bool generatesAlert = false;

                foreach (ManagementPackWriteActionModule wa in rule.WriteActionCollection)
                {
                    if (IsAlertGenerationWriteAction(wa.TypeID.Name))
                    {
                        generatesAlert = InternalExtractAlertGenerationInfo(row, wa);
                    }
                }

                row["Generate Alert"] = generatesAlert;
            }
            catch (Exception)
            {
            }
        }

        //---------------------------------------------------------------------
        private static bool InternalExtractAlertGenerationInfo(DataRow row, ManagementPackWriteActionModule wa)
        {
            bool generatesAnAlert = false;

            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(string.Format("<Config>{0}</Config>", wa.Configuration));

                if (wa.TypeID.Name == "System.Mom.BackwardCompatibility.AlertResponse")
                {
                    XmlNode generatesAlert = document.SelectSingleNode("//Config//AlertGeneration//GenerateAlert");

                    generatesAnAlert = (string.Compare(generatesAlert.InnerText, "true", true) == 0);

                    if (generatesAnAlert)
                    {
                        row["Alert Priority"] = ManagementPackWorkflowPriority.Low.ToString();

                        XmlNode alertSeverity = document.SelectSingleNode("//Config//AlertGeneration//AlertLevel");

                        if (alertSeverity != null)
                        {
                            Int32 severityValue = Convert.ToInt32(alertSeverity.InnerText);

                            if (severityValue == 10 || severityValue == 20)
                            {
                                row["Alert Severity"] = ManagementPackAlertSeverity.Information.ToString();
                            }
                            else if (severityValue == 30)
                            {
                                row["Alert Severity"] = ManagementPackAlertSeverity.Warning.ToString();
                            }
                            else if (severityValue == 40 || severityValue == 50 || severityValue == 60 || severityValue == 70)
                            {
                                row["Alert Severity"] = ManagementPackAlertSeverity.Error.ToString();
                            }
                        }
                    }

                }
                else
                {
                    XmlNode priorityNode = document.SelectSingleNode("//Config//Priority");
                    XmlNode severityNode = document.SelectSingleNode("//Config//Severity");

                    generatesAnAlert = true;

                    ManagementPackAlertSeverity severity = (ManagementPackAlertSeverity)Convert.ToInt32(severityNode.InnerText);
                    ManagementPackWorkflowPriority priority = (ManagementPackWorkflowPriority)Convert.ToInt32(priorityNode.InnerText);

                    row["Alert Severity"] = severity.ToString();
                    row["Alert Priority"] = priority.ToString();
                }
            }
            catch (Exception)
            {
            }

            return (generatesAnAlert);
        }

        //---------------------------------------------------------------------
        private void ExtractEventLogData(ManagementPackRule rule, DataRow row)
        {
            try
            {
                foreach (ManagementPackDataSourceModule ds in rule.DataSourceCollection)
                {
                    if (IsEventLogDataSource(ds.TypeID.Name))
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(string.Format("<Config>{0}</Config>", ds.Configuration));

                        ExtractEventLogName(document,row);
                        ExtractEventId(document, row);
                        ExtractEventSource(document, row);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //---------------------------------------------------------------------
        //[borisyan] The filter expression can be very complex. The current implementation is only meant to support
        //the scenario where the workflow is looking at a single event ID from a particular source.
        //In all other scenarios, the info will not be populated.
        //[dmuscett] 2014/02/17 - refactoring to check EventId and Source separately, look across multiple expressions recursively, 
        //and accumulate results; still not perfect but works in several more cases than before
        private void ExtractEventSource(XmlDocument document, DataRow row)
        {
            XmlNode expressionNode = document.SelectSingleNode("//Config//Expression");

            // we'll use this to accumulate the results in case of complex expressions in a single rule
            List<string> ExpressionInfo = new List<string>();

                if (expressionNode.ChildNodes.Count > 1)
                {
                    foreach (XmlNode node in expressionNode.ChildNodes)
                    {
                        if (node.Name == "And" || node.Name == "Or")
                        {
                            foreach (XmlNode n in node.ChildNodes)
                            {
                                ExtractEventSourceFromExpressionNode(n, ExpressionInfo);
                            }
                        }
                        else
                        {
                            ExtractEventSourceFromExpressionNode(expressionNode, ExpressionInfo);
                        }
                    }
                }
                else
                {
                    ExtractEventSourceFromExpressionNode(expressionNode, ExpressionInfo);
                }


                // loop thru whatever we have accumulated
                if (ExpressionInfo.Count != 0)
                {
                    String EventSourceString = String.Empty;

                    foreach (String info in ExpressionInfo)
                    {
                        EventSourceString = EventSourceString + info + "; ";
                    }

                    //Adding up the accumulated values as a string with a separator and trimming off the last "; " (if present)
                    if (!String.IsNullOrEmpty(EventSourceString))
                        row["Event Source"] = EventSourceString.Remove(EventSourceString.Length - 2);
                    else
                        row["Event Source"] = EventSourceString;
                }

        }
        private void ExtractEventSourceFromExpressionNode(XmlNode expressionNode, List<string> ExpressionInfo)
        {
            String SingleExpressionInfo = String.Empty;



            if (expressionNode.FirstChild.Name == "And" || expressionNode.FirstChild.Name == "Or")
            {
                foreach (XmlNode c in expressionNode.FirstChild.ChildNodes)
                {
                    //then it's composite, start recursing within
                    ExtractEventSourceFromExpressionNode(c, ExpressionInfo);
                }
            }
            else
            {
                XmlNode expressionRoot = expressionNode.FirstChild;

                //might be a SimpleExpression...
                XmlNodeList eventSourceNodes = expressionRoot.SelectNodes("..//SimpleExpression//ValueExpression[XPathQuery='PublisherName']");
                //... or not...
                if (eventSourceNodes.Count == 0)
                {
                    eventSourceNodes = expressionRoot.SelectNodes("..//ValueExpression[XPathQuery='PublisherName']");
                }
                // or it might have another field name...
                if (eventSourceNodes.Count == 0)
                {
                    eventSourceNodes = expressionRoot.SelectNodes("..//ValueExpression[XPathQuery='EventSourceName']");
                }

                //if we found anything at all with any of the approaches above...
                if (eventSourceNodes.Count > 0)
                {
                    foreach (XmlNode n in eventSourceNodes)
                    {
                        XmlNode simpleExpressionNode = n.ParentNode;

                        XmlNode operatorNode = simpleExpressionNode.SelectSingleNode("..//Operator");

                        if (operatorNode != null)
                        {
                            XmlNode eventSourceNode = simpleExpressionNode.SelectSingleNode("..//ValueExpression//Value");

                            if (eventSourceNode != null)
                            {
                                SingleExpressionInfo = eventSourceNode.InnerText;
                            }
                        }
                    }
                }
            }


            //if we found something, lets accumulate it, if not let's not bother.
            if (!(String.IsNullOrEmpty(SingleExpressionInfo)))
                ExpressionInfo.Add(SingleExpressionInfo);
        }

        private void ExtractEventId(XmlDocument document, DataRow row)
        {
            XmlNode expressionNode = document.SelectSingleNode("//Config//Expression");

            // we'll use this to accumulate the results in case of complex expressions in a single rule
            List<string> ExpressionInfo = new List<string>();

                if (expressionNode.ChildNodes.Count > 1)
                {
                    foreach (XmlNode node in expressionNode.ChildNodes)
                    {
                        if (node.Name == "And" || node.Name == "Or")
                        {
                            foreach (XmlNode n in node.ChildNodes)
                            {
                                ExtractEventIdFromExpressionNode(n, ExpressionInfo);
                            }
                        }
                        else
                        {
                            ExtractEventIdFromExpressionNode(expressionNode, ExpressionInfo);
                        }
                    }
                }
                else
                {
                    ExtractEventIdFromExpressionNode(expressionNode, ExpressionInfo);
                }


                // loop thru whatever we have accumulated
                if (ExpressionInfo.Count != 0)
                {
                    String EventIDString = String.Empty;

                    foreach (String info in ExpressionInfo)
                    {
                        EventIDString = EventIDString + info + "; ";
                    }

                    //Adding up the accumulated values as a string with a separator and trimming off the last "; " (if present)
                    if (!String.IsNullOrEmpty(EventIDString))
                        row["Event ID"] = EventIDString.Remove(EventIDString.Length - 2);
                    else
                        row["Event ID"] = EventIDString;
                }
        }
        private void ExtractEventIdFromExpressionNode(XmlNode expressionNode, List<string> ExpressionInfo)
        {
            String SingleExpressionInfo = String.Empty;

            if (expressionNode.FirstChild.Name == "And" || expressionNode.FirstChild.Name == "Or")
            {
                //then it's composite, start recursing within
                foreach (XmlNode c in expressionNode.FirstChild.ChildNodes)
                {
                    ExtractEventIdFromExpressionNode(c, ExpressionInfo);
                }
                
            }
            else
            {
                XmlNode expressionRoot = expressionNode.FirstChild;

                //might be a SimpleExpression...
                XmlNodeList eventIdNodes = expressionRoot.SelectNodes("..//SimpleExpression//ValueExpression[XPathQuery='EventDisplayNumber']");
                //... or not...
                if (eventIdNodes.Count == 0)
                {
                    eventIdNodes = expressionRoot.SelectNodes("..//ValueExpression[XPathQuery='EventDisplayNumber']");
                }

                //if we found anything at all with any of the approaches above...
                if (eventIdNodes.Count > 0)
                {
                    foreach (XmlNode n in eventIdNodes)
                    {
                        XmlNode simpleExpressionNode = n.ParentNode;

                        XmlNode operatorNode = simpleExpressionNode.SelectSingleNode("..//Operator");

                        if (operatorNode != null)
                        {
                            XmlNodeList eventIdNodeList = simpleExpressionNode.SelectNodes("..//ValueExpression//Value");
                            if (eventIdNodeList.Count == 0)
                                eventIdNodeList = simpleExpressionNode.SelectNodes("..//Pattern");

                            foreach (XmlNode eventIdNode in eventIdNodeList)
                            {
                                if (eventIdNode != null)
                                {
                                    SingleExpressionInfo = eventIdNode.InnerText;
                                }
                            }
                        }
                    }
                }
            }

            //if we found something, lets accumulate it, if not let's not bother.
            if (!(String.IsNullOrEmpty(SingleExpressionInfo)))
                ExpressionInfo.Add(SingleExpressionInfo);
        }


        //---------------------------------------------------------------------
        private void ExtractEventLogName(XmlDocument document,DataRow row)
        {
            row["Event Log"] = document.SelectSingleNode("//Config/LogName").InnerText;
        }

        //---------------------------------------------------------------------
        private bool IsEventLogDataSource(string dataSourceName)
        {
            return (string.Compare(dataSourceName, "Microsoft.Windows.EventProvider", true) == 0);
        }

        //---------------------------------------------------------------------
        private void ExtractPerfCounterInfo(ManagementPackRule rule, DataRow row)
        {
            try
            {
                foreach (ManagementPackDataSourceModule ds in rule.DataSourceCollection)
                {
                    if (IsPerformanceCollectionDataSource(ds.TypeID.Name))
                    {
                        string counterName;
                        string objectName;
                        string frequency;

                        ExtractCounterAndObjectNameFromConfig(ds.Configuration, out objectName, out counterName, out frequency);

                        row["Object Name"]  = objectName;
                        row["Counter Name"] = counterName;
                        row["Frequency"]    = frequency;

                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //---------------------------------------------------------------------
        private bool IsPerformanceCollectionDataSource(
            string datasourceName
            )
        {
            if (datasourceName == "System.Performance.DataProvider" ||
                datasourceName == "System.Performance.OptimizedDataProvider" ||
                datasourceName == "System.Mom.BackwardCompatibility.Performance.FilteredDataProvider" ||
                datasourceName == "System.NetworkManagement.ComputedPerfProvider" ||
                datasourceName == "System.NetworkManagement.ComputedPerfProvider.Division" ||
                datasourceName == "System.NetworkManagement.ExtendedSnmpPerformanceProvider" ||
                datasourceName == "System.NetworkManagement.LargestFreeBufferProvider" ||
                datasourceName == "System.NetworkManagement.MemoryPerfProvider" ||
                datasourceName == "System.NetworkManagement.SnmpPerformanceProvider.CiscoRouter.Rate" ||
                datasourceName == "System.NetworkManagement.SnmpPerformanceProvider.dot3_Ethernet.Rate" ||
                datasourceName == "System.NetworkManagement.SnmpPerformanceProvider.ProcessorUtilization" ||
                datasourceName == "System.NetworkManagement.SnmpPerformanceProvider.Rate" ||
                datasourceName == "Microsoft.Unix.WSMan.PerfCounterProvider" ||
                datasourceName == "Microsoft.Unix.WSMan.PerfCounterProvider.Filtered" || 
                datasourceName == "Microsoft.Unix.WSMan.PerfCounterProvider.Filtered.Delta")
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        //---------------------------------------------------------------------
        private bool IsAlertGenerationWriteAction(
            string writeActionName
            )
        {
            if (writeActionName == "System.Health.GenerateAlert" ||
                writeActionName == "Microsoft.Windows.2003.Cluster.GenerateAlertAction" ||
                writeActionName == "Microsoft.Windows.Cluster.GenerateAlertAction.SuppressedByCustom" ||
                writeActionName == "Microsoft.Windows.Cluster.GenerateAlertAction.SuppressedByDescription" ||
                writeActionName == "System.Mom.BackwardCompatibility.AlertResponse" ||
                writeActionName == "Microsoft.Windows.Server.IIS.6.2.GenerateAlertAction.SuppressedByDescription" ||
                writeActionName == "Microsoft.Windows.Server.IIS.2008.GenerateAlertAction.SuppressedByDescription" ||
                writeActionName == "Microsoft.Forefront.TMG.Rule.AlertGenerate.WA" )
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        //---------------------------------------------------------------------
        private void ExtractCounterAndObjectNameFromConfig(
            string      config,
            out string  objectName,
            out string  counterName,
            out string  frequency
            )
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(string.Format("<Config>{0}</Config>", config));

            objectName  = string.Empty;
            counterName = string.Empty;
            frequency   = string.Empty;

            XmlNode objectNameNode = document.SelectSingleNode("//Config//ObjectName");
            XmlNode counterNameNode = document.SelectSingleNode("//Config//CounterName");
            XmlNode frequencyNode = document.SelectSingleNode("//Config//Frequency");
            //sometimes "Interval" is used as opposed to "Frequency" in newer MPs...
            XmlNode intervalNode = document.SelectSingleNode("//Config//Interval");


            if (objectNameNode != null)
            {
                objectName = objectNameNode.InnerText;
            }

            if (counterNameNode != null)
            {
                counterName = counterNameNode.InnerText;
            }

            if (frequencyNode != null)
            {
                frequency = frequencyNode.InnerText;
            }
            else
            {
                if (intervalNode != null)
                {
                    frequency = intervalNode.InnerText;
                }
            }
        }

        //---------------------------------------------------------------------
        private void CreateDiscoveriesTable()
        {
            DataTable table = new DataTable("Discoveries");

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Target", Type.GetType("System.String"));
            table.Columns.Add("Enabled", Type.GetType("System.Boolean"));
            table.Columns.Add("Frequency", Type.GetType("System.String"));
            table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            table.Columns.Add("Description", Type.GetType("System.String"));
            table.Columns.Add("ObjectRef");

            foreach (ManagementPackDiscovery discovery in m_managementPack.GetDiscoveries())
            {
                DataRow row = table.NewRow();

                row["Name"]         = Utilities.GetBestMPElementName(discovery);
                row["Enabled"]      = (discovery.Enabled != ManagementPackMonitoringLevel.@false);
                row["Frequency"]    = RetrieveDiscoveryFrequency(discovery);
                row["Description"]  = discovery.Description;
                row["ObjectRef"]    = discovery.Name;
                row["Target"]       = AttempToResolveName(discovery.Target);
                row["Remotable"]    = discovery.Remotable;

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private void CreateGenericTable<MPElementType>(
            ManagementPackElementCollection<MPElementType>  mpElementCollection,
            string                                          tableName,
            bool                                            isWorkflow,
            bool                                            hasTarget
            ) where MPElementType : ManagementPackElement
        {
            DataTable table = new DataTable(tableName);

            m_dataset.Tables.Add(table);

            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Description", Type.GetType("System.String"));

            if (isWorkflow)
            {
                table.Columns.Add("Remotable", Type.GetType("System.Boolean"));
            }

            if (hasTarget)
            {
                table.Columns.Add("Target", Type.GetType("System.String"));
            }
            
            table.Columns.Add("ObjectRef");

            foreach (MPElementType element in mpElementCollection)
            {
                DataRow row = table.NewRow();

                row["Name"]         = Utilities.GetBestMPElementName(element);
                row["Description"]  = element.Description;

                if (element is ManagementPackRecovery)
                {
                    row["Remotable"] = ((ManagementPackRecovery)(ManagementPackElement)element).Remotable;
                    row["Target"] = AttempToResolveName(((ManagementPackRecovery)(ManagementPackElement)element).Target);
                }
                else if (element is ManagementPackDiagnostic)
                {
                    row["Remotable"] = ((ManagementPackDiagnostic)(ManagementPackElement)element).Remotable;
                    row["Target"] = AttempToResolveName(((ManagementPackDiagnostic)(ManagementPackElement)element).Target);
                }
                else if (element is ManagementPackTask)
                {
                    row["Remotable"] = ((ManagementPackTask)(ManagementPackElement)element).Remotable;
                    row["Target"] = AttempToResolveName(((ManagementPackTask)(ManagementPackElement)element).Target);
                }
                else if (element is ManagementPackLinkedReport)
                {
                    row["Target"] = AttempToResolveName(((ManagementPackLinkedReport)(ManagementPackElement)element).Target);
                }
                else if (element is ManagementPackReport)
                {
                    row["Target"] = AttempToResolveName(((ManagementPackReport)(ManagementPackElement)element).Target);
                }
                else if (element is ManagementPackConsoleTask)
                {
                    row["Target"] = AttempToResolveName(((ManagementPackConsoleTask)(ManagementPackElement)element).Target);
                }
                else if (element is ManagementPackView)
                {
                    row["Target"] = AttempToResolveName(((ManagementPackView)(ManagementPackElement)element).Target);
                }

                row["ObjectRef"]    = element.Name;

                table.Rows.Add(row);
            }
        }

        //---------------------------------------------------------------------
        private string RetrieveDiscoveryFrequency(
            ManagementPackDiscovery managementPackDiscovery
            )
        {
            XmlDocument document = new XmlDocument();
            XmlNode     frequencyNode;
            string      xmlContent;
            string      frequency = string.Empty;

            xmlContent = string.Format("<config>{0}</config>", managementPackDiscovery.DataSource.Configuration);

            document.LoadXml(xmlContent);

            frequencyNode = document.SelectSingleNode("//config/Frequency");

            // maybe it wasn't "Frequency" after all
            if (frequencyNode == null)
            {
                frequencyNode = document.SelectSingleNode("//config/IntervalSeconds");
            }
            // maybe it wasn't "IntervalSeconds" either...
            if (frequencyNode == null)
            {
                frequencyNode = document.SelectSingleNode("//config/Interval");
            }

            if (frequencyNode != null)
            {
                frequency = frequencyNode.InnerText;
            }

            if (frequencyNode == null)
            {
                frequencyNode = document.SelectSingleNode("//config/PeriodInSeconds");
            }

            if (frequencyNode != null)
            {
                frequency = frequencyNode.InnerText;
            }

            return (frequency);
        }

        //---------------------------------------------------------------------
        private string GetFriendlyMonitorTypeName(string monitorType)
        {
            //Resolve some of the common monitor types to user friendly names
            if(monitorType == "Microsoft.Windows.CheckNTServiceStateMonitorType")
            {return "Basic NT Service Monitor"; }
            else if(monitorType == "Microsoft.Windows.3SingleEventLog3StateUnitMonitorType")
            {return "3 State Event Log Monitor";}
            else if (monitorType == "Microsoft.Windows.2SingleEventLog2StateMonitorType")
            { return "2 State Event Log Monitor"; }
            else if(monitorType == "System.Performance.ThresholdMonitorType")
            {return "Simple Performance Counter Threshold";}
            else if(monitorType == "System.Performance.DeltaThreshold")
            { return "Delta Performance Counter Threshold"; }
            else if (monitorType == "System.Performance.DoubleThreshold")
            { return "Double Performance Counter Threshold"; }
            else if (monitorType == "System.Performance.AverageThreshold")
            { return "Average Performance Counter Threshold"; }
            else if (monitorType == "System.Performance.ConsecutiveSamplesThreshold")
            { return "Consecutive Performance Counter Samples over Threshold"; }
            else if (monitorType == "System.Performance.TwoStateBaseliningMonitorWithoutCompression")
            { return "2 State Baselining Performance Counter"; }
            else
            {
                return (monitorType);
            }
        }

        //---------------------------------------------------------------------
        internal DataSet Dataset
        {
            get
            {
                return (m_dataset);
            }
        }
    }
}
