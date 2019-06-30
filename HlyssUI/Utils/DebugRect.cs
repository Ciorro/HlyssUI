using HlyssUI.Components;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Utils
{
    class DebugRect
    {
        private RectangleShape _rect = new RectangleShape();
        private RectangleShape _margin = new RectangleShape();

        private RectangleShape _paddingRect1 = new RectangleShape();
        private RectangleShape _paddingRect2 = new RectangleShape();
        private RectangleShape _paddingRect3 = new RectangleShape();
        private RectangleShape _paddingRect4 = new RectangleShape();
        private RectangleShape _paddingRect5 = new RectangleShape();

        private VertexArray _marginLines = new VertexArray(PrimitiveType.Lines);

        public void Draw(Component component)
        {
            View tmpView = component.Gui.Window.GetView();
            component.Gui.Window.SetView(new View()
            {
                Size = (Vector2f)component.Gui.Window.Size,
                Viewport = new FloatRect(0, 0, 1, 1),
                Center = (Vector2f)(component.Gui.Window.Size / 2)
            });

            component.Gui.Window.Draw(_rect);
            component.Gui.Window.Draw(_margin);
            component.Gui.Window.Draw(_marginLines);
            component.Gui.Window.Draw(_paddingRect1);
            component.Gui.Window.Draw(_paddingRect2);
            component.Gui.Window.Draw(_paddingRect3);
            component.Gui.Window.Draw(_paddingRect4);
            component.Gui.Window.Draw(_paddingRect5);

            component.Gui.Window.SetView(tmpView);
        }

        public void Update(Component component)
        {
            setRect(component);
            setMargin(component);
            setPadding(component);
        }

        private void setPadding(Component component)
        {
            _paddingRect1.Position = (Vector2f)component.GlobalPosition;
            _paddingRect2.Position = (Vector2f)component.GlobalPosition + new Vector2f(component.Size.X - component.Pr, 0);
            _paddingRect3.Position = (Vector2f)component.GlobalPosition + new Vector2f(component.Size.X - component.Pr, component.Size.Y - component.Pb);
            _paddingRect4.Position = (Vector2f)component.GlobalPosition + new Vector2f(0, component.Size.Y - component.Pb);
            _paddingRect5.Position = (Vector2f)component.GlobalPosition + new Vector2f(component.Pl, component.Pt);

            _paddingRect1.Size = new Vector2f(component.Pl, component.Pt);
            _paddingRect2.Size = new Vector2f(component.Pr, component.Pt);
            _paddingRect3.Size = new Vector2f(component.Pr, component.Pb);
            _paddingRect4.Size = new Vector2f(component.Pl, component.Pb);
            _paddingRect5.Size = (Vector2f)component.Size - new Vector2f(component.Pl + component.Pr, component.Pt + component.Pb);

            _paddingRect1.OutlineColor = _paddingRect2.OutlineColor = _paddingRect3.OutlineColor = _paddingRect4.OutlineColor = _paddingRect5.OutlineColor = Color.Transparent;
            _paddingRect1.FillColor = _paddingRect2.FillColor = _paddingRect3.FillColor = _paddingRect4.FillColor = _paddingRect5.FillColor = Color.Transparent;
            _paddingRect1.OutlineThickness = _paddingRect2.OutlineThickness = _paddingRect3.OutlineThickness = _paddingRect4.OutlineThickness = -1;
            _paddingRect5.OutlineThickness = 1;

            if (component.Hovered)
            {
                _paddingRect1.OutlineColor = _paddingRect2.OutlineColor = _paddingRect3.OutlineColor = _paddingRect4.OutlineColor = _paddingRect5.OutlineColor = Color.Magenta;
            }
        }

        private void setRect(Component component)
        {
            _rect.Position = (Vector2f)component.GlobalPosition;
            _rect.Size = (Vector2f)component.Size;
            _rect.OutlineThickness = -1;
            _rect.FillColor = Color.Transparent;
            _rect.OutlineColor = Color.Transparent;

            if (component.Hovered)
            {
                _rect.FillColor = new Color(70, 255, 80, 50);
                _rect.OutlineColor = Color.Magenta;
            }
        }

        private void setMargin(Component component)
        {
            _margin.Position = (Vector2f)component.GlobalPosition - new Vector2f(component.Ml, component.Mt);
            _margin.Size = (Vector2f)component.MarginSize;

            _margin.OutlineThickness = -1;

            _margin.FillColor = Color.Transparent;
            _margin.OutlineColor = Color.Transparent;

            if (component.Hovered)
            {
                _margin.OutlineColor = Color.Magenta;
            }

            _marginLines.Clear();
            _marginLines.Append(new Vertex(_margin.Position, _margin.OutlineColor));
            _marginLines.Append(new Vertex((Vector2f)component.GlobalPosition, _margin.OutlineColor));
            _marginLines.Append(new Vertex(_margin.Position + new Vector2f(_margin.Size.X, 0), _margin.OutlineColor));
            _marginLines.Append(new Vertex((Vector2f)component.GlobalPosition + new Vector2f(component.Size.X, 0), _margin.OutlineColor));
            _marginLines.Append(new Vertex(_margin.Position + new Vector2f(0, _margin.Size.Y), _margin.OutlineColor));
            _marginLines.Append(new Vertex((Vector2f)component.GlobalPosition + new Vector2f(0, component.Size.Y), _margin.OutlineColor));
            _marginLines.Append(new Vertex(_margin.Position + _margin.Size, _margin.OutlineColor));
            _marginLines.Append(new Vertex((Vector2f)component.GlobalPosition + (Vector2f)component.Size, _margin.OutlineColor));
        }
    }
}
