using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SensorKitSDK
{
    public static class SensorKitExtensions
    {
        
        public static Task<T> BeginInvokeOnMainThreadAsync<T>(Func<T> a)
        {
            var tcs = new TaskCompletionSource<T>();
            InvokeHelper.Invoke(() =>
            {
                try
                {
                    var result = a();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        public static string GetHashName(string name)
        {
            if (name != null)
                return $"SensorKit{Convert.ToBase64String(Encoding.UTF8.GetBytes(name)).Replace("=", "")}";
            else
                return null;
        }

        public static byte[] StringToByteArray(this String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string ToHexString(this byte[] bytes)
        {
            return bytes != null ? BitConverter.ToString(bytes) : string.Empty;
        }

    }
}
