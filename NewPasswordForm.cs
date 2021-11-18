using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography;
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
            password1TextBox.UseSystemPasswordChar = true;
            password1TextBox.Text = "";
            password2TextBox.Text = "";
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

        private void generatePasswordButton_Click(object sender, EventArgs e)
        {
            int length = Properties.Settings.Default.generated_password_length;
            bool includeSpecial = Properties.Settings.Default.generated_password_include_special;

            string chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            if (includeSpecial)
                chars += "+-*/.!?$_ ";

            var rng = RandomNumberGenerator.Create();
            byte[] rnd = new byte[length];
            rng.GetBytes(rnd);

            string password = "";

            for (int i = 0; i < length; i++)
            {
                int m = rnd[i] % chars.Length;
                password += chars[m];
            }

            password1TextBox.UseSystemPasswordChar = false;
            password1TextBox.Text = password;
            password2TextBox.Text = password;
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
