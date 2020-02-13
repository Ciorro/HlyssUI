using SFML.Window;
using System.Diagnostics;

namespace HlyssUI.Components
{
    public class LinkLabel : Label
    {
        public string Link { get; set; }

        public LinkLabel() { }

        public LinkLabel(string text, string link) : base(text)
        {
            Link = link;
        }

        //TODO: loading cursors in app
        public override void OnClicked()
        {
            base.OnClicked();

            if(!string.IsNullOrEmpty(Link))
            {
                if (Link.StartsWith("http") || Link.StartsWith("www"))
                {
                    ProcessStartInfo info = new ProcessStartInfo("cmd");
                    info.Arguments = $"/c start {Link}";
                    Process.Start(info);
                }
            }
        }

        public override void OnMouseEntered()
        {
            base.OnMouseEntered();
            Form.Window.SetMouseCursor(new Cursor(Cursor.CursorType.Hand));
            TextStyle = SFML.Graphics.Text.Styles.Underlined;
        }

        public override void OnMouseLeft()
        {
            base.OnMouseLeft();
            Form.Window.SetMouseCursor(new Cursor(Cursor.CursorType.Arrow));
            TextStyle = SFML.Graphics.Text.Styles.Regular;
        }
    }
}
