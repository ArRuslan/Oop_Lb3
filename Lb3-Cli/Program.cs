﻿using System;

namespace Lb3_Cli {
class Program {
    public static void Main(string[] args) {
        TruncatedCone tc = new TruncatedCone(50, 25, 10);
        tc.Scale = 5;
        Console.WriteLine(tc);
    } 
}
}