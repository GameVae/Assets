
public partial class CustomInputField
{
    public abstract partial class ContentValidate
    {
        protected ContentType type;

        public ContentType Type
        {
            get { return type; }
        }

        public abstract string CheckContent(string str);
    }

    public class IntergerValidate : ContentValidate
    {
        public IntergerValidate()
        {
            type = ContentType.Interger;
        }

        public override string CheckContent(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            char[] chars = str.ToCharArray();
            string temp = "";
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsNumber(chars[i]) || chars[i] == '\n' || chars[i] == '\r' || chars[i] == '\b')
                {
                    temp += chars[i];
                }
            }
            return temp;
        }
    }

    public class TextValidate : ContentValidate
    {
        public TextValidate()
        {
            type = ContentType.Text;
        }

        public override string CheckContent(string str)
        {
            return str;
        }
    }
}
