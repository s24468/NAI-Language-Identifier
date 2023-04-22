using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mini_projekt_LanguageIdentifier
{
    public class DataProvider
    {
        private readonly string _path;
        public readonly List<(string Key, string Value)> ListOfKeyValuePairs;
        public readonly List<string> Languages;

        public DataProvider(string path)
        {
            _path = path;
            ListOfKeyValuePairs = LoadListOfKeyValuePairs();
            Languages = GetLanguages();
        }

        private List<string> GetLanguages()
        {
            IEnumerable<string> uniqueKeys = ListOfKeyValuePairs.Select(kvp => kvp.Key).Distinct();
            var result = new List<string>();

            foreach (string key in uniqueKeys)
            {
                result.Add(key);
            }

            return result;
        }

        private List<(string Key, string Value)> LoadListOfKeyValuePairs()
        {
            var listResult = new List<(string Key, string Value)>();
            using (var reader = new StreamReader(_path))
            {
                // Read each line of the file until the end is reached
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    bool isOk = false;
                    string keyLanguage = "", sentence = "";
                    foreach (var letter in line.ToCharArray())
                    {
                        if (letter == ',')
                        {
                            isOk = true;
                        }

                        if (isOk == false)
                        {
                            keyLanguage += letter;
                        }

                        if (isOk)
                        {
                            sentence += letter;
                        }
                    }
                    listResult.Add((keyLanguage, sentence));
                }
            }

            return listResult;
        }
    }
}