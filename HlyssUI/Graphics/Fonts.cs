using SFML.Graphics;

namespace HlyssUI.Graphics
{
    public static class Fonts
    {
        private static Font _montserratRegular = null;
        private static Font _montserratMedium = null;
        private static Font _montserratSemiBold = null;
        private static Font _montserratBold = null;
        private static Font _lineAwesome = null;

        public static Font MontserratRegular
        {
            get 
            {
                if(_montserratRegular == null)
                    _montserratRegular = new Font(Properties.Resources.Montserrat_Regular);
                return _montserratRegular;
            }
        }

        public static Font MontserratMedium
        {
            get
            {
                if (_montserratMedium == null)
                    _montserratMedium = new Font(Properties.Resources.Montserrat_Medium);
                return _montserratMedium;
            }
        }

        public static Font MontserratSemiBold
        {
            get
            {
                if (_montserratSemiBold == null)
                    _montserratSemiBold = new Font(Properties.Resources.Montserrat_SemiBold);
                return _montserratSemiBold;
            }
        }

        public static Font MontserratBold
        {
            get
            {
                if (_montserratBold == null)
                    _montserratBold = new Font(Properties.Resources.Montserrat_Bold);
                return _montserratBold;
            }
        }

        public static Font LineAwesome
        {
            get
            {
                if (_lineAwesome == null)
                    _lineAwesome = new Font(Properties.Resources.Line_Awesome);
                return _lineAwesome;
            }
        }
    }
}
