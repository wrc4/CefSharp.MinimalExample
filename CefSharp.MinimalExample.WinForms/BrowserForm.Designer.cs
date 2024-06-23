namespace CefSharp.MinimalExample.WinForms
{
    partial class BrowserForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            statusLabel = new System.Windows.Forms.Label();
            outputLabel = new System.Windows.Forms.Label();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showDevToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripContainer.ContentPanel.SuspendLayout();
            toolStripContainer.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            toolStripContainer.ContentPanel.Controls.Add(statusLabel);
            toolStripContainer.ContentPanel.Controls.Add(outputLabel);
            toolStripContainer.ContentPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1217, 880);
            toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer.LeftToolStripPanelVisible = false;
            toolStripContainer.Location = new System.Drawing.Point(0, 37);
            toolStripContainer.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            toolStripContainer.Name = "toolStripContainer";
            toolStripContainer.RightToolStripPanelVisible = false;
            toolStripContainer.Size = new System.Drawing.Size(1217, 905);
            toolStripContainer.TabIndex = 0;
            toolStripContainer.Text = "toolStripContainer1";
            // 
            // statusLabel
            // 
            statusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            statusLabel.Location = new System.Drawing.Point(0, 830);
            statusLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(1217, 25);
            statusLabel.TabIndex = 1;
            // 
            // outputLabel
            // 
            outputLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            outputLabel.Location = new System.Drawing.Point(0, 855);
            outputLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            outputLabel.Name = "outputLabel";
            outputLabel.Size = new System.Drawing.Size(1217, 25);
            outputLabel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(10, 4, 0, 4);
            menuStrip1.Size = new System.Drawing.Size(1217, 37);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showDevToolsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // showDevToolsToolStripMenuItem
            // 
            showDevToolsToolStripMenuItem.Name = "showDevToolsToolStripMenuItem";
            showDevToolsToolStripMenuItem.Size = new System.Drawing.Size(235, 34);
            showDevToolsToolStripMenuItem.Text = "Show DevTools";
            showDevToolsToolStripMenuItem.Click += ShowDevToolsMenuItemClick;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(235, 34);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitMenuItemClick;
            // 
            // BrowserForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1217, 942);
            Controls.Add(toolStripContainer);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Name = "BrowserForm";
            Text = "BrowserForm";
            toolStripContainer.ContentPanel.ResumeLayout(false);
            toolStripContainer.ResumeLayout(false);
            toolStripContainer.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ToolStripMenuItem showDevToolsToolStripMenuItem;

    }
}