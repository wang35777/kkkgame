using System;
using S = ServerPackets;
using C = ClientPackets;
using Client.MirControls;
using Client.MirScenes.Dialogs;
using Client.MirScenes;

public class MailLockedItemCmd
{
    public void MailLockedItem(S.MailLockedItem p)
    {
        MirItemCell cell = GameScene.Scene.InventoryDialog.GetCell(p.UniqueID);
        if (cell != null)
            cell.Locked = p.Locked;
    }
}