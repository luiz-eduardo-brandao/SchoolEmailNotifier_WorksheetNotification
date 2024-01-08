using bleep.Domain.Entities;
using SchoolEmailNotifier.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SchoolEmailNotifier.Business
{
    public class SendEmailBL
    {
        public void SendEmailWorksheetNotification(EmailModel emailInfo, List<StudentEntity> studentsList)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Timeout = 50000,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailInfo.EmailFrom, emailInfo.Password)
            };

            string subject = "Aviso de Alunos Excedentes em Faltas | Notificação Planilha";

            var studentsWithoutTutor = studentsList.Where(s => string.IsNullOrEmpty(s.Tutor));

            var studentsTutorAgrouped = studentsList.Where(s => !string.IsNullOrEmpty(s.Tutor)).GroupBy(s => s.EmailTutor).ToList();

            foreach (var studentsTutor in studentsTutorAgrouped)
            {
                var tutorEmail = studentsTutor.Key;

                //if (!(tutorEmail == "josianicristina@professor.educacao.sp.gov.br")) continue;

                if (string.IsNullOrWhiteSpace(tutorEmail))
                {
                    tutorEmail = emailInfo.EmailsCopy;
                }

                //debug
                //tutorEmail = emailInfo.EmailTo;

                string studentsTutorListHtml = GenerateHtmlEmail2(studentsTutor.ToList());

                SendEmail(client, subject, studentsTutorListHtml, emailInfo.EmailFrom, tutorEmail, emailInfo.EmailsCopy);            
            }

            if (studentsWithoutTutor.Count() > 0) 
            {
                string studentsWithoutTutorHtml = GenerateHtmlEmail2(studentsWithoutTutor.ToList());

                SendEmail(client, subject, studentsWithoutTutorHtml, emailInfo.EmailFrom, emailInfo.EmailsCopy, "");
            }
            // SendEmail(client, subject, studentsWithoutTutorHtml, emailInfo.EmailFrom, emailInfo.EmailTo, "");
        }

        private void SendEmail(SmtpClient client, string subject, string emailHtml, string emailFrom, string emailTo, string emailCopy) 
        {
            MailMessage message = new MailMessage(emailFrom, emailTo);

            message.Subject = subject;

            if (!string.IsNullOrWhiteSpace(emailCopy)) 
            {
                MailAddress copy = new MailAddress(emailCopy);

                message.CC.Add(copy);
            }

            message.Body = emailHtml;

            message.IsBodyHtml = true;

            client.Send(message);
        }

        private string GenerateHtmlEmail2(List<StudentEntity> studentsList) 
        {
            var mesDaPlanilha = studentsList.Select(s => s.MesDaPlanilha).First();

            var txtEmail = new StringBuilder("<html>");
            txtEmail.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            txtEmail.AppendLine("<head><link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN\" crossorigin=\"anonymous\"></head>");
            txtEmail.AppendLine("<style>.title {padding: 8px; background-color: dodgerblue;color: #fff;/* border-radius: 6px; */}</style>");
            txtEmail.AppendLine("<body>");

            if (string.IsNullOrWhiteSpace(mesDaPlanilha)) 
            {
                txtEmail.AppendLine("<table width=\"1100\" align=\"center\"><tr bgcolor=\"dodgerblue\" style=\"color:#fff;\"><td align=\"center\"><b>Alunos que excederam o limite de faltas: </b></td></tr></table>");
            }
            else 
            {
                txtEmail.AppendLine($"<table width=\"1100\" align=\"center\"><tr bgcolor=\"dodgerblue\" style=\"color:#fff;\"><td align=\"center\"><b>Alunos que excederam o limite de faltas - {mesDaPlanilha.ToUpper()}</b></td></tr></table>");
            }

            txtEmail.AppendLine("<br/>");
            txtEmail.AppendLine("<table width=\"1100\" align=\"center\">");
            txtEmail.AppendLine("</table>");
            txtEmail.AppendLine("<br/>");

            txtEmail.AppendLine("<table width=\"1100\" align=\"center\" cellpadding=\"2\" cellspacing=\"2\">");
            txtEmail.AppendLine("<tr bgcolor=\"dodgerblue\"><td align=\"center\"><b>N°</b></td><td align=\"center\"><b>Nome do Aluno</b></td>");
            txtEmail.AppendLine("<td align=\"center\"><b>RA</b></td><td align=\"center\"><b>Total de Faltas</b></td><td align=\"center\"><b>Turma</b></td><td align=\"center\"><b>Tutor</b></td>");
            txtEmail.AppendLine("<td align=\"center\"><b>Responsável</b></td><td align=\"center\"><b>Contato</b></td><td align=\"center\"><b>Situação</b></td>");
            txtEmail.AppendLine("<td align=\"center\"><b>Justificativa Semana 1</b></td><td align=\"center\"><b>Justificativa Semana 2</b></td><td align=\"center\"><b>Justificativa Semana 3</b></td>");
            txtEmail.AppendLine("<td align=\"center\"><b>Justificativa Semana 4</b></td><td align=\"center\"><b>Justificativa Semana 5</b></td><td align=\"center\"><b>Observação</b></td></tr>");

            for (int i = 0; i < studentsList.Count(); i++) 
            {
                if (i % 2 == 0)
                    txtEmail.AppendLine("    <tr>");
                else
                    txtEmail.AppendLine("<tr bgcolor=\"#B0E0E6\">");

                txtEmail.AppendLine("</tr>");

                var situacao = studentsList[i].Status.DangerStatus ? "Perigo" : "Alerta";
                var cor = studentsList[i].Status.DangerStatus ? "#ff0000" : "#00308F";

                txtEmail.AppendLine("<td align=\"center\"><b>" + studentsList[i].Numero + "</b></td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].NomeAluno + "</td>");
                txtEmail.AppendLine($"<td align=\"center\"> <b>{studentsList[i].Ra}</b>-<b>{studentsList[i].Digito}</b> </td>");
                txtEmail.AppendLine("<td align=\"center\"><b>" + studentsList[i].TotalFaltas + "</b></td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].Turma + "</td>");
                txtEmail.AppendLine("<td align=\"center\"><b>" + studentsList[i].Tutor + "</b></td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].Responsavel + "</td>");
                txtEmail.AppendLine("<td align=\"center\"><b>" + studentsList[i].Telefone + "</b></td>");
                txtEmail.AppendLine($"<td align=\"center\" style=\"color:{cor};\"><b>" + situacao + "</b></td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].JustificativaSemana1 + "</td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].JustificativaSemana2 + "</td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].JustificativaSemana3 + "</td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].JustificativaSemana4 + "</td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].JustificativaSemana5 + "</td>");
                txtEmail.AppendLine("<td align=\"center\">" + studentsList[i].Observacao + "</td>");
                
                txtEmail.AppendLine("</tr>");
            }

            txtEmail.AppendLine("</table>");
            txtEmail.AppendLine("</body>");
            txtEmail.AppendLine("<html>");

            return txtEmail.ToString();
        }

        private string GenerateHtmlEmail(List<StudentEntity> studentsList)
        {
            var initHtml = @"
                <!DOCTYPE html>
                <html lang='pt-br'>
                <head>
                    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                    <meta name='viewport' content='width=device-width'>
                    <title></title>

                    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css' rel='stylesheet' 
                        integrity='sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN' crossorigin='anonymous'>
                </head>
                    
                <style>
                    .title {
                        padding: 8px;
                        background-color: dodgerblue;
                        color: #fff;
                        /* border-radius: 6px; */
                    }
                </style>

                <body>
                    <div class='card mx-5'>
                        <div class='card-header title'>
                            <h2 class='mx-4 my-2'>Alunos que excederam o limite de faltas: </h2>    
                        </div>

                    <div class='card-body'>
                        <table class='table'>
                            <thead>
                            <tr style='padding: 3px; margin-top: 2px;'>
                                <th scope='col'>N°</th>
                                <th></th>
                                <th scope='col'>Nome do Aluno</th>
                                <th></th>
                                <th scope='col'>RA</th>
                                <th></th>
                                <th scope='col'>Total de Faltas</th>
                                <th></th>
                                <th scope='col'>Turma</th>
                                <th></th>
                                <th scope='col'>Tutor</th>
                                <th></th>
                                <th scope='col'>Responsável</th>
                                <th></th>
                                <th scope='col'>Contato</th>
                                <th></th>
                                <th scope='col'>Situação</th>
                                <th></th>                               
                                <th scope='col'>Justificativa Semana 1</th>
                                <th></th>                         
                                <th scope='col'>Justificativa Semana 2</th>
                                <th></th>                         
                                <th scope='col'>Justificativa Semana 3</th>
                                <th></th>                         
                                <th scope='col'>Justificativa Semana 4</th>
                                <th></th>                         
                                <th scope='col'>Justificativa Semana 5</th>
                                <th></th>
                            </tr>
                            </thead>
                            <tbody class='table-group-divider'>
            ";

            var contentHtml = @"";

            foreach (var student in studentsList)
            {
                var situacao = student.Status.DangerStatus ? "Perigo" : "Alerta";
                var cor = student.Status.DangerStatus ? "#ff0000" : "#e6e600";

                contentHtml += $@"
                    <tr>
                        <th scope='row'>  {student.Numero}  </th>
                        <td></td>
                        <td>  {student.NomeAluno}  </td>
                        <td></td>
                        <td>  <strong>{student.Ra}-{student.Digito}</strong>   </td>
                        <td></td>
                        <td style='color:{cor};'>  <strong>{student.TotalFaltas}</strong>   </td>
                        <td></td>
                        <td>  <strong>{student.Turma}</strong>  </td>
                        <td></td>
                        <td>  {student.Tutor} </td>
                        <td></td>
                        <td>  <strong>{student.Responsavel}</strong>  </td>
                        <td></td>
                        <td>  <strong>{student.Telefone}</strong>  </td>
                        <td></td>
                        <td style='color:{cor};'>  <strong>{situacao}</strong>  </td>
                        <td></td>
                        <td style='color:{cor};'>  {student.JustificativaSemana1}  </td>
                        <td></td>
                        <td style='color:{cor};'>  {student.JustificativaSemana2}  </td>
                        <td></td>
                        <td style='color:{cor};'>  {student.JustificativaSemana3}  </td>
                        <td></td>
                        <td style='color:{cor};'>  {student.JustificativaSemana4}  </td>
                        <td></td>
                        <td style='color:{cor};'>  {student.JustificativaSemana5}  </td>
                        <td></td>
                    </tr>
                    ";
            }

            var endHtml = @"
                            </tbody>
                        </table>
                    </div>
                </div> 
            ";

            return initHtml += contentHtml + endHtml;
        }
    }
}
