using HlyssUI.Layout;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class ScrollArea : Component
    {
        private Component _contentBox;
        private Component _scrollBox;
        private VScrollBar _vScroll;
        private HScrollBar _hScroll;

        public override void OnAdded(Component parent)
        {
            base.OnAdded(parent);

            Layout = LayoutType.Relative;

            _contentBox = new Component();
            AddChild(_contentBox);

            _scrollBox = new Component();
            _scrollBox.Width = "100%";
            _scrollBox.Height = "100%";
            _scrollBox.Layout = LayoutType.Relative;
            _scrollBox.Reversed = true;
            AddChild(_scrollBox);

            _vScroll = new VScrollBar(400);
            _scrollBox.AddChild(_vScroll);

            _hScroll = new HScrollBar(400);
            _scrollBox.AddChild(_hScroll);
        }
    }
}
