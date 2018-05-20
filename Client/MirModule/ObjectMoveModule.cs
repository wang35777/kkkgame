using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using S = ServerPackets;
using C = ClientPackets;
using Client.MirScenes.Dialogs;
using Client.MirObjects;
using Client.MirScenes;
using Client.MirNetwork;

namespace Client.MirModule
{
    public class ObjectMoveModule
    {
        private UserObject User;

        public void ProcessPacket(Packet p)
        {
            switch (p.Index)
            {
                case (short)ServerPacketIds.UserLocation:
                    UserLocation((S.UserLocation)p);
                    break;
                case (short)ServerPacketIds.ObjectRemove:
                    ObjectRemove((S.ObjectRemove)p);
                    break;
                case (short)ServerPacketIds.ObjectTurn:
                    ObjectTurn((S.ObjectTurn)p);
                    break;
                case (short)ServerPacketIds.ObjectWalk:
                    ObjectWalk((S.ObjectWalk)p);
                    break;
            }
        }

        private void UserLocation(S.UserLocation p)
        {
            MapControl.NextAction = 0;
            if (User.CurrentLocation == p.Location && User.Direction == p.Direction)
            {
                if (Settings.autoPick)
                {
                    if (CMain.Time > GameScene.PickUpTime)
                    {
                        GameScene.PickUpTime = CMain.Time + 200;
                        Network.Enqueue(new C.PickUp());
                    }
                }
                return;
            }

            if (Settings.DebugMode)
                GameScene.Scene.ReceiveChat(new S.Chat { Message = "Displacement", Type = ChatType.System });

            GameScene.Scene.MapControl.RemoveObject(User);
            User.CurrentLocation = p.Location;
            User.MapLocation = p.Location;
            GameScene.Scene.MapControl.AddObject(User);

            GameScene.Scene.MapControl.FloorValid = false;
            MapControl.InputDelay = CMain.Time + 400;

            if (User.Dead) return;

            User.ClearMagic();
            User.QueuedAction = null;

            for (int i = User.ActionFeed.Count - 1; i >= 0; i--)
            {
                if (User.ActionFeed[i].Action == MirAction.Pushed) continue;
                User.ActionFeed.RemoveAt(i);
            }

            User.SetAction();
        }

        private void ObjectRemove(S.ObjectRemove p)
        {
            if (p.ObjectID == User.ObjectID) return;

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];
                if (ob.ObjectID != p.ObjectID) continue;
                ob.Remove();
            }
        }

        private void ObjectTurn(S.ObjectTurn p)
        {
            if (p.ObjectID == User.ObjectID) return;

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];
                if (ob.ObjectID != p.ObjectID) continue;
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Standing, Direction = p.Direction, Location = p.Location });
                return;
            }
        }

        private void ObjectWalk(S.ObjectWalk p)
        {
            if (p.ObjectID == User.ObjectID) return;

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];
                if (ob.ObjectID != p.ObjectID) continue;
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Walking, Direction = p.Direction, Location = p.Location });
                return;
            }
        }

        private void ObjectRun(S.ObjectRun p)
        {
            if (p.ObjectID == User.ObjectID) return;

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];
                if (ob.ObjectID != p.ObjectID) continue;
                ob.ActionFeed.Add(new QueuedAction { Action = MirAction.Running, Direction = p.Direction, Location = p.Location });
                return;
            }
        }
    }
}
