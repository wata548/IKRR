using System.Collections.Generic;
using Data;
using DG.Tweening;
using Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Shop {
    public class ShopManager: MonoBehaviour {
        [SerializeField] private Book _bookPrefab;
        [SerializeField] private RectTransform _bookPlace;
        [SerializeField] private Vector2Int _tableSize;
        [SerializeField] private GameObject _pannel;
        [SerializeField] private TMP_Text _moneyShower;

        [Space, Header("Refresh")] 
        [SerializeField] private Button _refresh;
        [SerializeField] private TMP_Text _refreshCost;
        [SerializeField] private int _defaultCost = 8;
        [SerializeField] private int _interval = 3;
        
        [Space, Header("UI")] 
        [SerializeField] private Button _uiButton;
        
        private List<Book> _books = new();
        private List<int> _purchasable;
        private int _cost = 0;
        private Tween _buttonAnimation;
        
        public void Show() {
            _uiButton.gameObject.SetActive(true);
            _buttonAnimation = _uiButton.ButtonHighlight();
            _cost = _defaultCost;
            _pannel.SetActive(true);
            SetBooks();
        }

        public void Hide() {
            _buttonAnimation?.Kill();
            _uiButton.gameObject.SetActive(false);
            _pannel.SetActive(false);
            UIManager.Instance.Map.ClearStage(true);
            UIManager.Instance.Map.SetActive(true);
        }

        private void SetBooks() {
            _refreshCost.text = _cost.ToString();
            var amount = _tableSize.x * _tableSize.y;
            _purchasable ??= DataManager.Symbol.Query(symbol => symbol.Price > 0);
            _bookPlace.Place(_books, new(
                Vector2.zero,
                amount,
                _tableSize,
                _bookPrefab,
                null,
                (book, idx) => Set(book, _purchasable)
            ));
        }

        private void Refresh() {
            PlayerData.GetMoney(-_cost);
            _cost += _interval;
            SetBooks();
        }

        private void Set(Book pBook, List<int> pPurchasable) {
            var rarity = DataManager.LevelUp.GetRarity();
            var candidate = DataManager.Symbol.MiniQuery(pPurchasable, new(rarity));
            var target = candidate.Count > 0
                ? candidate[Random.Range(0, candidate.Count)]
                : DataManager.ERROR_SYMBOL;
            pBook.Set(target);
        }

        private void Awake() {
            _refresh.onClick.AddListener(Refresh);
            _uiButton.onClick.AddListener(() => _pannel.SetActive(!_pannel.activeSelf));
        }
        
        private void Update() {
            _moneyShower.text = PlayerData.Money + "G";
            if (!_pannel.gameObject.activeSelf)
                return;
            _refresh.interactable = PlayerData.Money >= _cost;
        }
    }
}