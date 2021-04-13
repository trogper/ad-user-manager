using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Windows.Forms;

namespace AdUserManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        List<GroupPrincipal> managedGroups;
        List<UserPrincipal> managedUsers;
        List<List<GroupPrincipal>> managedUsersGroups;

        UserEditForm userEditForm = new UserEditForm();
        NewPasswordForm newPasswordForm = new NewPasswordForm();

        private void MainForm_Load(object sender, EventArgs e)
        {
            LdapUtils.init();

            LoadGroups();
            LoadUsers();
        }

        private void LoadGroups()
        {
            managedGroups = LdapUtils.ListManagedGroups().ToList();
            userEditForm.LoadManagedGroups(managedGroups);
        }

        private void LoadUsers()
        {
            var selIndex = userGridView.SelectedCells.Count > 0
                ? userGridView.SelectedCells[0].RowIndex
                : 0;

            userGridView.Rows.Clear();

            managedUsers = LdapUtils.ListManagedUsers().ToList();
            managedUsersGroups = new List<List<GroupPrincipal>>();

            foreach (var user in managedUsers)
            {
                var groups = user.GetGroups()
                    .OfType<GroupPrincipal>()
                    .Where(IsManaged);

                managedUsersGroups.Add(groups.ToList());

                var i = userGridView.Rows.Add(new object[] {
                    user.SamAccountName,
                    user.DisplayName,
                    string.Join(",", groups.Select(p => p.Name)),
                    user.Enabled,
                    user.IsAccountLockedOut()
                });

                userGridView.Rows[i].ContextMenuStrip = rowContextMenuStrip;

            }

            userGridView.Rows[selIndex].Selected = true;
        }



        private bool IsManaged(GroupPrincipal group)
        {
            var ism =  managedGroups.Contains(group);
            return ism;

            //DirectoryEntry de = group.GetUnderlyingObject() as DirectoryEntry;

            //if (de != null)
            //{
            //    string managedby = (string)de.Properties["managedBy"].Value;
            //    if (managedby == UAMGroupDN)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        private void EditUser(UserPrincipal user)
        {
            var index = managedUsers.IndexOf(user);
            var groups = managedUsersGroups[index];

            userEditForm.LoadUser(user, groups);
            var res = userEditForm.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
                LoadUsers();
        }

        private void EnableDisableUser(UserPrincipal user)
        {
            try
            {
                user.Enabled = !user.Enabled;
                user.Save();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Account could not be enabled or disabled\n" + ex.Message, "Enable/disable failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnlockUser(UserPrincipal user)
        {
            try
            {
                user.UnlockAccount();
                user.Save();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Account could not be unlocked\n" + ex.Message, "Unlock failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NewPassword(UserPrincipal user)
        {
            newPasswordForm.LoadUser(user);
            var res = newPasswordForm.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
                LoadUsers();
        }

        private void DeleteUser(UserPrincipal user)
        {
            try
            {
                user.Delete();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "User could not be deleted\n" + ex.Message, "Delete failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private UserPrincipal GetSelectedUser()
        {
            var index = userGridView.SelectedCells[0].RowIndex;
            return managedUsers[index];
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var user = GetSelectedUser();
            var enabled = user.Enabled == true;
            var lockedOut = user.IsAccountLockedOut();

            enableDisableToolStripMenuItem.Text = enabled ? "Disable" : "Enable";
            unlockAccountToolStripMenuItem.Enabled = lockedOut;
        }

        private void newUser_Click(object sender, EventArgs e)
        {
            var user = LdapUtils.NewUser();
            userEditForm.LoadUser(user, new List<GroupPrincipal>());
            var res = userEditForm.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
                LoadUsers();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUsers();
            LoadGroups();
        }

        private void editUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditUser(GetSelectedUser());
        }

        private void enableDisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnableDisableUser(GetSelectedUser());
        }

        private void unlockAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnlockUser(GetSelectedUser());
        }

        private void newPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPassword(GetSelectedUser());
        }

        private void deleteUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUser(GetSelectedUser());
        }
    }
}
