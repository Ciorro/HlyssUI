using HlyssUI.Controllers.Tweens;
using HlyssUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components
{
    public class FlipView : Component
    {
        private Tween _tween = new TweenOut();
        private int _startX = 0;
        private int _scrollOffsetX = 0;
        private int _currentView = 0;
        private DeltaTime _deltaTime = new DeltaTime();
        private float _timePassed = 0;
        
        public int ViewsCount
        {
            get { return Slot.Children.Count; }
        }

        public int CurrentView
        {
            get { return _currentView; }
            set
            {
                if (value != _currentView)
                {
                    if (Continous && value >= ViewsCount)
                        _currentView = 0;
                    else if (Continous && value < 0)
                        _currentView = ViewsCount - 1;
                    else if (value >= 0 && value < ViewsCount)
                        _currentView = value;

                    SetView();
                }
            }
        }

        public float TweenDuration
        {
            get { return _tween.Duration; }
            set { _tween.Duration = value; }
        }

        public string TweenType
        {
            set
            {
                _tween = TweenResolver.GetTween(value);
            }
        }

        public bool DisplayArrows
        {
            get { return FindChild("arrows_container").Visible; }
            set { FindChild("arrows_container").Visible = value; }
        }

        public bool Continous { get; set; }
        public bool Cycle { get; set; }
        public float Interval { get; set; } = 2;

        public FlipView()
        {
            Children = new List<Component>()
            {
                new Component()
                {
                    Width = "100%",
                    Height = "100%",
                    Name = "flipview_content",
                    Overflow = HlyssUI.Layout.OverflowType.Scroll
                },
                new Component()
                {
                    Width = "100%",
                    Height = "100%",
                    Name = "arrows_container",
                    CenterContent = true,
                    Hoverable = false,
                    PositionType = HlyssUI.Layout.PositionType.Absolute,
                    Children = new List<Component>()
                    {
                        new Button()
                        {
                            Padding = "5px 5px",
                            MarginLeft = "5px",
                            Style = "outline_button_default flipview_button_default",
                            Action = Previous,
                            Children = new List<Component>()
                            {
                                new Icon(Graphics.Icons.AngleLeft)
                            }
                        },
                        new Spacer(),
                        new Button()
                        {
                            Padding = "5px 5px",
                            MarginRight = "5px",
                            Style = "outline_button_default flipview_button_default",
                            Action = Next,
                            Children = new List<Component>()
                            {
                                new Icon(Graphics.Icons.AngleRight)
                            }
                        }
                    }
                }
            };

            Style = "flipview_default";
            SlotName = "flipview_content";
            _tween.OnFinish += HideViews;
            _tween.Duration = 1f;
        }

        public override void Update()
        {
            base.Update();

            _deltaTime.Update();
            _timePassed += _deltaTime.Current;

            if(_timePassed >= Interval && Cycle)
            {
                Next();
            }

            _tween.Update();
            _scrollOffsetX = -(int)(_startX + (CurrentView * Size.X - _startX) * _tween.Percentage);
            if (_tween.IsRunning)
            {
                Slot.ScrollToX(_scrollOffsetX);
            }
        }

        public void Next()
        {
            CurrentView++;
        }

        public void Previous()
        {
            CurrentView--;
        }

        private void SetView()
        {
            _tween.Start();
            _timePassed = 0;
            _startX = -_scrollOffsetX;
            ShowViews();
        }
        
        private void HideViews()
        {
            for (int i = 0; i < ViewsCount; i++)
            {
                if (i != CurrentView)
                    Slot.Children[i].Visible = false;
            }

            Slot.ScrollToX(0);
        }

        private void ShowViews()
        {
            for (int i = 0; i < ViewsCount; i++)
            {
                Slot.Children[i].Visible = true;
            }

            Slot.ScrollToX(_scrollOffsetX);
        }
    }
}
