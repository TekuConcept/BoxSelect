using System.Collections.Generic;
using System.Drawing;

namespace CS_BoxSelect
{
    class ConvexHull
    {
        //
        // https://www.geeksforgeeks.org/convex-hull-set-1-jarviss-algorithm-or-wrapping/
        // Complexity: O(n^2)
        //

        enum Orientation { COLLINEAR, CLOCKWISE, COUNTERCLOCKWISE }

        private static Orientation GetOrientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) return Orientation.COLLINEAR;
            return (val > 0) ? Orientation.CLOCKWISE : Orientation.COUNTERCLOCKWISE;
        }

        public static List<Point> Solve(List<Point> points)
        {
            if (points.Count < 3) return points;
            List<Point> hull = new List<Point>();

            int l = 0;
            for (int i = 1; i < points.Count; i++)
                if (points[i].X < points[l].X)
                    l = i;

            int p = l, q;
            do
            {
                hull.Add(points[p]);
                q = (p + 1) % points.Count;

                for (int i = 0; i < points.Count; i++)
                    if (GetOrientation(points[p], points[i], points[q])
                        == Orientation.COUNTERCLOCKWISE)
                        q = i;

                p = q;
            } while (p != l);

            return hull;
        }
    }
}
