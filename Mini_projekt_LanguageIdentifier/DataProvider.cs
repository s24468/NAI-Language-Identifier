using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mini_projekt_LanguageIdentifier
{
    public class DataProvider
    {
        private readonly string _path;
        public readonly Dictionary<string, string> Dictionary;
        public readonly List<string> Languages;
       
        
        public DataProvider(string path)
        {
            _path = path;
            Dictionary = LoadDictionary();
            Languages = GetLanguages();
        }

        private List<string> GetLanguages()
        {
            IEnumerable<string> uniqueKeys = Dictionary.Keys.Distinct();
            var result = new List<string>();

            foreach (string key in uniqueKeys)
            {
                result.Add(key);
            }

            return result;
        }

        private Dictionary<string, string> LoadDictionary()
        {
            var dictionaryResult = new Dictionary<string, string>();
            using (var reader = new StreamReader(this._path))
            {
                // Read each line of the file until the end is reached
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    bool isOk = false;
                    string keyLanguage = "", sentence = "";
                    foreach (var letter in line.ToCharArray())
                    {
                        if (letter != ',' && isOk == false)
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

                    dictionaryResult.Add(keyLanguage, sentence);
                }
            }

            return dictionaryResult;
        }
    }
}