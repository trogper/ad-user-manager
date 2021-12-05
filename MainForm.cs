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
        Dictionary<UserPrincipal, List<GroupPrincipal>> managedUsersGroups;

        UserEditForm userEditForm = new UserEditForm();
        NewPasswordForm newPasswordForm = new NewPasswordForm();

        private void MainForm_Load(object sender, EventArgs e)
        {
            Utils.validateProperties();

            LdapUtils.init();
            Logging.init();

            userEditForm.StartPosition = FormStartPosition.CenterParent;
            newPasswordForm.StartPosition = FormStartPosition.CenterParent;

            if (!LdapUtils.WritePermitted)
            {
                userGridView.ContextMenuStrip = null;
                newUserButton.Enabled = false;
            }

            LoadGroups();
            LoadUsers();

            Logging.logInfo("loaded");
        }

        private void LoadGroups()
        {
            managedGroups = LdapUtils.ListManagedGroups().ToList();
            userEditForm.LoadManagedGroups(managedGroups);
        }

        private DataGridViewRow DisplayUser(UserPrincipal user, DataGridViewRow row = null)
        {
            var groups = user.GetGroups()
                    .OfType<GroupPrincipal>()
                    .Where(IsManaged);

            managedUsersGroups.Add(user, groups.ToList());

            bool expires = user.AccountExpirationDate.HasValue;
            DateTime? expiration = user.AccountExpirationDate?.Date.AddDays(-1);
            bool expired = expires && expiration.Value.CompareTo(DateTime.Now) < 0;

            string expiresStr = expires
                ? expiration.Value.ToShortDateString()
                : "never";

            var values = new object[]
            {
                user.SamAccountName,
                user.DisplayName,
                string.Join(", ", groups.Select(p => p.Name)),
                user.Enabled,
                user.IsAccountLockedOut(),
                expiresStr
            };
            
            if (row == null)
            {
                int i = userGridView.Rows.Add();
                row = userGridView.Rows[i];
            }

            row.SetValues(values);
            row.Tag = user;
            
            if (expired)
                row.Cells[5].Style.BackColor = System.Drawing.Color.LightSalmon;

            if (LdapUtils.WritePermitted)
                row.ContextMenuStrip = rowContextMenuStrip;

            return row;
        }

        private void LoadUsers(bool retainSelection = true)
        {
            var selUser = retainSelection ? GetSelectedUser() : null;

            userGridView.Rows.Clear();

            managedUsers = LdapUtils.ListManagedUsers().ToList();
            managedUsersGroups = new Dictionary<UserPrincipal, List<GroupPrincipal>>();

            foreach (var user in managedUsers)
            {
                var row = DisplayUser(user);

                if (selUser != null && selUser.SamAccountName == user.SamAccountName)
                    row.Selected = true;
            }

            SortUserGridView();

        }

        private void ReloadUser(UserPrincipal user)
        {
            var row = GetUserRow(user);
            
            user = LdapUtils.GetManagedUser(user.SamAccountName);

            DisplayUser(user, row);
            SortUserGridView();
        }

        private void SortUserGridView()
        {
            var sortColumn = userGridView.SortedColumn == null ? userGridView.Columns[0] : userGridView.SortedColumn;
            var sortOrder = userGridView.SortOrder == SortOrder.Descending ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending;
            userGridView.Sort(sortColumn, sortOrder);
        }

        private bool IsManaged(GroupPrincipal group)
        {
            return managedGroups.Contains(group);
        }

        private void EditUser(UserPrincipal user)
        {
            var index = managedUsers.IndexOf(user);
            List<GroupPrincipal> groups;
            managedUsersGroups.TryGetValue(user, out groups);

            userEditForm.LoadUser(user, groups);
            var res = userEditForm.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
            {
                ReloadUser(user);
                Logging.logInfo($"Account {user.SamAccountName} edited");
            }
        }

        private void EnableDisableUser(UserPrincipal user)
        {
            try
            {
                user.Enabled = !user.Enabled;
                user.Save();

                ReloadUser(user);

                var endis = user.Enabled == true ? "en" : "dis";
                Logging.logInfo($"Account {user.SamAccountName} {endis}abled");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Account could not be enabled or disabled\n" + ex.Message, "Enable/disable failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.logError($"Account {user.SamAccountName} could not be enabled or disabled\n{ex.Message}");
            }
        }

        private void UnlockUser(UserPrincipal user)
        {
            try
            {
                user.UnlockAccount();
                user.Save();

                ReloadUser(user);

                Logging.logInfo($"Account {user.SamAccountName} unlocked");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Account could not be unlocked\n" + ex.Message, "Unlock failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.logError($"Account {user.SamAccountName} could not be unlocked\n{ex.Message}");
            }
        }

        private void ExtendExpiration(UserPrincipal user)
        {
            try
            {
                user.AccountExpirationDate = Utils.getNextExpirationDate(user.AccountExpirationDate.Value);
                user.Save();

                ReloadUser(user);

                Logging.logInfo($"Account {user.SamAccountName} expiration extended");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Account expiration could not be extended\n" + ex.Message, "Expiration extension failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.logError($"Account {user.SamAccountName} expiration could not be extended\n{ex.Message}");
            }
        }

        private void NewPassword(UserPrincipal user)
        {
            newPasswordForm.LoadUser(user);
            var res = newPasswordForm.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
            {
                ReloadUser(user);
                Logging.logInfo($"New password set for {user.SamAccountName}");
            }
        }

        private void DeleteUser(UserPrincipal user)
        {
            string samAccountName = user.SamAccountName;

            try
            {
                var result = MessageBox.Show(this, $"Do you really want to delete user {samAccountName}?", "Delete user", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result != DialogResult.OK)
                    return;

                user.Delete();

                userGridView.Rows.Remove(GetUserRow(user));

                Logging.logInfo($"Account {samAccountName} deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "User could not be deleted\n" + ex.Message, "Delete failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.logError($"Account {samAccountName} could not be deleted\n{ex.Message}");
            }
        }

        private UserPrincipal GetSelectedUser()
        {
            if (userGridView.SelectedRows.Count == 0)
                return null;

            var user = (UserPrincipal)userGridView.SelectedRows[0].Tag;
            return user;
        }

        private DataGridViewRow GetUserRow(UserPrincipal user)
        {
            return userGridView.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Tag == user);
        }

        private void userGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                userGridView.ClearSelection();
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                    userGridView.Rows[rowSelected].Selected = true;
            }
        }

        private void rowContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var user = GetSelectedUser();
            var enabled = user.Enabled == true;
            var lockedOut = user.IsAccountLockedOut();
            var expires = user.AccountExpirationDate.HasValue;

            extendExpirationToolStripMenuItem.Enabled = expires;
            enableDisableToolStripMenuItem.Text = enabled ? "Disable" : "Enable";
            unlockAccountToolStripMenuItem.Enabled = lockedOut;
        }

        private void newUser_Click(object sender, EventArgs e)
        {
            var user = LdapUtils.NewUser();
            userEditForm.LoadUser(user, new List<GroupPrincipal>());
            var res = userEditForm.ShowDialog();

            if (res.HasFlag(DialogResult.OK))
            {
                newPasswordForm.LoadUser(user);
                newPasswordForm.ShowDialog();

                LoadUsers();
            }

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

        private void extendExpirationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendExpiration(GetSelectedUser());
        }

        private void newPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPassword(GetSelectedUser());
        }

        private void deleteUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUser(GetSelectedUser());
        }

        private void userGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var user = GetSelectedUser();

            if (user != null)
                EditUser(user);
        }
    }
}
