using System.Drawing;

namespace CS_BoxSelect
{
    struct Line
    {
        public float A, B, C;

        public Line(float _A, float _B, float _C)
        {
            A = _A;
            B = _B;
            C = _C;
        }

        public Line(Point M, Point N) : this(M.X, M.Y, N.X, N.Y) { }
        public Line(PointF M, PointF N) : this(M.X, M.Y, N.X, N.Y) { }

        public Line(float X1, float Y1, float X2, float Y2)
        {
            A = Y1 - Y2;
            B = X2 - X1;
            C = X1 * Y2 - X2 * Y1;
        }

        public Point Normal
        {
            get
            {
                return new Point((int)A, (int)B);
            }
        }
    }
}
