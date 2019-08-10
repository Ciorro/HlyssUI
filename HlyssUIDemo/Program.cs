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

            Theme.Load("theme.ini", "light");

            Gui gui = new Gui(window);
            GuiScene scene = new GuiScene(gui);
            gui.PushScene(scene);

            gui.DefaultCharacterSize = 14;

            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            addComponents3(gui);

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
            Box box = new Box();
            gui.CurrentScene.AddChild(box);
            box.Padding = "5px";
            box.Layout = LayoutType.Row;

            Panel panel1 = new Panel();
            panel1.Width = "100px";
            panel1.Height = "200px";
            panel1.Margin = "20px";
            box.AddChild(panel1);

            Panel panel2 = new Panel();
            panel2.Width = "100px";
            panel2.Height = "200px";
            panel2.Margin = "20px";
            box.AddChild(panel2);

            gui.Window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.Right)
                    panel2.Width = $"{panel2.Size.X + 2}px";
                else if (e.Code == Keyboard.Key.Left)
                    panel2.Width = $"{panel2.Size.X - 2}px";
                else if (e.Code == Keyboard.Key.C)
                    Console.Clear();
            };
        }

        private static void addComponents2(Gui gui)
        {
            ScrollArea scrollArea = new ScrollArea();
            scrollArea.Width = "200px";
            scrollArea.Height = "200px";
            gui.CurrentScene.AddChild(scrollArea);

            Box box = new Box();
            box.Padding = "5px";
            box.Layout = LayoutType.Row;
            scrollArea.Content = box;

            Panel panel1 = new Panel();
            panel1.Width = "100px";
            panel1.Height = "200px";
            panel1.Margin = "20px";
            box.AddChild(panel1);

            Panel panel2 = new Panel();
            panel2.Width = "100px";
            panel2.Height = "200px";
            panel2.Margin = "20px";
            box.AddChild(panel2);

        }

        private static void addComponents3(Gui gui)
        {
            ScrollArea scrollArea = new ScrollArea();
            scrollArea.Width = "200px";
            scrollArea.Height = "200px";
            gui.CurrentScene.AddChild(scrollArea);

            CheckBox checkBox = new CheckBox($"CheckBox");
            gui.CurrentScene.AddChild(checkBox);

            Box box = new Box();
            box.Padding = "5px";
            box.Layout = LayoutType.Column;
            scrollArea.Content = box;

            for (int i = 0; i < 20; i++)
            {
                RadioButton radioButton = new RadioButton($"Radio button {i + 1}");
                box.AddChild(radioButton);
            }

            ToggleSwitch toggleSwitch = new ToggleSwitch("ToggleSwitch");
            gui.CurrentScene.AddChild(toggleSwitch);
            toggleSwitch.Top = "100px";

            scrollArea.Left = "50%";
            scrollArea.Top = "50%";

            HScrollBar h = new HScrollBar(4000);
            gui.CurrentScene.AddChild(h);
        }
    }
}
