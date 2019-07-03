﻿using HlyssUI;
using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HlyssUIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ContextSettings settings = new ContextSettings(1, 1, 2);

            RenderWindow window = new RenderWindow(new VideoMode(500, 768), "HlyssUI demo", Styles.Default, settings);
            window.SetFramerateLimit(300);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "light");

            Gui gui = new Gui(window, new Font("C:/Windows/Fonts/segoeui.ttf"));
            GuiScene scene = new GuiScene(gui);
            gui.PushScene(scene);

            gui.DefaultCharacterSize = 14;

            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            //addComponents(gui);
            //createClippyUI(gui);
            //testWrap(gui);
            testBox(gui);

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    gui.Debug = !gui.Debug;

                if(e.Code == Keyboard.Key.Enter)
                {
                    Container container = gui.CurrentScene.BaseNode.GetChild("container") as Container;
                    
                    if (container.Layout == Container.LayoutType.Column)
                        container.Layout = Container.LayoutType.Row;
                    else
                        container.Layout = Container.LayoutType.Column;
                }
            };

            while (window.IsOpen)
            {
                window.Clear(Theme.BackgroundColor);
                window.DispatchEvents();

                gui.Update();
                gui.Draw();

                window.Display();

                fps++;
                if(fpsTimer.ElapsedMilliseconds >= 1000)
                {
                    window.SetTitle($"HlyssUI demo ({fps} fps)");
                    fps = 0;
                    fpsTimer.Restart();
                }
            }

            HlyssUI.Utils.Logger.SaveLog();
        }

        private static void testBox(Gui gui)
        {
            Container container = new Container(gui);
            container.CenterContent = true;
            gui.CurrentScene.AddChild(container);

            Box box = new Box(gui);
            box.Margin = "20px";
            container.AddChild(box);

            Panel panel = new Panel(gui);
            panel.Width = "100px";
            panel.Height = "100px";
            panel.MarginRight = "10px";
            box.AddChild(panel);
            //Panel panel1 = new Panel(gui);
            //panel1.Width = "100%";
            //panel1.Height = "100px";
            //panel1.MarginLeft = "120px";
            //panel1.MarginTop = "120px";
            //box.AddChild(panel1);
        }

        private static void testWrap(Gui gui)
        {
            Container container = new Container(gui);
            container.Name = "container";
            container.Layout = Container.LayoutType.Wrap;
            gui.CurrentScene.BaseNode.AddChild(container);
            container.Padding = "0px";

            for (int i = 0; i < 25; i++)
            {
                Panel p = new Panel(gui);
                p.Width = "90px";
                p.Height = "90px";
                p.Padding = "10px";
                p.Margin = "5px";

                container.AddChild(p);
            }
        }

        private static void createClippyUI(Gui gui)
        {
            Container app = new Container(gui);
            gui.CurrentScene.BaseNode.AddChild(app);
            app.DisableClipping = true;

            Panel topBar = new Panel(gui);
            topBar.Width = "100%";
            topBar.Height = "102px";
            app.AddChild(topBar);

            Panel content = new Panel(gui);
            content.Width = "100%";
            content.Height = $"{594 - 102}px";
            app.AddChild(content);

            Container topBarContainer = new Container(gui);
            topBar.AddChild(topBarContainer);
            topBarContainer.Padding = "10px";
            topBarContainer.Layout = Container.LayoutType.Row;

            Panel c1 = new Panel(gui);
            topBarContainer.AddChild(c1);
            c1.Padding = "10px";
            c1.Width = "50%";
            c1.Height = "100%";

            Panel c2 = new Panel(gui);
            topBarContainer.AddChild(c2);
            c2.Padding = "10px";
            c2.Width = "50%";
            c2.Height = "100%";
        }
        
        private static void addComponents(Gui gui)
        {
            Container container = new Container(gui);
            container.Name = "container";
            container.Padding = "1%";
            container.Layout = Container.LayoutType.ReversedColumn;
            container.CenterContent = true;
            gui.CurrentScene.BaseNode.AddChild(container);

            Panel panel1 = new Panel(gui);
            container.AddChild(panel1);
            panel1.Width = "30%";
            panel1.Height = "20%";
            panel1.Margin = "10px";

            Panel panel2 = new Panel(gui);
            container.AddChild(panel2);
            panel2.Width = "10%";
            panel2.Height = "5%";
            panel2.Margin = "20px";

            Container container1 = new Container(gui);
            panel1.AddChild(container1);

            container1.Padding = "10%";

            Panel panel3 = new Panel(gui);
            panel3.Width = "40px";
            panel3.Height = "40px";
            panel3.Margin = "5px";

            Panel panel4 = new Panel(gui);
            panel4.Width = "40px";
            panel4.Height = "40px";
            panel4.Margin = "5px";

            container1.AddChild(panel3);
            container1.AddChild(panel4);

            container1.Layout = Container.LayoutType.Row;
        }
    }
}
