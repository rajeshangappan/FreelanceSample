using DemoLibrary;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;

namespace Sqlitesamapp
{
    [RunInstaller(true)]
    public partial class SetupCustomAction : System.Configuration.Install.Installer
    {
        public SetupCustomAction()
        {
            InitializeComponent();
        }

        private static void GrantAccess(string file)
        {
            try
            {
                DirectorySecurity dirsec = null;
                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                DirectoryInfo dInfo = new DirectoryInfo(file);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                dSecurity.AddAccessRule(new FileSystemAccessRule(
                        new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                        FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                        PropagationFlags.NoPropagateInherit,
                        AccessControlType.Allow));
                dInfo.SetAccessControl(dSecurity);
            }
            catch(Exception ex)
            {
                MessageBox.Show("error");
            }

        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            string fileName = "DemoDB.db";
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string dbfile = System.IO.Path.Combine(folderPath, fileName);
            MessageBox.Show(dbfile);
            GrantAccess(dbfile);
        }

        //public override void Install(IDictionary stateSaver)
        //{
        //    try
        //    {
        //        //MessageBox.Show("installing");
        //        string fileName = "test.db";
        //        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //        string dbFilePath = System.IO.Path.Combine(folderPath, fileName);
        //        MessageBox.Show(dbFilePath);
        //        if (File.Exists(dbFilePath))
        //        {
        //            File.Delete(dbFilePath);
        //            MessageBox.Show("deleted");
        //        }
        //        MessageBox.Show("installing");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    base.Install(stateSaver);
        //}

        //public override void Uninstall(IDictionary savedState)
        //{

        //    try
        //    {
        //        MessageBox.Show("uninstalling");
        //        //if (File.Exists(SqliteDataAccess.dbFilePath))
        //        //{
        //        //    MessageBox.Show("uninstalling1");
        //        //    File.Delete(SqliteDataAccess.dbFilePath);
        //        //    MessageBox.Show("succ");
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    base.Uninstall(savedState);
        //}
        //protected override void OnAfterUninstall(IDictionary savedState)
        //{
        //    try
        //    {
        //        string fileName = "test.db";
        //        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        string dbFilePath = System.IO.Path.Combine(folderPath, fileName);
        //        MessageBox.Show(dbFilePath);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    //System.IO.Directory.Delete(dbFilePath, true);
        //}
    }
}
