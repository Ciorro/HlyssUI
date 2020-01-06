using HlyssUI.Components;
using HlyssUI.Controllers.Tweens;

namespace HlyssUI.Controllers
{
    abstract class Controller
    {
        protected Component component;
        protected Tween tween = new TweenOut();

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

        public abstract void Start();
        public abstract bool Update();

        protected virtual void UpdateTween()
        {
            if (component.StyleManager.GetString("ease") != tween.Name)
            {
                TweenType = component.StyleManager.GetString("ease");
            }

            if (component.StyleManager.GetString("ease-duration") != tween.Name)
            {
                tween.Duration = component.StyleManager.GetFloat("ease-duration");
            }
        }
    }
}
