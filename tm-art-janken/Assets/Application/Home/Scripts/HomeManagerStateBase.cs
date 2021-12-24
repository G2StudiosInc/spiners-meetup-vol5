[System.Serializable]
public abstract class HomeManagerStateBase
{

	/// <summary>
	/// ステートを開始した時に呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	/// <param name="prevState">前のステートを入力</param>
	public virtual void OnEnter(HomeManager owner, HomeManagerStateBase prevState) { }

	/// <summary>
	/// 毎フレーム呼ばれる
	/// </summary>
	/// <param name="owner">HomeManager</param>
	public virtual void OnUpdate(JankenManager owner) { }

	/// <summary>
	/// ステートを終了した時に呼ばれる
	/// </summary>
	/// <param name="owner">JankenManagerの参照</param>
	/// <param name="nextState">次のステートを入力</param>
	public virtual void OnExit(HomeManager owner, HomeManagerStateBase nextState) { }

}