using System.Collections;
using Data;
using Extension;
using UI;
using UnityEngine;

namespace Character.Skill {
    public class Effect: SkillBase {

        public enum EffectType {
            OX,
        }
        
        [SkillParameter]public EffectType Type { get; private set; }
        [SkillParameter]public string Args { get; private set; }
        public Effect(string[] pArgs) : base(pArgs) {}
        
        protected override void Implement(Positions pCaster) {
            switch (Type) {
                case EffectType.OX:
                    var arg = bool.Parse(Args);
                    UIManager.Instance.OXEffect.Show(arg);
                    break;
                default:
                    break;
            }

            ExRoutine.StartRoutine(Wait());

            IEnumerator Wait() {
                yield return null;
                switch (Type) {
                    case EffectType.OX:
                        yield return new WaitUntil(() => !UIManager.Instance.OXEffect.IsActive);
                        End();
                        break;
                    default:
                        End();
                        break;
                }
            }
        }
    }
}