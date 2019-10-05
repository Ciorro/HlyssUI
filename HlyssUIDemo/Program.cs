using HlyssUI;
using HlyssUI.Components;
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
            window.SetFramerateLimit(60);
            //window.SetVerticalSyncEnabled(true);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "dark");

            Gui gui = new Gui(window);

            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            gui.Navigator.AddScene(GetIntelScene(gui), "intel");
            gui.Navigator.AddScene(GetProgressBarTest(gui), "pb");
            gui.Navigator.AddScene(GetComponents2(gui), "pic");
            gui.Navigator.AddScene(GetComponents1(gui), "txt");
            gui.Navigator.AddScene(GetListTest(gui), "list");
            gui.Navigator.AddScene(GetLonczer(gui), "lon");
            gui.Navigator.Navigate("list");

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    Gui.Debug = !Gui.Debug;
                if (e.Code == Keyboard.Key.C)
                    Console.Clear();
            };

            window.MouseButtonPressed += (object sender, MouseButtonEventArgs e) =>
            {
                if (e.Button == Mouse.Button.XButton1)
                    gui.Navigator.Back();
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
                    window.SetTitle($"{caption} ({fps} fps)");
                    fps = 0;
                    fpsTimer.Restart();
                }
            }

            HlyssUI.Utils.Logger.SaveLog();
        }

        private static GuiScene GetIntelScene(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);

            caption = "Intel® Driver & Support Assistant";

            scene.Root.Layout = LayoutType.Column;
            scene.Root.CenterContent = true;

            Panel topBar = new Panel()
            {
                Width = "100%",
                Height = "60px",
                Name = "TopBar"
            };
            topBar.Style.SetValue("primary-color", "0071c5");
            topBar.Style.SetValue("border-thickness", "0");
            topBar.Style.SetValue("border-radius", "0");
            scene.AddChild(topBar);

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
            scene.AddChild(licenseArea);
            licenseArea.Content.AutosizeY = true;
            licenseArea.Content.Width = "100%";

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
            scene.AddChild(bottomBar);

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
            install.Clicked += (object sender) => gui.Navigator.Navigate("pb");

            Button close = new Button("Close")
            {
                MarginRight = "5px",
                Name = "CloseButton"
            };
            bottomBarRight.AddChild(close);
            close.Clicked += (object sender) => gui.Navigator.PushOverlay("pb");

            CheckBox agreement = new CheckBox("I agree to the license terms and conditions")
            {
                MarginTop = "8px",
                Name = "CheckBox"
            };
            bottomBarLeft.AddChild(agreement);
            agreement.Checked += (object sender, bool isChecked) => install.Enabled = isChecked;

            install.InsertChild(0, new PictureBox("shield.png") { MarginRight = "2px", MarginTop = "2px" });

            return scene;
        }

        private static GuiScene GetComponents1(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);

            Panel panel = new Panel();
            scene.AddChild(panel);
            panel.Padding = "5px";
            //panel.Width = "50%";
            //panel.Height = "200px";
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

            TextArea textArea = new TextArea();
            textArea.Text = "ale w sumie to ni jst takie asz tduen jak jusz sie oharnie jak cokolwiek zrobic w syfony";
            panel6.AddChild(textArea);
            textArea.Width = "90%";
            textArea.Height = "90%";
            textArea.Margin = "5px";
            panel6.DisableClipping = false;

            TextBox textBox = new TextBox();
            textBox.Width = "200px";
            textBox.MaxLines = 10;
            textBox.MarginLeft = "20px";
            scene.AddChild(textBox);
            textBox.InsertChild(0, new Icon(Icons.Search) { Margin = "10px" });
            textBox.Children[0].MarginRight = "10px";
            textBox.Placeholder = "Search";
            textBox.SelectOnFocus = true;

            panel5.Texture = new Texture("gradient.jpg");

            return scene;
        }

        public static GuiScene GetComponents2(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);

            scene.Root.Layout = LayoutType.Wrap;
            scene.AddChild(new HScrollBar(4000));

            ScrollArea picScroll = new ScrollArea();
            scene.AddChild(picScroll);
            picScroll.Width = "100%";
            picScroll.Height = "50%";
            picScroll.Content.Width = "100%";
            picScroll.Content.AutosizeY = true;
            picScroll.Content.Layout = LayoutType.Wrap;

            Texture texture = new Texture("img.jpg");

            for (int i = 0; i < 100; i++)
            {
                PictureBox pictureBox = new PictureBox(texture);
                pictureBox.Width = "100px";
                pictureBox.Height = "100px";
                pictureBox.Margin = "1px";

                picScroll.Content.AddChild(pictureBox);

                Icon icon = new Icon(Icons.Heart);
                icon.Margin = "5px";
                icon.Style.SetValue("text-color", "ffffff");
                icon.Style.SetValue("character-size", 18);

                pictureBox.AddChild(icon);
                pictureBox.Reversed = true;
            }

            return scene;
        }

        public static GuiScene GetComponents3(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);

            scene.Root.Layout = LayoutType.Wrap;
            scene.Root.Padding = "20px";

            Button button = new Button("Zmień rozmiar ");
            scene.AddChild(button);
            button.Appearance = Button.ButtonStyle.Filled;
            button.AddChild(new Icon(Icons.Arrows));
            button.MarginRight = "20px";
            button.Clicked += (object sender) =>
            {

                Panel panel = scene.Root.FindChild("panel6") as Panel;

                panel.Transition = "out";
                panel.Width = $"{panel.TargetSize.X + 50}px";
            };

            return scene;
        }

        private static GuiScene GetProgressBarTest(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);

            scene.Root.Padding = "20px";
            scene.Root.Layout = LayoutType.Column;

            scene.Root.Children = new List<Component>()
            {
                new ToggleSwitch("Intermediate")
                {
                    Name = "pbtest_toggle"
                },
                new ProgressBar()
                {
                    Value = 25,
                    MarginTop = "20px",
                    Width = "100%",
                    Name = "pbtest_progressbar"
                },
                new TrackBar()
                {
                    Width = "200px",
                    Height = "100px",
                    Name = "pbtest_trackbar"
                },
                new TextBox()
                {
                    Width = "400px",
                    Name = "pbtest_textbox"
                }
            };

            (scene.Root.GetChild("pbtest_toggle") as ToggleSwitch).Toggled += (object sender, bool isToggled) =>
            {
                (scene.Root.GetChild("pbtest_progressbar") as ProgressBar).Intermediate = isToggled;
            };

            return scene;
        }

        public static GuiScene GetListTest(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);
            scene.Root.Layout = LayoutType.Column;

            ScrollArea list = new ScrollArea()
            {
                Width = "100%",
                Height = "100%"
            };
            scene.Root.AddChild(list);

            list.Content.Width = "101%";
            list.Content.AutosizeY = true;
            list.Content.Layout = LayoutType.Column;
            list.Content.CenterContent = true;

            for (int i = 0; i < 1000; i++)
            {
                ListItem listItem = new ListItem($"ListItem {i + 1}")
                {
                    Width = "100%"
                };
                list.Content.AddChild(listItem);

                listItem.Icon = Icons.Cogs;

                list.Content.AddChild(new Divider());
            }

            return scene;
        }

        public static GuiScene GetLonczer(Gui gui)
        {
            GuiScene scene = new GuiScene(gui);

            caption = "DibrySoft Launcher";

            scene.Root.Children = new List<Component>()
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
                }
            };

            scene.Root.FindChild("nav").DefaultStyle = new Style()
            {
                {"border-radius", "0" },
                {"border-thickness", "0" },
                { "text-color", "ffffff"},
                { "primary-color", "ffffff"},
            };

            scene.Root.FindChild("header").DefaultStyle = new Style()
            {
                {"character-size", "25" }
            };

            scene.Root.FindChild("user_img").DefaultStyle = new Style()
            {
                {"border-radius", "100" }
            };

            scene.Root.FindChild("user_menu_btn").HoverStyle = new Style()
            {
                {"text-color", "secondary +40" }
            };

            scene.Root.FindChild("user_menu_btn").PressedStyle = new Style()
            {
                {"text-color", "secondary +20" }
            };

            scene.Root.FindChild("list").DefaultStyle = new Style()
            {
                {"primary-color", "00ffffff" },
                {"border-radius", "5" }
            };

            scene.Root.FindChild("l1").HoverStyle =
            scene.Root.FindChild("l2").HoverStyle =
            scene.Root.FindChild("l3").HoverStyle =
            scene.Root.FindChild("l4").HoverStyle = new Style()
            {
                {"primary-color", "11ffffff" }
            };

            scene.Root.FindChild("l1").PressedStyle =
            scene.Root.FindChild("l2").PressedStyle =
            scene.Root.FindChild("l3").PressedStyle =
            scene.Root.FindChild("l4").PressedStyle = new Style()
            {
                {"primary-color", "22ffffff" }
            };

            return scene;
        }
    }
}
