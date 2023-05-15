using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib {
public static class CalculateIntersectionArea {
    public static double Circle_Circle(Circle circle1, Circle circle2) {
        double radius1 = circle1.Scaled(circle1.Radius);
        double radius2 = circle2.Scaled(circle2.Radius);

        Point centersVector = new Point(circle2.Center.X-circle1.Center.X, circle2.Center.Y-circle1.Center.Y);
        double centersDistance = Math.Sqrt(Math.Pow(centersVector.X, 2)+Math.Pow(centersVector.Y, 2));
        if(centersDistance >= circle1.Scaled(circle1.Radius)+circle2.Scaled(circle2.Radius))
            return 0;

        if (centersDistance <= Math.Abs(radius1 - radius2)) // One circle inside another
            return Math.PI * Math.Pow(Math.Min(radius1, radius2), 2);
        
        double alpha = 2 * Math.Acos((Math.Pow(radius1, 2) + Math.Pow(centersDistance, 2) - Math.Pow(radius2, 2)) / (2 * radius1 * centersDistance));
        double area1 = 0.5 * Math.Pow(radius1, 2) * (alpha - Math.Sin(alpha));
        double area2 = 0.5 * Math.Pow(radius2, 2) * (alpha - Math.Sin(alpha));
        
        return area1 + area2;
    }

    public static double Circle_Ellipse(Circle circle, Ellipse ellipse) {
        double ex = ellipse.Center.X - circle.Center.X;
        double ey = ellipse.Center.Y - circle.Center.Y;
    
        double a = Math.Pow(ellipse.Width / 2, 2);
        double b = Math.Pow(ellipse.Height / 2, 2);
    
        double distSq = Math.Pow(ex, 2) + Math.Pow(ey, 2);
        double rSq = Math.Pow(circle.Radius, 2);
        if (distSq > rSq + 1e-10)
            return 0;

        if (a * Math.Pow(ex, 2) + b * Math.Pow(ey, 2) <= rSq + 1e-10) {
            return Math.PI * Math.Pow(circle.Radius, 2);
        }
    
        double[] xs = new double[2];
        double[] ys = new double[2];
        double A = Math.Pow(ex, 2) / a + Math.Pow(ey, 2) / b;
        double B = -2 * ex / a;
        double C = -2 * ey / b;
        double delta = B * B - 4 * A * C;
        
        if (delta < 0)
            return 0;
            
        if (delta == 0) {
            xs[0] = -B / (2 * A);
            ys[0] = -C / (2 * A);
        } else {
            xs[0] = (-B + Math.Sqrt(delta)) / (2 * A);
            xs[1] = (-B - Math.Sqrt(delta)) / (2 * A);
            ys[0] = (-C - xs[0] * B) / A / 2;
            ys[1] = (-C - xs[1] * B) / A / 2;
        }
    
        double intersectionArea = 0;
        for (int i = 0; i < 2; i++) {
            if (Math.Pow(xs[i], 2) / a + Math.Pow(ys[i], 2) / b <= 1 + 1e-10) {
                intersectionArea += Math.Pow(circle.Radius, 2) * Math.Acos(xs[i] / circle.Radius) - xs[i] * Math.Sqrt(Math.Pow(circle.Radius, 2) - Math.Pow(xs[i], 2));
            }
        }
    
        return intersectionArea;
    }

    private static double SignedArea(Point[] polygonVertices) {
        double area = 0;

        for (int i = 0; i < polygonVertices.Length; i++) {
            Point currentVertex = polygonVertices[i];
            Point nextVertex = polygonVertices[(i + 1) % polygonVertices.Length];

            area += currentVertex.X * nextVertex.Y - nextVertex.X * currentVertex.Y;
        }

        return 0.5 * area;
    }
    
    private static double AreaOfIntersectionOfCircleWithPolygon(Circle circle, Point[] polygonVertices) {
        if (SignedArea(polygonVertices) < 0) {
            Array.Reverse(polygonVertices);
        }
    
        double area = 0;

        for (int i = 0; i < polygonVertices.Length; i++) {
            Point currentVertex = polygonVertices[i];
            Point nextVertex = polygonVertices[(i + 1) % polygonVertices.Length];
            double distanceToSegment = DistanceToSegment(circle.Center.X, circle.Center.Y, currentVertex.X, currentVertex.Y, nextVertex.X, nextVertex.Y);
            if (distanceToSegment < circle.Radius) {
                double segmentArea = SegmentArea(circle.Radius, distanceToSegment);
                double angle = Angle(circle.Center.X, circle.Center.Y, currentVertex.X, currentVertex.Y, nextVertex.X, nextVertex.Y);
                area += segmentArea * angle;
            }
        }

        return area;
    }

    private static double DistanceToSegment(double x, double y, double x1, double y1, double x2, double y2) {
        double dx = x2 - x1;
        double dy = y2 - y1;
        double t = ((x - x1) * dx + (y - y1) * dy) / (dx * dx + dy * dy);

        if (t < 0) {
            dx = x - x1;
            dy = y - y1;
        } else if (t > 1) {
            dx = x - x2;
            dy = y - y2;
        } else {
            double cx = x1 + t * dx;
            double cy = y1 + t * dy;
            dx = x - cx;
            dy = y - cy;
        }

        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double SegmentArea(double r, double c) {
        double theta = 2 * Math.Acos((r - c) / r);
        double segmentArea = 0.5 * r * r * (theta - Math.Sin(theta));
        return segmentArea;
    }

    private static double Angle(double cx, double cy, double x1, double y1, double x2, double y2) {
        double dx1 = x1 - cx;
        double dy1 = y1 - cy;
        double dx2 = x2 - cx;
        double dy2 = y2 - cy;

        double dot = dx1 * dx2 + dy1 * dy2;
        double cross = dx1 * dy2 - dx2 * dy1;

        return Math.Atan2(cross, dot);
    }
    
    public static double Circle_Triangle(Circle circle, Cone cone) {
        return AreaOfIntersectionOfCircleWithPolygon(circle, cone.Points);
    }

    public static double Circle_Trapeze(Circle circle, TruncatedCone truncatedCone) {
        return AreaOfIntersectionOfCircleWithPolygon(circle, truncatedCone.Points);
    }
    
    private static bool IsInsideEllipse(double x, double y, Ellipse ellipse) {
        double a = ellipse.Width / 2;
        double b = ellipse.Height / 2;
        double dx = x - ellipse.Center.X;
        double dy = y - ellipse.Center.Y;
        return dx * dx / (a * a) + dy * dy / (b * b) <= 1;
    }

    public static double Ellipse_Ellipse(Ellipse ellipse1, Ellipse ellipse2) {
        return AreaOfIntersectionOfPolygons(new EllipseWithPoints(ellipse1), new EllipseWithPoints(ellipse2));
        
        double left = Math.Min(ellipse1.X, ellipse2.X);
        double right = Math.Max(ellipse1.X+ellipse1.Scaled(ellipse1.Width), ellipse2.X+ellipse2.Scaled(ellipse2.Width));
        double top = Math.Min(ellipse1.Y, ellipse2.Y);
        double bottom = Math.Max(ellipse1.Y+ellipse1.Scaled(ellipse1.Height), ellipse2.Y+ellipse2.Scaled(ellipse2.Height));
    
        double intersectionArea = 0;
    
        int steps = 100;
        double dx = (right - left) / steps;
        double dy = (bottom - top) / steps;
        for (int i = 0; i < steps; i++) {
            for (int j = 0; j < steps; j++) {
                double x = left + i * dx + dx / 2;
                double y = top + j * dy + dy / 2;
            
                if (IsInsideEllipse(x, y, ellipse1) && IsInsideEllipse(x, y, ellipse2)) {
                    intersectionArea += dx * dy;
                }
            }
        }
    
        return intersectionArea;
    }

    public static double Ellipse_Triangle(Ellipse ellipse, Cone cone) {
        return AreaOfIntersectionOfPolygons(new EllipseWithPoints(ellipse), cone);
    }

    public static double Ellipse_Trapeze(Ellipse ellipse, TruncatedCone truncatedCone) {
        return AreaOfIntersectionOfPolygons(new EllipseWithPoints(ellipse), truncatedCone);
    }
    
    private static bool IsInside(Point cEdgeStart, Point cEdgeEnd, Point sEdge) {
        return (cEdgeEnd.X - cEdgeStart.X) * (sEdge.Y - cEdgeStart.Y) > (cEdgeEnd.Y - cEdgeStart.Y) * (sEdge.X - cEdgeStart.X);
    }
    
    private static Point ComputeIntersection(Point sEdgeStart, Point sEdgeEnd, Point cEdgeStart, Point cEdgeEnd) {
        double x, y, m1, m2, b1, b2;
        if (sEdgeEnd.X - sEdgeStart.X == 0) {
            x = sEdgeStart.X;
            m2 = (cEdgeEnd.Y - cEdgeStart.Y) / (cEdgeEnd.X - cEdgeStart.X);
            b2 = cEdgeStart.Y - m2 * cEdgeStart.X;
            y = m2 * x + b2;
        } else if (cEdgeEnd.X - cEdgeStart.X == 0) {
            x = cEdgeStart.X;
            m1 = (sEdgeEnd.Y - sEdgeStart.Y) / (sEdgeEnd.X - sEdgeStart.X);
            b1 = sEdgeStart.Y - m1 * sEdgeStart.X;
            y = m1 * x + b1;
        } else {
            m1 = (sEdgeEnd.Y - sEdgeStart.Y) / (sEdgeEnd.X - sEdgeStart.X);
            b1 = sEdgeStart.Y - m1 * sEdgeStart.X;
            m2 = (cEdgeEnd.Y - cEdgeStart.Y) / (cEdgeEnd.X - cEdgeStart.X);
            b2 = cEdgeStart.Y - m2 * cEdgeStart.X;
            x = (b2 - b1) / (m1 - m2);
            y = m1 * x + b1;
        }

        return new Point(x, y);
    }
    
    // Sutherland-Hodgman algorithm: http://en.wikipedia.org/wiki/Sutherland%E2%80%93Hodgman_algorithm
    private static Point[] ClipPolygon(FigureWithPoints subjectPolygon, FigureWithPoints clippingPolygon) {
        Point[] clippingPolygonPoints = clippingPolygon.Points;
        
        List<Point> finalPolygon = new List<Point>(subjectPolygon.Points);
        for(int i = 0; i < clippingPolygonPoints.Length; i++) {
            List<Point> nextPolygon = new List<Point>(finalPolygon);
            finalPolygon = new List<Point>();
            
            Point cEdgeStart = clippingPolygonPoints[i == 0 ? clippingPolygonPoints.Length - 1 : i - 1];
            Point cEdgeEnd = clippingPolygonPoints[i];
            
            for(int j = 0; j < nextPolygon.Count; j++) {
                Point sEdgeStart = nextPolygon[j == 0 ? nextPolygon.Count - 1 : j - 1];
                Point sEdgeEnd = nextPolygon[j];
                
                if(IsInside(cEdgeStart, cEdgeEnd, sEdgeEnd)) {
                    if(!IsInside(cEdgeStart, cEdgeEnd, sEdgeStart)) {
                        Point intersection = ComputeIntersection(sEdgeStart, sEdgeEnd, cEdgeStart, cEdgeEnd);
                        finalPolygon.Add(intersection);
                    }
                    finalPolygon.Add(sEdgeEnd);
                } else if(IsInside(cEdgeStart, cEdgeEnd, sEdgeStart)) {
                    Point intersection = ComputeIntersection(sEdgeStart, sEdgeEnd, cEdgeStart, cEdgeEnd);
                    finalPolygon.Add(intersection);
                }
            }
        }
        
        return finalPolygon.Where(point => !Double.IsNaN(point.X) && !Double.IsNaN(point.Y)).ToArray();
    }

    // Area of a convex polygon: http://www.mathwords.com/a/area_convex_polygon.htm
    private static double AreaOfIntersectionOfPolygons(FigureWithPoints polygon1, FigureWithPoints polygon2) {
        Point[] intersectionPoints = ClipPolygon(polygon1, polygon2);
        if(intersectionPoints.Length < 3) return 0;
        double result = 0;
        for(int i = 0; i < intersectionPoints.Length; i++) {
            result += intersectionPoints[i].X * intersectionPoints[i == intersectionPoints.Length - 1 ? 0 : i + 1].Y - 
                      intersectionPoints[i].Y * intersectionPoints[i == intersectionPoints.Length - 1 ? 0 : i + 1].X;
        }
        return result/2;
    }

    public static double Triangle_Triangle(Cone cone1, Cone cone2) {
        return AreaOfIntersectionOfPolygons(cone1, cone2);
    }

    public static double Triangle_Trapeze(Cone cone, TruncatedCone truncatedCone) {
        return AreaOfIntersectionOfPolygons(cone, truncatedCone);
    }

    public static double Trapeze_Trapeze(TruncatedCone truncatedCone1, TruncatedCone truncatedCone2) {
        return AreaOfIntersectionOfPolygons(truncatedCone1, truncatedCone2);
    }
}
}