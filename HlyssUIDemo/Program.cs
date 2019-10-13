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

            (app.Root.GetChild("router") as Router).Navigate(GetLonczer());

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

        private static Component GetIntelScene()
        {
            caption = "Intel® Driver & Support Assistant";

            Component component = new Component()
            {
                Width = "100%",
                Height = "100%",
                Layout = LayoutType.Column,
                CenterContent = true
            };

            Panel topBar = new Panel()
            {
                Width = "100%",
                Height = "60px",
                Name = "TopBar"
            };
            topBar.Style.SetValue("primary-color", "0071c5");
            topBar.Style.SetValue("border-thickness", "0");
            topBar.Style.SetValue("border-radius", "0");
            component.AddChild(topBar);

            Component topBarLeft = new Component()
            {
                Width = "75%",
                Height = "100%",
                CenterContent = true,
                Name = "TobBarLeft"
            };
            topBar.AddChild(topBarLeft);

            Component topBarRight = new Component()
            {
                Width = "25%",
                Height = "100%",
                CenterContent = true,
                Reversed = true,
                Name = "TopBarRight"
            };
            topBar.AddChild(topBarRight);

            Label header = new Label("Intel® Driver & Support Assistant")
            {
                MarginLeft = "15px",
                Name = "Header"
            };
            header.Style.SetValue("text-color", "ffffff");
            header.Style.SetValue("character-size", 21);
            header.Font = Fonts.MontserratMedium;
            topBarLeft.AddChild(header);

            PictureBox intelLogo = new PictureBox("intel.png")
            {
                MarginRight = "15px",
                Name = "Logo"
            };
            intelLogo.Style.SetValue("opacity", 0);
            topBarRight.AddChild(intelLogo);

            ScrollArea licenseArea = new ScrollArea()
            {
                Width = "95%",
                Height = "250px",
                Margin = "10px",
                Name = "ScrollArea"
            };
            component.AddChild(licenseArea);
            licenseArea.Content.AutosizeY = true;
            licenseArea.Content.Width = "100%";
            licenseArea.Content.Padding = "15px";

            TextArea license = new TextArea()
            {
                Text = File.ReadAllText("license.txt"),
                Width = "100%",
                Height = "1970px",
                Name = "License (TextArea)"
            };
            licenseArea.Content.AddChild(license);

            Component bottomBar = new Component()
            {
                Width = "95%",
                Layout = LayoutType.Row,
                AutosizeY = true,
                Name = "BottomBar"
            };
            component.AddChild(bottomBar);

            Component bottomBarLeft = new Component()
            {
                Width = "60%",
                AutosizeY = true,
                Name = "BottomBarLeft"
            };
            bottomBar.AddChild(bottomBarLeft);

            Component bottomBarRight = new Component()
            {
                Width = "40%",
                AutosizeY = true,
                Reversed = true,
                PaddingTop = "2px",
                Name = "BottomBarRight"
            };
            bottomBar.AddChild(bottomBarRight);

            Button install = new Button("Install")
            {
                Appearance = Button.ButtonStyle.Filled,
                Name = "InstallButton"
            };
            bottomBarRight.AddChild(install);
            //install.Clicked += (object sender) => gui.Navigator.Navigate("pb");

            Button close = new Button("Close")
            {
                MarginRight = "5px",
                Name = "CloseButton"
            };
            bottomBarRight.AddChild(close);
            //close.Clicked += (object sender) => gui.Navigator.PushOverlay("pb");

            CheckBox agreement = new CheckBox("I agree to the license terms and conditions")
            {
                MarginTop = "8px",
                Name = "CheckBox"
            };
            bottomBarLeft.AddChild(agreement);
            agreement.Checked += (object sender, bool isChecked) => install.Enabled = isChecked;

            install.InsertChild(0, new PictureBox("shield.png") { MarginRight = "2px", MarginTop = "2px" });

            component.FindChild("CloseButton").Clicked += (object sender) =>
            {
                (component.Parent as Router).Navigate(GetLonczer());
            };

            component.FindChild("CloseButton").Initialized += (object sender) =>
            {
                Console.WriteLine("close init");
            };

            return component;
        }

        public static Component GetLonczer()
        {
            caption = "DibrySoft Launcher";

            Component component = new Component()
            {
                Width = "100%",
                Height = "100%",
                Layout = LayoutType.Row,
                Children = new List<Component>()
                {
                    new Panel()
                    {
                        Width = "270px",
                        Height = "100%",
                        Padding = "15px",
                        Texture = new Texture("gradient.jpg"),
                        Name = "nav",
                        Layout = LayoutType.Column,
                        Children = new List<Component>()
                        {
                            new Label()
                            {
                                Text = "DIBRYSOFT",
                                Font = new Font("NEXT ART_SemiBold.otf"),
                                CharacterSize = 25,
                                Name = "header"
                            },
                            new Component()
                            {
                                AutosizeY = true,
                                Width = "100%",
                                CenterContent = true,
                                Name = "user",
                                MarginTop = "5px",
                                Children = new List<Component>()
                                {
                                    new PictureBox()
                                    {
                                        Image = new Texture("img.jpg"),
                                        Width = "30px",
                                        Height = "30px",
                                        Name = "user_img"
                                    },
                                    new Label()
                                    {
                                        Text = "Suchy",
                                        MarginLeft = "5px",
                                        Name = "username"
                                    },
                                    new Icon(Icons.AngleDown)
                                    {
                                        MarginLeft = "5px",
                                        Name = "user_menu_btn"
                                    }
                                }
                            },
                            new Divider()
                            {
                                MarginTop = "10px",
                                MarginBottom = "10px"
                            },
                            new ScrollArea()
                            {
                                Width = "100%",
                                Height = "500px",
                                Name = "list",
                                Content = new Component()
                                {
                                    AutosizeY = true,
                                    Width = "100%",
                                    Layout = LayoutType.Column,
                                    Children = new List<Component>()
                                    {
                                        new ListItem("Aktualności")
                                        {
                                            Icon = Icons.Newspaper,
                                            Name = "l1"
                                        },
                                        new ListItem("Gry")
                                        {
                                            Icon = Icons.Gamepad,
                                            Name = "l2"
                                        },
                                        new ListItem("Aplikacje")
                                        {
                                            Icon = Icons.Cogs,
                                            Name = "l3"
                                        },
                                        new ListItem("DibryStore")
                                        {
                                            Icon = Icons.ShoppingCart,
                                            Name = "l4"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Button("Czytaj dalej...")
                    {
                        Margin = "20px",
                        Appearance = Button.ButtonStyle.Outline,
                        Name = "btn"
                    },
                    new ToggleSwitch()
                    {
                        Label = "Dark theme",
                        Name = "toggle",
                        Margin = "20px"
                    },
                    new LinkLabel()
                    {
                        Text = ">> Link <<"
                    }
                }
            };

            component.FindChild("nav").DefaultStyle = new Style()
            {
                {"border-radius", "0" },
                {"border-thickness", "0" },
                { "text-color", "ffffff"},
                { "primary-color", "ffffff"},
            };

            component.FindChild("header").DefaultStyle = new Style()
            {
                {"character-size", "25" }
            };

            component.FindChild("user_img").DefaultStyle = new Style()
            {
                {"border-radius", "100" }
            };

            component.FindChild("user_menu_btn").HoverStyle = new Style()
            {
                {"text-color", "secondary +40" }
            };

            component.FindChild("user_menu_btn").PressedStyle = new Style()
            {
                {"text-color", "secondary +20" }
            };

            component.FindChild("list").DefaultStyle = new Style()
            {
                {"primary-color", "00ffffff" },
                {"border-radius", "5" }
            };

            component.FindChild("l1").HoverStyle =
            component.FindChild("l2").HoverStyle =
            component.FindChild("l3").HoverStyle =
            component.FindChild("l4").HoverStyle = new Style()
            {
                {"primary-color", "11ffffff" }
            };

            component.FindChild("l1").PressedStyle =
            component.FindChild("l2").PressedStyle =
            component.FindChild("l3").PressedStyle =
            component.FindChild("l4").PressedStyle = new Style()
            {
                {"primary-color", "22ffffff" }
            };

            (component.FindChild("toggle") as ToggleSwitch).Toggled += (object sender, bool toggled) =>
            {
                Theme.Load("theme.ini", (toggled) ? "dark" : "light");
            };

            (component.FindChild("toggle") as ToggleSwitch).IsToggled = Theme.Name == "dark";
            
            component.FindChild("btn").Clicked += (object sender) =>
            {
                (component.Parent as Router).Navigate(GetIntelScene());
            };

            return component;
        }

        public static Component GetVisibilityTest()
        {
            Component component = new Component()
            {
                Width = "100%",
                Height = "100%",
                Children = new List<Component>()
                {
                    new Button()
                    {
                        Label = "Button 1",
                        Name = "b1"
                    },
                    new TextBox()
                    {
                        //Text= "Button 2",
                        Name = "b2",
                        Visible = false,
                        Width = "200px"
                    },
                    new Button()
                    {
                        Label = "Button 3",
                        Name = "b3"
                    }
                }
            };

            component.FindChild("b1").Clicked += (object sender) =>
            {
                component.FindChild("b2").Visible = !component.FindChild("b2").Visible;
                component.FindChild("b1").ScheduleRefresh();
            };

            return component;
        }
    }
}
