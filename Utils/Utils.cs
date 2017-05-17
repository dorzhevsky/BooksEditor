using System.IO;

namespace Utils
{
    public static class Utils
    {
        public static byte[] StreamToArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static bool TryParse(this char c, out int val)
        {
            if (c < '0' || c > '9')
            {
                val = 0;
                return false;
            }
            val = (c - '0');
            return true;
        }
    }
}