namespace Chaso.Push.Server
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChasoPushServerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ChasoPushServerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ChasoPushServerServiceProcessInstaller
            // 
            this.ChasoPushServerServiceProcessInstaller.Password = null;
            this.ChasoPushServerServiceProcessInstaller.Username = null;
            // 
            // ChasoPushServerServiceInstaller
            // 
            this.ChasoPushServerServiceInstaller.Description = "A SignalR Hub for Push Notification";
            this.ChasoPushServerServiceInstaller.DisplayName = "Chaso.Push.Server";
            this.ChasoPushServerServiceInstaller.ServiceName = "Chaso.Push.Server";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ChasoPushServerServiceProcessInstaller,
            this.ChasoPushServerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ChasoPushServerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ChasoPushServerServiceInstaller;
    }
}