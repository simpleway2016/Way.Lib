using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.UI
{
    class PathBuilder
    {
        const int Distance = 10;
        enum Direction
        {
            H = 0,
            V = 1,
        }
        static bool isErrorPath(Point[] points)
        {
            int samecount = 0;
            //check x
            int lastvalue = int.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X != lastvalue)
                {
                    lastvalue = points[i].X;
                    samecount = 1;
                }
                else
                {
                    samecount++;
                    if (samecount == 3)
                    {
                        if (points[i].Y > points[i - 1].Y && points[i - 1].Y > points[i - 2].Y)
                        {
                        }
                        else if (points[i].Y < points[i - 1].Y && points[i - 1].Y < points[i - 2].Y)
                        {
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }

            samecount = 0;
            //check y
            lastvalue = int.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].Y != lastvalue)
                {
                    lastvalue = points[i].Y;
                    samecount = 1;
                }
                else
                {
                    samecount++;
                    if (samecount == 3)
                    {
                        if (points[i].X > points[i - 1].X && points[i - 1].X > points[i - 2].X)
                        {
                        }
                        else if (points[i].X < points[i - 1].X && points[i - 1].X < points[i - 2].X)
                        {
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
        }
        public static Point[] GetPath(System.Drawing.Rectangle rect1, System.Drawing.Point p1, System.Drawing.Rectangle rect2, System.Drawing.Point p2)
        {
            var path1 = GetPath(Direction.H, rect1, p1, rect2, p2);
            var path2 = GetPath(Direction.V, rect1, p1, rect2, p2);
            if (isErrorPath(path1))
                return path2;
            if (isErrorPath(path2))
                return path1;

            if (path1.Length < path2.Length)
                return path1;
            else
                return path2;
        }
        public static Point[] GetDirectPath( System.Drawing.Rectangle rect1, System.Drawing.Point p1, System.Drawing.Rectangle rect2, System.Drawing.Point p2)
        {
            p1 = new System.Drawing.Point(p1.X + rect1.X, p1.Y + rect1.Y);
            p2 = new System.Drawing.Point(p2.X + rect2.X, p2.Y + rect2.Y);

            if (rect1.Contains(p2) || rect2.Contains(p1))
                return new Point[0];

            List<Point> results = new List<Point>();
            results.Add(p1);

            Point movingPoint = GetNotIntersetPoint(rect1, p1);
            Point endPoint = GetNotIntersetPoint(rect2, p2);
            results.Add(movingPoint);
            results.Add(endPoint);
            results.Add(p2);
            return results.ToArray();
        }
        static Point[] GetPath(Direction _dir, System.Drawing.Rectangle rect1, System.Drawing.Point p1, System.Drawing.Rectangle rect2, System.Drawing.Point p2)
        {
            p1 = new System.Drawing.Point(p1.X + rect1.X, p1.Y + rect1.Y);
            p2 = new System.Drawing.Point(p2.X + rect2.X, p2.Y + rect2.Y);

            if (rect1.Contains(p2) || rect2.Contains(p1))
                return new Point[0];

            List<Point> results = new List<Point>();
            results.Add(p1);

            Point movingPoint = GetNotIntersetPoint(rect1, p1);
            Point endPoint = GetNotIntersetPoint(rect2, p2);

            Point touchpoint;
            results.Add(movingPoint);

            int done = 0;
            Direction direction = _dir;
            while (true)
            {
                done++;
                if (done > 50)
                    return new Point[0];

                if (rect1.Contains(endPoint) || rect2.Contains(movingPoint))
                    return new Point[0];

                Point target;
                if (direction == Direction.H)
                {
                    //横着走
                    target = new Point(endPoint.X, movingPoint.Y);

                    if (target == movingPoint)
                    {
                        direction = Direction.V;
                        continue;
                    }
                }
                else
                {
                    //竖着走
                    target = new Point(movingPoint.X, endPoint.Y);
                    if (target == movingPoint)
                    {
                        direction = Direction.H;
                        continue;
                    }
                }

                //先判断是否会和自己矩形或者对方矩形相交
                if (IsInterset(rect1, movingPoint, target, out touchpoint))
                {
                    if (direction == Direction.H)
                    {
                        //和节点在同一水平线了，必须升上或者下降
                        if (Math.Abs(rect1.Y - rect2.Y) + Math.Abs(endPoint.Y - rect2.Y + movingPoint.Y - rect1.Y) < Math.Abs(rect1.Bottom - rect2.Bottom) + Math.Abs(rect2.Bottom - endPoint.Y + rect1.Bottom - movingPoint.Y))
                        {
                            //上升路径短
                            movingPoint = new Point(movingPoint.X, rect1.Y - Distance);
                        }
                        else
                        {
                            //下降路径短
                            movingPoint = new Point(movingPoint.X, rect1.Bottom + Distance);
                        }
                        results.Add(movingPoint);
                        continue;
                    }
                    else if (direction == Direction.V)
                    {

                        //和节点在同一垂直线了，必须向左或向右移动
                        if (Math.Abs(rect1.X - rect2.X) + Math.Abs(movingPoint.X - rect1.X + endPoint.X - rect2.X) < Math.Abs(rect1.Right - rect2.Right) + Math.Abs(rect1.Right - movingPoint.X + rect2.Right - endPoint.X))
                        {
                            //向左路径短
                            movingPoint = new Point(rect1.X - Distance, movingPoint.Y);
                        }
                        else
                        {
                            //向右路径短
                            movingPoint = new Point(rect1.Right + Distance, movingPoint.Y);
                        }
                        results.Add(movingPoint);
                        continue;
                    }
                    direction = (direction == Direction.H) ? Direction.V : Direction.H;
                    continue;
                }
                if (IsInterset(rect2, movingPoint, target, out touchpoint))
                {
                    if (direction == Direction.H)
                    {
                        //和节点在同一水平线了，必须升上或者下降
                        if (Math.Abs(rect1.Y - rect2.Y) + Math.Abs(endPoint.Y - rect2.Y + movingPoint.Y - rect1.Y) < Math.Abs(rect1.Bottom - rect2.Bottom) + Math.Abs(rect2.Bottom - endPoint.Y + rect1.Bottom - movingPoint.Y))
                        {
                            //上升路径短
                            movingPoint = new Point(movingPoint.X, rect2.Y - Distance);
                        }
                        else
                        {
                            //下降路径短
                            movingPoint = new Point(movingPoint.X, rect2.Bottom + Distance);
                        }
                        results.Add(movingPoint);
                        continue;
                    }
                    else if (direction == Direction.V)
                    {

                        //和节点在同一垂直线了，必须向左或向右移动
                        if (Math.Abs(rect1.X - rect2.X) + Math.Abs(movingPoint.X - rect1.X + endPoint.X - rect2.X) < Math.Abs(rect1.Right - rect2.Right) + Math.Abs(rect1.Right - movingPoint.X + rect2.Right - endPoint.X))
                        {
                            //向左路径短
                            movingPoint = new Point(rect2.X - Distance, movingPoint.Y);
                        }
                        else
                        {
                            //向右路径短
                            movingPoint = new Point(rect2.Right + Distance, movingPoint.Y);
                        }
                        results.Add(movingPoint);
                        continue;
                    }
                    direction = (direction == Direction.H) ? Direction.V : Direction.H;
                    continue;
                }

                if (target == endPoint)
                    break;
                else
                {
                    movingPoint = target;
                    results.Add(movingPoint);
                }
            }

            results.Add(endPoint);
            results.Add(p2);

            return results.ToArray();
        }

        /// <summary>
        /// 直线是否会碰上矩形
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static bool IsInterset(Rectangle rect, Point startPoint, Point endPoint, out Point touchPoint)
        {
            Rectangle myrect = new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2);
            touchPoint = Point.Empty;
            if (startPoint.Y == endPoint.Y)
            {
                //横线
                if (startPoint.X < rect.X && endPoint.X < rect.X)//两个点都在矩形的一边
                    return false;
                else if (startPoint.X > rect.Right && endPoint.X > rect.Right)
                    return false;
                int right = rect.X + rect.Width;
                int left = rect.X;
                int x;
                if (Math.Abs(right - startPoint.X) < Math.Abs(left - startPoint.X))
                {
                    //右边更近
                    x = right;
                }
                else
                {
                    x = left;
                }
                Point p2 = new Point(x, startPoint.Y);
                if (myrect.Contains(p2))
                {
                    touchPoint = p2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //竖线
                if (startPoint.Y < rect.Y && endPoint.Y < rect.Y)//两个点都在矩形的一边
                    return false;
                else if (startPoint.Y > rect.Bottom && endPoint.Y > rect.Bottom)
                    return false;

                int bottom = rect.Y + rect.Height;
                int top = rect.Y;
                int y;
                if (Math.Abs(bottom - startPoint.Y) < Math.Abs(top - startPoint.Y))
                {
                    //下边更近
                    y = bottom;
                }
                else
                {
                    y = top;
                }
                Point p2 = new Point(startPoint.X, y);
                if (myrect.Contains(p2))
                {
                    touchPoint = p2;
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// 获取不与矩形相交的距离点
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static System.Drawing.Point GetNotIntersetPoint(Rectangle rect, Point p)
        {
            Point p1 = new Point(p.X - Distance, p.Y);
            if (rect.Contains(p1) == false)
                return p1;

            p1 = new Point(p.X + Distance, p.Y);
            if (rect.Contains(p1) == false)
                return p1;

            p1 = new Point(p.X, p.Y - Distance);
            if (rect.Contains(p1) == false)
                return p1;

            p1 = new Point(p.X, p.Y + Distance);
            if (rect.Contains(p1) == false)
                return p1;

            return Point.Empty;
        }
    }
}
