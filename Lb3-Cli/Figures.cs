using System;

namespace Lb3_Cli {
public abstract class Figure {
    public Point Location = new Point();
    public double Scale { get; set; } = 1;
    
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
        return Scale * val;
    }
    
    public override string ToString() {
        return $"{GetType().Name}(Location = {Location}, Area = {GetArea()}, Perimeter = {GetPerimeter()}, " +
               $"Scale = {Scale})";
    }
}

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

class Circle : Figure {
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
}

class FilledCircle : Circle {
    public FilledCircle() : this(0) {}
    
    public FilledCircle(long color) {
        Color = color;
    }

    public long Color { get; set; }
    
    public override string ToString() {
        return $"FilledCircle(Location = {Location}, Area = {GetArea()}, Perimeter = {GetPerimeter()}, " +
               $"Scale = {Scale}, Radius = {Radius}, Color = {Color})";
    }
}

class Ellipse : Figure {
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
}

class Cone : Figure {
    public double Radius { get; set; }
    public double Height { get; set; }
    
    public double Volume => Math.PI * Math.Pow(Scaled(Radius), 2) * Scaled(Height) / 3;

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
}

class TruncatedCone : Figure {
    public double RadiusTop { get; set; }
    public double RadiusBottom { get; set; }
    public double Height { get; set; }
        
    public double Volume => Math.PI * Scaled(Height) * (Math.Pow(Scaled(RadiusTop), 2) + Scaled(RadiusTop) * Scaled(RadiusBottom) + Math.Pow(Scaled(RadiusBottom), 2)) / 3;

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
}

}