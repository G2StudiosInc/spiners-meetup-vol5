using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace TestSpineModel {

    public class ButtonAnimPlay : MonoBehaviour {

        [SerializeField]
        private ButtonAnimPlayManager buttonAnimPlayManager = default;

        [SerializeField]
        private Button btn;

        [SerializeField]
        private Text txt;

        private Action<string, TrackIndex, bool, float> btnAction = default;

        // Start is called before the first frame update
        private void Start() {

            buttonAnimPlayManager = transform.parent.GetComponent<ButtonAnimPlayManager>();
            btn.OnClickAsObservable().Subscribe(_ => {
                btnAction(txt.text, TrackIndex.ID_MAIN_TRACK, false, 0.2f);
            }).AddTo(this);
        }

        public void SetButtonStatus(string name, Action<string, TrackIndex, bool, float> callback) {
            txt.text = name;
            btnAction = callback;
        }

    }

}