using System;
using System.Collections.Generic;
using SFML;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace SFML2.Source
{
    public static class Game
    {
        public const int FramerateLimit = 60;
        public static float deltaTime = 0.1f;
        public static float frameRate { get { try { return 1.0f / deltaTime; } catch { return 0.001f; } } }

        public static RenderWindow Window { get; } = new RenderWindow(new VideoMode(1000, 1000), "XD");
        public static Random rng = new Random();

        public static Action<float>? OnUpdate { get; set; }
        public static Action<RenderWindow>? OnDraw { get; set; }
        public static Action? OnResize { get; set; }

        static Clock dtclock = new Clock();
        static World world;
        static float Zoom { get => zoom; set { zoom = value; Window.GetView().Zoom(value); } }
        static float zoom = 0.4f;

        static Game()
        {
            Window.Closed += new EventHandler(OnCloseWindow!);
            Window.Resized += new EventHandler<SizeEventArgs>(Resized!);

            Window.SetFramerateLimit(FramerateLimit);
            //zoom = 1;

            var windowSize = new SizeEvent();
            windowSize.Width = Window.Size.X;
            windowSize.Height = Window.Size.Y;
            Resized(null!, new SizeEventArgs(windowSize));

            world = new World(new Vector2i(250, 250));
        }

        public static void Resized(object sender, SizeEventArgs e)
        {
            FloatRect visibleArea = new FloatRect(0, 0, e.Width, e.Height);
            var newView = new View(visibleArea);
            newView.Zoom(zoom);
            Window.SetView(newView);
            OnResize?.Invoke();
        }

        public static void Update()
        {
            while (Window.IsOpen)
            {
                deltaTime = dtclock.Restart().AsSeconds();
                Window.DispatchEvents();
                Window.Clear(Color.Black);
                OnUpdate?.Invoke(deltaTime);

                Game.Draw();
                Window.Display();
            }
        }

        public static void Draw()
        {
            OnDraw?.Invoke(Window);
        }

        static void OnCloseWindow(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
    }
}