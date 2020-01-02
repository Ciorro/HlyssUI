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

        public abstract void Start();
        public abstract bool Update();

        protected virtual void UpdateTween()
        {
            //if(component.DefaultStyle.ContainsKey("ease") && component.DefaultStyle.GetString("ease") != tween.Name)
            //{
            //    TweenType = component.DefaultStyle.GetString("ease");
            //}

            //if (component.DefaultStyle.ContainsKey("ease-duration") && component.DefaultStyle.GetString("ease-duration") != tween.Name)
            //{
            //    tween.Duration = component.DefaultStyle.GetFloat("ease-duration");
            //}
        }
    }
}
