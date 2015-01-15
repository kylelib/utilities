using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace Common.Tools.IO
{

    public class ImpersonateUser
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);
        private static IntPtr tokenHandle = new IntPtr(0);
        private static WindowsImpersonationContext impersonatedUser;

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public void Impersonate(string domainName, string userName, string password)
        {
            try
            {
                const int LOGON32_PROVIDER_DEFAULT = 0;
                const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
                tokenHandle = IntPtr.Zero;
                bool returnValue = LogonUser(userName, domainName, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref tokenHandle);
                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    Console.WriteLine("LogonUser call failed with error code : " + ret);
                    throw new System.ComponentModel.Win32Exception(ret);
                }
                WindowsIdentity newId = new WindowsIdentity(tokenHandle);
                impersonatedUser = newId.Impersonate();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred. " + ex.Message);
            }
        }

        public void Undo()
        {
            impersonatedUser.Undo();
            if (!(tokenHandle.Equals(IntPtr.Zero)))
            {
                CloseHandle(tokenHandle);
            }
        }
    }

}
