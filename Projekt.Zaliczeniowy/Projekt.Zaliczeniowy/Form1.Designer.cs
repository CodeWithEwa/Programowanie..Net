namespace Projekt.Zaliczeniowy
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wybierzPlikButton = new System.Windows.Forms.Button();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.folderPathTextBox = new System.Windows.Forms.TextBox();
            this.wybierzFolderButton = new System.Windows.Forms.Button();
            this.uploadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // wybierzPlikButton
            // 
            this.wybierzPlikButton.Location = new System.Drawing.Point(60, 52);
            this.wybierzPlikButton.Name = "wybierzPlikButton";
            this.wybierzPlikButton.Size = new System.Drawing.Size(102, 24);
            this.wybierzPlikButton.TabIndex = 0;
            this.wybierzPlikButton.Text = "Wybierz Plik";
            this.wybierzPlikButton.UseVisualStyleBackColor = true;
            this.wybierzPlikButton.Click += new System.EventHandler(this.WybierzPlik_Click);
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(196, 52);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(458, 23);
            this.filePathTextBox.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(247, 225);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Szyfruj";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(328, 225);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Deszyfruj";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // folderPathTextBox
            // 
            this.folderPathTextBox.Location = new System.Drawing.Point(196, 103);
            this.folderPathTextBox.Name = "folderPathTextBox";
            this.folderPathTextBox.Size = new System.Drawing.Size(458, 23);
            this.folderPathTextBox.TabIndex = 10;
            // 
            // wybierzFolderButton
            // 
            this.wybierzFolderButton.Location = new System.Drawing.Point(60, 103);
            this.wybierzFolderButton.Name = "wybierzFolderButton";
            this.wybierzFolderButton.Size = new System.Drawing.Size(102, 23);
            this.wybierzFolderButton.TabIndex = 11;
            this.wybierzFolderButton.Text = "Wybierz Folder";
            this.wybierzFolderButton.UseVisualStyleBackColor = true;
            this.wybierzFolderButton.Click += new System.EventHandler(this.wybierzFolderButton_Click);
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(493, 390);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(75, 23);
            this.uploadButton.TabIndex = 12;
            this.uploadButton.Text = "Prześlij";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uploadButton);
            this.Controls.Add(this.wybierzFolderButton);
            this.Controls.Add(this.folderPathTextBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.wybierzPlikButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button wybierzPlikButton;
        private TextBox filePathTextBox;
        private Button button3;
        private Button button4;
        private TextBox folderPathTextBox;
        private Button wybierzFolderButton;
        private Button uploadButton;
    }
}