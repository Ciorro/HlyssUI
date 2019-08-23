using HlyssUI.Components;
using HlyssUI.Controllers.Tweens;

namespace HlyssUI.Controllers
{
    abstract class Controller
    {
        protected Component component;
        protected Tween tween = new TweenInstant();

        public string TweenType
        {
            set
            {
                tween = TweenResolver.GetTween(value);
            }
        }

        public Controller(Component component)
        {
            this.component = component;
        }

        public abstract void OnValueChanged();
        public abstract bool Update();
    }
}
