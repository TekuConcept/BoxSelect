using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS_BoxSelect
{
    
    public partial class Canvas : Control
    {
        bool mouseDown;
        Point startPoint;
        Point endPoint;

        List<Point> cube;
        List<Point> hull;

        Pen cubePen;
        Pen hullPen;
        Pen selectGood;
        Pen selectBad;

        RichTextBox log;

        public Canvas()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            MouseDown += Canvas_MouseDown;
            MouseMove += Canvas_MouseMove;
            MouseUp   += Canvas_MouseUp;

            mouseDown  = false;
            startPoint = new Point();
            endPoint   = new Point();

            // screen is about 800 x 450
            cube = new List<Point> {
                new Point(350, 150), new Point(450, 150),
                new Point(350, 250), new Point(450, 250),
                new Point(375, 125), new Point(475, 125),
                new Point(375, 225), new Point(475, 225)
            };
            hull = ConvexHull.Solve(cube);

            cubePen    = new Pen(new SolidBrush(ForeColor));
            hullPen    = new Pen(new SolidBrush(Color.BlueViolet));
            selectGood = new Pen(new SolidBrush(Color.GreenYellow));
            selectBad  = new Pen(new SolidBrush(Color.Brown));
        }

        public void SetLog(RichTextBox log)
        {
            this.log = log;
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            startPoint = e.Location;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                endPoint = e.Location;
                this.Invalidate();
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private bool IsSelected(Rectangle rect)
        {
            //
            // Separating Axis Theorem (SAT)
            //

            //bool result = false;

            //Line top    = new Line(rect.Left,  rect.Top,    rect.Right, rect.Top);
            //Line bottom = new Line(rect.Right, rect.Bottom, rect.Left,  rect.Bottom);
            //Line left   = new Line(rect.Left,  rect.Bottom, rect.Left,  rect.Top);
            //Line right  = new Line(rect.Right, rect.Top,    rect.Right, rect.Bottom);

            //log.Clear();

            //for (int i = 0; i < cube.Count; i++)
            //{
            //    float l1 = top.A    * cube[i].X + top.B    * cube[i].Y + top.C;
            //    float l2 = bottom.A * cube[i].X + bottom.B * cube[i].Y + bottom.C;
            //    float l3 = left.A   * cube[i].X + left.B   * cube[i].Y + left.C;
            //    float l4 = right.A  * cube[i].X + right.B  * cube[i].Y + right.C;
            //    result |= (l1 >= 0 && l2 >= 0 && l3 >= 0 && l4 >= 0);
            //    log.Text += string.Format("Point {0}: {1}, {2}, {3}, {4}\n",
            //        i,
            //        l1 >= 0 ? 1 : 0,
            //        l2 >= 0 ? 1 : 0,
            //        l3 >= 0 ? 1 : 0,
            //        l4 >= 0 ? 1 : 0);
            //}

            List<Point> selection = new List<Point>()
            {
                new Point(rect.Left, rect.Top),    new Point(rect.Right, rect.Top),
                new Point(rect.Left, rect.Bottom), new Point(rect.Right, rect.Bottom)
            };
            return SATCollision.Intersects(selection, hull);
        }

        private void DrawCube(Graphics gfx)
        {
            gfx.DrawLines(cubePen, new Point[] { cube[0], cube[1], cube[3], cube[2], cube[0] });
            gfx.DrawLines(cubePen, new Point[] { cube[4], cube[5], cube[7], cube[6], cube[4] });
            gfx.DrawLine(cubePen, cube[0], cube[4]);
            gfx.DrawLine(cubePen, cube[1], cube[5]);
            gfx.DrawLine(cubePen, cube[2], cube[6]);
            gfx.DrawLine(cubePen, cube[3], cube[7]);

            List<Point> drawableHull = new List<Point>(hull);
            drawableHull.Add(drawableHull[0]);
            gfx.DrawLines(hullPen, drawableHull.ToArray());
        }

        private void DrawSelection(Graphics gfx)
        {
            if (startPoint.X == endPoint.X ||
                startPoint.Y == endPoint.Y) return;

            Rectangle selection = new Rectangle();
            selection.X = Math.Min(startPoint.X, endPoint.X);
            selection.Y = Math.Min(startPoint.Y, endPoint.Y);
            var X = Math.Max(startPoint.X, endPoint.X);
            var Y = Math.Max(startPoint.Y, endPoint.Y);
            selection.Width  = X - selection.X;
            selection.Height = Y - selection.Y;

            if (IsSelected(selection))
                gfx.DrawRectangle(selectGood, selection);
            else gfx.DrawRectangle(selectBad, selection);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            DrawCube(pe.Graphics);
            DrawSelection(pe.Graphics);
        }
    }
}
