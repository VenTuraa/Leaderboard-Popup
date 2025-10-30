using System;
using SimplePopupManager;

namespace Leaderboard
{
    public class LeaderboardPopupParams
    {
        public IPopupManagerService PopupManager;
        public Action OnClosed;
        public Action OnOpened;
    }
}


