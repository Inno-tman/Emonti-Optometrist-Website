using System;
using System.Configuration;

namespace Emonti_Optometrist_Website
{
    public sealed class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
        public bool EnableSsl { get; set; }

        public static SmtpSettings Load()
        {
            return new SmtpSettings
            {
                Host = ReadString("Host", "smtp.gmail.com"),
                Port = ReadInt("Port", 587),
                Username = ReadString("Username", ReadString("Email", "")),
                Email = ReadString("Email", ReadString("Username", "")),
                Password = ReadString("Password", ""),
                FromName = ReadString("FromName", "Emonti Optometrist"),
                EnableSsl = ReadBool("EnableSsl", true)
            };
        }

        private static string ReadString(string key, string defaultValue)
        {
            string value = ReadSetting(key);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        private static int ReadInt(string key, int defaultValue)
        {
            string value = ReadSetting(key);
            if (int.TryParse(value, out int parsedValue))
            {
                return parsedValue;
            }

            return defaultValue;
        }

        private static bool ReadBool(string key, bool defaultValue)
        {
            string value = ReadSetting(key);
            if (bool.TryParse(value, out bool parsedValue))
            {
                return parsedValue;
            }

            return defaultValue;
        }

        private static string ReadSetting(string key)
        {
            string value = Environment.GetEnvironmentVariable($"Smtp__{key}");
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = Environment.GetEnvironmentVariable($"Smtp:{key}");
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = ConfigurationManager.AppSettings[$"Smtp__{key}"];
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            value = ConfigurationManager.AppSettings[$"Smtp:{key}"];
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            switch (key)
            {
                case "Host":
                    return ConfigurationManager.AppSettings["SmtpHost"];
                case "Port":
                    return ConfigurationManager.AppSettings["SmtpPort"];
                case "Username":
                    return ConfigurationManager.AppSettings["SmtpUsername"];
                case "Email":
                    return ConfigurationManager.AppSettings["SmtpEmail"];
                case "Password":
                    return ConfigurationManager.AppSettings["SmtpPassword"];
                case "FromName":
                    return ConfigurationManager.AppSettings["SmtpFromName"];
                case "EnableSsl":
                    return ConfigurationManager.AppSettings["SmtpEnableSsl"];
                default:
                    return null;
            }
        }
    }
}
