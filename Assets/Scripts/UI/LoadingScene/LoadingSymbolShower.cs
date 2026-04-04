using System.Text;
using Data;
using Lang;
using TMPro;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LoadingScene {
    public class LoadingSymbolShower: MonoBehaviour {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _context;

        private void Awake() {
            _name.font = TMP_LangText.GetFont(LanguageManager.LangPack);
            _context.font = TMP_LangText.GetFont(LanguageManager.LangPack);
            
            var code = UseInfo.GetRandomGetInfo();
            var data = DataManager.Symbol.GetData(code);
            _name.text = data.Name.ApplyLang();
            _icon.sprite = code.GetIcon();

            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(data.Condition)) {
                builder.Append("조건".ApplyLang());
                builder.Append(": ");
                builder.Append(data.Condition.ApplyLang());
                builder.AppendLine();
                builder.AppendLine();
            }
            builder.Append("정보".ApplyLang());
            builder.Append(": ");
            builder.Append(data.Description.ApplyLang());
            builder.AppendLine();
            builder.AppendLine();
            if (!string.IsNullOrWhiteSpace(data.EvolveDescription)) {
                builder.Append("진화".ApplyLang());
                builder.Append(": ");
                builder.Append(UseInfo.GetEvolve(code) ? data.EvolveDescription.ApplyLang() : "???");
            }

            _context.text = builder.ToString();
        }
        
    }
}