using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using S = ServerPackets;
using C = ClientPackets;
using Client.MirScenes.Dialogs;
using Client.MirObjects;
using Client.MirSounds;

namespace Client.MirScenes
{
    class PacketHandler
    {
        private void PlayerUpdate(S.PlayerUpdate p)
        {
            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                if (MapControl.Objects[i].ObjectID != p.ObjectID) continue;

                PlayerObject player = MapControl.Objects[i] as PlayerObject;
                if (player != null) player.Update(p);
                return;
            }
        }

        private void PlayerInspect(S.PlayerInspect p)
        {
            InspectDialog InspectDialog = GameScene.Scene.InspectDialog;
            InspectDialog.Items = p.Equipment;

            InspectDialog.Name = p.Name;
            InspectDialog.GuildName = p.GuildName;
            InspectDialog.GuildRank = p.GuildRank;
            InspectDialog.Class = p.Class;
            InspectDialog.Gender = p.Gender;
            InspectDialog.Hair = p.Hair;
            InspectDialog.Level = p.Level;
            InspectDialog.LoverName = p.LoverName;

            InspectDialog.RefreshInferface();
            InspectDialog.Show();
        }

        private void LogOutSuccess(S.LogOutSuccess p)
        {
            for (int i = 0; i <= 3; i++)//Fix for orbs sound
                SoundManager.StopSound(20000 + 126 * 10 + 5 + i);

            GameScene.User = null;
            if (Settings.Resolution != 800)
                CMain.SetResolution(800, 600);
            GameScene.ActiveScene = new SelectScene(p.Characters);
            GameScene.Scene.Dispose();
        }

        private void LogOutFailed(S.LogOutFailed p)
        {
            GameScene.Scene.Enabled = true;
        }

        private void TimeOfDay(S.TimeOfDay p)
        {
            GameScene.Scene.Lights = p.Lights;
            switch (Lights)
            {
                case LightSetting.Day:
                case LightSetting.Normal:
                    MiniMapDialog.LightSetting.Index = 2093;
                    break;
                case LightSetting.Dawn:
                    MiniMapDialog.LightSetting.Index = 2095;
                    break;
                case LightSetting.Evening:
                    MiniMapDialog.LightSetting.Index = 2094;
                    break;
                case LightSetting.Night:
                    MiniMapDialog.LightSetting.Index = 2092;
                    break;
            }
        }
        private void ChangeAMode(S.ChangeAMode p)
        {
            AMode = p.Mode;

            switch (p.Mode)
            {
                case AttackMode.Peace:
                    ChatDialog.ReceiveChat("[Attack Mode: Peaceful]", ChatType.Hint);
                    break;
                case AttackMode.Group:
                    ChatDialog.ReceiveChat("[Attack Mode: Group]", ChatType.Hint);
                    break;
                case AttackMode.Guild:
                    ChatDialog.ReceiveChat("[Attack Mode: Guild]", ChatType.Hint);
                    break;
                case AttackMode.EnemyGuild:
                    ChatDialog.ReceiveChat("[Attack Mode: Enemy Guild]", ChatType.Hint);
                    break;
                case AttackMode.RedBrown:
                    ChatDialog.ReceiveChat("[Attack Mode: Red+Brown]", ChatType.Hint);
                    break;
                case AttackMode.All:
                    ChatDialog.ReceiveChat("[Attack Mode: All]", ChatType.Hint);
                    break;
            }
        }
    }
}
