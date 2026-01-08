using System;
using UnityEngine;

namespace Extension {
    public abstract class MonoSingleton<T> : MonoBehaviour 
        where T: MonoBehaviour {

        private static bool exitGame = false;
        public static T Instance { get; private set; }
        protected abstract bool IsNarrowSingleton { get; }

        private static void EnterGame() => exitGame = false;
        private static void ExitGame() => exitGame = true;
        
        protected virtual void Awake() {
            
            if (Instance != null) {

                if (Instance != this as T) {

                    Debug.Log(
                        $"{gameObject.name} was deleted,\n" 
                        + "singleton can exist just one\n"
                        + $"(Current Singleton: {Instance.gameObject})"
                    );
                    Destroy(gameObject);
                }
                       
                return;
            }

            var type = GetType();
            if (type != typeof(T))
                throw new ArgumentException($"T must be {type}");
            Instance = this as T;
            if(!IsNarrowSingleton)
                DontDestroyOnLoad(gameObject);
        }

        protected virtual void Update() {
            if (exitGame) {
                Destroy(gameObject);
                Instance = null;
            }
        }
    }
}