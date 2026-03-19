using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Extension;
using Extension.Test;
using Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tutorial {
    public class Tutorial: MonoSingleton<Tutorial> {
        [SerializeField] private GameObject _panel; 
        [SerializeField] private Button _button; 
        [SerializeField] private Image _focus;
        [SerializeField] private Image _textShower;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private List<SerializableKVP<string, TutorialSO>> _kvps;
        
        protected override bool IsNarrowSingleton => false;
        private Dictionary<string, TutorialSO> _matches;
        private List<TurorialData> _list;
        private int _idx;
        private Tween _animation;
        private float _time;

        public void InitTutorial() {
            PlayerPrefs.DeleteAll();
        }
        
        [TestMethod]
        public void Set(string pTutorial) {
            if (PlayerPrefs.HasKey($"Tutorial:{pTutorial}"))
                return;

            PlayerPrefs.SetInt($"Tutorial:{pTutorial}", 1);
            Set(_matches[pTutorial].Datas);
        }
        
        public void Set(List<TurorialData> pData) {

            _time = Time.timeScale;
            Time.timeScale = 0.3f;
            _idx = 0;
            _list = pData;
            _panel.SetActive(true);
            Execute();
        }
        
        private void Execute() {
            const float ANIMATION_DURATION = 0.4f;
            
            if (_animation is { active: true })
                return;
            
            if (_list.Count <= _idx) {
                Time.timeScale = _time;
                _panel.SetActive(false);
                return;
            }
            var data = _list[_idx++];

            _text.font = TMP_LangText.GetFont(LanguageManager.LangPack);
            var duration = ANIMATION_DURATION * Time.timeScale;
            _text.text = "";
            if (data.Size == Vector2.zero) {
                _animation = DOTween.Sequence()
                    .Append(_text.DOText(data.Context.ApplyLang(), duration));
                return;
            }
            
            var textPos = new Vector2(0.75f * data.Size.x, 0.75f * data.Size.y) * data.Direction 
                          + data.Pos
                          + data.Direction * new Vector2(395 ,70);
            _textShower.rectTransform.localPosition = textPos;
            _animation = DOTween.Sequence()
                .Append(_focus.rectTransform.DOLocalMove(data.Pos, duration))
                .Join(_focus.rectTransform.DOScale(data.Size * 0.01f, duration))
                .AppendCallback(() => _text.gameObject.SetActive(true))
                .Append(_text.DOText(data.Context.ApplyLang(), duration));
        }


        protected override void Awake() {
            base.Awake();
            _matches = _kvps
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            _button.onClick.AddListener(Execute);
        }       
    }
}