using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OrionServer.Utilities
{
    public class Authenticator
    {
        private static readonly List<Triple<string, string, DateTime>> _keys = new();

        /// <summary>
        /// Loads all keys from AuthenticationKeys file in WWWData
        /// generated each time the Orion Server restarts
        /// </summary>
        public static void LoadAuthenticationKeys()
        {
            try
            {
                string fileContent = File.ReadAllText(Constants.AuthenticationKeysFile, Encoding.UTF8);
                JToken tokens = JsonConvert.DeserializeObject<JToken>(fileContent);
                string[] keys = tokens.SelectToken("keys").ToObject<string[]>();
                foreach (string key in keys)
                    _keys.Add(new Triple<string, string, DateTime>(key, "", new DateTime()));
                RemoveDuplicateKeys();
                SaveKeys();
            }
            catch (DirectoryNotFoundException e)
            {
                Utilities.ExceptionConsoleWriter<DirectoryNotFoundException>
                    .ShowException(e, "Orion Server can not locate authentication keys file", true, 1);
            }
            catch (UnauthorizedAccessException e)
            {
                Utilities.ExceptionConsoleWriter<UnauthorizedAccessException>
                    .ShowException(e, "Orion Server does not have the right permission to create a read the authentication keys.", true, 1);
            }
            catch (FileNotFoundException e)
            {
                Utilities.ExceptionConsoleWriter<FileNotFoundException>
                    .ShowException(e, "Orion Server can not locate authentication keys file", true, 1);
            }
            catch (IOException e)
            {
                Utilities.ExceptionConsoleWriter<IOException>
                    .ShowException(e, "Orion Server does can not delete previous authentication files because they are still used by an other proccess.", true, 1);
            }
            catch (Exception e)
            {
                Utilities.ExceptionConsoleWriter<Exception>
                    .ShowException(e, "Orion Server encountered a fatal exception while trying to load authentication keys.", true, 1);
            }
        }

        /// <summary>
        /// Checks if a key is authorized
        /// </summary>
        /// <param name="key">Key to be checked</param>
        /// <returns>Returns true if key is authorized</returns>
        public static bool IsAuthorizedKey(string key)
        {
            return FindIndexOfKey(key) >= 0;
        }

        /// <summary>
        /// Returns the index of the key given
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>Index of the key in the list</returns>
        private static int FindIndexOfKey(string key)
        {
            for (int i = 0; i < _keys.Count; i++)
            {
                if (_keys[i].Item1 == key)
                {
                    _keys[i].Item3 = DateTime.Now;
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Authorized key
        /// </summary>
        /// <param name="key">Key to be authorized</param>
        public static void AuthorizeKey(string key)
        {
            _keys.Add(new Triple<string, string, DateTime>(key, "", new DateTime()));
            RemoveDuplicateKeys();
            SaveKeys();
        }

        /// <summary>
        /// Removed key
        /// </summary>
        /// <param name="key">Key to remove</param>
        public static void RemoveKey(string key)
        {
            if (FindIndexOfKey(key) >= 0)
                _keys.RemoveAt(FindIndexOfKey(key));

            RemoveDuplicateKeys();
            SaveKeys();
        }

        /// <summary>
        /// Removed any duplicate keys.
        /// Internal Use ONLY
        /// </summary>
        private static void RemoveDuplicateKeys()
        {
            HashSet<string> uniqueKeySet = new HashSet<string>(from key in _keys select key.Item1);
            string[] keys = new string[uniqueKeySet.Count];
            uniqueKeySet.CopyTo(keys);

            _keys.Clear();
            foreach (string key in keys)
                _keys.Add(new Triple<string, string, DateTime>(key, "", new DateTime()));

            _keys.Sort();
        }

        /// <summary>
        /// Saves Changes made to the Key List
        /// to Authentication file
        /// Internal Use ONLY
        /// </summary>
        private static void SaveKeys()
        {
            List<string> lines = new();
            lines.Add("{");
            lines.Add("\t\"keys\": [");
            for (int i = 0; i < _keys.Count - 1; i++)
                lines.Add($"\t\t\"{_keys[i].Item1}\",");
            lines.Add($"\t\t\"{_keys[_keys.Count - 1].Item1}\"\n\t]");
            lines.Add("}");

            File.WriteAllLines(
                Constants.AuthenticationKeysFile,
                lines.ToArray(),
                Encoding.UTF8
            );
        }

        internal static List<Triple<string, string, DateTime>> GetAllKeys()
        {
            return new List<Triple<string, string, DateTime>>(_keys);
        }
    }
}