using bleep.Domain.Entities;
using SchoolEmailNotifier.Business;
using SchoolEmailNotifier.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SchoolEmailNotifier.Application
{
    public class CoreApplication
    {
        public static string sourceDirName;
        public static string destinyDirName;
        public static string errorLogDirName;
        public static int headerRowsCount;
        public static int absentsLimit;
        private readonly WorksheetBL _worksheetBL;
        private readonly StudentWorksheetBL _studentWorksheetBL;
        private readonly ValidateTotalAbsentsBL _validateTotalAbsentsBL;
        private readonly SendEmailBL _sendEmailBL;
        private EmailModel emailInfo;

        public CoreApplication(
            WorksheetBL worksheetBL, 
            StudentWorksheetBL studentWorksheetBL, 
            ValidateTotalAbsentsBL validateTotalAbsentsBL,
            SendEmailBL sendEmailBL)
        {
            _worksheetBL = worksheetBL;
            _studentWorksheetBL = studentWorksheetBL;
            _validateTotalAbsentsBL = validateTotalAbsentsBL;
            _sendEmailBL = sendEmailBL;
        }

        public void Initialize()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            sourceDirName = currentDirectory + @"\planilhas";
            destinyDirName = currentDirectory +  @"\processado";
            errorLogDirName = currentDirectory + @"\erro";
            
            headerRowsCount = 6;

            emailInfo = new EmailModel(
                "notificacaoplanilha@gmail.com",
                "dusilvabrandao@gmail.com", 
                "ronovaes24@gmail.com", 
                "guav qzip qmuc vdmf"
                );
        }

        public void Execute()
        {
            try
            {
                Initialize();

                var worksheetsList = _worksheetBL.ReadWorksheets(sourceDirName, destinyDirName);

                if (!(worksheetsList.Count() > 0)) return;

                List<StudentEntity> studentAbsentsExceededList = new List<StudentEntity>();

                foreach (var subWorksheets in worksheetsList)
                {
                    foreach (var worksheet in subWorksheets)
                    {
                        int rowsCount = worksheet.RowCount();
                        string className = worksheet.Name;

                        if (className == "FORM" || className == "TRANSPORTE"
                            || className == "TOTAL DE ALUNOS" || className.Contains("Planilha") || className.Contains("EXP")
                            || className.Contains("AM") || className.Contains("PRESENÇA") || className.Contains("IF")) continue;

                        //if (!(className == "1A" || className == "1B" || className == "2A" || className == "2B"
                        //    || className == "2C" || className == "2D" || className == "3A" || className == "3B"))
                        //    continue;

                        string mesDaPlanilha = worksheet.Cell($"O2").Value.ToString();

                        for (int r = headerRowsCount; r < rowsCount; r++)
                        {
                            var columnNumber = worksheet.Cell($"A{r}").Value.ToString();

                            if (string.IsNullOrEmpty(columnNumber)) break;
                    
                            var totalMonthClasses = worksheet.Cell($"BN{3}").Value.ToString();
                            
                            // var studentTotalMonthAbsents = worksheet.Cell($"DP{r}").Value.ToString();

                            var totalCurrentClasses = worksheet.Cell($"BM{3}").Value.ToString();

                            var studentTotalMonthPresences = worksheet.Cell($"BN{r}").Value.ToString();

                            if (string.IsNullOrEmpty(totalMonthClasses) || string.IsNullOrEmpty(totalCurrentClasses) || string.IsNullOrEmpty(studentTotalMonthPresences)) 
                                continue;

                            var studentTotalMonthAbsents = int.Parse(totalCurrentClasses) - int.Parse(studentTotalMonthPresences);

                            var studentStatus = _validateTotalAbsentsBL.ValidateTotalAbsentsExceeded(
                                int.Parse(totalMonthClasses),
                                studentTotalMonthAbsents);

                            if(studentStatus != null)
                            {
                                var student = _studentWorksheetBL.GetStudentWorksheet(worksheet, r, className);

                                //var studentMonthPresencePercent = _studentWorksheetBL.GetStudentMonthPresencePercent(
                                //    int.Parse(totalMonthClasses),
                                //    int.Parse(studentTotalMonthPresences));

                                if (student != null)
                                {
                                    student.Status = studentStatus;

                                    student.MesDaPlanilha = mesDaPlanilha;

                                    student.TotalAulasDadas = int.Parse(totalCurrentClasses);

                                    student.TotalFaltas = studentTotalMonthAbsents;

                                    //student.PorcentagemFrequenciaMensal = studentMonthPresencePercent;

                                    studentAbsentsExceededList.Add(student);
                                }
                            }
                        }
                    }
                }

                if (studentAbsentsExceededList.Count() > 0)
                {
                    _sendEmailBL.SendEmailWorksheetNotification(emailInfo, studentAbsentsExceededList);
                }
            }
            catch(Exception ex)
            {
                string errorFileName = $"{errorLogDirName}\\Log_Error_{DateTime.Now.ToString("yyyyMMddhhmmss")}.txt";

                File.WriteAllText(errorFileName, ex.Message);
            }
        }
    }
}
