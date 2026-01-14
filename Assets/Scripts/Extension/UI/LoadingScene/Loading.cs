using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSVData.Extensions;
using Data;
using Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Extension.Scene {
#if UNITY_EDITOR
    public class TipLoader: IDataLoader<string> {
        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "Tips";
            
        public IEnumerable<string> Load() =>
            SpreadSheet.LoadData(_path, _sheet, _apiKey).SelectMany(row => row);
    }
#else
    public class TipLoader: IDataLoader<string> {
        public IEnumerable<string> Load() =>
            File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Tips.txt"));
    }
#endif
    
    public class Loading: MonoBehaviour {

        [SerializeField] private TMP_LangText _tip;
        [SerializeField] private Image _progress;
        [SerializeField] private TMP_Text _progressDetail;
        private static List<string> _tipElement = null;
        
        private void Awake() {
            _tipElement ??= new TipLoader().Load().ToList();

            var idx = Random.Range(0, _tipElement.Count);
            _tip.text = _tipElement[idx];
            StartCoroutine(SceneManager.LoadScene());
        }

        private void Update() {
            var progress = SceneManager.Progress;
            _progressDetail.text = $"{progress:N0}%";
            _progress.fillAmount = progress;
        }
    }
}