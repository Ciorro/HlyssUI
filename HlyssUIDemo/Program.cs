using HlyssUI;
using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Diagnostics;
using System.IO;

namespace HlyssUIDemo
{
    class Program
    {
        static string caption = "HlyssUI demo";

        static void Main(string[] args)
        {
            ContextSettings contextSettings = new ContextSettings();
            contextSettings.AntialiasingLevel = 8;

            RenderWindow window = new RenderWindow(new VideoMode(630, 380), caption, Styles.Default, contextSettings);
            //window.SetFramerateLimit(60);
            //window.SetVerticalSyncEnabled(true);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "dark");

            Gui gui = new Gui(window);
            
            Stopwatch fpsTimer = Stopwatch.StartNew();
            int fps = 0;

            gui.Navigator.AddScene(GetIntelScene(gui), "intel");
            gui.Navigator.AddScene(GetProgressBarTest(gui), "pb");
            gui.Navigator.Navigate("intel");

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    Gui.Debug = !Gui.Debug;
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
                Height = "60px"
            };
            topBar.Style["primary"] = Theme.GetColor("0071c5");
            topBar.Style["secondary"] = Theme.GetColor("0071c5");
            topBar.Style.BorderRadius = 0;
            scene.AddChild(topBar);

            Component topBarLeft = new Component()
            {
                Width = "75%",
                Height = "100%",
                CenterContent = true
            };
            topBar.AddChild(topBarLeft);

            Component topBarRight = new Component()
            {
                Width = "25%",
                Height = "100%",
                CenterContent = true,
                Reversed = true
            };
            topBar.AddChild(topBarRight);

            Label header = new Label("Intel® Driver & Support Assistant")
            {
                MarginLeft = "15px"
            };
            header.Style["text"] = Theme.GetColor("ffffff");
            header.Style.CharacterSize = 21;
            header.Font = Fonts.MontserratMedium;
            topBarLeft.AddChild(header);

            PictureBox intelLogo = new PictureBox("intel.png")
            {
                MarginRight = "15px"
            };
            intelLogo.Style["primary"] = Theme.GetColor("transparent");
            topBarRight.AddChild(intelLogo);

            ScrollArea licenseArea = new ScrollArea()
            {
                Width = "95%",
                Height = "250px",
                Margin = "10px"
            };
            scene.AddChild(licenseArea);
            licenseArea.Content.AutosizeY = true;
            licenseArea.Content.Width = "100%";

            TextArea license = new TextArea()
            {
                Text = File.ReadAllText("license.txt"),
                Width = "100%",
                Height = "1970px"
            };
            licenseArea.Content.AddChild(license);

            Component bottomBar = new Component()
            {
                Width = "95%",
                Layout = LayoutType.Row,
                AutosizeY = true
            };
            scene.AddChild(bottomBar);

            Component bottomBarLeft = new Component()
            {
                Width = "60%",
                AutosizeY = true
            };
            bottomBar.AddChild(bottomBarLeft);

            Component bottomBarRight = new Component()
            {
                Width = "40%",
                AutosizeY = true,
                Reversed = true,
                PaddingTop = "2px"
            };
            bottomBar.AddChild(bottomBarRight);

            Button install = new Button("Install")
            {
                Appearance = Button.ButtonStyle.Filled
            };
            bottomBarRight.AddChild(install);
            install.Clicked += (object sender) => gui.Navigator.Navigate("pb");

            Button close = new Button("Close")
            {
                MarginRight = "5px"
            };
            bottomBarRight.AddChild(close);
            close.Clicked += (object sender) => gui.Navigator.PushOverlay("pb");

            CheckBox agreement = new CheckBox("I agree to the license terms and conditions")
            {
                MarginTop = "8px"
            };
            bottomBarLeft.AddChild(agreement);

            install.InsertChild(0, new PictureBox("shield.png") { MarginRight="2px", MarginTop = "2px"});

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
            textBox.InsertChild(0, new Icon(Icons.Search));
            textBox.Children[0].MarginRight = "10px";
            textBox.Placeholder = "Search";

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
                icon.Style["text"] = Color.White;
                icon.Style.CharacterSize = 18;

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

            ToggleSwitch toggle = new ToggleSwitch("Intermediate");
            scene.AddChild(toggle);

            ProgressBar progressBar = new ProgressBar()
            {
                Value = 25,
                MarginTop = "20px",
                Width = "100%"
            };
            scene.AddChild(progressBar);

            toggle.Toggled += (object sender, bool isToggled) =>
            {
                progressBar.Intermediate = isToggled;
            };

            return scene;
        }
    }
}
