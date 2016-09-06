using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertBankStatementToYnab
{
    public class GermanDateTimeFormat : IFormatProvider, ICustomFormatter
    {
        public GermanDateTimeFormat()
        {
        }
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            

            // Provide default formatting if arg is not an Int64.
            if (arg.GetType() != typeof(DateTime))
                try
                {
                    return HandleOtherFormats(format, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(String.Format("The format of '{0}' is invalid.", format), e);
                }
            return format;

        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        private string HandleOtherFormats(string format, object arg)
        {
            if (arg is IFormattable)
                return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
            else if (arg != null)
                return arg.ToString();
            else
                return String.Empty;
        }

    }
}
