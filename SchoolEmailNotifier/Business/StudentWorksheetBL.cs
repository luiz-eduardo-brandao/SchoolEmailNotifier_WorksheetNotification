using bleep.Domain.Entities;
using ClosedXML.Excel;

namespace SchoolEmailNotifier.Business
{
    public class StudentWorksheetBL
    {
        public string GetStudentMonthPresencePercent(int totalClassesMonth, int studentTotalMonthPresences) 
        {
            var studentMonthPresencePercent = (totalClassesMonth * studentTotalMonthPresences) / 100;

            return $"{studentMonthPresencePercent} %";
        }

        public StudentEntity GetStudentWorksheet(IXLWorksheet worksheet, int row, string className)
        {
            StudentEntity student = new StudentEntity();

            student.Numero = worksheet.Cell($"A{row}").Value.ToString();
            student.NomeAluno = worksheet.Cell($"B{row}").Value.ToString();
            student.Ra = worksheet.Cell($"C{row}").Value.ToString();
            student.Digito = worksheet.Cell($"D{row}").Value.ToString();
            student.Turma = className;
            student.Responsavel = worksheet.Cell($"BT{row}").Value.ToString();
            student.Telefone = worksheet.Cell($"BU{row}").Value.ToString();
            student.Tutor = worksheet.Cell($"BR{row}").Value.ToString();
            student.EmailTutor = worksheet.Cell($"BS{row}").Value.ToString();
            //student.TotalFaltas = int.Parse(worksheet.Cell($"DP{row}").Value.ToString());

            student.TotalPresencas = int.Parse(worksheet.Cell($"BN{row}").Value.ToString());

            string just1 = worksheet.Cell($"AV{row}").Value.ToString();

            if (just1.Contains("SELECIONE"))
                just1 = "";

            student.JustificativaSemana1 = just1;

            string just2 = worksheet.Cell($"AZ{row}").Value.ToString();

            if (just2.Contains("SELECIONE"))
                just2 = "";

            student.JustificativaSemana2 = just2;

            string just3 = worksheet.Cell($"BD{row}").Value.ToString();

            if (just3.Contains("SELECIONE"))
                just3 = "";

            student.JustificativaSemana3 = just3;

            string just4 = worksheet.Cell($"BH{row}").Value.ToString();

            if (just4.Contains("SELECIONE"))
                just4 = "";

            student.JustificativaSemana4 = just4;

            string just5 = worksheet.Cell($"BL{row}").Value.ToString();

            if (just5.Contains("SELECIONE"))
                just5 = "";

            student.JustificativaSemana5 = just5;

            student.Observacao = worksheet.Cell($"BM{row}").Value.ToString();

            //student.PorcentagemFrequenciaMensal = worksheet.Cell($"BO{row}").Value.ToString();
            //student.Situacao = worksheet.Cell($"E{row}").Value.ToString();
            //student.Movimentacao = worksheet.Cell($"F{row}").Value.ToString();
            //student.AuxilioBrasil = worksheet.Cell($"G{row}").Value.ToString();
            //student.Tutor = worksheet.Cell($"H{row}").Value.ToString();
            //student.MotivoFaltas = worksheet.Cell($"AR{row}").Value.ToString();
            //student.AtestadoDe = worksheet.Cell($"AS{row}").Value.ToString();
            //student.AtestadoAte = worksheet.Cell($"AT{row}").Value.ToString();
            //student.Encaminhamento = worksheet.Cell($"AV{row}").Value.ToString();

            return student;
        }
    }
}
