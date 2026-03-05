using Data;
using TMPro;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop {
    public class Book: MonoBehaviour {
        [SerializeField] private Image _book;
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private SymbolShower _icon;
        [SerializeField] private Image _moneyIcon;
        [SerializeField] private TMP_Text _priceShower;

        private int _price;
        private int _code;
        private bool _purchasable = true;

        public void Set(int pCode) {
            var data = DataManager.Symbol.GetData(pCode);
            var rarity = data.Rarity;
            if (rarity is (Rarity.Etc or Rarity.Evolution))
                rarity = Rarity.Normal;
            _book.sprite = Resources.Load<Sprite>($"Books/{rarity}");
            _icon.Set(pCode);
            _price = data.Price;
            _code = pCode;
            _priceShower.text = $"{_price}G";
        }

        private void Purchase() {
            if (_code == DataManager.ERROR_SYMBOL || !_purchasable)
                return;
            PlayerData.GetMoney(-_price);
            UIManager.Instance.Selector.Add(_code);
            _code = DataManager.ERROR_SYMBOL;
            _priceShower.text = "<size=70%>SOLD OUT!";
        }
        
        private void Start() {
            _purchaseButton.onClick.AddListener(Purchase);
        } 
        
        public void Update() {
            var temp = _price <= PlayerData.Money;
            if (temp != _purchasable) {
                _purchasable = temp;
                _moneyIcon.material = temp ? null : MaterialStore.Get("GrayScale");
            }
        }
    }
}