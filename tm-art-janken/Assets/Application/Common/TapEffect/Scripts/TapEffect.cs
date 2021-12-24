using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;

public class TapEffect : MonoBehaviour
{

	[SerializeField]
	private Camera uiCamera = default;

	[SerializeField]
	private ParticleSystem particleFlower = default;

	private ParticleSystem.ShapeModule shapeFlower = default;

	[SerializeField]
	private ParticleSystem particleCircle = default;

	private ParticleSystem.ShapeModule shapeCircle = default;

	[SerializeField]
	private ParticleSystem particleFlowerDrag = default;

	private ParticleSystem.EmissionModule emissionFlowerDrag = default;

	private Vector3 particleRotation = default;

	private void Start()
	{
		uiCamera = GameObject.Find("UICamera")?.GetComponent<Camera>();
		shapeFlower = particleFlower.shape;
		shapeCircle = particleCircle.shape;
		emissionFlowerDrag = particleFlowerDrag.emission;

		// タップ
		// 単発エフェクトを再生
		// 1フレーム後にドラッグエフェクトを起動
		this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(_ =>
		{
			SetPosition();
			RunOneShotParticle();
			StartCoroutine(SetFlowerDragEnabled(true));
		}).AddTo(this);

		// タップ：ドラッグ
		this.UpdateAsObservable().Where(_ => Input.GetMouseButton(0)).Subscribe(_ =>
		{
			SetPosition();
		}).AddTo(this);

		// タップ：リリース
		this.UpdateAsObservable().Where(_ => Input.GetMouseButtonUp(0)).Subscribe(_ =>
		{
			StartCoroutine(SetFlowerDragEnabled(false));
		}).AddTo(this);
	}

	private void RunOneShotParticle()
	{
		particleRotation = Vector3.forward * Random.value * 360;
		shapeFlower.rotation = particleRotation;
		shapeCircle.rotation = particleRotation + Vector3.forward * 36;
		particleFlower.Emit(5);
		particleCircle.Emit(5);
	}

	/// <summary>
	/// ドラッグエフェクトを起動
	/// 1フレーム遅らせることで前回の位置から出てしまうエフェクトを抑制
	/// </summary>
	/// <returns></returns>
	private IEnumerator SetFlowerDragEnabled(bool flag)
	{
		yield return null;

		emissionFlowerDrag.enabled = flag;
		emissionFlowerDrag.rateOverDistance = flag ? 2 : 0;
	}

	private void SetPosition()
	{
		// マウスのワールド座標までパーティクルを移動し、パーティクルエフェクトを1つ生成する
		Vector3 posValue = uiCamera.ScreenToWorldPoint(Input.mousePosition + uiCamera.transform.forward * 100);
		this.transform.position = posValue;
	}

}
