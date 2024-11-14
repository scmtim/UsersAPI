namespace UsersAPI.Utility
{
    public static class YearCalculator
    {
        public static int YearDifference(DateTime start, DateTime end)
        {
            if (end.DayOfYear > start.DayOfYear)
            {
                return end.Year - start.Year;
            }
            else
            {
                //case where Year is the same and would result in negative age handled by requirement that users are 18+
                return end.Year - start.Year - 1;
            }

        }
    }
}
