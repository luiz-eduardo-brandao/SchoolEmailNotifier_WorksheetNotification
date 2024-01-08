using SchoolEmailNotifier.Domain.Models;

namespace SchoolEmailNotifier.Business
{
    public class ValidateTotalAbsentsBL
    {
        public StudentStatusModel ValidateTotalAbsentsExceeded(int totalClassesMonth, int studentTotalAbsentsMonth)
        {
            var totalAbsentsOk = (totalClassesMonth * 10) / 100;

            var totalAbsentsWarning = (totalClassesMonth * 15) / 100;

            if (studentTotalAbsentsMonth <= totalAbsentsOk)
                return null;

            if (studentTotalAbsentsMonth > totalAbsentsOk && studentTotalAbsentsMonth <= totalAbsentsWarning) 
                return new StudentStatusModel { AbsencesExceeded = true, DangerStatus = false };

            if (studentTotalAbsentsMonth > totalAbsentsWarning)
                return new StudentStatusModel { AbsencesExceeded = true, DangerStatus = true };

            return null;
        }

        public StudentStatusModel ValidateTotalMonthPresences(int totalClassesMonth, int studentTotalMonthPresences)
        {
            var totalPresencesOk = (totalClassesMonth * 90) / 100;

            var totalPresencesWarning = (totalClassesMonth * 85) / 100;

            if (studentTotalMonthPresences >= totalPresencesOk)
                return null;

            if (studentTotalMonthPresences < totalPresencesOk && studentTotalMonthPresences >= totalPresencesWarning) 
                return new StudentStatusModel { AbsencesExceeded = true, DangerStatus = false };

            if (studentTotalMonthPresences < totalPresencesWarning)
                return new StudentStatusModel { AbsencesExceeded = true, DangerStatus = true };
            
            return null;
        }
    }
}
