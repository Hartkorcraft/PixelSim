using System;
using System.Collections.Generic;
using SFML;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Linq;
using System.Threading.Tasks;

namespace SFML2.Source
{
    public class World
    {
        public Vector2i Size { get; init; }
        public float GravityRate { get; private set; } = -0.5f;
        public float EnergyLoss { get; private set; } = 0.95f;
        List<MetaBall> MetaBalls = new List<MetaBall>();

        Image ImageDisplay;
        Texture texture;
        Sprite sprite = new Sprite();
        Byte[] pixelBuffer0;
        int pixelIndex(int x, int y) => (x + y * Size.X) * sizeof(uint);
        Color backgroundColor = new Color((byte)(255 * 0.07f), (byte)(255 * 0.13f), (byte)(255 * 0.17f), (byte)(255));

        void OnResize()
        {
            sprite.Position = new Vector2f(
                Game.Window.Size.X / 2 - Size.X / 2,
                Game.Window.Size.Y / 2 - Size.Y / 2);
        }

        void UpdateView()
            => texture.Update(ImageDisplay);


        void Update(float deltaTime)
        {
            UpdateView();

            texture.Update(pixelBuffer0);

            Parallel.For(0, MetaBalls.Count, i =>
              {
                  MetaBalls[i].Pos += new Vector2f(MetaBalls[i].Vel.X, MetaBalls[i].Vel.Y);
                  MetaBalls[i].Vel += new Vector2f(0, GravityRate);

                  if (MetaBalls[i].Pos.Y < 0)
                  {
                      MetaBalls[i].Pos = new Vector2f(MetaBalls[i].Pos.X, 1);
                      MetaBalls[i].Vel = new Vector2f(MetaBalls[i].Vel.X, -MetaBalls[i].Vel.Y * EnergyLoss);
                  }
                  else if (MetaBalls[i].Pos.Y > this.Size.X)
                  {
                      MetaBalls[i].Pos = new Vector2f(MetaBalls[i].Pos.X, this.Size.X - 1);
                      MetaBalls[i].Vel = new Vector2f(MetaBalls[i].Vel.X, -MetaBalls[i].Vel.Y * EnergyLoss);
                  }

                  if (MetaBalls[i].Pos.X < 0)
                  {
                      MetaBalls[i].Pos = new Vector2f(1, MetaBalls[i].Pos.Y);
                      MetaBalls[i].Vel = new Vector2f(-MetaBalls[i].Vel.X * EnergyLoss, MetaBalls[i].Vel.Y);
                  }
                  else if (MetaBalls[i].Pos.X > this.Size.X)
                  {
                      MetaBalls[i].Pos = new Vector2f(this.Size.X - 1, MetaBalls[i].Pos.Y);
                      MetaBalls[i].Vel = new Vector2f(-MetaBalls[i].Vel.X * EnergyLoss, MetaBalls[i].Vel.Y);
                  }
              });

        }

        void Draw(RenderWindow window)
        {
            ImageDisplay = new Image(ImageDisplay.Size.X, ImageDisplay.Size.Y, Color.Cyan);

            Parallel.For(0, Size.Y, y =>
             {
                 Parallel.For(0, Size.X, x =>
                 {
                     var _pixelIndex = pixelIndex(x, y);
                     var newColor = Color.Red;
                     var pixelColor = backgroundColor;
                     float funcSum = 0;
                     for (int i = 0; i < MetaBalls.Count; i++)
                     {
                         funcSum += (MetaBalls[i].radius) / MetaBalls[i].ObjectFunc.Invoke((x, y)); // Kosztowne ale to robi metaballs
                         newColor.B += (byte)(100000 * funcSum); // To daje fajny szum
                     }

                     if (funcSum >= 1)
                         pixelColor = new Color(newColor.R, newColor.G, newColor.B, (byte)255);

                     pixelBuffer0[_pixelIndex] = pixelColor.R;
                     pixelBuffer0[_pixelIndex + 1] = pixelColor.G;
                     pixelBuffer0[_pixelIndex + 2] = pixelColor.B;
                     pixelBuffer0[_pixelIndex + 3] = pixelColor.A;

                 });
             });

            texture.Update(pixelBuffer0);
            Game.Window.Draw(sprite);
        }

        public World(Vector2i _size)
        {
            Game.OnUpdate += Update;
            Game.OnDraw += Draw;
            Game.OnResize += OnResize;

            Size = _size;

            texture = new Texture(((uint)Size.X), ((uint)Size.Y));

            sprite = new Sprite(texture);
            sprite.TextureRect = new IntRect(0, (int)Size.Y, (int)Size.X, -(int)Size.Y);
            sprite.Position = new Vector2f(
                Game.Window.Size.X / 2 - Size.X / 2,
                Game.Window.Size.Y / 2 - Size.Y / 2);

            ImageDisplay = new Image(((uint)Size.X), ((uint)Size.Y), Color.Blue);

            pixelBuffer0 = new Byte[Size.X * Size.Y * 4];

            var color = Color.Cyan;

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    var _pixelIndex = pixelIndex(x, y);
                    pixelBuffer0[_pixelIndex] = color.R;
                    pixelBuffer0[_pixelIndex + 1] = color.G;
                    pixelBuffer0[_pixelIndex + 2] = color.B;
                    pixelBuffer0[_pixelIndex + 3] = color.A;
                }
            }

            MetaBalls = new List<MetaBall>()
            {
              new MetaBall(new Vector2f(50,50),10),
              new MetaBall(new Vector2f(50,50),20),
              new MetaBall(new Vector2f(50,50),30),
              new MetaBall(new Vector2f(50,50),40),
              new MetaBall(new Vector2f(50,50),50),
              new MetaBall(new Vector2f(50,50),60),
              new MetaBall(new Vector2f(50,50),70),
              new MetaBall(new Vector2f(50,50),80),
              new MetaBall(new Vector2f(50,50),100),
              new MetaBall(new Vector2f(50,50),200),
              new MetaBall(new Vector2f(50,50),300),
              new MetaBall(new Vector2f(50,50),250),
              new MetaBall(new Vector2f(50,50),500),
              new MetaBall(new Vector2f(50,50),100),
              new MetaBall(new Vector2f(50,50),150),
              new MetaBall(new Vector2f(50,50),80),
              new MetaBall(new Vector2f(50,50),10),
              new MetaBall(new Vector2f(50,50),20),
              new MetaBall(new Vector2f(50,50),30),
              new MetaBall(new Vector2f(50,50),40),
              new MetaBall(new Vector2f(50,50),50),
              new MetaBall(new Vector2f(50,50),60),
              new MetaBall(new Vector2f(50,50),70),
              new MetaBall(new Vector2f(50,50),80),
              new MetaBall(new Vector2f(50,50),100),
              new MetaBall(new Vector2f(50,50),200),
              new MetaBall(new Vector2f(50,50),300),
              new MetaBall(new Vector2f(50,50),400),
              new MetaBall(new Vector2f(50,50),130),
              new MetaBall(new Vector2f(50,50),120),
              new MetaBall(new Vector2f(50,50),100),
              new MetaBall(new Vector2f(50,50),500),
            };

            foreach (var metaBall in MetaBalls)
            {
                metaBall.Vel = new Vector2f((float)Utils.NextDouble(-20, 20), (float)Utils.NextDouble(-20, 20));
            }

        }
    }
}