using HlyssUI;
using HlyssUI.Components;
using HlyssUI.Layout;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Diagnostics;
using System.Linq;

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

            //addComponents(gui);
            //createClippyUI(gui);
            //testWrap(gui);
            testBox(gui);

            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.F3)
                    gui.Debug = !gui.Debug;

                if (e.Code == Keyboard.Key.Enter)
                {
                    Container container = gui.CurrentScene.BaseNode.GetChild("container") as Container;

                    if (container.Layout == LayoutType.Column)
                        container.Layout = LayoutType.Row;
                    else
                        container.Layout = LayoutType.Column;
                }
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

        private static void testBox(Gui gui)
        {
            Container container = new Container(gui.CurrentScene);
            container.CenterContent = true;
            gui.CurrentScene.AddChild(container);

            Card card = new Card(gui.CurrentScene);
            card.Margin = "20px";
            card.Padding = "20px";
            container.AddChild(card);
            card.Layout = LayoutType.Row;

            Panel panel = new Panel(gui.CurrentScene);
            panel.Width = "100px";
            panel.Height = "100px";
            panel.MarginRight = "10px";
            card.AddChild(panel);

            Panel panel1 = new Panel(gui.CurrentScene);
            panel1.Width = "100px";
            panel1.Height = "100px";
            card.AddChild(panel1);

            Label label = new Label(gui.CurrentScene);
            label.Text = "Label";
            card.AddChild(label);
            label.MarginLeft = "10px";
            label.MarginRight = "10px";

            Button button = new Button(gui.CurrentScene);
            button.InsertChild(0, new Icon(gui.CurrentScene, HlyssUI.Utils.Icons.Windows));
            button.Children.First().MarginRight = "5px";
            card.AddChild(button);

            button.ButtonAppearance = Button.ButtonStyle.Filled;
            button.Label = "Transition";
            button.DoubleClicked += (object button) => { Environment.Exit(0); };
            button.Clicked += (object button) => 
            {
                (button as Button).Style.Round = !(button as Button).Style.Round;
                if((button as Button).Style.Round)
                    panel.Transition("size: to 100px 200px", "color: primary to accent");
                else
                    panel.Transition("size: to 100px 100px", "color: primary to primary");
            };

            gui.Window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.Left)
                    panel.Transition("size: by -50px 0px");
                if (e.Code == Keyboard.Key.Right)
                    panel.Transition("size: by 50px 0px");
                if (e.Code == Keyboard.Key.Up)
                    panel.Transition("size: by 0px -50px");
                if (e.Code == Keyboard.Key.Down)
                    panel.Transition("size: by 0px 50px");
            };

            CheckBox checkBox = new CheckBox(gui.CurrentScene);
            container.AddChild(checkBox);
            checkBox.Label = "CheckBox";

            PictureBox pictureBox = new PictureBox(gui.CurrentScene, "img.jpg");
            container.AddChild(pictureBox);
            pictureBox.StretchMode = PictureBox.Stretch.Fill;
            pictureBox.Width = "100px";
            pictureBox.Height = "100px";

            ToggleSwitch toggleSwitch = new ToggleSwitch(gui.CurrentScene);
            container.AddChild(toggleSwitch);
            toggleSwitch.Clicked += (object sender) => { pictureBox.Style.Round = !pictureBox.Style.Round; };
            toggleSwitch.Label = "Zaokrąglony PictureBox";
            toggleSwitch.Toggled = true;

            TextArea textArea = new TextArea(gui.CurrentScene, "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. Fusce ut porta ipsum, at aliquet tellus. In hac habitasse platea dictumst. Morbi fringilla lectus sed lacinia tempus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Phasellus blandit quis arcu vitae faucibus.");
            container.AddChild(textArea);
            textArea.Width = "500px";
            textArea.Height = "100px";

            ProgressBar progressBar = new ProgressBar(gui.CurrentScene);
            container.AddChild(progressBar);
            progressBar.Value = 50;

            for (int i = 0; i < 3; i++)
            {
                RadioButton radioButton = new RadioButton(gui.CurrentScene, "Radio Button");
                container.AddChild(radioButton);
            }

            HScrollBar hScrollBar = new HScrollBar(gui.CurrentScene, 400);
            container.AddChild(hScrollBar);

            //VScrollBar vScrollBar = new VScrollBar(gui.CurrentScene, 400);
            //container.AddChild(vScrollBar);

            //ScrollArea scrollArea = new ScrollArea(gui.CurrentScene);
            //container.AddChild(scrollArea);

            //Button saBtn = new Button(gui.CurrentScene, "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
            //scrollArea.AddChild(saBtn);
        }

        private static void testWrap(Gui gui)
        {
            Container container = new Container(gui.CurrentScene);
            container.Name = "container";
            container.Layout = LayoutType.Wrap;
            gui.CurrentScene.BaseNode.AddChild(container);
            container.Padding = "0px";

            for (int i = 0; i < 25; i++)
            {
                Panel p = new Panel(gui.CurrentScene);
                p.Width = "90px";
                p.Height = "90px";
                p.Padding = "10px";
                p.Margin = "5px";

                container.AddChild(p);
            }
        }

        private static void createClippyUI(Gui gui)
        {
            Container app = new Container(gui.CurrentScene);
            gui.CurrentScene.BaseNode.AddChild(app);
            gui.CurrentScene.BaseNode.DisableClipping = true;
            app.DisableClipping = true;

            Panel topBar = new Panel(gui.CurrentScene);
            topBar.Width = "100%";
            topBar.Height = "102px";
            topBar.DisableClipping = true;
            app.AddChild(topBar);

            Panel content = new Panel(gui.CurrentScene);
            content.Width = "100%";
            content.Height = $"{594 - 102}px";
            app.AddChild(content);

            Container topBarContainer = new Container(gui.CurrentScene);
            topBar.AddChild(topBarContainer);
            topBarContainer.Padding = "10px";
            topBarContainer.Layout = LayoutType.Row;
            topBarContainer.DisableClipping = true;

            Panel c1 = new Panel(gui.CurrentScene);
            topBarContainer.AddChild(c1);
            c1.Padding = "10px";
            c1.Width = "50%";
            c1.Height = "100%";

            Panel c2 = new Panel(gui.CurrentScene);
            topBarContainer.AddChild(c2);
            c2.Padding = "10px";
            c2.Width = "50%";
            c2.Height = "100%";
        }

        private static void addComponents(Gui gui)
        {
            Container container = new Container(gui.CurrentScene);
            container.Name = "container";
            container.Padding = "1%";
            container.Layout = LayoutType.ReversedColumn;
            container.CenterContent = true;
            gui.CurrentScene.BaseNode.AddChild(container);

            Panel panel1 = new Panel(gui.CurrentScene);
            container.AddChild(panel1);
            panel1.Width = "30%";
            panel1.Height = "20%";
            panel1.Margin = "10px";

            Panel panel2 = new Panel(gui.CurrentScene);
            container.AddChild(panel2);
            panel2.Width = "10%";
            panel2.Height = "5%";
            panel2.Margin = "20px";

            Container container1 = new Container(gui.CurrentScene);
            panel1.AddChild(container1);

            container1.Padding = "10%";

            Panel panel3 = new Panel(gui.CurrentScene);
            panel3.Width = "40px";
            panel3.Height = "40px";
            panel3.Margin = "5px";

            Panel panel4 = new Panel(gui.CurrentScene);
            panel4.Width = "40px";
            panel4.Height = "40px";
            panel4.Margin = "5px";

            container1.AddChild(panel3);
            container1.AddChild(panel4);

            container1.Layout = LayoutType.Row;

            Label label = new Label(gui.CurrentScene, "Label\nno i co");
            container.AddChild(label);
        }
    }
}
