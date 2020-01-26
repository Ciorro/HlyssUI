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

namespace HlyssUIDemo
{
    class Program
    {
        static string caption = "HlyssUI demo";

        static void Main(string[] args)
        {
            Vector2u winSize = new Vector2u(1280, 720);

            ContextSettings contextSettings = new ContextSettings();
            contextSettings.AntialiasingLevel = 8;

            RenderWindow window = new RenderWindow(new VideoMode(winSize.X, winSize.Y), caption, Styles.Default, contextSettings);
            window.SetVerticalSyncEnabled(true);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "dark");

            HlyssApp app = new HlyssApp(window);
            app.Root.AddChild(new BasicRouter()
            {
                Name = "router"
            });

            (app.Root.GetChild("router") as Router).Navigate(Test());

            Handle(app);

            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    HlyssApp.Debug = !HlyssApp.Debug;
                if (e.Code == Keyboard.Key.C)
                    Console.Clear();
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

        private static void Handle(HlyssApp app)
        {
            (app.Root.FindChild("ToolTip 1") as ToolTip).Target = app.Root.FindChild("Panel 1");
            app.Root.FindChild("Panel 1").Clicked += (object sender) =>
            {
                (app.Root.FindChild("Menu 1") as Menu).Show(app.MousePosition);
            };

            app.Root.FindChild("show_form").Clicked += (object sender) =>
            {
                (app.Root.FindChild("form1") as Form).Show();
            };
        }

        static Component Test()
        {
            return new Panel()
            {
                Width = "100%",
                Height = "100%",
                Padding = "5px",
                Layout = LayoutType.Wrap,
                Children = new List<Component>()
                {
                    new Button("Button 1")
                    {
                        Appearance = Button.ButtonStyle.Filled,
                        Name = "show_form"
                    },
                    new CheckBox("CheckBox 1"),
                    new Dropdown()
                    {
                        Items = new List<string>()
                        {
                            "Action 1", "Action 2", "Action 3"
                        }
                    },
                    new ExpansionPanel()
                    {
                        Width = "200px",
                        Header = "Expansion panel 1",
                        SlotContent = new List<Component>()
                        {
                            new Button("Hidden button")
                        }
                    },
                    new Icon(Icons.User),
                    new Label("Label 1"),
                    new LinkLabel("Link 1", "https://google.com"),
                    new Panel()
                    {
                        Width = "100px",
                        Height = "100px",
                        Name = "Panel 1"
                    },
                    new PictureBox("img.jpg")
                    {
                        Width = "100px",
                        Height = "100px"
                    },
                    new ProgressBar()
                    {
                        Intermediate = true
                    },
                    new RadioButton("RadioButton 1"),
                    new RadioButton("RadioButton 2"),
                    new SpinButton()
                    {
                        Width = "150px",
                        Name = "SpinButton 1"
                    },
                    new ToggleSwitch("ToggleSwitch 1"),
                    new ToolTip()
                    {
                        Text = "ToolTip 1",
                        Name = "ToolTip 1"
                    },
                    new TrackBar()
                    {
                        Width = "150px",
                        Height = "20px"
                    },
                    new Panel()
                    {
                        Width = "200px",
                        AutosizeY = true,
                        Layout = LayoutType.Column,
                        Padding = "5px 1px",
                        Children = new List<Component>()
                        {
                            new ListItem("ListItem 1"),
                            new ListItem("ListItem 2"),
                            new ListItem("ListItem 3"),
                        }
                    },
                    new Menu()
                    {
                        Name = "Menu 1",
                        Items = new List<MenuItem>()
                        {
                            new MenuItem("MenuItem 1"),
                            new MenuItem("MenuItem 2"),
                            new MenuItem("MenuItem 3")
                            {
                                Menu = new Menu()
                                {
                                    Items = new List<MenuItem>()
                                    {
                                        new MenuItem("MenuItem 1"),
                                        new MenuItem("MenuItem 2"),
                                    }
                                }
                            }
                        }
                    },
                    new TextBox()
                    {
                        Text = "TextBox 1",
                        Width = "200px"
                    },
                    new Form()
                    {
                        Name = "form1"
                    }
                }
            };
        }
    }
}
