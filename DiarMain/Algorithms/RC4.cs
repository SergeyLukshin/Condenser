using System;
using System.Globalization;
//using System.Linq;

namespace Condenser
{
    public class RC4
    {
        /*
        byte[] key = ASCIIEncoding.ASCII.GetBytes("Key");

        RC4 encoder = new RC4(key);
        string testString = "Plaintext";
        byte[] testBytes = ASCIIEncoding.ASCII.GetBytes(testString);
        byte[] result = encoder.Encode(testBytes, testBytes.Length);

        RC4 decoder = new RC4(key);
        byte[] decryptedBytes = decoder.Decode(result, result.Length);
        string decryptedString = ASCIIEncoding.ASCII.GetString(decryptedBytes);
        */

        byte[] S = new byte[256];

        int x = 0;
        int y = 0;

        public RC4(byte[] key)
        {
            init(key);
        }

        // Key-Scheduling Algorithm 
        // Алгоритм ключевого расписания 
        private void init(byte[] key)
        {
            int keyLength = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + key[i % keyLength]) % 256;
                byte tmp = S[i];
                S[i] = S[j];
                S[j] = tmp;
            }
        }

        public string GetByteString(byte[] result)
        {
            string str = "";
            for (int i = 0; i < result.GetLength(0); i++)
            {
                str += result[i].ToString("X2");
            }
            return str;
        }

        public byte[] SetByteString(string result)
        {
            byte[] bytes = new byte[result.Length / 2];
            byte b;
            char[] chars = result.ToCharArray();
            for (int i = 0; i < result.Length; i+= 2)
            {
                string str = chars[i].ToString();
                str += chars[i + 1];
                Byte.TryParse(str, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out b);
                bytes[i / 2] = b;
            }
            return bytes;
        }

        public byte[] Encode(byte[] dataB, int size)
        {
            byte[] data = (byte [])dataB.Clone();//.Take(size).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int m = 0; m < data.Length; m++)
            {
                cipher[m] = (byte)(data[m] ^ keyItem());
            }

            return cipher;
        }
        public byte[] Decode(byte[] dataB, int size)
        {
            return Encode(dataB, size);
        }

        // Pseudo-Random Generation Algorithm 
        // Генератор псевдослучайной последовательности 
        private byte keyItem()
        {
            x = (x + 1) % 256;
            y = (y + S[x]) % 256;

            //S.Swap(x, y);
            byte tmp = S[x];
            S[x] = S[y];
            S[y] = tmp;

            return S[(S[x] + S[y]) % 256];
        }
    }

    /*static class SwapExt
    {
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }*/
}