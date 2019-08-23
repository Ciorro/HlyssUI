﻿using HlyssUI;
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
            ContextSettings settings = new ContextSettings(1, 1, 8);

            RenderWindow window = new RenderWindow(new VideoMode(1366, 768), "HlyssUI demo", Styles.Default, settings);
            window.SetFramerateLimit(1);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "light");

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
            Panel panel = new Panel();
            gui.CurrentScene.AddChild(panel);
            panel.Padding = "5px";
            //box.Width = "50%";
            //box.Height = "200px";
            panel.Layout = LayoutType.Row;
            panel.Autosize = true;
            panel.Name = "panel";

            Panel panel1 = new Panel();
            panel1.Width = "100px";
            panel1.Height = "200px";
            panel1.Margin = "20px";
            panel1.CenterContent = true;
            panel.AddChild(panel1);
            panel1.Name = "panel1";

            Panel panel2 = new Panel();
            panel2.Width = "100px";
            panel2.Height = "200px";
            panel2.Left = "50%";
            panel.AddChild(panel2);
            panel2.Name = "panel2";

            Panel panel3 = new Panel();
            panel3.Width = "90%";
            panel3.Height = "40%";
            panel1.AddChild(panel3);
            panel3.Name = "panel3";

            Panel panel4 = new Panel();
            panel4.Width = "50%";
            panel4.Height = "40px";
            panel1.AddChild(panel4);
            panel4.Name = "panel4";

            panel1.Layout = LayoutType.Column;

            Panel panel5 = new Panel();
            panel5.Autosize = true;
            panel.AddChild(panel5);
            //panel5.CenterContent = true;
            panel5.Name = "panel5";

            Panel panel6 = new Panel();
            panel6.Width = "50px";
            panel6.Height = "150px";
            panel6.Margin = "5px";
            panel6.Name = "panel6";

            panel5.AddChild(panel6);

            Panel panel7 = new Panel();
            panel7.Width = "80px";
            panel7.Height = "70px";
            panel7.Margin = "5px";
            panel.AddChild(panel7);
            panel7.Name = "panel7";

            Panel panel8 = new Panel();
            panel8.Width = "80px";
            panel8.Height = "70px";
            panel8.Margin = "5px";
            panel5.AddChild(panel8);
            panel8.Name = "panel8";

            panel5.Margin = "2px";
            panel5.Padding = "3px";

            gui.Window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.Right)
                    panel6.Width = $"{panel6.Size.X + 2}px";
                else if (e.Code == Keyboard.Key.Left)
                    panel6.Width = $"{panel6.Size.X - 2}px";
                else if (e.Code == Keyboard.Key.C)
                    Console.Clear();
            };
        }

        public static void addComponents2(Gui gui)
        {
            gui.CurrentScene.BaseNode.Layout = LayoutType.Wrap;
            gui.CurrentScene.AddChild(new HScrollBar(4000));

            for (int i = 0; i < 200; i++)
            {
                gui.CurrentScene.AddChild(new Icon(HlyssUI.Utils.Icons.Windows));
            }
        }
    }
}
