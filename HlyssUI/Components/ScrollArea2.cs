using HlyssUI.Layout;
using SFML.System;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class ScrollArea2 : Component
    {
        private HScrollBar _hScroll;
        private VScrollBar _vScroll;
        private bool _disableHScroll;
        private bool _disableVScroll;

        public bool DisableHorizontalScroll
        {
            get { return _disableHScroll; }
            set
            {
                _hScroll.Visible = !value;
                _hScroll.Enabled = !value;
                _disableHScroll = value;
            }
        }

        public bool DisableVerticalScroll
        {
            get { return _disableVScroll; }
            set
            {
                _vScroll.Visible = !value;
                _vScroll.Enabled = !value;
                _disableVScroll = value;
            }
        }

        public ScrollArea2()
        {
            Children = new List<Component>()
            {
                new Component()
                {
                    Expand = true,
                    Layout = LayoutType.Column,
                    Height = "100%",
                    Children = new List<Component>()
                    {
                        new Component()
                        {
                            Name = "scrollarea_content",
                            Expand = true,
                            Width = "100%",
                            Layout = LayoutType.Scroll,
                            DisableClipping = false
                        },
                        new HScrollBar(1000)
                        {
                            Name = "scrollarea_hscroll",
                            Target = this
                        }
                    }
                },
                new VScrollBar(1000)
                {
                    Name = "scrollarea_vscroll",
                    Target = this
                }
            };

            Layout = LayoutType.Row;
            SlotName = "scrollarea_content";

            _hScroll = FindChild("scrollarea_hscroll") as HScrollBar;
            _vScroll = FindChild("scrollarea_vscroll") as VScrollBar;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();

            if (SlotContent.Count == 0)
                return;

            int maxX = SlotContent[0].TargetSize.X + FindChild("scrollarea_content").Paddings.Horizontal;
            int maxY = SlotContent[0].TargetSize.Y + FindChild("scrollarea_content").Paddings.Vertical;

            System.Console.WriteLine(maxY + "  |  " + TargetSize.Y);

            _hScroll.Visible = maxX > TargetSize.X && !_disableHScroll;
            _vScroll.Visible = maxY > TargetSize.Y && !_disableVScroll;
            _hScroll.ContentWidth = maxX;
            _vScroll.ContentHeight = maxY;

            if (!_hScroll.Visible)
                _hScroll.Percentage = 0;

            if (!_vScroll.Visible)
                _vScroll.Percentage = 0;

            int x = (int)((maxX - TargetSize.X) * _hScroll.Percentage) * -1;
            int y = (int)((maxY - TargetSize.Y) * _vScroll.Percentage) * -1;

            foreach (var child in Slot.Children)
            {
                child.ScrollOffset = new Vector2i(x, y);
            }
        }
    }
}
