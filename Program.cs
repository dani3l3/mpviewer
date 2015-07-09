using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Configuration.IO;
using Common;
using Microsoft.EnterpriseManagement.Packaging;

namespace MPViewer
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0].Length > 0)
                {
                    if (args[0].Equals("--help", StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageBox.Show("Command line usage examples: \n MPViewer.exe c:\\Microsoft.Windows.Server.2003.mp c:\\win2003.html AlertGeneratingWorkflowsOnly \n MPViewer.exe c:\\Microsoft.Windows.Server.2003.mp c:\\win2003.html \n MPViewer.exe c:\\Microsoft.Windows.Server.2003.mp c:\\win2003.xls", "MPViewer Help");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Unsupported number of arguments or format. Launching MPViewer.exe with no parameters will bring up the UI, or use MPViewer.exe --help to get help on the implemented command line options.");
                        return;
                    }
                }

            }
            
            // gotta love the undocumented command line usage for bulk HTML export... only works for MPs at this point, not MPBs
            // TODO - refactor so only one piece of code is used to load the MP/MPB, rather than this duplicate logic...
            if (args.Length >= 2)
            {
                try
                {
                    if (args[0].Length == 0)
                    {
                        throw new ApplicationException("Invalid management pack file path");
                    }
                    if (args[1].Length == 0)
                    {
                        throw new ApplicationException("Invalid file path");
                    }

                    //HTML output desired
                    if ((args[1].EndsWith(".html", StringComparison.InvariantCultureIgnoreCase)) || (args[1].EndsWith(".htm", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        GenerateHTMLForMP(args);
                    }

                    //Excel output desired
                    if ((args[1].EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase)) || (args[1].EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        GenerateExcelForMP(args);
                    }
                }
                catch(Exception exception)
                {
                    //Console.WriteLine(exception.Message);
                    MessageBox.Show(exception.Message);
                }
                
            }
            // load the GUI - standard execution path
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MPViewer());
            }
        }


        private static ManagementPack LoadManagementPack(string[] args)
        {

            ManagementPackBundle bundle;

            ManagementPackFileStore store = Utilities.GetManagementPackStoreFromPath(args[0]);

            ManagementPack mp;

            if (System.IO.Path.GetExtension(args[0]).Equals(".mpb"))
            {
                ManagementPackBundleReader reader = ManagementPackBundleFactory.CreateBundleReader();
                bundle = reader.Read(args[0], store);

                // 1 at the time is ok
                if (bundle.ManagementPacks.Count == 1)
                {
                    mp = bundle.ManagementPacks[0];
                    return mp;
                }
                else
                {
                    // too many MPs contained in this MPB! - can onlhy open one at the time!
                    // do something sensible here
                    throw new ApplicationException("This MPB contains multiple MPs. " +
                        "In an upcoming version a dialog will open, asking you to choose which one you want to see. " +
                        "For now, we just are going to crash.");
                }
            }
            else // we are dealing with an MP or XML - the old stuff works as it did for 2007
            {
                mp = new ManagementPack(args[0], store);
                return mp;
            }

        }

        private static void GenerateHTMLForMP(string[] args)
        {
            try
            {
                bool exportAlertGeneratingWorkflowsOnly = false;

                ManagementPack mp = LoadManagementPack(args);

                DatasetCreator datasetCreator = new DatasetCreator(mp);

                ReportGenerator reportGenerator = new ReportGenerator(datasetCreator.Dataset,mp);

                exportAlertGeneratingWorkflowsOnly = (args.Length == 3 && string.Compare(args[2],"AlertGeneratingWorkflowsOnly",true) == 0);

                reportGenerator.GenerateHTMLReport(args[1], exportAlertGeneratingWorkflowsOnly);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private static void GenerateExcelForMP(string[] args)
        {
            try
            {
                bool exportAlertGeneratingWorkflowsOnly = false;

                ManagementPack mp = LoadManagementPack(args);

                DatasetCreator datasetCreator = new DatasetCreator(mp);

                ReportGenerator reportGenerator = new ReportGenerator(datasetCreator.Dataset, mp);

                exportAlertGeneratingWorkflowsOnly = (args.Length == 3 && string.Compare(args[2], "AlertGeneratingWorkflowsOnly", true) == 0);
                if (exportAlertGeneratingWorkflowsOnly)
                    Console.WriteLine("AlertGeneratingWorkflowsOnly command line switch doesn't work with Excel export, only HTML.");

                reportGenerator.GenerateExcelReport(args[1]);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

        }
    }
}