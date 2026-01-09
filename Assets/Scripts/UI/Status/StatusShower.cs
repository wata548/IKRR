using System;
using System.IO;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Status {
    
    public class StatusShower: MonoBehaviour {
        
        [SerializeField] private TMP_Text _shower;
        [SerializeField] private Image _image;
        private TargetStatus _status;
        private int _value = -1;
        private float _fontSize; 

        public void SetStatus(TargetStatus pStatus) {
            _status = pStatus;
            
            _image.sprite = Resources.Load<Sprite>(Path.Combine("Status", pStatus.ToString()));
            Refresh();
        }

        public void RawRefresh() {
            _value = Data.Status.GetValue(_status);
            _shower.text = _value.ToString();
        }
        
        public Tween Refresh() {
            if (_value == -1) {
                _value = 0;
                _shower.text = _value.ToString();
                return null;
            }
            
            var newValue = Data.Status.GetValue(_status);
            if (_value == newValue)
                return null;
            var result =  Animation(_value, newValue);
            _value = newValue;
            return result;
        }

        private void Awake() {
            _fontSize = _shower.fontSize;
        }

        private Tween Animation(int pStart, int pEnd) => DOTween.Sequence()
            .Append(_shower.transform.DOShakePosition(0.4f,
                4 * Mathf.Log(Mathf.Abs(pEnd - pStart) + 1) * Vector3.up))
            .Join(_shower.DOCounter(pStart, pEnd, 0.35f))
            .Join(_shower.DOFontSize(_fontSize * 1.5f, 0.2f).SetEase(Ease.OutCirc))
            .Append(_shower.DOFontSize(_fontSize, 0.2f).SetEase(Ease.OutQuad));
    }
}