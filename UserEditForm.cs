using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Windows.Forms;

namespace AdUserManager
{
    public partial class UserEditForm : Form
    {
        public UserEditForm()
        {
            InitializeComponent();
        }

        List<GroupPrincipal> managedGroups;

        bool isNewUser;

        UserPrincipal user;
        List<GroupPrincipal> userGroups;

        public void LoadManagedGroups(IEnumerable<GroupPrincipal> managedGroups)
        {
            this.managedGroups = new List<GroupPrincipal>(managedGroups);

            groupListBox.Items.Clear();
            groupListBox.DisplayMember = "Name";

            foreach (var group in managedGroups)
                groupListBox.Items.Add(group.Name);
        }

        public void LoadUser(UserPrincipal user, IEnumerable<GroupPrincipal> userGroups)
        {
            this.user = user;
            this.userGroups = new List<GroupPrincipal>(userGroups);

            isNewUser = user.Guid == null;

            logonNameTextBox.ReadOnly = !isNewUser;

            firstNameTextBox.Text = user.GivenName;
            lastNameTextBox.Text = user.Surname;
            displayNameTextBox.Text = user.DisplayName;
            logonNameTextBox.Text = user.SamAccountName;
            emailTextBox.Text = user.EmailAddress;
            enabledCheckBox.Checked = user.Enabled == true || user.Enabled == null;

            logonNameTextBox.ReadOnly = !isNewUser;

            for (var i = 0; i < managedGroups.Count; i++)
            {
                var check = userGroups.Contains(managedGroups[i]);
                while (groupListBox.GetItemChecked(i) != check)
                    groupListBox.SetItemChecked(i, check);

            }

        }

        private string CapitalizeFirst(string input)
        {
            if (input == "")
                return "";

            return input[0].ToString().ToUpper() + input.Substring(1);
        }

        private string CapitalizeTextBox(TextBox textBox)
        {
            var text = textBox.Text;

            if (text.Length == 0)
                return text;

            if (char.IsLower(text[0]))
            {
                var selpos = textBox.SelectionStart;
                var sellen = textBox.SelectionLength;
                text = textBox.Text = CapitalizeFirst(text);
                textBox.SelectionStart = selpos;
                textBox.SelectionLength = sellen;
            }

            return text;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            
            var fname = CapitalizeTextBox(firstNameTextBox);
            var lname = CapitalizeTextBox(lastNameTextBox);

            var fLetter = (fname == "") ? "" : fname.Substring(0, 1);

            displayNameTextBox.Text = fname + " " + lname;

            if (isNewUser)
                logonNameTextBox.Text = Unaccent(fLetter + lname);
        }

        private void groupListBox_Click(object sender, EventArgs e)
        {
            var clientClickPos = groupListBox.PointToClient(Cursor.Position);
            if (groupListBox.IndexFromPoint(clientClickPos) <= -1)
            {
                groupListBox.SelectedItem = null;
            }
        }

        private string Unaccent(string input)
        {
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(input);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

            return asciiStr;
        }

        private (UserPrincipal, List<GroupPrincipal>) ModifyUser()
        {
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
                throw new ApplicationException("First name cannot be empty");

            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
                throw new ApplicationException("Last name cannot be empty");

            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
                throw new ApplicationException("Logon name cannot be empty");


            user.GivenName = firstNameTextBox.Text;
            user.Surname = lastNameTextBox.Text;
            user.DisplayName = displayNameTextBox.Text;

            if (isNewUser)
            {
                // Do not change logon name for existing users
                user.SamAccountName = Unaccent(logonNameTextBox.Text);
                user.UserPrincipalName = user.SamAccountName + "@" + LdapUtils.DomainName;
            }
            else
            {
                // Update user.Name
                var dirEntry = (DirectoryEntry) user.GetUnderlyingObject();
                dirEntry.Rename("CN=" + user.DisplayName);
            }

            user.EmailAddress = emailTextBox.Text;
            user.Enabled = enabledCheckBox.Checked;

            if (string.IsNullOrWhiteSpace(user.EmailAddress))
                user.EmailAddress = null;

            userGroups = managedGroups.Where((g, i) => groupListBox.CheckedIndices.Contains(i)).ToList();

            return (user, userGroups);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO validate userPrincipalName collision

                var userTuple = ModifyUser();
                userTuple.Item1.Save();

                var userGroups = userTuple.Item2;

                foreach (var group in managedGroups)
                {
                    if (userGroups.Contains(group) && !group.Members.Contains(user))
                    {
                        group.Members.Add(user);
                        group.Save();
                    }
                    else if (!userGroups.Contains(group) && group.Members.Contains(user))
                    {
                        group.Members.Remove(user);
                        group.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error saving user\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void logonNameTextBox_TextChanged(object sender, EventArgs e)
        {
            logonNameTextBox.Text = Unaccent(logonNameTextBox.Text);
        }
    }
}
