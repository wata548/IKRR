using Character;
using UI;

namespace Data {
    public class AlcoholCurse: EffectBase {
        public override int Code => 3011;
        public override int ShowCount => -1;

        public override void OnAdded(IEntity pTarget) =>
            UIManager.Instance.Distortion.SetDistortion(true);
        public override void OnTurnEnd(IEntity pTarget) {}
    }
}