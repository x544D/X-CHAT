namespace IbtiChat.UC
{
    partial class ChatWindow
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
            this.worldChatWindow = new System.Windows.Forms.Panel();
            this.SendBtn = new System.Windows.Forms.Button();
            this.sendMessageTxtBox = new System.Windows.Forms.TextBox();
            this.adminPanel = new System.Windows.Forms.Panel();
            this.shutDownServer = new System.Windows.Forms.PictureBox();
            this.deleteAllMessages = new System.Windows.Forms.PictureBox();
            this.adminPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shutDownServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteAllMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // worldChatWindow
            // 
            this.worldChatWindow.Location = new System.Drawing.Point(2, 1);
            this.worldChatWindow.Name = "worldChatWindow";
            this.worldChatWindow.Size = new System.Drawing.Size(787, 420);
            this.worldChatWindow.TabIndex = 1;
            this.worldChatWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.worldChatWindow_Paint);
            // 
            // SendBtn
            // 
            this.SendBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SendBtn.Enabled = false;
            this.SendBtn.Location = new System.Drawing.Point(675, 440);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(80, 46);
            this.SendBtn.TabIndex = 2;
            this.SendBtn.Text = "ENVOYER";
            this.SendBtn.UseVisualStyleBackColor = true;
            this.SendBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // sendMessageTxtBox
            // 
            this.sendMessageTxtBox.Location = new System.Drawing.Point(15, 440);
            this.sendMessageTxtBox.Multiline = true;
            this.sendMessageTxtBox.Name = "sendMessageTxtBox";
            this.sendMessageTxtBox.Size = new System.Drawing.Size(509, 46);
            this.sendMessageTxtBox.TabIndex = 3;
            this.sendMessageTxtBox.TextChanged += new System.EventHandler(this.sendMessageTxtBox_TextChanged);
            // 
            // adminPanel
            // 
            this.adminPanel.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.adminPanel.Controls.Add(this.shutDownServer);
            this.adminPanel.Controls.Add(this.deleteAllMessages);
            this.adminPanel.Enabled = false;
            this.adminPanel.Location = new System.Drawing.Point(555, 440);
            this.adminPanel.Name = "adminPanel";
            this.adminPanel.Size = new System.Drawing.Size(91, 46);
            this.adminPanel.TabIndex = 5;
            this.adminPanel.Visible = false;
            // 
            // shutDownServer
            // 
            this.shutDownServer.BackColor = System.Drawing.Color.Transparent;
            this.shutDownServer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.shutDownServer.Image = global::IbtiChat.Properties.Resources.shutDownServer;
            this.shutDownServer.Location = new System.Drawing.Point(48, 3);
            this.shutDownServer.Name = "shutDownServer";
            this.shutDownServer.Size = new System.Drawing.Size(39, 40);
            this.shutDownServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.shutDownServer.TabIndex = 6;
            this.shutDownServer.TabStop = false;
            this.shutDownServer.Click += new System.EventHandler(this.shutDownServer_Click);
            // 
            // deleteAllMessages
            // 
            this.deleteAllMessages.BackColor = System.Drawing.Color.Transparent;
            this.deleteAllMessages.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deleteAllMessages.Image = global::IbtiChat.Properties.Resources.deleteAllMessages;
            this.deleteAllMessages.Location = new System.Drawing.Point(3, 3);
            this.deleteAllMessages.Name = "deleteAllMessages";
            this.deleteAllMessages.Size = new System.Drawing.Size(39, 40);
            this.deleteAllMessages.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.deleteAllMessages.TabIndex = 5;
            this.deleteAllMessages.TabStop = false;
            this.deleteAllMessages.Click += new System.EventHandler(this.deleteAllMessages_Click);
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.adminPanel);
            this.Controls.Add(this.sendMessageTxtBox);
            this.Controls.Add(this.SendBtn);
            this.Controls.Add(this.worldChatWindow);
            this.Name = "ChatWindow";
            this.Size = new System.Drawing.Size(771, 507);
            this.Load += new System.EventHandler(this.ChatWindow_Load);
            this.adminPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shutDownServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteAllMessages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel worldChatWindow;
        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.TextBox sendMessageTxtBox;
        private System.Windows.Forms.Panel adminPanel;
        private System.Windows.Forms.PictureBox shutDownServer;
        private System.Windows.Forms.PictureBox deleteAllMessages;
    }
}
