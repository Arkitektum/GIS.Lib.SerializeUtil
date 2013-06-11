using System.IO;
using System.Text;

namespace Arkitektum.GIS.Lib.SerializeUtil
{
    // http://stackoverflow.com/questions/371930/net-xmlwriter-unexpected-encoding-is-confusing-me?lq=1
    public sealed class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding _encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this._encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return _encoding; }
        }
    }
}
