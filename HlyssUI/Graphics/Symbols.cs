using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Graphics
{
    public static class Symbols
    {
        public static Texture Check
        {
            get
            {
                return new Texture(HlyssUI.Properties.Resources.check);
            }
        }

        public static Texture ProgressRing
        {
            get
            {
                return new Texture(HlyssUI.Properties.Resources.progress_ring);
            }
        }
    }
}
