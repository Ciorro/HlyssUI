using HlyssUI;
using HlyssUI.Components;
using HlyssUI.Layout;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;

namespace HlyssUIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ContextSettings settings = new ContextSettings(1, 1, 2);

            RenderWindow window = new RenderWindow(new VideoMode(1366, 768), "HlyssUI demo", Styles.Default, settings);
            //window.SetFramerateLimit(300);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "dark");

            Gui gui = new Gui(window);
            GuiScene scene = new GuiScene(gui);
            gui.PushScene(scene);

            gui.DefaultCharacterSize = 14;

            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            addComponents1(gui);

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    gui.Debug = !gui.Debug;
            };

            while (window.IsOpen)
            {
                window.Clear(Theme.GetColor("Primary"));
                window.DispatchEvents();

                gui.Update();
                gui.Draw();

                window.Display();

                fps++;
                if (fpsTimer.ElapsedMilliseconds >= 1000)
                {
                    window.SetTitle($"HlyssUI demo ({fps} fps)");
                    fps = 0;
                    fpsTimer.Restart();
                }
            }

            HlyssUI.Utils.Logger.SaveLog();
        }

        private static void addComponents1(Gui gui)
        {
            Panel panel1 = new Panel();
            gui.CurrentScene.AddChild(panel1);

            panel1.Width = "50%";
            panel1.Height = "25%";
            panel1.Left = "10%";
            panel1.Top = "200px";
            panel1.Padding = "50px";

            Panel panel2 = new Panel();
            panel1.AddChild(panel2);

            panel2.Width = "50px";
            panel2.Height = "25px";
            //panel2.MarginLeft = "50%";
            //panel2.MarginTop = "50%";
        }
    }
}
