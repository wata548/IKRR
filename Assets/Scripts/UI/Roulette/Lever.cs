using UnityEngine;
using UnityEngine.UI;

namespace UI.Roulette {

    public class Lever: Button {
        [SerializeField] private Animator _animator;
        
        private new void Awake() {
            base.Awake();

            _animator = GetComponent<Animator>();
            onClick.AddListener(() => _animator.Play("Push"));
        }
    }
}