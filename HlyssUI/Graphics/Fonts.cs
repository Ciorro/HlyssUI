using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HlyssUI.Graphics
{
    public static class Fonts
    {
        public static Font MontserratRegular
        {
            get { return new Font(HlyssUI.Properties.Resources.Montserrat_Regular); }
        }

        public static Font MontserratMedium
        {
            get { return new Font(HlyssUI.Properties.Resources.Montserrat_Medium); }
        }

        public static Font MontserratSemiBold
        {
            get { return new Font(HlyssUI.Properties.Resources.Montserrat_SemiBold); }
        }

        public static Font MontserratBold
        {
            get { return new Font(HlyssUI.Properties.Resources.Montserrat_Bold); }
        }
    }
}
