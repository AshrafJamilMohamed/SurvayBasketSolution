namespace SurvayBasket.Helper
{
    public static class DateChecker
    {
        public static bool IsDateValid(DateOnly StartDate, DateOnly EndDate)
        {
            DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);
            if (CurrentDate > StartDate) return false;
            if (CurrentDate > EndDate) return false;
            if (StartDate > EndDate) return false;

            return true;

        }
    }
}
