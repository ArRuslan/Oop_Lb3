using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Clipper2Lib;
using FillRule = System.Windows.Media.FillRule;

// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace Lib {
[Serializable]
public abstract class Figure {
    public Point Location = new Point();
    public Image ParentImage { get; set; }
    public double Scale { get; set; } = 1;
    public double X => (ParentImage?.Scale ?? 1) * (ParentImage?.Location.X ?? 0) + Location.X * (ParentImage?.Scale ?? 1);
    public double Y => (ParentImage?.Scale ?? 1) * (ParentImage?.Location.Y ?? 0) + Location.Y * (ParentImage?.Scale ?? 1);

    public void Move(double x = 0, double y = 0) {
        Location.X += x;
        Location.Y += y;
    }
    
    public void MoveTo(double x, double y) {
        Location.X = x;
        Location.Y = y;
    }
    
    public abstract double GetArea();
    
    public virtual double GetPerimeter() {
        return 0;
    }

    public virtual double Scaled(double val) {
        return Scale * val * (ParentImage?.Scale ?? 1);
    }
    
    public override string ToString() {
        return ToString("");
    }
    
    public virtual string ToString(string p) {
        return $"{p}{GetType().Name}:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  Area = {GetArea()}\n" +
               $"{p}{p}  Perimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}";
    }
    
    public static Figure Get(string name) {
        switch (name) {
            case "Circle":
                return new Circle();
            case "FilledCircle":
                return new FilledCircle();
            case "Ellipse":
                return new Ellipse();
            case "Cone":
                return new Cone();
            case "TruncatedCone":
                return new TruncatedCone();
            default:
                return new Circle();
        }
    }
    
    public abstract void Draw(Canvas canvas);

    public Paths64 ToPaths64() {
        Paths64 result = new Paths64();
        Path64 path = new Path64();
        FigureWithPoints f;
        if(this is Circle) f = new CircleWithPoints((Circle)this);
        else if(this is Ellipse) f = new EllipseWithPoints((Ellipse)this);
        else f = (FigureWithPoints)this;
        foreach(Point p in f.Points) {
            path.Add(p.ToPoint64());
        }
        result.Add(path);
        return result;
    }
}

[Serializable]
public class Point {
    public Point() : this(0, 0) {}
    
    public Point(double x, double y) {
        X = x;
        Y = y;
    }

    public double X { get; set; }
    public double Y { get; set; }
    
    public override string ToString() {
        return $"Point(X = {X}, Y = {Y})";
    } 
    
    public Point64 ToPoint64() {
        return new Point64(X, Y);
    }
}

[Serializable]
public class Circle : Figure {
    public Circle() : this(25) {}

    public Circle(double radius) {
        Radius = radius;
    }
    
    public double Radius { get; set; }
    public Point Center => new Point(X + Scaled(Radius), Y + Scaled(Radius));
    
    public override double GetArea() {
        return Math.PI * Scaled(Radius) * Scaled(Radius);
    }

    public override double GetPerimeter() {
        return 2 * Math.PI * Scaled(Radius);
    }

    public override string ToString(string p) {
        return $"{p}Circle:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  Area = {GetArea()}\n" +
               $"{p}{p}  Perimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}\n" +
               $"{p}{p}  Radius = {Radius}";
    }
    
    public override void Draw(Canvas canvas) {
        System.Windows.Shapes.Ellipse circle = new System.Windows.Shapes.Ellipse() {
            Width = Scaled(Radius)*2,
            Height = Scaled(Radius)*2,
            Stroke = Brushes.Black,
            StrokeThickness = 3
        };
        canvas.Children.Add(circle);
        circle.SetValue(Canvas.LeftProperty, X);
        circle.SetValue(Canvas.TopProperty, Y);
    }
}

[Serializable]
public class FilledCircle : Circle {
    public override string ToString(string p) {
        return $"{p}FilledCircle:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  Area = {GetArea()}\n" +
               $"{p}{p}  Perimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}\n" +
               $"{p}{p}  Radius = {Radius}";
    }
    
