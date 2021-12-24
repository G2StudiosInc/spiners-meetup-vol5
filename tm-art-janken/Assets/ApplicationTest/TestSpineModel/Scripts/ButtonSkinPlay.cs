using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace TestSpineModel {

    public class ButtonSkinPlay : MonoBehaviour {

        [SerializeField]
        private ButtonSkinPlayManager buttonSkinPlayManager = default;

        [SerializeField]
        private Button btn;

        [SerializeField]
        private Text txt;

        private Action<string> btnAction = default;

        private void Start() {
            buttonSkinPlayManager = transform.parent.GetComponent<ButtonSkinPlayManager>();
            btn.OnClickAsObservable().Subscribe(_ => {
                btnAction(txt.text);
            }).AddTo(this);
        }

        public void SetButtonStatus(string name, Action<string> callback) {
            txt.text = name;
            btnAction = callback;
        }

    }

}