namespace HlyssUI.Controllers.Tweens
{
    class TweenInstant : Tween
    {
        public TweenInstant() : base("instant") { }

        public override Tween Get()
        {
            return new TweenInstant();
        }

        public override void Update()
        {
            if (progress == 1)
                Finish();

            progress = 1;
        }
    }
}
