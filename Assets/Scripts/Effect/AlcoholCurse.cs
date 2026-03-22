using Character;
using UI;
using Unity.VisualScripting;

namespace Data {
    public class AlcoholCurse: EffectBase {
        public override int Code => 3011;
        public override int ShowCount => -1;

        public override void OnGameStart() =>
            DistortionManager.Instance.SetDistortion(true);
        
        public override void OnTurnEnd(IEntity pTarget) {}
        public override void OnDisable() {
            DistortionManager.Instance.SetDistortion(false);
        }
    }
}