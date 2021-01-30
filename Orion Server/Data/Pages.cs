using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OrionServer.Data
{
    public class Pages
    {
        public static readonly Dictionary<string, string> pages = new();

        public static void InitializePages()
        {
            Pages.pages.Clear();

            string[] pageNames = Directory.GetFiles(Constants.PagesFolder);

            foreach (string pageName in pageNames)
                Pages.pages.Add(Path.GetFileName(pageName), File.ReadAllText(pageName));
        }

        public static void SavePages()
        {
            string[] pageNames = Directory.GetFiles(Constants.PagesFolder);

            foreach (string pageName in pageNames)
                File.Delete($"{Constants.PagesFolder}/{Path.GetFileName(pageName)}");

            foreach (KeyValuePair<string, string> page in Pages.pages)
                File.WriteAllText($"{Constants.PagesFolder}/{page.Key}", page.Value);
        }
    }
}
