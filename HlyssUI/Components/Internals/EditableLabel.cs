using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Components.Internals
{
    internal class EditableLabel : Label
    {
        public EditableLabel()
        {
        }

        public Vector2f GetLetterPosition(uint letterIndex)
        {
            return _text.FindCharacterPos(letterIndex);
        }
    }
}
