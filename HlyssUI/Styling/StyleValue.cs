namespace HlyssUI.Styling
{
    public class StyleValue
    {
        public readonly string Name;
        public readonly bool Inheritable;

        public string Value { get; set; }

        public StyleValue(string name, string value, bool inheritable = false)
        {
            Name = name;
            Value = value;
            Inheritable = inheritable;
        }
    }
}
