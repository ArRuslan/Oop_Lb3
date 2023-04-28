using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;

namespace Lib { 
[Serializable]
public class Image : Figure {
    public double Width { get; set; }
    public double Height { get; set; }
    public List<Figure> Figures { get; } = new List<Figure>();
    public string FiguresString {
        get {
            if(Figures.Count == 0) return "";
            string result = "";
            foreach(Figure figure in Figures) {
                result += $"{figure}, ";
            }
            result = result.Trim();
            result = result.Remove(result.Length - 1, 1);
            return result;
        }
    }
    public string FiguresInfoString {
        get {
            if(Figures.Count == 0) return "";
            string result = "";
            foreach(Figure figure in Figures) {
                result += "  "+figure.ToInfoString("  ") + "\n";
            }
            result = result.TrimEnd();
            return result;
        }
    }

    public Image() : this(400, 400) {}
    
    public Image(double width, double height) {
        Width = width;
        Height = height;
    }
    
    public override double GetArea() {
        return Scaled(Width) * Scaled(Height);
    }

    public override double GetPerimeter() {
        return (Scaled(Width) + Scaled(Height)) * 2;
    }
    
    public double GetAreas() {
        double result = 0;
        foreach(Figure figure in Figures) {
            result += figure.GetArea();
        }
        return result;
    }

    public double GetPerimeters() {
        double result = 0;
        foreach(Figure figure in Figures) {
            result += figure.GetPerimeter();
        }
        return result;
    }
    
    public void MoveAll(double x = 0, double y = 0) {
        foreach(Figure figure in Figures) {
            figure.Move(x, y);
        }
    }

    public void MoveAllTo(double x, double y) {
        foreach(Figure figure in Figures) {
            figure.MoveTo(x, y);
        }
    }
    
    public void Merge(Image other) {
        foreach (Figure figure in other.Figures) {
            AddFigure(figure);
        }
    }

    public Figure this[int idx] => Figures[idx];

    public void SaveToFile(string path) {
        BinaryFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, this);
        stream.Close();
    }
    
    public static Image LoadFromFile(string path) {
        BinaryFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return (Image) formatter.Deserialize(stream);
    }
    
    public void AddFigure(Figure figure) {
        Figures.Add(figure);
        figure.ParentImage = this;
    }
    
    public override string ToString() {
        return $"Image(Location = {Location}, Area = {GetArea()}, Perimeter = {GetPerimeter()}, " +
               $"Scale = {Scale}, Figures = [{FiguresString}])";
    }
    
    public override string ToInfoString(string p = "") {
        return $"Image: \n" + 
               $"  Location = {Location}\n" +
               $"  Area = {GetArea()}\n" +
               $"  Perimeter = {GetPerimeter()}\n" +
               $"  Sum of areas = {GetAreas()}\n" +
               $"  Sum of perimeters = {GetPerimeters()}\n" +
               $"  Scale = {Scale}\n" +
               $"  Figures = [\n{FiguresInfoString}\n  ]";
    }
    
    public override void Draw(Canvas cv) {}
}
}