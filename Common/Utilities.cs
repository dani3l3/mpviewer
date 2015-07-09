using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Configuration.IO;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Packaging;
using System.Windows.Forms;
using System.IO;

namespace Common
{
    public class Utilities
    {
        
        //---------------------------------------------------------------------
        internal static ManagementPackFileStore GetManagementPackStoreFromPath(string filePath)
        {
            ManagementPackFileStore store = new ManagementPackFileStore();

            store.AddDirectory(System.IO.Path.GetDirectoryName(filePath));
            //standard OpsMgr paths
            store.AddDirectory(@"%ProgramFiles%\System Center Operations Manager 2007");
            store.AddDirectory(@"%ProgramFiles%\System Center 2012\Operations Manager\Console");
            store.AddDirectory(@"%ProgramFiles%\System Center 2012\Operations Manager\Server");
            //standard VSAC's paths
            store.AddDirectory(@"%ProgramFiles%\System Center 2012 Visual Studio Authoring Extensions\References\OM2007R2");
            store.AddDirectory(@"%ProgramFiles(x86)%\System Center 2012 Visual Studio Authoring Extensions\References\OM2007R2");
            store.AddDirectory(@"%ProgramFiles%\System Center 2012 Visual Studio Authoring Extensions\References\OM2012");
            store.AddDirectory(@"%ProgramFiles(x86)%\System Center 2012 Visual Studio Authoring Extensions\References\OM2012");
            store.AddDirectory(@"%ProgramFiles%\System Center 2012 Visual Studio Authoring Extensions\References\SM2012");
            store.AddDirectory(@"%ProgramFiles(x86)%\System Center 2012 Visual Studio Authoring Extensions\References\SM2012");

            return store;
        }
        
        
        //---------------------------------------------------------------------
        internal static string GetBestMPElementName(
            ManagementPackElement element
            )
        {
            if (element.DisplayName == null || element.DisplayName.Length == 0)
            {
                return (element.Name);
            }
            else
            {
                string elementName = element.DisplayName;

                elementName = elementName.Replace("\r\n", "");

                elementName = elementName.TrimStart(new char[] { ' ' });

                elementName = elementName.Replace("\t", "");

                return (elementName);
            }
        }

        //---------------------------------------------------------------------
        internal static string GetBestManagementPackName(
            ManagementPack managementPack
            )
        {
            if (managementPack.DisplayName != null && managementPack.DisplayName.Length > 0)
            {
                return (managementPack.DisplayName);
            }
            else
            {
                return (managementPack.Name);
            }
        }

        //---------------------------------------------------------------------
        internal static string GetBestMonitoringObjectName(
            PartialMonitoringObject monitoringObject
            )
        {
            if (monitoringObject.DisplayName != null && monitoringObject.DisplayName.Length > 0)
            {
                return (monitoringObject.DisplayName);
            }
            else
            {
                return (monitoringObject.Name);
            }
        }
    }
}