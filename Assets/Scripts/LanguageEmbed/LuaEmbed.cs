using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Object = System.Object;

namespace LanguageEmbed {
    public class LuaEmbed: ILanguageEmbed {
        private LuaEnv _env = null;

        public LuaEmbed(List<(string Name, object Obj)> pGlobalSetting = null) {
            _env = new LuaEnv();
            if (pGlobalSetting == null)
                return;
            
            foreach (var setting in pGlobalSetting)
                _env.Global.Set(setting.Name, setting.Obj);
        }

        public void Invoke(string pCode, string pFunctionName, object[] pArgs) =>
            InvokeRaw(pCode, pFunctionName, pArgs);
        
        private Object[] InvokeRaw(string pCode, string pFunctionName, object[] pArgs) {

            var scriptEnv = _env.NewTable();
            var meta = _env.NewTable();
            meta.Set("__index", _env.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            try {
                
                _env.DoString(pCode);
                var function = scriptEnv.Get<LuaFunction>(pFunctionName);
                if (function == null) {
                    Debug.LogError($"{pFunctionName} isn't exist");
                    scriptEnv.Dispose();
                    return null;
                }

                return function.Call(pArgs);
            }
            catch(Exception error) {
                var context = pCode;
                throw new Exception($"{context} ---------------\n{error.Message}");
            }
            finally {
                scriptEnv.Dispose();
            }
        }

        public T Invoke<T>(string pCode, string pFunctionName, object[] pArgs) {
            return (T)InvokeRaw(pCode, pFunctionName, pArgs)?[0];
        }

        public void Update() {
            _env.Tick();
        }
    }
}