using SFML.Window;

namespace HlyssUI.Components
{
    public class LinkLabel : Label
    {
        public LinkLabel()
        {
            DefaultStyle = new Themes.Style()
            {
                { "text-color", "0000FF" }
            };

        }

        public override void OnClicked()
        {
            base.OnClicked();

            DefaultStyle = new Themes.Style()
            {
                { "text-color", "8000ff" }
            };
        }

        //TODO: loading cursors in app

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            App.Window.SetMouseCursor(new Cursor(Cursor.CursorType.Hand));
            TextStyle = SFML.Graphics.Text.Styles.Underlined;
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            App.Window.SetMouseCursor(new Cursor(Cursor.CursorType.Arrow));
            TextStyle = SFML.Graphics.Text.Styles.Regular;
        }
    }
}
