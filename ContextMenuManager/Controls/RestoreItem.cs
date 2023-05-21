﻿using BluePointLilac.Controls;
using ContextMenuManager.Controls.Interfaces;
using ContextMenuManager.Methods;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static ContextMenuManager.Methods.BackupList;

namespace ContextMenuManager.Controls
{
    interface ITsiRestoreFile
    {
        void RestoreItems(string restoreFile, RestoreMode restoreMode);
    }

    sealed class RestoreItem : MyListItem, IBtnShowMenuItem, ITsiFilePathItem, ITsiDeleteItem, ITsiRestoreItem
    {
        public RestoreItem(ITsiRestoreFile item, string filePath, string deviceName, string creatTime)
        {
            InitializeComponents();
            restoreInterface = item;
            FilePath = filePath;
            Text = $@"备份（源计算机：{deviceName}；创建于{creatTime}）";
            Image = AppImage.BackupItem;
        }

        // 恢复函数接口对象
        private readonly ITsiRestoreFile restoreInterface;

        // 备份文件目录
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }
        public string ItemFilePath { get { return filePath; } }

        public MenuButton BtnShowMenu { get; set; }
        public FilePropertiesMenuItem TsiFileProperties { get; set; }
        public FileLocationMenuItem TsiFileLocation { get; set; }
        public DeleteMeMenuItem TsiDeleteMe { get; set; }
        public RestoreMeMenuItem TsiRestoreMe { get; set; }

        readonly ToolStripMenuItem TsiDetails = new ToolStripMenuItem(AppString.Menu.Details);

        private void InitializeComponents()
        {
            BtnShowMenu = new MenuButton(this);
            TsiFileLocation = new FileLocationMenuItem(this);
            TsiFileProperties = new FilePropertiesMenuItem(this);
            TsiDeleteMe = new DeleteMeMenuItem(this);
            TsiRestoreMe = new RestoreMeMenuItem(this);

            // 设置菜单：详细信息；删除备份；恢复备份
            ContextMenuStrip.Items.AddRange(new ToolStripItem[] { TsiDetails, new ToolStripSeparator(), 
                TsiRestoreMe, new ToolStripSeparator(), TsiDeleteMe });

            // 详细信息
            TsiDetails.DropDownItems.AddRange(new ToolStripItem[] { TsiFileProperties, TsiFileLocation });
        }

        public void DeleteMe()
        {
            File.Delete(filePath);
        }

        public void RestoreMe()
        {
            RestoreMode restoreMode;
            using (RestoreDialog dlg = new RestoreDialog())
            {
                dlg.Items = new[] { "不处理不存在于备份列表上的菜单项", "关闭不存在于备份列表上的菜单项" };
                dlg.Title = "恢复一个备份";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                restoreMode = dlg.SelectedIndex == 0 ? RestoreMode.NotHandleNotOnList : RestoreMode.DisableNotOnList;
            }
            restoreInterface.RestoreItems(filePath, restoreMode);
        }
    }
}