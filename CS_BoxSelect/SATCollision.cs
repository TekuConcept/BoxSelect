using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CS_BoxSelect
{
    class SATCollision
    {
        //
        // Separating Axis Theorem (SAT)
        // https://youtu.be/RBya4M6SWwk
        //

        struct Extent
        {
            public float Min { get; set; }
            public float Max { get; set; }
        }

        private static float Dot(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        private static void ProjectToAxis(Point axis, List<Point> poly, out Extent ext)
        {
            Extent extent = new();
            extent.Min = Dot(axis, poly[0]);
            extent.Max = extent.Min;
            for (int i = 1; i < poly.Count; i++)
            {
                var p = Dot(axis, poly[i]);
                if (p < extent.Min) extent.Min = p;
                if (p > extent.Max) extent.Max = p;
            }
            ext = extent;
        }

        public static bool Intersects(List<Point> polyA, List<Point> polyB)
        {
            if (polyA.Count < 2 || polyB.Count < 2) return false;

            for (int i = 0; i < polyA.Count; i++)
            {
                Line side = new(polyA[i], polyA[(i + 1) % polyA.Count]);
                Point axis = side.Normal;
                ProjectToAxis(axis, polyA, out Extent projectionA);
                ProjectToAxis(axis, polyB, out Extent projectionB);
                var overlap =
                    Math.Min(projectionA.Max, projectionB.Max) -
                    Math.Max(projectionA.Min, projectionB.Min);
                if (overlap < 0) return false;
            }

            for (int i = 0; i < polyB.Count; i++)
            {
                Line side = new(polyB[i], polyB[(i + 1) % polyB.Count]);
                Point axis = side.Normal;
                ProjectToAxis(axis, polyA, out Extent projectionA);
                ProjectToAxis(axis, polyB, out Extent projectionB);
                var overlap =
                    Math.Min(projectionA.Max, projectionB.Max) -
                    Math.Max(projectionA.Min, projectionB.Min);
                if (overlap < 0) return false;
            }

            return true;

            /**
             * axes1 = o1.dir.normal()
             * axes2 = o2.dir.normal()
             * proj1, proj2 = 0
             * 
             * for (i = 0; i < axes1.Count; i++) {
             *     proj1 = ProjectToAxis(axes1[i], o1)
             *     proj2 = ProjectToAxis(axes1[i], o2)
             *     overlap = Math.min(proj1.max, proj.max) - Math.max(proj1.min, proj2.min)
             *     if (overlap < 0) return false
             * }
             * 
             * for (i = 0; i < axes2.Count; i++) {
             *     proj1 = ProjectToAxis(axes2[i], o1)
             *     proj2 = ProjectToAxis(axes2[i], o2)
             *     overlap = Math.min(proj1.max, proj.max) - Math.max(proj1.min, proj2.min)
             *     if (overlap < 0) return false
             * }
             * 
             * return true
             */
        }
    }
}
