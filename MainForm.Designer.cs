namespace AdUserManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.userGridView = new System.Windows.Forms.DataGridView();
            this.dgContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newUserToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rowContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableDisableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlockAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newUserButton = new System.Windows.Forms.Button();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Groups = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LockedOut = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.userGridView)).BeginInit();
            this.dgContextMenuStrip.SuspendLayout();
            this.rowContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // userGridView
            // 
            this.userGridView.AllowUserToAddRows = false;
            this.userGridView.AllowUserToDeleteRows = false;
            this.userGridView.AllowUserToResizeRows = false;
            this.userGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.userGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LogonName,
            this.Username,
            this.Groups,
            this.Enabled,
            this.LockedOut});
            this.userGridView.ContextMenuStrip = this.dgContextMenuStrip;
            this.userGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userGridView.Location = new System.Drawing.Point(0, 0);
            this.userGridView.MultiSelect = false;
            this.userGridView.Name = "userGridView";
            this.userGridView.ReadOnly = true;
            this.userGridView.RowHeadersVisible = false;
            this.userGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.userGridView.ShowCellToolTips = false;
            this.userGridView.ShowEditingIcon = false;
            this.userGridView.Size = new System.Drawing.Size(800, 450);
            this.userGridView.TabIndex = 0;
            // 
            // dgContextMenuStrip
            // 
            this.dgContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newUserToolStripMenuItem1,
            this.refreshToolStripMenuItem});
            this.dgContextMenuStrip.Name = "dgContextMenuStrip";
            this.dgContextMenuStrip.Size = new System.Drawing.Size(134, 48);
            // 
            // newUserToolStripMenuItem1
            // 
            this.newUserToolStripMenuItem1.Name = "newUserToolStripMenuItem1";
            this.newUserToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.newUserToolStripMenuItem1.Text = "New User...";
            this.newUserToolStripMenuItem1.Click += new System.EventHandler(this.newUser_Click);
            // 
            // rowContextMenuStrip
            // 
            this.rowContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newUserToolStripMenuItem,
            this.toolStripSeparator1,
            this.editUserToolStripMenuItem,
            this.enableDisableToolStripMenuItem,
            this.unlockAccountToolStripMenuItem,
            this.newPasswordToolStripMenuItem,
            this.deleteUserToolStripMenuItem});
            this.rowContextMenuStrip.Name = "contextMenuStrip";
            this.rowContextMenuStrip.Size = new System.Drawing.Size(161, 142);
            this.rowContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // newUserToolStripMenuItem
            // 
            this.newUserToolStripMenuItem.Name = "newUserToolStripMenuItem";
            this.newUserToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.newUserToolStripMenuItem.Text = "New User...";
            this.newUserToolStripMenuItem.Click += new System.EventHandler(this.newUser_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // editUserToolStripMenuItem
            // 
            this.editUserToolStripMenuItem.Name = "editUserToolStripMenuItem";
            this.editUserToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.editUserToolStripMenuItem.Text = "Edit User";
            this.editUserToolStripMenuItem.Click += new System.EventHandler(this.editUserToolStripMenuItem_Click);
            // 
            // enableDisableToolStripMenuItem
            // 
            this.enableDisableToolStripMenuItem.Name = "enableDisableToolStripMenuItem";
            this.enableDisableToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.enableDisableToolStripMenuItem.Text = "Enable/Disable";
            this.enableDisableToolStripMenuItem.Click += new System.EventHandler(this.enableDisableToolStripMenuItem_Click);
            // 
            // unlockAccountToolStripMenuItem
            // 
            this.unlockAccountToolStripMenuItem.Name = "unlockAccountToolStripMenuItem";
            this.unlockAccountToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.unlockAccountToolStripMenuItem.Text = "Unlock Account";
            this.unlockAccountToolStripMenuItem.Click += new System.EventHandler(this.unlockAccountToolStripMenuItem_Click);
            // 
            // newPasswordToolStripMenuItem
            // 
            this.newPasswordToolStripMenuItem.Name = "newPasswordToolStripMenuItem";
            this.newPasswordToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.newPasswordToolStripMenuItem.Text = "New Password...";
            this.newPasswordToolStripMenuItem.Click += new System.EventHandler(this.newPasswordToolStripMenuItem_Click);
            // 
            // deleteUserToolStripMenuItem
            // 
            this.deleteUserToolStripMenuItem.Name = "deleteUserToolStripMenuItem";
            this.deleteUserToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.deleteUserToolStripMenuItem.Text = "Delete";
            this.deleteUserToolStripMenuItem.Click += new System.EventHandler(this.deleteUserToolStripMenuItem_Click);
            // 
            // newUserButton
            // 
            this.newUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.newUserButton.Location = new System.Drawing.Point(713, 415);
            this.newUserButton.Name = "newUserButton";
            this.newUserButton.Size = new System.Drawing.Size(75, 23);
            this.newUserButton.TabIndex = 1;
            this.newUserButton.Text = "New user";
            this.newUserButton.UseVisualStyleBackColor = true;
            this.newUserButton.Click += new System.EventHandler(this.newUser_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // LogonName
            // 
            this.LogonName.HeaderText = "Logon name";
            this.LogonName.Name = "LogonName";
            this.LogonName.ReadOnly = true;
            // 
            // Username
            // 
            this.Username.HeaderText = "Display name";
            this.Username.Name = "Username";
            this.Username.ReadOnly = true;
            this.Username.Width = 200;
            // 
            // Groups
            // 
            this.Groups.HeaderText = "Groups";
            this.Groups.Name = "Groups";
            this.Groups.ReadOnly = true;
            this.Groups.Width = 300;
            // 
            // Enabled
            // 
            this.Enabled.DataPropertyName = "Enabled";
            this.Enabled.HeaderText = "Enabled";
            this.Enabled.MinimumWidth = 50;
            this.Enabled.Name = "Enabled";
            this.Enabled.ReadOnly = true;
            this.Enabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Enabled.Width = 70;
            // 
            // LockedOut
            // 
            this.LockedOut.HeaderText = "Locked out";
            this.LockedOut.MinimumWidth = 50;
            this.LockedOut.Name = "LockedOut";
            this.LockedOut.ReadOnly = true;
            this.LockedOut.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LockedOut.Width = 70;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.newUserButton);
            this.Controls.Add(this.userGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "AD User Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.userGridView)).EndInit();
            this.dgContextMenuStrip.ResumeLayout(false);
            this.rowContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView userGridView;
        private System.Windows.Forms.Button newUserButton;
        private System.Windows.Forms.ContextMenuStrip rowContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem editUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableDisableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteUserToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip dgContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newUserToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem unlockAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewTextBoxColumn Groups;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enabled;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LockedOut;
    }
}

