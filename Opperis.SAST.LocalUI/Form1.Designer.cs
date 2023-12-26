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
            lblStatusTrufflehog = new Label();
            label0 = new Label();
            label9 = new Label();
            lblStatusNuGet = new Label();
            lblStatusValueShadowing = new Label();
            label3 = new Label();
            lblStatusCsrf = new Label();
            label5 = new Label();
            lblStatusSqlInjection = new Label();
            label7 = new Label();
            label4 = new Label();
            lblStatusCryptoIVs = new Label();
            label8 = new Label();
            lblStatusCryptoECB = new Label();
            label11 = new Label();
            lblStatusDeprecatedCrypto = new Label();
            label13 = new Label();
            lblStatusRedirect = new Label();
            label15 = new Label();
            lblStatusCryptoKeys = new Label();
            label17 = new Label();
            lblStatusFile = new Label();
            label19 = new Label();
            lblStatusJSTag = new Label();
            label21 = new Label();
            lblStatusStyleTag = new Label();
            label23 = new Label();
            lblStatusLinkTag = new Label();
            label25 = new Label();
            lblStatusXssViaHelper = new Label();
            label27 = new Label();
            lblStatusXssViaRaw = new Label();
            label29 = new Label();
            lblStatusOverposting = new Label();
            label31 = new Label();
            lblStatusCookies = new Label();
            label35 = new Label();
            lblStatusConnectionString = new Label();
            label6 = new Label();
            label10 = new Label();
            label12 = new Label();
            label14 = new Label();
            label16 = new Label();
            label18 = new Label();
            label20 = new Label();
            lblStatusStep = new Label();
            lblStatusFindings = new Label();
            label22 = new Label();
            lblStatusUnclosedConnection = new Label();
            label24 = new Label();
            lblStatusInputValidation = new Label();
            label26 = new Label();
            lblStatusJWT = new Label();
            label28 = new Label();
            lblStatusRSA = new Label();
            label30 = new Label();
            lblStatusHashingAlgorithm = new Label();
            label32 = new Label();
            lblStatusSQLiViaEF = new Label();
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
            txtSolutionFile.Size = new Size(323, 23);
            txtSolutionFile.TabIndex = 1;
            // 
            // btnChooseSolution
            // 
            btnChooseSolution.Location = new Point(341, 27);
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
            txtResultsFolder.Size = new Size(323, 23);
            txtResultsFolder.TabIndex = 4;
            // 
            // btnChooseFolder
            // 
            btnChooseFolder.Location = new Point(341, 86);
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
            // lblStatusTrufflehog
            // 
            lblStatusTrufflehog.AutoSize = true;
            lblStatusTrufflehog.Location = new Point(10, 280);
            lblStatusTrufflehog.Name = "lblStatusTrufflehog";
            lblStatusTrufflehog.Size = new Size(23, 15);
            lblStatusTrufflehog.TabIndex = 12;
            lblStatusTrufflehog.Text = "0%";
            lblStatusTrufflehog.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label0
            // 
            label0.AutoSize = true;
            label0.Location = new Point(59, 280);
            label0.Name = "label0";
            label0.Size = new Size(61, 15);
            label0.TabIndex = 13;
            label0.Text = "Trufflehog";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(59, 295);
            label9.Name = "label9";
            label9.Size = new Size(113, 15);
            label9.TabIndex = 21;
            label9.Text = "NuGet Components";
            // 
            // lblStatusNuGet
            // 
            lblStatusNuGet.AutoSize = true;
            lblStatusNuGet.Location = new Point(10, 295);
            lblStatusNuGet.Name = "lblStatusNuGet";
            lblStatusNuGet.Size = new Size(23, 15);
            lblStatusNuGet.TabIndex = 20;
            lblStatusNuGet.Text = "0%";
            lblStatusNuGet.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStatusValueShadowing
            // 
            lblStatusValueShadowing.AutoSize = true;
            lblStatusValueShadowing.Location = new Point(255, 295);
            lblStatusValueShadowing.Name = "lblStatusValueShadowing";
            lblStatusValueShadowing.Size = new Size(23, 15);
            lblStatusValueShadowing.TabIndex = 14;
            lblStatusValueShadowing.Text = "0%";
            lblStatusValueShadowing.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(304, 295);
            label3.Name = "label3";
            label3.Size = new Size(97, 15);
            label3.TabIndex = 15;
            label3.Text = "Value Shadowing";
            // 
            // lblStatusCsrf
            // 
            lblStatusCsrf.AutoSize = true;
            lblStatusCsrf.Location = new Point(255, 310);
            lblStatusCsrf.Name = "lblStatusCsrf";
            lblStatusCsrf.Size = new Size(23, 15);
            lblStatusCsrf.TabIndex = 16;
            lblStatusCsrf.Text = "0%";
            lblStatusCsrf.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(304, 310);
            label5.Name = "label5";
            label5.Size = new Size(141, 15);
            label5.TabIndex = 17;
            label5.Text = "Missing CSRF Protections";
            // 
            // lblStatusSqlInjection
            // 
            lblStatusSqlInjection.AutoSize = true;
            lblStatusSqlInjection.Location = new Point(10, 333);
            lblStatusSqlInjection.Name = "lblStatusSqlInjection";
            lblStatusSqlInjection.Size = new Size(23, 15);
            lblStatusSqlInjection.TabIndex = 18;
            lblStatusSqlInjection.Text = "0%";
            lblStatusSqlInjection.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(59, 333);
            label7.Name = "label7";
            label7.Size = new Size(77, 15);
            label7.TabIndex = 19;
            label7.Text = "SQL Injection";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(60, 431);
            label4.Name = "label4";
            label4.Size = new Size(167, 15);
            label4.TabIndex = 31;
            label4.Text = "Hard-Coded Cryptography IVs";
            // 
            // lblStatusCryptoIVs
            // 
            lblStatusCryptoIVs.AutoSize = true;
            lblStatusCryptoIVs.Location = new Point(11, 431);
            lblStatusCryptoIVs.Name = "lblStatusCryptoIVs";
            lblStatusCryptoIVs.Size = new Size(23, 15);
            lblStatusCryptoIVs.TabIndex = 30;
            lblStatusCryptoIVs.Text = "0%";
            lblStatusCryptoIVs.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(60, 446);
            label8.Name = "label8";
            label8.Size = new Size(98, 15);
            label8.TabIndex = 29;
            label8.Text = "Use of ECB Mode";
            // 
            // lblStatusCryptoECB
            // 
            lblStatusCryptoECB.AutoSize = true;
            lblStatusCryptoECB.Location = new Point(11, 446);
            lblStatusCryptoECB.Name = "lblStatusCryptoECB";
            lblStatusCryptoECB.Size = new Size(23, 15);
            lblStatusCryptoECB.TabIndex = 28;
            lblStatusCryptoECB.Text = "0%";
            lblStatusCryptoECB.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(60, 461);
            label11.Name = "label11";
            label11.Size = new Size(184, 15);
            label11.TabIndex = 27;
            label11.Text = "Deprecated Symmetric Algorithm";
            // 
            // lblStatusDeprecatedCrypto
            // 
            lblStatusDeprecatedCrypto.AutoSize = true;
            lblStatusDeprecatedCrypto.Location = new Point(11, 461);
            lblStatusDeprecatedCrypto.Name = "lblStatusDeprecatedCrypto";
            lblStatusDeprecatedCrypto.Size = new Size(23, 15);
            lblStatusDeprecatedCrypto.TabIndex = 26;
            lblStatusDeprecatedCrypto.Text = "0%";
            lblStatusDeprecatedCrypto.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(304, 511);
            label13.Name = "label13";
            label13.Size = new Size(164, 15);
            label13.TabIndex = 25;
            label13.Text = "Unprotected External Redirect";
            // 
            // lblStatusRedirect
            // 
            lblStatusRedirect.AutoSize = true;
            lblStatusRedirect.Location = new Point(255, 511);
            lblStatusRedirect.Name = "lblStatusRedirect";
            lblStatusRedirect.Size = new Size(23, 15);
            lblStatusRedirect.TabIndex = 24;
            lblStatusRedirect.Text = "0%";
            lblStatusRedirect.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(60, 416);
            label15.Name = "label15";
            label15.Size = new Size(176, 15);
            label15.TabIndex = 23;
            label15.Text = "Hard-Coded Cryptography Keys";
            // 
            // lblStatusCryptoKeys
            // 
            lblStatusCryptoKeys.AutoSize = true;
            lblStatusCryptoKeys.Location = new Point(11, 416);
            lblStatusCryptoKeys.Name = "lblStatusCryptoKeys";
            lblStatusCryptoKeys.Size = new Size(23, 15);
            lblStatusCryptoKeys.TabIndex = 22;
            lblStatusCryptoKeys.Text = "0%";
            lblStatusCryptoKeys.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(304, 496);
            label17.Name = "label17";
            label17.Size = new Size(143, 15);
            label17.TabIndex = 51;
            label17.Text = "Unsafe File Manipulations";
            // 
            // lblStatusFile
            // 
            lblStatusFile.AutoSize = true;
            lblStatusFile.Location = new Point(255, 496);
            lblStatusFile.Name = "lblStatusFile";
            lblStatusFile.Size = new Size(23, 15);
            lblStatusFile.TabIndex = 50;
            lblStatusFile.Text = "0%";
            lblStatusFile.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(304, 416);
            label19.Name = "label19";
            label19.Size = new Size(126, 15);
            label19.TabIndex = 49;
            label19.Text = "JavaScript Tag Analysis";
            // 
            // lblStatusJSTag
            // 
            lblStatusJSTag.AutoSize = true;
            lblStatusJSTag.Location = new Point(255, 416);
            lblStatusJSTag.Name = "lblStatusJSTag";
            lblStatusJSTag.Size = new Size(23, 15);
            lblStatusJSTag.TabIndex = 48;
            lblStatusJSTag.Text = "0%";
            lblStatusJSTag.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(304, 431);
            label21.Name = "label21";
            label21.Size = new Size(99, 15);
            label21.TabIndex = 47;
            label21.Text = "Style Tag Analysis";
            // 
            // lblStatusStyleTag
            // 
            lblStatusStyleTag.AutoSize = true;
            lblStatusStyleTag.Location = new Point(255, 431);
            lblStatusStyleTag.Name = "lblStatusStyleTag";
            lblStatusStyleTag.Size = new Size(23, 15);
            lblStatusStyleTag.TabIndex = 46;
            lblStatusStyleTag.Text = "0%";
            lblStatusStyleTag.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(304, 446);
            label23.Name = "label23";
            label23.Size = new Size(96, 15);
            label23.TabIndex = 45;
            label23.Text = "Link Tag Analysis";
            // 
            // lblStatusLinkTag
            // 
            lblStatusLinkTag.AutoSize = true;
            lblStatusLinkTag.Location = new Point(255, 446);
            lblStatusLinkTag.Name = "lblStatusLinkTag";
            lblStatusLinkTag.Size = new Size(23, 15);
            lblStatusLinkTag.TabIndex = 44;
            lblStatusLinkTag.Text = "0%";
            lblStatusLinkTag.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(304, 378);
            label25.Name = "label25";
            label25.Size = new Size(164, 15);
            label25.TabIndex = 43;
            label25.Text = "Reflected XSS via IHtmlHelper";
            // 
            // lblStatusXssViaHelper
            // 
            lblStatusXssViaHelper.AutoSize = true;
            lblStatusXssViaHelper.Location = new Point(255, 378);
            lblStatusXssViaHelper.Name = "lblStatusXssViaHelper";
            lblStatusXssViaHelper.Size = new Size(23, 15);
            lblStatusXssViaHelper.TabIndex = 42;
            lblStatusXssViaHelper.Text = "0%";
            lblStatusXssViaHelper.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(304, 363);
            label27.Name = "label27";
            label27.Size = new Size(159, 15);
            label27.TabIndex = 41;
            label27.Text = "Reflected XSS via Html.Raw()";
            // 
            // lblStatusXssViaRaw
            // 
            lblStatusXssViaRaw.AutoSize = true;
            lblStatusXssViaRaw.Location = new Point(255, 363);
            lblStatusXssViaRaw.Name = "lblStatusXssViaRaw";
            lblStatusXssViaRaw.Size = new Size(23, 15);
            lblStatusXssViaRaw.TabIndex = 40;
            lblStatusXssViaRaw.Text = "0%";
            lblStatusXssViaRaw.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(304, 280);
            label29.Name = "label29";
            label29.Size = new Size(170, 15);
            label29.TabIndex = 39;
            label29.Text = "Mass Assignment/Overposting";
            // 
            // lblStatusOverposting
            // 
            lblStatusOverposting.AutoSize = true;
            lblStatusOverposting.Location = new Point(255, 280);
            lblStatusOverposting.Name = "lblStatusOverposting";
            lblStatusOverposting.Size = new Size(23, 15);
            lblStatusOverposting.TabIndex = 38;
            lblStatusOverposting.Text = "0%";
            lblStatusOverposting.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(304, 481);
            label31.Name = "label31";
            label31.Size = new Size(143, 15);
            label31.TabIndex = 37;
            label31.Text = "Cookie Misconfigurations";
            // 
            // lblStatusCookies
            // 
            lblStatusCookies.AutoSize = true;
            lblStatusCookies.Location = new Point(255, 481);
            lblStatusCookies.Name = "lblStatusCookies";
            lblStatusCookies.Size = new Size(23, 15);
            lblStatusCookies.TabIndex = 36;
            lblStatusCookies.Text = "0%";
            lblStatusCookies.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new Point(59, 363);
            label35.Name = "label35";
            label35.Size = new Size(172, 15);
            label35.TabIndex = 33;
            label35.Text = "Hard-Coded Connection String";
            // 
            // lblStatusConnectionString
            // 
            lblStatusConnectionString.AutoSize = true;
            lblStatusConnectionString.Location = new Point(10, 363);
            lblStatusConnectionString.Name = "lblStatusConnectionString";
            lblStatusConnectionString.Size = new Size(23, 15);
            lblStatusConnectionString.TabIndex = 32;
            lblStatusConnectionString.Text = "0%";
            lblStatusConnectionString.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(10, 265);
            label6.Name = "label6";
            label6.Size = new Size(42, 15);
            label6.TabIndex = 52;
            label6.Text = "Global";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label10.Location = new Point(10, 401);
            label10.Name = "label10";
            label10.Size = new Size(82, 15);
            label10.TabIndex = 53;
            label10.Text = "Cryptography";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label12.Location = new Point(10, 318);
            label12.Name = "label12";
            label12.Size = new Size(85, 15);
            label12.TabIndex = 54;
            label12.Text = "SQL/Database";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label14.Location = new Point(255, 401);
            label14.Name = "label14";
            label14.Size = new Size(40, 15);
            label14.TabIndex = 55;
            label14.Text = "HTML";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label16.Location = new Point(255, 265);
            label16.Name = "label16";
            label16.Size = new Size(49, 15);
            label16.TabIndex = 56;
            label16.Text = "Binding";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label18.Location = new Point(255, 348);
            label18.Name = "label18";
            label18.Size = new Size(29, 15);
            label18.TabIndex = 57;
            label18.Text = "XSS";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label20.Location = new Point(255, 466);
            label20.Name = "label20";
            label20.Size = new Size(73, 15);
            label20.TabIndex = 58;
            label20.Text = "Other/Misc.";
            // 
            // lblStatusStep
            // 
            lblStatusStep.AutoSize = true;
            lblStatusStep.Location = new Point(12, 238);
            lblStatusStep.Name = "lblStatusStep";
            lblStatusStep.Size = new Size(116, 15);
            lblStatusStep.TabIndex = 59;
            lblStatusStep.Text = "Current Step: (None)";
            // 
            // lblStatusFindings
            // 
            lblStatusFindings.AutoSize = true;
            lblStatusFindings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusFindings.Location = new Point(12, 562);
            lblStatusFindings.Name = "lblStatusFindings";
            lblStatusFindings.Size = new Size(65, 15);
            lblStatusFindings.TabIndex = 60;
            lblStatusFindings.Text = "Findings: 0";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(59, 378);
            label22.Name = "label22";
            label22.Size = new Size(177, 15);
            label22.TabIndex = 62;
            label22.Text = "Unclosed Database Connections";
            // 
            // lblStatusUnclosedConnection
            // 
            lblStatusUnclosedConnection.AutoSize = true;
            lblStatusUnclosedConnection.Location = new Point(10, 378);
            lblStatusUnclosedConnection.Name = "lblStatusUnclosedConnection";
            lblStatusUnclosedConnection.Size = new Size(23, 15);
            lblStatusUnclosedConnection.TabIndex = 61;
            lblStatusUnclosedConnection.Text = "0%";
            lblStatusUnclosedConnection.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(304, 325);
            label24.Name = "label24";
            label24.Size = new Size(134, 15);
            label24.TabIndex = 64;
            label24.Text = "Missing Input Validation";
            // 
            // lblStatusInputValidation
            // 
            lblStatusInputValidation.AutoSize = true;
            lblStatusInputValidation.Location = new Point(255, 325);
            lblStatusInputValidation.Name = "lblStatusInputValidation";
            lblStatusInputValidation.Size = new Size(23, 15);
            lblStatusInputValidation.TabIndex = 63;
            lblStatusInputValidation.Text = "0%";
            lblStatusInputValidation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(304, 526);
            label26.Name = "label26";
            label26.Size = new Size(127, 15);
            label26.TabIndex = 66;
            label26.Text = "JWT Misconfigurations";
            // 
            // lblStatusJWT
            // 
            lblStatusJWT.AutoSize = true;
            lblStatusJWT.Location = new Point(255, 526);
            lblStatusJWT.Name = "lblStatusJWT";
            lblStatusJWT.Size = new Size(23, 15);
            lblStatusJWT.TabIndex = 65;
            lblStatusJWT.Text = "0%";
            lblStatusJWT.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(60, 491);
            label28.Name = "label28";
            label28.Size = new Size(95, 15);
            label28.TabIndex = 68;
            label28.Text = "RSA Key Lengths";
            // 
            // lblStatusRSA
            // 
            lblStatusRSA.AutoSize = true;
            lblStatusRSA.Location = new Point(11, 491);
            lblStatusRSA.Name = "lblStatusRSA";
            lblStatusRSA.Size = new Size(23, 15);
            lblStatusRSA.TabIndex = 67;
            lblStatusRSA.Text = "0%";
            lblStatusRSA.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(60, 476);
            label30.Name = "label30";
            label30.Size = new Size(171, 15);
            label30.TabIndex = 70;
            label30.Text = "Deprecated Hashing Algorithm";
            // 
            // lblStatusHashingAlgorithm
            // 
            lblStatusHashingAlgorithm.AutoSize = true;
            lblStatusHashingAlgorithm.Location = new Point(11, 476);
            lblStatusHashingAlgorithm.Name = "lblStatusHashingAlgorithm";
            lblStatusHashingAlgorithm.Size = new Size(23, 15);
            lblStatusHashingAlgorithm.TabIndex = 69;
            lblStatusHashingAlgorithm.Text = "0%";
            lblStatusHashingAlgorithm.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(59, 348);
            label32.Name = "label32";
            label32.Size = new Size(190, 15);
            label32.TabIndex = 72;
            label32.Text = "SQL Injection via Entity Framework";
            // 
            // lblStatusSQLiViaEF
            // 
            lblStatusSQLiViaEF.AutoSize = true;
            lblStatusSQLiViaEF.Location = new Point(10, 348);
            lblStatusSQLiViaEF.Name = "lblStatusSQLiViaEF";
            lblStatusSQLiViaEF.Size = new Size(23, 15);
            lblStatusSQLiViaEF.TabIndex = 71;
            lblStatusSQLiViaEF.Text = "0%";
            lblStatusSQLiViaEF.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(494, 596);
            Controls.Add(label32);
            Controls.Add(lblStatusSQLiViaEF);
            Controls.Add(label30);
            Controls.Add(lblStatusHashingAlgorithm);
            Controls.Add(label28);
            Controls.Add(lblStatusRSA);
            Controls.Add(label26);
            Controls.Add(lblStatusJWT);
            Controls.Add(label24);
            Controls.Add(lblStatusInputValidation);
            Controls.Add(label22);
            Controls.Add(lblStatusUnclosedConnection);
            Controls.Add(lblStatusFindings);
            Controls.Add(lblStatusStep);
            Controls.Add(label20);
            Controls.Add(label18);
            Controls.Add(label16);
            Controls.Add(label14);
            Controls.Add(label12);
            Controls.Add(label10);
            Controls.Add(label6);
            Controls.Add(label17);
            Controls.Add(lblStatusFile);
            Controls.Add(label19);
            Controls.Add(lblStatusJSTag);
            Controls.Add(label21);
            Controls.Add(lblStatusStyleTag);
            Controls.Add(label23);
            Controls.Add(lblStatusLinkTag);
            Controls.Add(label25);
            Controls.Add(lblStatusXssViaHelper);
            Controls.Add(label27);
            Controls.Add(lblStatusXssViaRaw);
            Controls.Add(label29);
            Controls.Add(lblStatusOverposting);
            Controls.Add(label31);
            Controls.Add(lblStatusCookies);
            Controls.Add(label35);
            Controls.Add(lblStatusConnectionString);
            Controls.Add(label4);
            Controls.Add(lblStatusCryptoIVs);
            Controls.Add(label8);
            Controls.Add(lblStatusCryptoECB);
            Controls.Add(label11);
            Controls.Add(lblStatusDeprecatedCrypto);
            Controls.Add(label13);
            Controls.Add(lblStatusRedirect);
            Controls.Add(label15);
            Controls.Add(lblStatusCryptoKeys);
            Controls.Add(label9);
            Controls.Add(lblStatusNuGet);
            Controls.Add(label7);
            Controls.Add(lblStatusSqlInjection);
            Controls.Add(label5);
            Controls.Add(lblStatusCsrf);
            Controls.Add(label3);
            Controls.Add(lblStatusValueShadowing);
            Controls.Add(label0);
            Controls.Add(lblStatusTrufflehog);
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
            Load += Form1_Load;
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
        private Label lblStatusTrufflehog;
        private Label label0;
        private Label label9;
        private Label lblStatusNuGet;
        private Label lblStatusValueShadowing;
        private Label label3;
        private Label lblStatusCsrf;
        private Label label5;
        private Label lblStatusSqlInjection;
        private Label label7;
        private Label label4;
        private Label lblStatusCryptoIVs;
        private Label label8;
        private Label lblStatusCryptoECB;
        private Label label11;
        private Label lblStatusDeprecatedCrypto;
        private Label label13;
        private Label lblStatusRedirect;
        private Label label15;
        private Label lblStatusCryptoKeys;
        private Label label17;
        private Label lblStatusFile;
        private Label label19;
        private Label lblStatusJSTag;
        private Label label21;
        private Label lblStatusStyleTag;
        private Label label23;
        private Label lblStatusLinkTag;
        private Label label25;
        private Label lblStatusXssViaHelper;
        private Label label27;
        private Label lblStatusXssViaRaw;
        private Label label29;
        private Label lblStatusOverposting;
        private Label label31;
        private Label lblStatusCookies;
        private Label label35;
        private Label lblStatusConnectionString;
        private Label label6;
        private Label label10;
        private Label label12;
        private Label label14;
        private Label label16;
        private Label label18;
        private Label label20;
        private Label lblStatusStep;
        private Label lblStatusFindings;
        private Label label22;
        private Label lblStatusUnclosedConnection;
        private Label label24;
        private Label lblStatusInputValidation;
        private Label label26;
        private Label lblStatusJWT;
        private Label label28;
        private Label lblStatusRSA;
        private Label label30;
        private Label lblStatusHashingAlgorithm;
        private Label label32;
        private Label lblStatusSQLiViaEF;
    }
}