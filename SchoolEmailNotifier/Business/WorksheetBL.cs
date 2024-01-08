using bleep.Domain.Entities;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace SchoolEmailNotifier.Business
{
    public class WorksheetBL
    {
        public List<IXLWorksheets> ReadWorksheets(string sourceDirName, string destinyDirName)
        {
            var files = Directory.GetFiles(sourceDirName);

            List<string> validWorksheetsDir = new List<string>();

            List<IXLWorksheets> worksheetsList = new List<IXLWorksheets>();

            foreach(var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
               
                if (fileInfo.Extension == ".xls" || fileInfo.Extension == ".xlsm")
                {
                    validWorksheetsDir.Add(fileInfo.FullName);
                }                
            }

            foreach(var worksheetDir in validWorksheetsDir)
            {
                XLWorkbook xls = new XLWorkbook(worksheetDir);
                worksheetsList.Add(xls.Worksheets);

                var newFile = new DirectoryInfo(worksheetDir);

                if (File.Exists($"{destinyDirName}\\{newFile.Name}"))
                {
                    File.Delete($"{destinyDirName}\\{newFile.Name}");
                }

                newFile.MoveTo($"{destinyDirName}\\{newFile.Name}");
            }

            return worksheetsList;
        }
    }
}
