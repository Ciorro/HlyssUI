using HlyssUI;
using HlyssUI.Components;
using HlyssUI.Components.Dialogs;
using HlyssUI.Components.Routers;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.ResourceManagement;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Timers;

namespace HlyssUIDemo
{
    class Program
    {
        static HlyssApplication app = new HlyssApplication();
        static string caption = "HlyssUI demo";
        static bool wide = true;

        static void Main(string[] args)
        {
            Theme.Load("theme.ini", "dark");

            HlyssApplication.InitializeStyles();

            HlyssForm form = new HlyssForm()
            {
                Size = new Vector2u(1280, 720),
                Title = "Cior"
            };
            form.Root.AddChild(new BasicRouter()
            {
                Name = "router"
            });
            (form.Root.GetChild("router") as Router).Navigate(TestExpand());
            form.Show();
            form.Window.SetFramerateLimit(0);

            app.RegisterForm("main", form);
            app.RegisterForm("browse_folder_dialog", new MessageBox("Galactic Dissent", "Czy na pewno chcesz odinstalować ten produkt?\n• Galactic Dissent", "Nie", "Tak"));

            //Handle(form);

            form.Root.FindChild("left_panel").Clicked += (_) =>
            {
                form.Root.FindChild("left_panel").Width = wide ? "100px" : "300px";
                wide = !wide;
            };

            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            form.Window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    HlyssForm.Debug = !HlyssForm.Debug;
                if (e.Code == Keyboard.Key.C)
                    Console.Clear();
            };

            while (form.IsOpen)
            {
                app.UpdateAllForms();
                app.DrawAllForms();

                fps++;
                if (fpsTimer.ElapsedMilliseconds >= 1000)
                {
                    form.Title = $"{caption} ({fps} fps)";
                    fps = 0;
                    fpsTimer.Restart();
                }
            }

            HlyssUI.Utils.Logger.SaveLog();
        }

        private static void Handle(HlyssForm form)
        {
            (form.Root.FindChild("ToolTip 1") as ToolTip).Target = form.Root.FindChild("Panel 1");
            form.Root.FindChild("Panel 1").Clicked += (object sender) =>
            {
                (form.Root.FindChild("Menu 1") as Menu).Show(form.MousePosition);
            };

            form.Root.FindChild("show_form").Clicked += (object sender) =>
            {
                app.GetForm("browse_folder_dialog").Show();
            };
        }

        static Component Test()
        {
            PictureBox.Stretch stretch = PictureBox.Stretch.Letterbox;

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
                        Width = "200px",
                        MaxLines = 10,
                        SelectOnFocus = false,
                        Placeholder = "Enter text here"
                    },
                    new ProgressRing(),
                    new Panel()
                    {
                        Width = "500px",
                        Height = "400px",
                    },
                    new FlipView()
                    {
                        Width = "640px",
                        Height = "360px",
                        Continous = true,
                        Cycle = true,
                        DisplayArrows = false,
                        SlotContent = new List<Component>()
                        {
                            new PictureBox("bgs/image (1).jpg")
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            },
                            new PictureBox("bgs/image (2).jpg")
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            },
                            new PictureBox("bgs/image (3).jpg")
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            },
                            new PictureBox("bgs/image (4).jpg")
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            },
                            new PictureBox("bgs/image (5).jpg")
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            },
                            new PictureBox("bgs/image (6).jpg")
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            },
                            new PictureBox("bgs/image (7).jpg")
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            },
                            new PictureBox(ResourceManager.GetAsync<Texture>("http://caps.fail/lonczer/images//Accounts/d/profile.png").Result)
                            {
                                Width = "100%",
                                Height = "100%",
                                SmoothImage = true,
                                StretchMode = stretch
                            }
                        }
                    }
                }
            };
        }

        static Component TestExpand()
        {
            return new Component()
            {
                Width = "100%",
                Height = "100%",
                Name = "container",
                Children = new List<Component>()
                {
                    new Panel()
                    {
                        Width = "300px",
                        Height = "100%",
                        Name = "left_panel"
                    },
                    new Panel()
                    {
                        Expand = true,
                        Height = "100%",
                        Padding = "50px",
                        Layout = LayoutType.Column,
                        Name = "middle_panel",
                        Children = new List<Component>()
                        {
                            new Panel()
                            {
                                Width = "100%",
                                Height = "100px",
                                Name = "testing_panel",
                                Children = new List<Component>()
                                {
                                    new Button(),
                                    new Spacer(),
                                    new Button()
                                }
                            }
                        }
                    },
                    new Panel()
                    {
                        Width = "200px",
                        Height = "100%",
                        Name = "right_panel"
                    }
                }
            };
        }
    }
}
