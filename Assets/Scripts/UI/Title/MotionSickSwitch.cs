using Extension;

namespace UI.Title {
    public class MotionSickSwitch: RotatingSwitch {
        
        protected override void Setting() {
            OnAppear += () => DistortionManager.Instance.PreventDistortion;
            OnClick += value => DistortionManager.Instance.PreventDistortion = value;
        }
    }
}