using Architecture.Core;
using SkiaSharp;
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

        internal static float CalculateYPositionForStraight(ChartValueItem firstItem, ChartValueItem secondItem, float xPosition)
        {
            if (secondItem == null)
            {
                return firstItem.Point.Y;
            }

            // Difference in value
            var diffVal = secondItem.Value - firstItem.Value;

            // Difference in Y position
            var diffY = secondItem.Point.Y - firstItem.Point.Y;

            if (diffY == 0)
            {
                return firstItem.Point.Y;
            }

            // Difference in X position
            var diffX = secondItem.Point.X - firstItem.Point.X;

            var yValue = diffVal / diffY;
            var xValue = diffVal / diffX;

            // Current X position over first item
            var x = xPosition - firstItem.Point.X;
            var points = x * xValue;

            return (points / yValue) + firstItem.Point.Y;
        }

        internal static SKPoint CalculateYPositionForSpline(ChartValueItem firstItem, ChartValueItem secondItem, float xPosition)
        {
            if (secondItem == null)
            {
                //return firstItem.Point.Y;
                return firstItem.Point;
            }

            var y = CalculateYPositionForStraight(firstItem, secondItem, xPosition).ToRounded();

            var point = firstItem.Point;
            var nextPoint = secondItem.Point;
            var offsetPoint = new SKPoint((nextPoint.X - point.X) * .5f, 0);

            var currentPoint = point + offsetPoint;
            var next = nextPoint - offsetPoint;

            float diffY;

            // Difference in Y position
            diffY = (firstItem.Point.Y > secondItem.Point.Y ? firstItem.Point.Y - secondItem.Point.Y : secondItem.Point.Y - firstItem.Point.Y).ToRounded();

            if (diffY == 0)
            {
                return new SKPoint(xPosition, firstItem.Point.Y);
            }

            var insideY = y > firstItem.Point.Y ? y - firstItem.Point.Y.ToRounded() : firstItem.Point.Y - y;

            var t = (insideY / diffY).ToRounded(2);

            var beziers = HandleBezier(point, currentPoint, next, nextPoint, t);

            var doits = DoIt(point.X, point.Y, currentPoint.X, currentPoint.Y, next.X, next.Y, nextPoint.X, nextPoint.Y, t);

            return doits;
        }

        internal static SKPoint DoIt(float p0x, float p0y, float cp0x, float cp0y, float cp1x, float cp1y, float p1x, float p1y, float t)
        {
            var Ax = ((1 - t) * p0x) + (t * cp0x);
            var Ay = ((1 - t) * p0y) + (t * cp0y);
            var Bx = ((1 - t) * cp0x) + (t * cp1x);
            var By = ((1 - t) * cp0y) + (t * cp1y);
            var Cx = ((1 - t) * cp1x) + (t * p1x);
            var Cy = ((1 - t) * cp1y) + (t * p1y);

            var Dx = ((1 - t) * Ax) + (t * Bx);
            var Dy = ((1 - t) * Ay) + (t * By);
            var Ex = ((1 - t) * Bx) + (t * Cx);
            var Ey = ((1 - t) * By) + (t * Cy);

            var Px = ((1 - t) * Dx) + (t * Ex);
            var Py = ((1 - t) * Dy) + (t * Ey);

            return new SKPoint(Px, Py);
        }

        internal static SKPoint HandleBezier(SKPoint startPoint, SKPoint offsetStartPoint, SKPoint offsetEndPoint, SKPoint endPoint, float t)
        {
            var x = Math.Pow(1 - t, 3) * startPoint.X + 3 * Math.Pow(1 - t, 2) * t * offsetStartPoint.X + 3 * (1 - t) * Math.Pow(t, 2) * offsetEndPoint.X + Math.Pow(t, 3) * endPoint.X;

            var y = Math.Pow(1 - t, 3) * startPoint.Y + 3 * Math.Pow(1 - t, 2) * t * offsetStartPoint.Y + 3 * (1 - t) * Math.Pow(t, 2) * offsetEndPoint.Y + Math.Pow(t, 3) * endPoint.Y;

            return new SKPoint((float)x, (float)y);
        }

        internal static Tuple<double, double> GetYValues(SKPoint StartPoint, SKPoint ControlPoint, SKPoint EndPoint, float X)
        {
            var Ax = StartPoint.X;
            var Bx = ControlPoint.X;
            var Cx = EndPoint.X;

            var q1 = 2 * Ax - 2 * Bx;
            var q2 = Math.Sqrt(5 * Ax * Ax - 10 * Ax * Bx + Ax * Cx - Ax * X + 2 * Bx * X + 4 * Bx * Bx);
            var q3 = 2 * Ax - 4 * Bx + 2 * Cx;
            var t1 = (q1 + q2) / q3;
            var t2 = (q1 - q2) / q3;

            var Ay = StartPoint.Y;
            var By = ControlPoint.Y;
            var Cy = EndPoint.Y;

            var Y1 = Ay * (1 - t1) * (1 - t1) + By * 2 * (1 - t1) * t1 + Cy * t1;
            var Y2 = Ay * (1 - t2) * (1 - t2) + By * 2 * (1 - t2) * t2 + Cy * t2;

            return new Tuple<double, double>(Y1, Y2);
        }
    }
}
