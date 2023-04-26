using System;

namespace Lb3_Cli {
class Program {
    public static void Main(string[] args) {
        TruncatedCone tc = new TruncatedCone(50, 25, 10);
        tc.Scale = 5;
        
        Image image = new Image();
        image.Figures.Add(tc);
        Console.WriteLine(image);
        image.SaveToFile("serializationTest");
        
        Image deserializedImage = Image.LoadFromFile("serializationTest");
        Console.WriteLine(deserializedImage);
        Console.WriteLine(deserializedImage.ToString() == image.ToString());
    } 
}
}