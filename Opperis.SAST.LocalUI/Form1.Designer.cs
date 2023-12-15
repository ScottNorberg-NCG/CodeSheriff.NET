namespace Opperis.SAST.LocalUI
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
            label1 = new Label();
            txtSolutionFile = new TextBox();
            btnChooseSolution = new Button();
            openFileDialog1 = new OpenFileDialog();
            folderBrowserDialog1 = new FolderBrowserDialog();
            label2 = new Label();
            txtResultsFolder = new TextBox();
            btnChooseFolder = new Button();
            btnScan = new Button();
            chkIncludeBindings = new CheckBox();
            chkNuGet = new CheckBox();
            chkTrufflehog = new CheckBox();
            lblTrufflehog = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 0;
            label1.Text = "Solution File:";
            // 
            // txtSolutionFile
            // 
            txtSolutionFile.Location = new Point(12, 27);
            txtSolutionFile.Name = "txtSolutionFile";
            txtSolutionFile.Size = new Size(292, 23);
            txtSolutionFile.TabIndex = 1;
            // 
            // btnChooseSolution
            // 
            btnChooseSolution.Location = new Point(310, 27);
            btnChooseSolution.Name = "btnChooseSolution";
            btnChooseSolution.Size = new Size(141, 23);
            btnChooseSolution.TabIndex = 2;
            btnChooseSolution.Text = "Choose Solution";
            btnChooseSolution.UseVisualStyleBackColor = true;
            btnChooseSolution.Click += btnChooseSolution_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "sln";
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 68);
            label2.Name = "label2";
            label2.Size = new Size(83, 15);
            label2.TabIndex = 3;
            label2.Text = "Results Folder:";
            // 
            // txtResultsFolder
            // 
            txtResultsFolder.Location = new Point(12, 86);
            txtResultsFolder.Name = "txtResultsFolder";
            txtResultsFolder.Size = new Size(292, 23);
            txtResultsFolder.TabIndex = 4;
            // 
            // btnChooseFolder
            // 
            btnChooseFolder.Location = new Point(310, 85);
            btnChooseFolder.Name = "btnChooseFolder";
            btnChooseFolder.Size = new Size(141, 23);
            btnChooseFolder.TabIndex = 5;
            btnChooseFolder.Text = "Choose Folder";
            btnChooseFolder.UseVisualStyleBackColor = true;
            btnChooseFolder.Click += btnChooseFolder_Click;
            // 
            // btnScan
            // 
            btnScan.Location = new Point(155, 203);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(118, 30);
            btnScan.TabIndex = 6;
            btnScan.Text = "Scan Now";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // chkIncludeBindings
            // 
            chkIncludeBindings.AutoSize = true;
            chkIncludeBindings.Location = new Point(12, 165);
            chkIncludeBindings.Name = "chkIncludeBindings";
            chkIncludeBindings.Size = new Size(185, 19);
            chkIncludeBindings.TabIndex = 7;
            chkIncludeBindings.Text = "Include diagnostics in output?";
            chkIncludeBindings.TextAlign = ContentAlignment.MiddleRight;
            chkIncludeBindings.UseVisualStyleBackColor = true;
            // 
            // chkNuGet
            // 
            chkNuGet.AutoSize = true;
            chkNuGet.Checked = true;
            chkNuGet.CheckState = CheckState.Checked;
            chkNuGet.Location = new Point(12, 140);
            chkNuGet.Name = "chkNuGet";
            chkNuGet.Size = new Size(224, 19);
            chkNuGet.TabIndex = 9;
            chkNuGet.Text = "Check for vulnerable NuGet packages";
            chkNuGet.TextAlign = ContentAlignment.MiddleRight;
            chkNuGet.UseVisualStyleBackColor = true;
            // 
            // chkTrufflehog
            // 
            chkTrufflehog.AutoSize = true;
            chkTrufflehog.Location = new Point(12, 115);
            chkTrufflehog.Name = "chkTrufflehog";
            chkTrufflehog.Size = new Size(107, 19);
            chkTrufflehog.TabIndex = 10;
            chkTrufflehog.Text = "Use Trufflehog?";
            chkTrufflehog.TextAlign = ContentAlignment.MiddleRight;
            chkTrufflehog.UseVisualStyleBackColor = true;
            // 
            // lblTrufflehog
            // 
            lblTrufflehog.AutoSize = true;
            lblTrufflehog.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblTrufflehog.ForeColor = SystemColors.Highlight;
            lblTrufflehog.Location = new Point(125, 116);
            lblTrufflehog.Name = "lblTrufflehog";
            lblTrufflehog.Size = new Size(20, 15);
            lblTrufflehog.TabIndex = 11;
            lblTrufflehog.Text = "(?)";
            lblTrufflehog.Click += lblTrufflehog_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(465, 248);
            Controls.Add(lblTrufflehog);
            Controls.Add(chkTrufflehog);
            Controls.Add(chkNuGet);
            Controls.Add(chkIncludeBindings);
            Controls.Add(btnScan);
            Controls.Add(btnChooseFolder);
            Controls.Add(txtResultsFolder);
            Controls.Add(label2);
            Controls.Add(btnChooseSolution);
            Controls.Add(txtSolutionFile);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Start Scan";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtSolutionFile;
        private Button btnChooseSolution;
        private OpenFileDialog openFileDialog1;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label2;
        private TextBox txtResultsFolder;
        private Button btnChooseFolder;
        private Button btnScan;
        private CheckBox chkIncludeBindings;
        private CheckBox chkNuGet;
        private CheckBox chkTrufflehog;
        private Label lblTrufflehog;
    }
}