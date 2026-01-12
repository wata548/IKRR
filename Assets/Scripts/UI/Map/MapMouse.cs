using System.Linq;
using UnityEngine;

namespace UI.Map {
    public class MapMouse: MonoBehaviour {

        private const float _power = -50;
        private const float _mapLower = -380f;
        private const float _mapHigher = -2600f;
        private bool _isMobile = false;
        
        private void Awake() {
            _isMobile = Application.isMobilePlatform;
        }
        
        private void Update() {
            
            transform.position = TouchInput(); 
            transform.position = MouseInput();

            var pos = transform.localPosition;
            pos.y = Mathf.Clamp(pos.y, _mapHigher, _mapLower);

            transform.localPosition = pos;
        }

        private Vector3 MouseInput() {
            var delta = Input.mouseScrollDelta.y;
            var pos = transform.position;
            pos.y += _power * delta;
            return pos;
        }
        private Vector3 TouchInput() {
            var touches = Input.touches;
            var delta = 0f;
            if(touches.Length != 0)
                delta = touches.Max(touch => touch.deltaPosition.y);
            var pos = transform.position;
            pos.y += delta;
            return pos;
        }  
    }
}