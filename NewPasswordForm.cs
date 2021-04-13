using System;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;

namespace AdUserManager
{
    public partial class NewPasswordForm : Form
    {
        public NewPasswordForm()
        {
            InitializeComponent();
        }

        UserPrincipal user;

        public void LoadUser(UserPrincipal user)
        {
            this.user = user;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var password1 = password1TextBox.Text;
            var password2 = password2TextBox.Text;

            if (password1 != password2)
            {
                MessageBox.Show(this, "Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password1 == "")
            {
                var mbresult = MessageBox.Show(this, "Password is empty. User will not be able to login", "Empty password", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (mbresult.HasFlag(DialogResult.Cancel))
                    return;
                    
            }

            SavePassword(password1);
        }

        private void SavePassword(string password)
        {
            try
            {
                user.SetPassword(password);
                user.UnlockAccount();
                user.Save();

                MessageBox.Show(this, "Password has been set", "Password saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            } catch (PasswordException ex)
            {
                MessageBox.Show(this, "Password does not meet security policy", "Weak password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
