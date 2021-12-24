public partial class HomeManager
{

	public class HomeManagerStateHome : HomeManagerStateBase
	{

		public override void OnEnter(HomeManager owner, HomeManagerStateBase prevState)
		{
			owner.mainManager.StateCallHome();
		}

	}

	public class HomeManagerStateRecord : HomeManagerStateBase
	{

		public override void OnEnter(HomeManager owner, HomeManagerStateBase prevState)
		{
			owner.mainManager.StateCallHomeRecord();
		}

		public override void OnExit(HomeManager owner, HomeManagerStateBase prevState)
		{
			owner.mainManager.StateCallHomeRecordEnd();
		}

	}

}