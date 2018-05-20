using Client.MirNetwork;
using Client.MirObjects;
using Client.MirScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using S = ServerPackets;
using C = ClientPackets;
using System.Drawing;
using Client.MirGraphics;

namespace Client.MirMagic
{
    // 仿半月
    class JinGangGunFa : BaseMagic
    {
        public override void OnUseSpell(PlayerObject player, ClientMagic magic)
        {
            GameScene scene = GameScene.Scene;
            if (CMain.Time < scene.ToggleTime) return;
            GameScene.JinGangGunFa = !GameScene.JinGangGunFa;
            scene.ChatDialog.ReceiveChat(GameScene.DaMoGunFa ? CMain.Tr("Use DaMoGunFan.") : CMain.Tr("Do not DaMoGunFa."), ChatType.Hint);
            scene.ToggleTime = CMain.Time + 1000;
            Network.Enqueue(new C.SpellToggle { Spell = magic.Spell, CanUse = GameScene.DaMoGunFa });
        }

        public override void OnDrawEffect(PlayerObject player, MirAction action)
        {
            if (action != MirAction.JumpDown)
                return;

            MirDirection Direction = player.Direction;
            int SpellLevel = player.SpellLevel;
            int FrameIndex = player.FrameIndex;
            Point DrawLocation = player.DrawLocation;
            int frame = 48 + ((int)Direction * 16) + SpellLevel * 128 + FrameIndex * 10 / 8;
            Libraries.Magic4.DrawBlend(frame, DrawLocation, Color.White, true, 0.7F);
        }
    }
}
