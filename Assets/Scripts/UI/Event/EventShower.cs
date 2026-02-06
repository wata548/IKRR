using System;
using Symbol;
using Data.Event;
using DG.Tweening;
using Extension.Test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace UI.Event {
    public class EventShower: MonoBehaviour {

        [SerializeField] private GameObject _pannel;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _context;
        [SerializeField] private Image _img;
        [SerializeField] private Button _button;
        [SerializeField] private EventButtonContainer _container;

        private Tween _animation;
        private Data.Event.Event _curEvent;

        [TestMethod]
        public void SetEvent(string pTitle, Data.Event.Event pEvent) {
            _button.gameObject.SetActive(true);
            _animation = ButtonAnimation();
            
            _pannel.SetActive(true);
            _title.text = pTitle;
            _curEvent = pEvent;
            var script = pEvent.Goto(0);
            SetScript(script);

            Tween ButtonAnimation() {
                const float DEGREE = 20;
                _button.transform.rotation = Quaternion.Euler(0, 0, DEGREE);
                return _button.transform.DORotate(new Vector3(0, 0, -DEGREE), 0.5f * Time.timeScale)
                    .SetLoops(-1, LoopType.Yoyo);
            } 
        }

        public void SetScript(SingleScript pScript) {
            const float DURATION = 1.8f;
            _context.text = "";
            if (pScript is not { TargetImage: null })
                _img.sprite = pScript.TargetImage;
            _context.DOText(pScript.Context, DURATION * Time.timeScale)
                .OnComplete(() => _container.SetData(pScript));
        }

        public Tween Typing() {
            const float DURATION = 0.8f;
            const float INTERVAL = 0.3f;
            
            var context = _context.text + '\n' + EventButton.LastClickButton;
            return DOTween.Sequence()
                .Append(_context.DOText(context, DURATION * Time.timeScale))
                .AppendInterval(INTERVAL * Time.timeScale);
        }

        private void ShowButton() {
            _pannel.SetActive(!_pannel.activeSelf);
        }
        
        public void Close() {
            _img.sprite = null;

            _animation.Kill();
            _button.gameObject.SetActive(false);
            
            UIManager.Instance.Map.ClearStage();
            _pannel.SetActive(false);
        }
        
        public void Goto(int pLabel) {
            _container.Clear();
            Typing()
                .OnComplete(() => SetScript(_curEvent.Goto(pLabel)));
        }

        private void Awake() {
            _button.onClick.AddListener(ShowButton);
        }
    }
}