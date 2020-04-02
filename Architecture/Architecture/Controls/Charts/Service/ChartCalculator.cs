using Architecture.Core;
using System;

namespace Architecture.Controls.Charts
{
    public static class ChartCalculator
    {
        public static float CalculateExactValue(ChartValueItem firstItem, ChartValueItem secondItem, float xPosition)
        {
            if (secondItem == null)
            {
                return firstItem.Value;
            }

            // Difference in value
            var diffVal = secondItem.Value - firstItem.Value;

            // Difference in X position
            var diffX = secondItem.Point.X - firstItem.Point.X;

            var xValue = diffVal / diffX;

            // Current X position over first item
            var x = xPosition - firstItem.Point.X;
            var points = x * xValue;

            float currentVal = 0;

            if (secondItem != null)
            {
                try
                {
                    currentVal = firstItem.Value + (int)Math.Round(decimal.Parse(points.ToString()), 0, MidpointRounding.AwayFromZero);
                }
                catch (Exception ex)
                {
                    ex.Print();
                }

                if (xPosition == firstItem.Point.X)
                {
                    currentVal = firstItem.Value;
                }
                else if (xPosition == secondItem?.Point.X)
                {
                    currentVal = secondItem.Value;
                }
            }
            else
            {
                currentVal = firstItem.Value;
            }

            return currentVal;
        }

        internal static float CalculateYPosition(ChartValueItem firstItem, ChartValueItem secondItem, float xPosition)
        {
            if (secondItem == null)
            {
                return firstItem.Point.Y;
            }

            // Difference in value
            var diffVal = secondItem.Value - firstItem.Value;

            // Difference in Y position
            var diffY = secondItem.Point.Y - firstItem.Point.Y;

            // Difference in X position
            var diffX = secondItem.Point.X - firstItem.Point.X;

            var yValue = diffVal / diffY;
            var xValue = diffVal / diffX;

            // Current X position over first item
            var x = xPosition - firstItem.Point.X;
            var points = x * xValue;

            return (points / yValue) + firstItem.Point.Y;
        }
    }
}
