using UnityEngine;
using Spine.Unity;
using System;

namespace TestSpineModel {

	public class ButtonSkinPlayManager : MonoBehaviour {

		[SerializeField]
		private GameObject spineObject = default;
		private SpineAnimationController spineAnimationController = default;
		private SkeletonAnimation skeletonAnimation = default;

		[SerializeField]
		private GameObject cloneBtn = default;
		
		private void Start() {
			spineAnimationController = spineObject.GetComponent<SpineAnimationController>();
			skeletonAnimation = spineObject.GetComponent<SkeletonAnimation>();
			
			GameObject tmpObj = default;
			string skinName = String.Empty;

			for (int i = 0; i < skeletonAnimation.Skeleton.Data.Skins.Items.Length; i++) {
				skinName = skeletonAnimation.Skeleton.Data.Skins.Items[i].ToString();
				tmpObj = Instantiate(cloneBtn, Vector3.zero, Quaternion.identity, this.gameObject.transform);
				tmpObj.GetComponent<ButtonSkinPlay>().SetButtonStatus(skinName, spineAnimationController.SetSkin);
				tmpObj.name = $"ButtonSkinPlay{i}";
			}

			cloneBtn.SetActive(false);
		}

	}

}