using SchoolEmailNotifier.Domain.Models;

namespace bleep.Domain.Entities
{
    public class StudentEntity
    {
        public string Turma { get; set; }
        public string NomePlanilha { get; set; }
        public string Numero { get; set; }
        public string NomeAluno { get; set; }
        public string Ra { get; set; }
        public string Digito { get; set; }
        public string Situacao { get; set; }
        public string Movimentacao { get; set; }
        public string AuxilioBrasil { get; set; }
        public string Tutor { get; set; }
        public string EmailTutor { get; set; }
        public string Telefone { get; set; }
        public string Responsavel { get; set; }
        public string MotivoFaltas { get; set; }
        public string AtestadoDe { get; set; }
        public string AtestadoAte { get; set; }
        public int TotalFaltas { get; set; }
        public int TotalPresencas { get; set; }
        public string Encaminhamento { get; set; }
        public string JustificativaSemana1 { get; set; }
        public string JustificativaSemana2 { get; set; }
        public string JustificativaSemana3 { get; set; }
        public string JustificativaSemana4 { get; set; }
        public string JustificativaSemana5 { get; set; }
        public string Observacao { get; set; }
        public int TotalAulasDadas { get; set; }
        public string MesDaPlanilha { get; set; }
        public string PorcentagemFrequenciaMensal { get; set; }
        public StudentStatusModel Status { get; set; }
    }
}
