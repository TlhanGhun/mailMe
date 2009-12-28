using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using libSnarlStyles;
using Microsoft.Win32;
using System.Reflection;
using System.Windows.Interop;
using prefs_kit_d2;
using Winkle;

namespace mailMe
{

    [Guid("A8913D3A-9805-429D-A2C0-58441EF511C5"), ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual), ComSourceInterfaces(typeof(libSnarlStyles.IStyleEngine))]
    [ProgId("mailMe.styleengine")]    
    public class styleengine : IStyleEngine
    {
        private int hwndOfPrferencesWindows = 0;
        private Options myPrefWindow;
               
        public styleengine() {
            myPrefWindow = new Options();
            hwndOfPrferencesWindows = (int)myPrefWindow.Handle;
            myPrefWindow.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Options_FormClosed);

            Winkle.VersionCheck myUpdateChecker = new Winkle.VersionCheck("Mail me", "http://tlhan-ghun.de/files/mailMe.xml");
            Winkle.UpdateInfo myUpdateResponse = myUpdateChecker.checkForUpdate(System.Reflection.Assembly.GetExecutingAssembly(), false);
        }

        private void Options_FormClosed(Object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            myPrefWindow = new Options();
            hwndOfPrferencesWindows = (int)myPrefWindow.Handle;
            myPrefWindow.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Options_FormClosed);  
        }

        #region IStyleEngine Members

        [ComVisible(true)]
        int IStyleEngine.CountStyles()
        {
            return 1;
        }

        [ComVisible(true)]
        IStyleInstance IStyleEngine.CreateInstance(string StyleName)
        {
            StyleInstance myInstance = new StyleInstance();
            return myInstance;

        }

        [ComVisible(true)]
        string IStyleEngine.Date()
        {
            return "2009-12-27";
        }

        [ComVisible(true)]
        string IStyleEngine.Description()
        {
            return "Sends the notification using email";
        }

        [ComVisible(true)]
        int IStyleEngine.GetConfigWindow(string StyleName)
        {
            return hwndOfPrferencesWindows;
        }

        [ComVisible(true)]
        melon.M_RESULT IStyleEngine.Initialize()
        {
            return melon.M_RESULT.M_OK;
        }

        [ComVisible(true)]
        string IStyleEngine.LastError()
        {
            return "Undefined error";
        }

        [ComVisible(true)]
        string IStyleEngine.Name()
        {
            return "Mail me";

        }

        [ComVisible(true)]
        string IStyleEngine.Path()
        {
            return Assembly.GetExecutingAssembly().CodeBase;
       
        }

        [ComVisible(true)]
        int IStyleEngine.Revision()
        {
            return 4;
        }

        [ComVisible(true)]
        void IStyleEngine.StyleAt(int Index, ref style_info Style)
        {
            string pathToIcon = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\mailMe.ico";
            pathToIcon = pathToIcon.Replace("file:\\", "");

            Style.Flags = S_STYLE_FLAGS.S_STYLE_IS_WINDOWLESS | S_STYLE_FLAGS.S_STYLE_IS_CONFIGURABLE;
            Style.IconPath = pathToIcon;
            Style.Major = Assembly.GetExecutingAssembly().GetName().Version.Major;
            Style.Minor = Assembly.GetExecutingAssembly().GetName().Version.Minor;
            Style.Name = "Mail me";
            Style.Path = Assembly.GetExecutingAssembly().CodeBase;
            Style.Schemes = "Default";
            Style.Copyright = "Tlhan Ghun";
            Style.SupportEmail = "info@tlhan-ghun.de";
            Style.URL = "http://tlhan-ghun.de/";
            Style.Description = "Sends the notification using email";
        }

        [ComVisible(true)]
        void IStyleEngine.TidyUp()
        {
            myPrefWindow.Close();
            return;
        }

        [ComVisible(true)]
        int IStyleEngine.Version()
        {
            return 39;
        }

        #endregion

        #region COM Registration Methods


        [ComRegisterFunction()]
        public static void RegisterClass(string key)
        {
            StringBuilder skey = new StringBuilder(key);
            skey.Replace(@"HKEY_CLASSES_ROOT\", "");
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(skey.ToString(), true);
            RegistryKey ctrl = regKey.CreateSubKey("Control");
            ctrl.Close();
            RegistryKey inprocServer32 = regKey.OpenSubKey("InprocServer32", true);
            inprocServer32.SetValue("CodeBase", Assembly.GetExecutingAssembly().CodeBase);
            inprocServer32.Close();
            regKey.Close();
        }


        [ComUnregisterFunction()]
        public static void UnregisterClass(string key)
        {
            StringBuilder skey = new StringBuilder(key);
            skey.Replace(@"HKEY_CLASSES_ROOT\", "");
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(skey.ToString(), true);
            regKey.DeleteSubKey("Control", false);
            RegistryKey inprocServer32 = regKey.OpenSubKey("InprocServer32", true);
            regKey.DeleteSubKey("CodeBase", false);
            regKey.Close();
        }
        #endregion
    }
}