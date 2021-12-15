using System;
using System.Collections.Generic;
using SFML;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace SFML2.Source
{
    public class MetaBall
    {
        public Func<(float, float), float> ObjectFunc;
        public Vector2f Pos { get; set; } = new Vector2f(0, 0);
        public Vector2f Vel { get; set; } = new Vector2f(0, 0);
        public float radius { get; } = 400;
        
        public MetaBall(Vector2f _pos, float _radius)
        {
            Pos = _pos;
            radius = _radius;
            ObjectFunc = CircleFunction;
        }

        public static bool InRange(float value, float checkingFor, float dif = 0.2f)
           => checkingFor - dif < value && checkingFor + dif > value;

        public float CircleFunction((float X, float Y) check)
            => MathF.Pow(check.X - Pos.X, 2) + MathF.Pow(check.Y - Pos.Y, 2);

    }
}