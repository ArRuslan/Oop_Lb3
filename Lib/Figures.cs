using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lib {
[Serializable]
public abstract class Figure {
    public Point Location = new Point();
    public Image ParentImage { get; set; } = null;
    public double Scale { get; set; } = 1;
    public double X => Scaled(Location.X) + (ParentImage != null ? ParentImage.X : 0);
    public double Y => Scaled(Location.Y) + (ParentImage != null ? ParentImage.Y : 0);

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
        return Scale * val * (ParentImage != null ? ParentImage.Scale : 1);
    }
    
    public override string ToString() {
        return $"{GetType().Name}(Location = {Location}, Area = {GetArea()}, Perimeter = {GetPerimeter()}, " +
               $"Scale = {Scale})";
    }
    
    public virtual string ToInfoString(string p = "") {
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
}

[Serializable]
public class Circle : Figure {
    public Circle() : this(50) {}

    public Circle(double radius) {
        Radius = radius;
    }
    
    public double Radius { get; set; }
    
    public override double GetArea() {
        return Math.PI * Scaled(Radius) * Scaled(Radius);
    }

    public override double GetPerimeter() {
        return 2 * Math.PI * Scaled(Radius);
    }
    
    public override string ToString() {
        return $"Circle(Location = {Location}, Area = {GetArea()}, Perimeter = {GetPerimeter()}, " +
               $"Scale = {Scale}, Radius = {Radius})";
    }
    
    public override string ToInfoString(string p = "") {
        return $"{p}Circle:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  Area = {GetArea()}\n" +
               $"{p}{p}  Perimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}\n" +
               $"{p}{p}  Radius = {Radius}";
    }
    
    public override void Draw(Canvas canvas) {
        System.Windows.Shapes.Ellipse circle = new System.Windows.Shapes.Ellipse() {
            Width = Scaled(Radius),
            Height = Scaled(Radius),
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
    public FilledCircle() : this(0) {}
    
    public FilledCircle(long color) {
        Color = color;
    }

    public long Color { get; set; }
    
    public override string ToString() {
        return $"FilledCircle(Location = {Location}, Area = {GetArea()}, Perimeter = {GetPerimeter()}, " +
               $"Scale = {Scale}, Radius = {Radius}, Color = {Color})";
    }
    
    public override string ToInfoString(string p = "") {
        return $"{p}FilledCircle:\n" +
               $"{p}{p}  Location = {Location}\n" +
               $"{p}{p}  Area = {GetArea()}\n" +
               $"{p}{p}  Perimeter = {GetPerimeter()}\n" +
               $"{p}{p}  Scale = {Scale}\n" +
               $"{p}{p}  Radius = {Radius}\n" +
               $"{p}{p}  Color = {Color}";
    }
    
    public override void Draw(Canvas canvas) {
        System.Windows.Shapes.Ellipse filledCircle = new System.Windows.Shapes.Ellipse() {
            Width = Scaled(Radius),
            Height = Scaled(Radius),
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
    
    public override double GetArea() {
        return Math.PI * Scaled(Width) * Scaled(Height);
    }

    public override double GetPerimeter() {
        return Math.PI * Math.Sqrt(Math.Pow(Scaled(Width), 2) + Math.Pow(Scaled(Height), 2));
    }
    
    public override string ToString() {
        return $"Ellipse(Location = {Location}, Area = {GetArea()}, Perimeter = {GetPerimeter()}, " +
               $"Scale = {Scale}, Width = {Width}, Height = {Height})";
    }
    
    public override string ToInfoString(string p = "") {
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

[Serializable]
public class Cone : Figure {
    public double Radius { get; set; }
    public double Height { get; set; }
    
    public double Volume => Math.PI * Math.Pow(Scaled(Radius), 2) * Scaled(Height) / 3;

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
    
    public override string ToString() {
        return $"Cone(Location = {Location}, ProjectionArea = {GetArea()}, ProjectionPerimeter = {GetPerimeter()}, " +
               $"Scale = {Scale}, Radius = {Radius}, Height = {Height}, Volume = {Volume})";
    }
    
    public override string ToInfoString(string p = "") {
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
        StreamGeometry geometry = new StreamGeometry();
        geometry.FillRule = FillRule.EvenOdd;
        
        using(StreamGeometryContext ctx = geometry.Open()) {
            ctx.BeginFigure(new System.Windows.Point(X + Scaled(Radius), Y), true, true);
            ctx.LineTo(new System.Windows.Point(X + Scaled(Radius) * 2, Y + Scaled(Height)), true, false);
            ctx.LineTo(new System.Windows.Point(X, Y + Scaled(Height)), true, false);
        }
        geometry.Freeze();
        triangle.Data = geometry;
        
        canvas.Children.Add(triangle);
    }
}

[Serializable]
public class TruncatedCone : Figure {
    public double RadiusTop { get; set; }
    public double RadiusBottom { get; set; }
    public double Height { get; set; }
        
    public double Volume => Math.PI * Scaled(Height) * (Math.Pow(Scaled(RadiusTop), 2) + Scaled(RadiusTop) * Scaled(RadiusBottom) + Math.Pow(Scaled(RadiusBottom), 2)) / 3;

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
    
    public override string ToString() {
        return $"TruncatedCone(Location = {Location}, ProjectionArea = {GetArea()}, " +
               $"ProjectionPerimeter = {GetPerimeter()}, Scale = {Scale}, RadiusTop = {RadiusTop}, " +
               $"RadiusBottom = {RadiusBottom}, Height = {Height}, Volume = {Volume})";
    }
    
    public override string ToInfoString(string p = "") {
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
        StreamGeometry geometry = new StreamGeometry();
        geometry.FillRule = FillRule.EvenOdd;
            
        using(StreamGeometryContext ctx = geometry.Open()) {
            ctx.BeginFigure(new System.Windows.Point(X + (Scaled(RadiusBottom) - Scaled(RadiusTop)), Y), true, true);
            ctx.LineTo(new System.Windows.Point(X + (Scaled(RadiusBottom) - Scaled(RadiusTop)) + Scaled(RadiusTop) * 2, Y), true, false);
            ctx.LineTo(new System.Windows.Point(X + Scaled(RadiusBottom) * 2, Y + Scaled(Height)), true, false);
            ctx.LineTo(new System.Windows.Point(X, Y + Scaled(Height)), true, false);
        }
        geometry.Freeze();
        trapeze.Data = geometry;
            
        canvas.Children.Add(trapeze);
    }
}

}