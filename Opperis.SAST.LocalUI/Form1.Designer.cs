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
            btnScan.Location = new Point(186, 166);
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
            chkIncludeBindings.Location = new Point(12, 127);
            chkIncludeBindings.Name = "chkIncludeBindings";
            chkIncludeBindings.Size = new Size(185, 19);
            chkIncludeBindings.TabIndex = 7;
            chkIncludeBindings.Text = "Include diagnostics in output?";
            chkIncludeBindings.TextAlign = ContentAlignment.MiddleRight;
            chkIncludeBindings.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(465, 212);
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
    }
}