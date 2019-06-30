﻿using HlyssUI.Themes;
using HlyssUI.Transitions;
using HlyssUI.Utils;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace HlyssUI
{
    public class Gui
    {
        public RenderWindow Window { get; private set; }
        public Font DefaultFont { get; private set; }
        public uint DefaultCharacterSize = 12;
        public float Scale = 1f;
        public bool Debug = false;

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


        public GuiScene CurrentScene
        {
            get
            {
                if (_scenes.Count > 0)
                    return _scenes.Peek();
                else return new GuiScene(this);
            }
        }

        private Stack<GuiScene> _scenes = new Stack<GuiScene>();

        public Gui(RenderWindow window, Font font)
        {
            Window = window;
            DefaultFont = font;
            Theme.OnThemeLoaded += Theme_OnThemeLoaded;
        }

        private void Theme_OnThemeLoaded()
        {
            CurrentScene.UpdateTheme();
        }

        public void Update()
        {
            DeltaTime.Update();
            CurrentScene.Update();
        }

        public void Draw()
        {
            CurrentScene.Draw();
        }

        public void PushScene(GuiScene scene)
        {
            _scenes.Push(scene);
            CurrentScene.Start();
        }

        public void PopScene()
        {
            CurrentScene.Stop();
            _scenes.Pop();
        }
    }
}