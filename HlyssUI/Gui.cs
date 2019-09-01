using HlyssUI.Graphics;
using HlyssUI.Themes;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace HlyssUI
{
    public class Gui
    {
        public static bool Debug = false;

        public RenderWindow Window { get; private set; }
        public float Scale = 1f;
        public Navigator Navigator = new Navigator();

        public View DefaultView
        {
            get
            {
                View view = new View((Vector2f)Window.Size / 2, (Vector2f)Window.Size);
                view.Viewport = new FloatRect(0, 0, 1, 1);
                return view;
            }
        }

        public Vector2i Size
        {
            get { return (Vector2i)Window.Size; }
        }

        public Gui(RenderWindow window)
        {
            Window = window;
            Theme.OnThemeLoaded += Theme_OnThemeLoaded;
        }

        private void Theme_OnThemeLoaded()
        {
            foreach (var scene in Navigator.GetAllScenes())
            {
                scene.UpdateTheme();
            }
        }

        public void Update()
        {
            DeltaTime.Update();

            for (int i = Navigator.GetCurrentStack().Count - 1; i >= 0; i--)
            {
                Navigator.GetCurrentStack().ElementAt(i).Update();
            }
        }

        public void Draw()
        {
            for (int i = Navigator.GetCurrentStack().Count - 1; i >= 0 ; i--)
            {
                Navigator.GetCurrentStack().ElementAt(i).Draw();
            }
        }
    }
}
