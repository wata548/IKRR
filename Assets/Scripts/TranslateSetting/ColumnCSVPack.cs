using System.Collections.Generic;
using System.Linq;
using System;
using CSVData;

namespace Lang {
    public class ColumnCSVPack: ILangTable {
       
        //==================================================||Fields 
        private readonly Dictionary<string, string[]> _table = new();
        private List<Language> _allowLanguages = new();
        private Language _mainLanguage; 
        
        //==================================================||Constructors 
        public ColumnCSVPack(Language pMainLanguage) => 
            _mainLanguage = pMainLanguage;

        //==================================================||Methods 
        public void Add(string pContext) =>
            Add(CSV.Parse(pContext));

        public void Add(List<List<string>> pContext) {
            var length = Enum.GetValues(typeof(Language)).Length;
            var header = pContext[0].Select(Enum.Parse<Language>).ToList();
            var mainLanguageIdx = header.FindIndex(lang => lang == _mainLanguage);
            
            foreach (var row in pContext.Skip(1)) {
                _table.TryAdd(row[mainLanguageIdx], new string[length]);
                var target= _table[row[mainLanguageIdx]];
                
                int idx = -1;
                foreach (var cell in row) {
                    idx++;
                    if (header[idx] == _mainLanguage)
                        continue;
                    
                    target[(int)header[idx]] = cell;
                }
            }
            
            _allowLanguages.AddRange(header);
            _allowLanguages = _allowLanguages.Distinct().OrderBy(lang => lang).ToList();
        }

        public string Text(Language pLang, string pContext) {
            if (pLang == _mainLanguage)
                return pContext;
            
            if(_table.TryGetValue(pContext, out string[] value))
                return value[(int)pLang];
            return pContext;
        }

        public IEnumerable<Language> AllowLanguages() => 
            _allowLanguages;
    }
}