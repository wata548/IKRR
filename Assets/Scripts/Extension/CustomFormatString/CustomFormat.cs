using System.Collections.Generic;
using System.Text;

namespace Extension.CustomFormatString {
    public static class CustomFormat {
        public static string ExtendFormat(this string pFormatString, Dictionary<string, object> pValues) {
            var builder = new StringBuilder();
            var buffer = "";
            var isLabel = false;
            foreach (var c in pFormatString) {
                switch (c) {
                    case '{':
                        builder.Append(buffer);
                        buffer = "";
                        isLabel = true;
                        break;
                    case '}':
                        if (isLabel) {
                            if (pValues.TryGetValue(buffer, out var value)) {
                                builder.Append(value);
                                buffer = "";
                            }
                            else {
                                buffer = '{' + buffer + '}';
                            }
                            isLabel = false;
                        }
                        else {
                            buffer += c;
                        }
                        break;
                    default:
                        buffer += c;
                        break;
                }
            }

            builder.Append(buffer);
            return builder.ToString();
        }
    }
}