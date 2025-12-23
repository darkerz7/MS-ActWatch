using MS_ActWatch.ActBan;

namespace MS_ActWatch
{
    internal class AWPlayer
    {
        public ActBanPlayer ButtonBannedPlayer;
        public ActBanPlayer TriggerBannedPlayer;
        public int PFormatPlayer;
        public bool Buttons;
        public bool Triggers;

        public AWPlayer()
        {
            ButtonBannedPlayer = new(true);
            TriggerBannedPlayer = new(false);
            PFormatPlayer = Cvar.PlayerFormat;
        }
    }
}
