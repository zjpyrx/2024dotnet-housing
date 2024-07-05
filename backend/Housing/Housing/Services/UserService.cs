using Housing.Data;
using StandardTest;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace Housing.Services
{
    public class CryptoService
    {
        private const int AES_BLOCK_SIZE = 16;

        [DllImport("DLL/GeoLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EncryptAES(string input, byte[] output, string key);

        [DllImport("DLL/GeoLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DecryptAES(byte[] input, byte[] output, string key);

        public static string EncryptStringAES(string input, string key)
        {
            int length = input.Length + AES_BLOCK_SIZE - input.Length % AES_BLOCK_SIZE;
            byte[] output = new byte[length];
            EncryptAES(input, output, key);
            return Convert.ToBase64String(output);
        }

        public static string DecryptStringAES(string input, string key)
        {
            byte[] inputBytes = Convert.FromBase64String(input);
            byte[] output = new byte[inputBytes.Length];
            DecryptAES(inputBytes, output, key);
            string result = Encoding.UTF8.GetString(output);
            return RemoveNonPrintableCharacters(result).TrimEnd('\0');
        }

        private static string RemoveNonPrintableCharacters(string input)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in input)
            {
                if (!char.IsControl(c))
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private const double EarthRadiusKm = 6371;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> RegisterAsync(string userId, string name, string password)
        {
            var existingUsers = await _context.Users.Where(u => u.UserId == int.Parse(userId)).ToListAsync();
            if (existingUsers.Count != 0)
            {
                return -102; // User ID already registered
            }

            try
            {
                var encryptedPassword = CryptoService.EncryptStringAES(password, "your-secret-key");

                var user = new User
                {
                    UserId = int.Parse(userId),
                    Name = name,
                    Password = encryptedPassword
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -404; // Special fault
            }

            return 1; // Registration successful
        }

        public async Task<int> LoginAsync(string userId, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userId));

            if (user == null)
            {
                return -101; // Illegal username or password
            }

            var decryptedPassword = CryptoService.DecryptStringAES(user.Password, "your-secret-key");

            // 输出调试信息
            Console.WriteLine($"Decrypted Password: '{decryptedPassword}'");
            Console.WriteLine($"Original Password: '{password}'");
            Console.WriteLine(decryptedPassword.Length);
            Console.WriteLine(password.Length);

            if (!decryptedPassword.Trim().Equals(password.Trim(), StringComparison.Ordinal))
            {
                return -101; // Illegal username or password
            }

            return 1; // Login successful
        }

    }
}