    public override void Draw(Canvas canvas) {
        System.Windows.Shapes.Ellipse filledCircle = new System.Windows.Shapes.Ellipse() {
            Width = Scaled(Radius)*2,
            Height = Scaled(Radius)*2,
            Stroke = Brushes.Black,
            StrokeThickness = 3,
            Fill = Brushes.Black
        };
        canvas.Children.Add(filledCircle);
        filledCircle.SetValue(Canvas.LeftProperty, X);
        filledCircle.SetValue(Canvas.TopProperty, Y);
    }
}

[Serializable]
public class Ellipse : Figure {
    public Ellipse() : this(50, 75) {}
    
    public Ellipse(double width, double height) {
        Width = width;
        Height = height;
    }
    
    public double Width { get; set; }
    public double Height { get; set; }
    public Point Center => new Point(X + Scaled(Width)/2, Y + Scaled(Height)/2);
    
    public override double GetArea() {
        return Math.PI * Scaled(Width/2) * Scaled(Height/2);
    }

    public override double GetPerimeter() {
        return Math.PI * Math.Sqrt(Math.Pow(Scaled(Width), 2) + Math.Pow(Scaled(Height), 2));
    }

    public override string ToString(string p) {
        return $"{p}Ellipse:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  Area = {GetArea()}\n" +
               $"{p}{p}  Perimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}\n" +
               $"{p}{p}  Width = {Width}\n" +
               $"{p}{p}  Height = {Height}";
    }
    
    public override void Draw(Canvas canvas) {
        System.Windows.Shapes.Ellipse ellipse = new System.Windows.Shapes.Ellipse() {
            Width = Scaled(Width),
            Height = Scaled(Height),
            Stroke = Brushes.Black,
            StrokeThickness = 3
        };
        canvas.Children.Add(ellipse);
        ellipse.SetValue(Canvas.LeftProperty, X);
        ellipse.SetValue(Canvas.TopProperty, Y);
    }
}

public interface FigureWithPoints {
    Point[] Points { get; }
}

public class EllipseWithPoints : FigureWithPoints {
    private Point[] _points;
    public Point[] Points => _points;
    private const int NUMBER_OF_POINTS = 100;
    
    public EllipseWithPoints(Ellipse ellipse) {
        List<Point> points = new List<Point>();
        double step = 2 * Math.PI / NUMBER_OF_POINTS;
        for (double theta = 0; theta < 2 * Math.PI; theta += step) {
            double x = ellipse.Center.X + ellipse.Scaled(ellipse.Width/2) * Math.Cos(theta);
            double y = ellipse.Center.Y + ellipse.Scaled(ellipse.Height/2) * Math.Sin(theta);
            points.Add(new Point(x, y));
        }
        _points = points.ToArray();
    }
}

public class CircleWithPoints : FigureWithPoints {
    private Point[] _points;
    public Point[] Points => _points;
    private const int NUMBER_OF_POINTS = 100;
        
    public CircleWithPoints(Circle circle) {
        List<Point> points = new List<Point>();
        double step = 2 * Math.PI / NUMBER_OF_POINTS;
        for (double theta = 0; theta < 2 * Math.PI; theta += step) {
            double x = circle.Center.X + circle.Scaled(circle.Radius) * Math.Cos(theta);
            double y = circle.Center.Y + circle.Scaled(circle.Radius) * Math.Sin(theta);
            points.Add(new Point(x, y));
        }
        _points = points.ToArray();
    }
}

[Serializable]
public class Cone : Figure, FigureWithPoints {
    public double Radius { get; set; }
    public double Height { get; set; }
    
    public double Volume => Math.PI * Math.Pow(Scaled(Radius), 2) * Scaled(Height) / 3;
    public Point[] Points => new [] {
        new Point(X + Scaled(Radius), Y),
        new Point(X, Y + Scaled(Height)),
        new Point(X + Scaled(Radius) * 2, Y + Scaled(Height)),
    };

    public Cone() : this(50, 100) {}
    
    public Cone(double radius, double height) {
        Radius = radius;
        Height = height;
    }
    
    public override double GetArea() {
        return Scaled(Radius) * Scaled(Height); // Площа проекції (трикутника)
    }
    
    public override double GetPerimeter() {
        double c = Math.Sqrt(Math.Pow(Scaled(Radius), 2) + Math.Pow(Scaled(Height), 2)); // Сторона трикутника (проекції)
        return Scaled(Radius) + 2 * c;
    }

