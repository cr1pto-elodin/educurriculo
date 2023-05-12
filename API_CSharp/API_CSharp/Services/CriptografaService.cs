using System.Security.Cryptography;
using System.Text;

namespace API_CSharp.Services
{
    public class CriptografaService
    {
        public static string EncryptMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {

                byte[] inputByte = Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputByte);

                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
