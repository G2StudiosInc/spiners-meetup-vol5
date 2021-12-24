using UnityEngine;
using Spine.Unity;
using System;

namespace TestSpineModel {

	public class ButtonAnimPlayManager : MonoBehaviour {

		[SerializeField]
		private GameObject spineObject = default;

		private SpineAnimationController spineAnimationController = default;
		
		private SkeletonAnimation skeletonAnimation = default;

		[SerializeField]
		private GameObject cloneBtn = default;

		[SerializeField]
		private Transform contentTransform = default;
		
		private void Start() {
			spineAnimationController = spineObject.GetComponent<SpineAnimationController>();
			skeletonAnimation = spineObject.GetComponent<SkeletonAnimation>();
			
			GameObject tmpObj = default;
			string animName = String.Empty;

			for (int i = 0; i < skeletonAnimation.AnimationState.Data.SkeletonData.Animations.Items.Length; i++) {
				animName = skeletonAnimation.AnimationState.Data.SkeletonData.Animations.Items[i].ToString();
				tmpObj = Instantiate(cloneBtn, Vector3.zero, Quaternion.identity, contentTransform);
				tmpObj.GetComponent<ButtonAnimPlay>().SetButtonStatus(animName,spineAnimationController.PlayAnimation);
				tmpObj.name = $"ButtonAnimPlay{i}";
			}
			cloneBtn.SetActive(false);
		}

	}

}