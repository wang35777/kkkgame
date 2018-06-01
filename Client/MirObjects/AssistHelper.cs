using Client.MirNetwork;
using Client.MirScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using C = ClientPackets;

namespace Client.MirObjects
{
    public class AssistHelper
    {
        private long autoFireTick;
        private long lastFireTick;

        private long lastUseItemTick;

        private int usePoisonShape = 1;

        public void process()
        {
            if (Settings.smartFireHit && CMain.Time - autoFireTick > 3000 && UserObject.User.MP >= 7 && (!GameScene.NextTimeFireHit)
                && (CMain.Time - GameScene.LastFireHitTick > 10000))
            {
                autoFireTick = CMain.Time;
                GameScene.Scene.UseSpell(Spell.FlamingSword);
            }

            if (Settings.smartSheild && !UserObject.User.MagicShield)
            {
                GameScene.Scene.UseSpell(Spell.MagicShield);
            }
           
            autoUseItem();
        }

        private void autoUseItem()
        {
            if (Settings.autoEatItem)
            {
                autoEatHp();
                autoEatMp();
            }
        }

        private void autoEatMp()
        {
            UserObject User = UserObject.User;
            if (UserObject.User.MP * 100 / UserObject.User.MaxMP < Settings.percentMpProtect && CMain.Time - lastUseItemTick > 3000)
            {
                lastUseItemTick = CMain.Time;
                //GameScene.UseItemTime
                for (int i = 0; i < User.Inventory.Length; i++)
                {
                    UserItem item = User.Inventory[i];

                    if (item != null && item.Info != null && item.Info.Name.Equals(Settings.mpItemName))
                    {
                        Network.Enqueue(new C.UseItem { UniqueID = item.UniqueID });
                        break;
                    }
                }
            }
        }

        private void autoEatHp()
        {
            UserObject User = UserObject.User;
            if (UserObject.User.PercentHealth < Settings.percentHpProtect && CMain.Time - lastUseItemTick > 3000)
            {
                lastUseItemTick = CMain.Time;
                //GameScene.UseItemTime
                for (int i = 0; i < User.Inventory.Length; i++)
                {
                    UserItem item = User.Inventory[i];

                    if (item != null && item.Info != null && item.Info.Name.Equals(Settings.hpItemName))
                    {
                        Network.Enqueue(new C.UseItem { UniqueID = item.UniqueID });
                        break;
                    }
                }
            }
        }

        public void prevSendUseMagic(ClientMagic magic)
        {
            UserObject User = UserObject.User;
            switch (magic.Spell)
            {
                case Spell.Poisoning:
                    {
                        if (Settings.smartChangePoison)
                        {
                            UserItem item = User.GetPoison(usePoisonShape);
                            if (item == null)
                            {
                                usePoisonShape++;
                                if (usePoisonShape > 2)
                                    usePoisonShape = 1;

                                item = User.GetPoison(usePoisonShape);
                            }

                            if (item != null)
                                Network.Enqueue(new C.EquipItem { Grid = MirGridType.Inventory, UniqueID = item.UniqueID, To = (int)EquipmentSlot.Amulet });

                            usePoisonShape++;
                            if (usePoisonShape > 2)
                                usePoisonShape = 1;
                        }
                    }
                    break;

                case Spell.SoulFireBall:
                case Spell.SummonSkeleton:
                case Spell.SummonHolyDeva:
                case Spell.SummonShinsu:
                case Spell.Hiding:
                case Spell.MassHiding:
                case Spell.SoulShield:
                case Spell.TrapHexagon:
                case Spell.Curse:
                case Spell.Plague:
                    {
                        UserItem item = User.GetAmulet(1);
                        if (item != null)
                            Network.Enqueue(new C.EquipItem { Grid = MirGridType.Inventory, UniqueID = item.UniqueID, To = (int)EquipmentSlot.Amulet });
                    }
                    break;
            }
        }
    }
}
