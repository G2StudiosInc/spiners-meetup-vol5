[System.Serializable]
public abstract class JankenManagerStateBase
{

	/// <summary>
	/// ステートを開始した時に呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	/// <param name="prevState">前のステートを入力</param>
	public virtual void OnEnter(JankenManager owner, JankenManagerStateBase prevState) { }

	/// <summary>
	/// 毎フレーム呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	public virtual void OnUpdate(JankenManager owner) { }

	/// <summary>
	/// ステートを終了した時に呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	/// <param name="nextState">次のステートを入力</param>
	public virtual void OnExit(JankenManager owner, JankenManagerStateBase nextState) { }

}