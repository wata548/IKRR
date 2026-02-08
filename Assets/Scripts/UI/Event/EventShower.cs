using System;
using Symbol;
using Data.Event;
using DG.Tweening;
using Extension;
using Extension.Test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace UI.Event {
    public class EventShower: MonoBehaviour {

        [SerializeField] private GameObject _pannel;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private MoreEffectTMP _context;
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

        const float INTERVAL = 0.05f;
        const float SELECT_SHOW_TERM = 0.5f;
        public void SetScript(SingleScript pScript) {
            _context.TMP.text = "";
            if (pScript is not { TargetImage: null })
                _img.sprite = pScript.TargetImage;
            var animation = _context.Typing(
                pScript.Context,
                INTERVAL * Time.timeScale,
                0,
                () => _container.SetData(pScript),
                () => !_pannel.activeSelf
            );
            ExRoutine.StartRoutine(animation);
        }

        public void Typing(Action pOnComplete) {

            var animation = _context.Typing(
                '\n' + EventButton.LastClickButton,
                INTERVAL * Time.timeScale,
                SELECT_SHOW_TERM * Time.timeScale,
                pOnComplete,
                () => !_pannel.activeSelf
            );
            ExRoutine.StartRoutine(animation);
        }

        private void ShowButton() {
            _pannel.SetActive(!_pannel.activeSelf);
        }
        
        public void Close() {
            _img.sprite = null;

            _container.Clear();
            _animation.Kill();
            _button.gameObject.SetActive(false);
            
            UIManager.Instance.Map.ClearStage();
            _pannel.SetActive(false);
        }
        
        public void Goto(int pLabel) {
            _container.Clear();
            Typing(() => SetScript(_curEvent.Goto(pLabel)));
        }

        private void Awake() {
            _button.onClick.AddListener(ShowButton);
        }
    }
}