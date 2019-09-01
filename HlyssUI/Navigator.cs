using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HlyssUI
{
    public class Navigator
    {
        private Stack<Stack<GuiScene>> _sceneStack = new Stack<Stack<GuiScene>>();
        private Dictionary<string, GuiScene> _scenes = new Dictionary<string, GuiScene>();

        public void AddScene(GuiScene scene, string name)
        {
            _scenes.Add(name, scene);
        }

        public void RemoveScene(string name)
        {
            _scenes.Remove(name);
        }

        public List<GuiScene> GetAllScenes()
        {
            return _scenes.Values.ToList();
        }

        public Stack<GuiScene> GetCurrentStack()
        {
            return _sceneStack.Peek();
        }

        public void Navigate(string name)
        {
            if (_sceneStack.Count > 0)
            {
                for (int i = 0; i < GetCurrentStack().Count; i++)
                {
                    GetCurrentStack().ElementAt(i).Stop();
                }
            }

            Stack<GuiScene> stack = new Stack<GuiScene>();
            stack.Push(_scenes[name]);
            StartStack(stack);

            _sceneStack.Push(stack);
        }

        public void Back()
        {
            if (_sceneStack.Count <= 1)
                return;

            Stack<GuiScene> stack = _sceneStack.Pop();

            while(stack.Count > 0)
            {
                stack.Pop().Stop();
            }

            StartStack(GetCurrentStack());
        }

        public void PushOverlay(string name)
        {
            GetCurrentStack().Push(_scenes[name]);
            GetCurrentStack().Peek().Start();
        }

        public void PopOverlay()
        {
            GetCurrentStack().Pop().Stop();
        }

        private void StartStack(Stack<GuiScene> stack)
        {
            for (int i = 0; i < stack.Count; i++)
            {
                stack.ElementAt(i).Start();
            }
        }
    }
}
