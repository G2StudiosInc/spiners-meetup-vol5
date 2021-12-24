[System.Serializable]
public abstract class MainManagerStateBase
{

	/// <summary>
	/// ステートを開始した時に呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	/// <param name="prevState">前のステートを入力</param>
	public virtual void OnEnter(MainManager owner, MainManagerStateBase prevState) { }

	/// <summary>
	/// 毎フレーム呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	public virtual void OnUpdate(MainManager owner) { }

	/// <summary>
	/// ステートを終了した時に呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	/// <param name="nextState">次のステートを入力</param>
	public virtual void OnExit(MainManager owner, MainManagerStateBase nextState) { }

}