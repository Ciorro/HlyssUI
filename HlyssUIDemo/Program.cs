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
            //Vector2u winSize = new Vector2u(630, 380);

            ContextSettings contextSettings = new ContextSettings();
            contextSettings.AntialiasingLevel = 8;

            RenderWindow window = new RenderWindow(new VideoMode(winSize.X, winSize.Y), caption, Styles.Default, contextSettings);
            //window.SetFramerateLimit(60);
            //window.SetVerticalSyncEnabled(true);
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

        private static void Handle(HlyssApp app)
        {
            (app.Root.FindChild("Dymek podpowiedzi 1") as ToolTip).Target = app.Root.FindChild("Panel 1");
            app.Root.FindChild("Panel 1").Clicked += (object sender) =>
            {
                (app.Root.FindChild("Menu 1") as Menu).Show(app.MousePosition);
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
                    new Button("Przycisk 1")
                    {
                        Name = "Przycisk 1",
                        Appearance = Button.ButtonStyle.Filled
                    },
                    new CheckBox("Pole wyboru 1")
                    {
                        Name = "Pole wyboru 1"
                    },
                    new Dropdown()
                    {
                        Name = "Menu rozwijane 1",
                        Items = new List<string>()
                        {
                            "Akcja 1", "Akcja 2", "Akcja 3"
                        }
                    },
                    new ExpansionPanel()
                    {
                        Name = "Panel rozwijany 1",
                        Width = "200px",
                        Header = "Panel rozwijany 1",
                        SlotContent = new List<Component>()
                        {
                            new Component()
                            {
                                Width = "100px",
                                Height = "100px"
                            }
                        }
                    },
                    new Icon(Icons.User)
                    {
                        Name = "Ikona 1"
                    },
                    new Label("Tekst 1")
                    {
                        Name = "Napis 1"
                    },
                    new LinkLabel("Link 1", "")
                    {
                        Name = "Link 1"
                    },
                    new Panel()
                    {
                        Width = "100px",
                        Height = "100px",
                        Name = "Panel 1"
                    },
                    new PictureBox("img.jpg")
                    {
                        Width = "100px",
                        Height = "100px",
                        Name = "Zdjęcie 1"
                    },
                    new ProgressBar()
                    {
                        Intermediate = true,
                        Name = "Pasek postępu 1"
                    },
                    new RadioButton("Przycisk radiowy 1")
                    {
                        Name = "Przycisk radiowy 1"
                    },
                    new RadioButton("Przycisk radiowy 2")
                    {
                        Name = "Przycisk radiowy 2"
                    },
                    new SpinButton()
                    {
                        Width = "150px",
                        Name = "Przycisk numeryczny 1"
                    },
                    new ToggleSwitch("Przełącznik 1")
                    {
                        Name = "Przełącznik 1"
                    },
                    new ToolTip()
                    {
                        Text = "Dymek podpowiedzi 1",
                        Name = "Dymek podpowiedzi 1"
                    },
                    new TrackBar()
                    {
                        Width = "150px",
                        Height = "20px",
                        Name = "Suwak 1"
                    },
                    new Panel()
                    {
                        Width = "200px",
                        AutosizeY = true,
                        Layout = LayoutType.Column,
                        Padding = "5px 1px",
                        Children = new List<Component>()
                        {
                            new ListItem("Element listy 1"),
                            new ListItem("Element listy 2"),
                            new ListItem("Element listy 3"),
                        }
                    },
                    new Menu()
                    {
                        Name = "Menu 1",
                        Items = new List<MenuItem>()
                        {
                            new MenuItem("Akcja menu 1"),
                            new MenuItem("Akcja menu 2"),
                            new MenuItem("Akcja menu 3"),
                        }
                    },
                }
            };
        }
    }
}
