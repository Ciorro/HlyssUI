using SFML.Window;

namespace HlyssUI.Graphics
{
    public static class Cursors
    {
        private static Cursor _arrow = null;
        private static Cursor _arrowWait = null;
        private static Cursor _wait = null;
        private static Cursor _text = null;
        private static Cursor _hand = null;
        private static Cursor _sizeHorizontal = null;
        private static Cursor _sizeVertical = null;
        private static Cursor _sizeTopLeftBottomRight = null;
        private static Cursor _sizeBottomLeftTopRight = null;
        private static Cursor _sizeAll = null;
        private static Cursor _cross = null;
        private static Cursor _help = null;
        private static Cursor _notAllowed = null;

        public static Cursor Arrow
        {
            get
            {
                if (_arrow == null)
                    _arrow = new Cursor(Cursor.CursorType.Arrow);
                return _arrow;
            }
        }

        public static Cursor ArrowWait
        {
            get
            {
                if (_arrowWait == null)
                    _arrowWait = new Cursor(Cursor.CursorType.ArrowWait);
                return _arrowWait;
            }
        }

        public static Cursor Wait
        {
            get
            {
                if (_wait == null)
                    _wait = new Cursor(Cursor.CursorType.Wait);
                return _wait;
            }
        }

        public static Cursor Text
        {
            get
            {
                if (_text == null)
                    _text = new Cursor(Cursor.CursorType.Text);
                return _text;
            }
        }

        public static Cursor Hand
        {
            get
            {
                if (_hand == null)
                    _hand = new Cursor(Cursor.CursorType.Hand);
                return _hand;
            }
        }

        public static Cursor SizeHorizontal
        {
            get
            {
                if (_sizeHorizontal == null)
                    _sizeHorizontal = new Cursor(Cursor.CursorType.SizeHorinzontal);
                return _sizeHorizontal;
            }
        }

        public static Cursor SizeVertical
        {
            get
            {
                if (_sizeVertical == null)
                    _sizeVertical = new Cursor(Cursor.CursorType.SizeVertical);
                return _sizeVertical;
            }
        }

        public static Cursor SizeTopLeftBottomRight
        {
            get
            {
                if (_sizeTopLeftBottomRight == null)
                    _sizeTopLeftBottomRight = new Cursor(Cursor.CursorType.SizeTopLeftBottomRight);
                return _sizeTopLeftBottomRight;
            }
        }

        public static Cursor SizeBottomLeftTopRight
        {
            get
            {
                if (_sizeBottomLeftTopRight == null)
                    _sizeBottomLeftTopRight = new Cursor(Cursor.CursorType.SizeBottomLeftTopRight);
                return _sizeBottomLeftTopRight;
            }
        }

        public static Cursor SizeAll
        {
            get
            {
                if (_sizeAll == null)
                    _sizeAll = new Cursor(Cursor.CursorType.SizeAll);
                return _sizeAll;
            }
        }

        public static Cursor Cross
        {
            get
            {
                if (_cross == null)
                    _cross = new Cursor(Cursor.CursorType.Cross);
                return _cross;
            }
        }

        public static Cursor Help
        {
            get
            {
                if (_help == null)
                    _help = new Cursor(Cursor.CursorType.Help);
                return _help;
            }
        }

        public static Cursor NotAllowed
        {
            get
            {
                if (_notAllowed == null)
                    _notAllowed = new Cursor(Cursor.CursorType.NotAllowed);
                return _notAllowed;
            }
        }
    }
}
