using HlyssUI;
using HlyssUI.Components;
using HlyssUI.Components.Routers;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HlyssUIDemo
{
    class Program
    {
        static string caption = "HlyssUI demo";

        static void Main(string[] args)
        {
            Vector2u winSize = new Vector2u(1280, 720);
            //Vector2u winSize = new Vector2u(630, 380);

            ContextSettings contextSettings = new ContextSettings();
            contextSettings.AntialiasingLevel = 8;

            RenderWindow window = new RenderWindow(new VideoMode(winSize.X, winSize.Y), caption, Styles.Default, contextSettings);
            //window.SetFramerateLimit(60);
            //window.SetVerticalSyncEnabled(true);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "light");

            HlyssApp app = new HlyssApp(window);
            app.Root.AddChild(new BasicRouter()
            {
                Name = "router"
            });

            (app.Root.GetChild("router") as Router).Navigate(Test());

            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    HlyssApp.Debug = !HlyssApp.Debug;
                if (e.Code == Keyboard.Key.C)
                    Console.Clear();
            };

            window.MouseButtonPressed += (object sender, MouseButtonEventArgs e) =>
            {
                //if (e.Button == Mouse.Button.XButton1)
                //    gui.Navigator.Back();
            };

            while (window.IsOpen)
            {
                window.Clear(Theme.GetColor("Primary"));
                window.DispatchEvents();

                app.Update();
                app.Draw();

                window.Display();

                fps++;
                if (fpsTimer.ElapsedMilliseconds >= 1000)
                {
                    window.SetTitle($"{caption} ({fps} fps)");
                    fps = 0;
                    fpsTimer.Restart();
                }
            }

            HlyssUI.Utils.Logger.SaveLog();
        }

        static Component Test()
        {
            return new Panel()
            {
                Width = "100%",
                Height = "100%",
                Padding = "5px",
                Children = new List<Component>()
                {
                    new Button("Przycisk 1")
                    {
                        Name = "Przycisk 1",
                        Appearance = Button.ButtonStyle.Filled
                    },
                    new CheckBox("Pole wyboru 1")
                    {
                        Name = "Pole wyboru 1"
                    },
                    new Panel()
                    {
                        Width = "100px",
                        Height = "100px",
                        Name = "Panel 1",
                        Children = new List<Component>()
                        {
                            new Label("Label 1")
                            {
                                Name = "Label 1"
                            }
                        }
                    }
                }
            };
        }
    }
}