    public override string ToString(string p) {
        return $"{p}Cone:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  Area = {GetArea()}\n" +
               $"{p}{p}  Perimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}\n" +
               $"{p}{p}  Radius = {Radius}\n" +
               $"{p}{p}  Height = {Height}\n" +
               $"{p}{p}  Volume = {Volume}";
    }
    
    public override void Draw(Canvas canvas) {
        Path triangle = new Path() {
            Stroke = Brushes.Black,
            StrokeThickness = 3,
            Fill = Brushes.Black
        };
        StreamGeometry geometry = new StreamGeometry {
            FillRule = FillRule.EvenOdd
        };
        
        Point[] points = Points;

        using(StreamGeometryContext ctx = geometry.Open()) {
            ctx.BeginFigure(new System.Windows.Point(points[0].X, points[0].Y), true, true);
            ctx.LineTo(new System.Windows.Point(points[1].X, points[1].Y), true, false);
            ctx.LineTo(new System.Windows.Point(points[2].X, points[2].Y), true, false);
        }
        geometry.Freeze();
        triangle.Data = geometry;
        
        canvas.Children.Add(triangle);
    }
}

[Serializable]
public class TruncatedCone : Figure, FigureWithPoints {
    public double RadiusTop { get; set; }
    public double RadiusBottom { get; set; }
    public double Height { get; set; }
        
    public double Volume => Math.PI * Scaled(Height) * (Math.Pow(Scaled(RadiusTop), 2) + Scaled(RadiusTop) * Scaled(RadiusBottom) + Math.Pow(Scaled(RadiusBottom), 2)) / 3;
    public Point[] Points => new [] {
        new Point(X + (Scaled(RadiusBottom) - Scaled(RadiusTop)), Y),
        new Point(X + (Scaled(RadiusBottom) - Scaled(RadiusTop)) + Scaled(RadiusTop) * 2, Y),
        new Point(X + Scaled(RadiusBottom) * 2, Y + Scaled(Height)),
        new Point(X, Y + Scaled(Height)),
    };

    public TruncatedCone() : this(50, 25, 50) {}

    public TruncatedCone(double radiusBottom, double radiusTop, double height) {
        RadiusTop = radiusTop;
        RadiusBottom = radiusBottom;
        Height = height;
    }
        
    public override double GetArea() {
        return (Scaled(RadiusTop) * 2 + Scaled(RadiusBottom) * 2) / 2 * Scaled(Height); // Площа проекції (трапеції)
    }
        
    public override double GetPerimeter() {
        double c = Math.Sqrt(Math.Pow(Scaled(RadiusBottom) - Scaled(RadiusTop), 2) + Math.Pow(Scaled(Height), 2)); // Сторона трапеції (проекції)
        return Scaled(RadiusTop) * 2 + Scaled(RadiusBottom) * 2 + 2 * c;
    }

    public override string ToString(string p) {
        return $"{p}TruncatedCone:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  ProjectionArea = {GetArea()}\n" +
               $"{p}{p}  ProjectionPerimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}\n" +
               $"{p}{p}  RadiusTop = {RadiusTop}\n" +
               $"{p}{p}  RadiusBottom = {RadiusBottom}\n" +
               $"{p}{p}  Height = {Height}\n" +
               $"{p}{p}  Volume = {Volume}";
    }

    public override void Draw(Canvas canvas) {
        Path trapeze = new Path() {
            Stroke = Brushes.Black,
            StrokeThickness = 3,
            Fill = Brushes.Black
        };
        StreamGeometry geometry = new StreamGeometry {
            FillRule = FillRule.EvenOdd
        };

        Point[] points = Points;

        using(StreamGeometryContext ctx = geometry.Open()) {
            ctx.BeginFigure(new System.Windows.Point(points[0].X, points[0].Y), true, true);
            ctx.LineTo(new System.Windows.Point(points[1].X, points[1].Y), true, false);
            ctx.LineTo(new System.Windows.Point(points[2].X, points[2].Y), true, false);
            ctx.LineTo(new System.Windows.Point(points[3].X, points[3].Y), true, false);
        }
        geometry.Freeze();
        trapeze.Data = geometry;
            
        canvas.Children.Add(trapeze);
    }
}

}