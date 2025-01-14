﻿using BluePointLilac.Methods;
using ContextMenuManager.Methods;
using System;
using System.Windows.Forms;

namespace ContextMenuManager.Controls
{
    sealed class DetailedEditDialog : CommonDialog
    {
        public Guid GroupGuid { get; set; }

        public override void Reset() { }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            using(SubItemsForm frm = new SubItemsForm())
            using(DetailedEditList list = new DetailedEditList())
            {
                var location = GuidInfo.GetIconLocation(GroupGuid);
                frm.Icon = ResourceIcon.GetIcon(location.IconPath, location.IconIndex);
                frm.Text = AppString.Dialog.DetailedEdit.Replace("%s", GuidInfo.GetText(GroupGuid));
                frm.TopMost = AppConfig.TopMost;
                frm.AddList(list);
                list.GroupGuid = GroupGuid;
                list.UseUserDic = XmlDicHelper.DetailedEditGuidDic[GroupGuid];
                list.LoadItems();
                frm.ShowDialog();
            }
            return false;
        }
    }
}