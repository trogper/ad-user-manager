using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;

namespace AdUserManager
{
    class LdapUtils
    {

        static public string DomainName;
        static public string currentUser;
        static public bool WritePermitted;

        static string ManagersGroupDN;

        static PrincipalContext usersContext, groupsContext;

        static public void init()
        {
            string ManagedGroupsDN = Properties.Settings.Default.groups_dn;
            string ManagedUsersDN = Properties.Settings.Default.users_dn;
            string dcName = Properties.Settings.Default.dc_name;
            string username = Properties.Settings.Default.username;
            string password = Properties.Settings.Default.password;

            DomainName = Properties.Settings.Default.domain_name;
            ManagersGroupDN = Properties.Settings.Default.managers_group_dn;

            if (string.IsNullOrEmpty(DomainName))
                DomainName = ExtractDomainName(ManagedUsersDN);

            if (string.IsNullOrEmpty(dcName))
            {
                var ctx = new DirectoryContext(DirectoryContextType.Domain);
                var dc = DomainController.FindOne(ctx);
                dcName = dc.Name;
            }

            if (string.IsNullOrEmpty(username))
            {
                usersContext = new PrincipalContext(ContextType.Domain, dcName, ManagedUsersDN);
                groupsContext = new PrincipalContext(ContextType.Domain, dcName, ManagedGroupsDN);
            }
            else
            {
                usersContext = new PrincipalContext(ContextType.Domain, dcName, ManagedUsersDN, username, password);
                groupsContext = new PrincipalContext(ContextType.Domain, dcName, ManagedGroupsDN, username, password);
            }

            // validate credentials
            try
            {
                new PrincipalSearcher(new UserPrincipal(usersContext) { Name = "*" }).FindOne();
            }
            // catch Principal not found
            catch (PrincipalOperationException poex) { }

            WritePermitted = IsReadWrite();
            currentUser = UserPrincipal.Current.SamAccountName;
        }

        public static string ExtractDomainName(string DN)
        {
            var dcs = new List<string>();

            Regex regex = new Regex("(?<=,DC\\=)[^,.]*(?=,)");

            foreach (Match match in regex.Matches($",${DN},"))
            {
                dcs.Add(match.Value);
            }

            return string.Join(".", dcs);
        }

        private static bool IsReadWrite()
        {
            if (string.IsNullOrWhiteSpace(ManagersGroupDN))
                return true;

            var currentUser = UserPrincipal.Current;
            var isManager = currentUser
                .GetGroups()
                .Select(g => g.DistinguishedName)
                .Contains(ManagersGroupDN);

            return isManager;
        }

        static public UserPrincipal NewUser()
        {
            return new UserPrincipal(usersContext);
        }

        static public IEnumerable<GroupPrincipal> SearchGroups(GroupPrincipal parGroupPrincipal)
        {
            PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();
            insPrincipalSearcher.QueryFilter = parGroupPrincipal;
            var results = insPrincipalSearcher.FindAll();
            return results.OfType<GroupPrincipal>();
        }
        static public IEnumerable<UserPrincipal> SearchUsers(UserPrincipal parUserPrincipal)
        {
            PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();
            insPrincipalSearcher.QueryFilter = parUserPrincipal;
            var results = insPrincipalSearcher.FindAll();
            return results.OfType<UserPrincipal>();
        }
        static public IEnumerable<GroupPrincipal> ListGroups(PrincipalContext principalContext)
        {
            GroupPrincipal insGroupPrincipal = new GroupPrincipal(principalContext);
            insGroupPrincipal.Name = "*";
            return SearchGroups(insGroupPrincipal);
        }

        static public IEnumerable<UserPrincipal> ListUsers(PrincipalContext principalContext)
        {
            UserPrincipal insUserPrincipal = new UserPrincipal(principalContext);
            insUserPrincipal.Name = "*";
            return SearchUsers(insUserPrincipal);
        }
        static public IEnumerable<GroupPrincipal> ListManagedGroups()
        {
            return ListGroups(groupsContext);
        }

        static public IEnumerable<UserPrincipal> ListManagedUsers()
        {
            return ListUsers(usersContext);
        }
    }
}
