using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace MPViewer
{
    class ExcelWriter
    {
        StringBuilder m_contents;

        //---------------------------------------------------------------------
        internal ExcelWriter()
        {
            m_contents = new StringBuilder();

            GenerateHeader();
        }

        //---------------------------------------------------------------------
        private void GenerateHeader()
        {
            m_contents.Append("<?xml version=\"1.0\"?>\n");
            m_contents.Append("<?mso-application progid=\"Excel.Sheet\"?>\n");
            m_contents.Append("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
            m_contents.Append("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
            m_contents.Append("xmlns:x=\"urn:schemas-microsoft-com:office:excel\" ");
            m_contents.Append("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
            m_contents.Append("xmlns:html=\"http://www.w3.org/TR/REC-html40\">\n");
            m_contents.Append("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
            m_contents.Append("</DocumentProperties>");
            m_contents.Append("<ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">\n");
            m_contents.Append("<ProtectStructure>False</ProtectStructure>\n");
            m_contents.Append("<ProtectWindows>False</ProtectWindows>\n");
            m_contents.Append("</ExcelWorkbook>\n");

            m_contents.Append("<Styles>\n");
            m_contents.Append("<Style ss:ID=\"Default\" ss:Name=\"Normal\">\n");
            m_contents.Append("<Alignment ss:Vertical=\"Bottom\"/>\n");
            m_contents.Append("<Borders/>\n");
            m_contents.Append("<Font/>\n");
            m_contents.Append("<Interior/>\n");
            m_contents.Append("<NumberFormat/>\n");
            m_contents.Append("<Protection/>\n");
            m_contents.Append("</Style>\n");
            m_contents.Append("<Style ss:ID=\"s21\">\n");
            m_contents.Append("<Font ss:Bold=\"1\"/>\n");
            m_contents.Append("<Alignment ss:Vertical=\"Bottom\"/>\n");
            m_contents.Append("</Style>\n");
            m_contents.Append("<Style ss:ID=\"s28\">\n");
            m_contents.Append("<Borders>\n");
            m_contents.Append("<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\n");
            m_contents.Append("<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\n");
            m_contents.Append("<Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\n");
            m_contents.Append("<Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>\n");
            m_contents.Append("</Borders>\n");
            m_contents.Append("<Font x:Family=\"Swiss\" ss:Bold=\"1\"/>\n");
            m_contents.Append("<Interior ss:Color=\"#FFCC00\" ss:Pattern=\"Solid\"/>\n");
            m_contents.Append("</Style>\n");
            m_contents.Append("</Styles>\n");
        }

        //---------------------------------------------------------------------
        internal void WriteDataTable(DataTable table)
        {
            m_contents.AppendFormat(@"<Worksheet ss:Name=""{0}"">", table.TableName);
            m_contents.Append("<Table>");

            WriterTableHeader(table);

            WriteTableRows(table);

            m_contents.Append("</Table>");
            m_contents.AppendFormat("</Worksheet>");
        }

        //---------------------------------------------------------------------
        private void WriteTableRows(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                m_contents.AppendLine("<Row>");

                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    //CDATA added dmuscett march 8th 2012 because Exchange MP contains invalid characters such as "<" and ">" and that was breaking the XML file format
                    m_contents.AppendFormat(@"<Cell><Data ss:Type=""String""><![CDATA[{0}]]></Data></Cell>", row[i].ToString());
                }

                m_contents.AppendLine("</Row>");
            }
        }

        //---------------------------------------------------------------------
        private void WriterTableHeader(DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                m_contents.AppendLine("<ss:Column ss:Width=\"80\"/>");
            }

            m_contents.AppendLine("<Row>");
            
            foreach (DataColumn column in table.Columns)
            {
                m_contents.AppendFormat(@"<Cell ss:StyleID=""s28""><Data ss:Type=""String"">{0}</Data></Cell>", column.ColumnName);
            }

            m_contents.AppendLine("</Row>");
        }

        //---------------------------------------------------------------------
        internal void SaveToFile(string filePath)
        {
            m_contents.Append("</Workbook>");

            TextWriter tw = new StreamWriter(filePath);

            tw.Write(m_contents.ToString());

            tw.Close();
        }
    }
}
