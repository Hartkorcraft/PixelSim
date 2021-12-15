using System;
using System.Collections.Generic;


namespace SFML2.Source
{
    public static class Utils
    {
        public static readonly Random rng = new Random();

        public static double NextDouble(double minimum, double maximum)
            => rng.NextDouble() * (maximum - minimum) + minimum;

    }
}