using HlyssUI;
using HlyssUI.Components;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Themes;
using SFML.Graphics;
using SFML.System;
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
            RenderWindow window = new RenderWindow(new VideoMode(1366, 768), "HlyssUI demo", Styles.Default);
            //window.SetFramerateLimit(60);
            window.Closed += (object sender, EventArgs e) => { window.Close(); };

            Theme.Load("theme.ini", "light");

            Gui gui = new Gui(window);
            GuiScene scene = new GuiScene(gui);
            gui.PushScene(scene);

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
                //Stopwatch s = Stopwatch.StartNew();
                gui.Draw();
                //System.Console.WriteLine(s.ElapsedMilliseconds);

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

            //gui.Window.KeyPressed += (object sender, KeyEventArgs e) =>
            //{
            //    if (e.Code == Keyboard.Key.Right)
            //        panel6.Width = $"{panel6.Size.X + 2}px";
            //    else if (e.Code == Keyboard.Key.Left)
            //        panel6.Width = $"{panel6.Size.X - 2}px";
            //    else if (e.Code == Keyboard.Key.Escape)
            //        Console.Clear();
            //};

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
            gui.CurrentScene.AddChild(textBox);
            textBox.InsertChild(0, new Icon(Icons.Search));
            textBox.Children[0].MarginRight = "10px";
            textBox.Placeholder = "Search";

            ScrollArea scrollArea = new ScrollArea()
            {
                Width = "200px",
                Height = "100px"
            };
            gui.CurrentScene.Root.AddChild(scrollArea);
            scrollArea.Content.Width = "400px";
            scrollArea.Content.Layout = LayoutType.Wrap;
            scrollArea.Content.AutosizeY = true;
            scrollArea.DisableHorizontalScroll = true;

            for (int i = 0; i < 50; i++)
            {
                scrollArea.Content.AddChild(new Button($"Button {i + 1}"));
            }
        }

        public static void addComponents2(Gui gui)
        {
            gui.CurrentScene.Root.Layout = LayoutType.Wrap;
            gui.CurrentScene.AddChild(new HScrollBar(4000));

            Texture texture = new Texture("img.jpg");

            for (int i = 0; i < 100; i++)
            {
                PictureBox pictureBox = new PictureBox(texture);
                pictureBox.Width = "100px";
                pictureBox.Height = "100px";
                pictureBox.Margin = "1px";

                gui.CurrentScene.Root.AddChild(pictureBox);

                Icon icon = new Icon(Icons.Heart);
                icon.Margin = "5px";
                icon.Style["text"] = Color.White;
                icon.Style.CharacterSize = 18;

                pictureBox.AddChild(icon);
                pictureBox.Reversed = true;
            }
        }

        public static void addComponents3(Gui gui)
        {
            gui.CurrentScene.Root.Layout = LayoutType.Wrap;
            gui.CurrentScene.Root.Padding = "20px";

            Button button = new Button("Zmień rozmiar ");
            gui.CurrentScene.AddChild(button);
            button.Appearance = Button.ButtonStyle.Filled;
            button.AddChild(new Icon(Icons.Arrows));
            button.MarginRight = "20px";
            button.Clicked += (object sender) => {

                Panel panel = gui.CurrentScene.Root.FindChild("panel6") as Panel;

                panel.Transition = "out";
                panel.Width = $"{panel.TargetSize.X + 50}px";
            };

            addComponents1(gui);
            addComponents2(gui);
        }
    }
}
