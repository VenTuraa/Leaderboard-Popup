using SimplePopupManager;
using Zenject;

namespace Leaderboard.Installers
{
	public class MockInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IPopupManagerService>()
				.To<PopupManagerServiceService>()
				.AsSingle();
		}
	}
}

