namespace Architecture.Controls.Charts
{
    public static class FloatExtensions
    {
        public static float GetLowestValue(this float value, float compareValue)
        {
            return value < compareValue ? value : compareValue;
        }

        public static float GetHighestValue(this float value, float compareValue)
        {
            return value > compareValue ? value : compareValue;
        }
    }
}
