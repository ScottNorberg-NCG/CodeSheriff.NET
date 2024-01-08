namespace Opperis.SAST.LocalUI
{
    partial class ScanStatusForm
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
            label33 = new Label();
            lblStatusPasswordLockout = new Label();
            label34 = new Label();
            lblStatusIUserStore = new Label();
            label36 = new Label();
            label1 = new Label();
            lblStatusDBAnalysis = new Label();
            SuspendLayout();
            // 
            // lblStatusTrufflehog
            // 
            lblStatusTrufflehog.AutoSize = true;
            lblStatusTrufflehog.Location = new Point(10, 67);
            lblStatusTrufflehog.Name = "lblStatusTrufflehog";
            lblStatusTrufflehog.Size = new Size(23, 15);
            lblStatusTrufflehog.TabIndex = 12;
            lblStatusTrufflehog.Text = "0%";
            lblStatusTrufflehog.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label0
            // 
            label0.AutoSize = true;
            label0.Location = new Point(59, 67);
            label0.Name = "label0";
            label0.Size = new Size(61, 15);
            label0.TabIndex = 13;
            label0.Text = "Trufflehog";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(59, 82);
            label9.Name = "label9";
            label9.Size = new Size(113, 15);
            label9.TabIndex = 21;
            label9.Text = "NuGet Components";
            // 
            // lblStatusNuGet
            // 
            lblStatusNuGet.AutoSize = true;
            lblStatusNuGet.Location = new Point(10, 82);
            lblStatusNuGet.Name = "lblStatusNuGet";
            lblStatusNuGet.Size = new Size(23, 15);
            lblStatusNuGet.TabIndex = 20;
            lblStatusNuGet.Text = "0%";
            lblStatusNuGet.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblStatusValueShadowing
            // 
            lblStatusValueShadowing.AutoSize = true;
            lblStatusValueShadowing.Location = new Point(10, 97);
            lblStatusValueShadowing.Name = "lblStatusValueShadowing";
            lblStatusValueShadowing.Size = new Size(23, 15);
            lblStatusValueShadowing.TabIndex = 14;
            lblStatusValueShadowing.Text = "0%";
            lblStatusValueShadowing.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(59, 97);
            label3.Name = "label3";
            label3.Size = new Size(97, 15);
            label3.TabIndex = 15;
            label3.Text = "Value Shadowing";
            // 
            // lblStatusCsrf
            // 
            lblStatusCsrf.AutoSize = true;
            lblStatusCsrf.Location = new Point(10, 112);
            lblStatusCsrf.Name = "lblStatusCsrf";
            lblStatusCsrf.Size = new Size(23, 15);
            lblStatusCsrf.TabIndex = 16;
            lblStatusCsrf.Text = "0%";
            lblStatusCsrf.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(59, 112);
            label5.Name = "label5";
            label5.Size = new Size(141, 15);
            label5.TabIndex = 17;
            label5.Text = "Missing CSRF Protections";
            // 
            // lblStatusSqlInjection
            // 
            lblStatusSqlInjection.AutoSize = true;
            lblStatusSqlInjection.Location = new Point(10, 145);
            lblStatusSqlInjection.Name = "lblStatusSqlInjection";
            lblStatusSqlInjection.Size = new Size(23, 15);
            lblStatusSqlInjection.TabIndex = 18;
            lblStatusSqlInjection.Text = "0%";
            lblStatusSqlInjection.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(59, 145);
            label7.Name = "label7";
            label7.Size = new Size(77, 15);
            label7.TabIndex = 19;
            label7.Text = "SQL Injection";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(60, 243);
            label4.Name = "label4";
            label4.Size = new Size(167, 15);
            label4.TabIndex = 31;
            label4.Text = "Hard-Coded Cryptography IVs";
            // 
            // lblStatusCryptoIVs
            // 
            lblStatusCryptoIVs.AutoSize = true;
            lblStatusCryptoIVs.Location = new Point(11, 243);
            lblStatusCryptoIVs.Name = "lblStatusCryptoIVs";
            lblStatusCryptoIVs.Size = new Size(23, 15);
            lblStatusCryptoIVs.TabIndex = 30;
            lblStatusCryptoIVs.Text = "0%";
            lblStatusCryptoIVs.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(60, 258);
            label8.Name = "label8";
            label8.Size = new Size(98, 15);
            label8.TabIndex = 29;
            label8.Text = "Use of ECB Mode";
            // 
            // lblStatusCryptoECB
            // 
            lblStatusCryptoECB.AutoSize = true;
            lblStatusCryptoECB.Location = new Point(11, 258);
            lblStatusCryptoECB.Name = "lblStatusCryptoECB";
            lblStatusCryptoECB.Size = new Size(23, 15);
            lblStatusCryptoECB.TabIndex = 28;
            lblStatusCryptoECB.Text = "0%";
            lblStatusCryptoECB.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(60, 273);
            label11.Name = "label11";
            label11.Size = new Size(184, 15);
            label11.TabIndex = 27;
            label11.Text = "Deprecated Symmetric Algorithm";
            // 
            // lblStatusDeprecatedCrypto
            // 
            lblStatusDeprecatedCrypto.AutoSize = true;
            lblStatusDeprecatedCrypto.Location = new Point(11, 273);
            lblStatusDeprecatedCrypto.Name = "lblStatusDeprecatedCrypto";
            lblStatusDeprecatedCrypto.Size = new Size(23, 15);
            lblStatusDeprecatedCrypto.TabIndex = 26;
            lblStatusDeprecatedCrypto.Text = "0%";
            lblStatusDeprecatedCrypto.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(304, 254);
            label13.Name = "label13";
            label13.Size = new Size(164, 15);
            label13.TabIndex = 25;
            label13.Text = "Unprotected External Redirect";
            // 
            // lblStatusRedirect
            // 
            lblStatusRedirect.AutoSize = true;
            lblStatusRedirect.Location = new Point(255, 254);
            lblStatusRedirect.Name = "lblStatusRedirect";
            lblStatusRedirect.Size = new Size(23, 15);
            lblStatusRedirect.TabIndex = 24;
            lblStatusRedirect.Text = "0%";
            lblStatusRedirect.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(60, 228);
            label15.Name = "label15";
            label15.Size = new Size(176, 15);
            label15.TabIndex = 23;
            label15.Text = "Hard-Coded Cryptography Keys";
            // 
            // lblStatusCryptoKeys
            // 
            lblStatusCryptoKeys.AutoSize = true;
            lblStatusCryptoKeys.Location = new Point(11, 228);
            lblStatusCryptoKeys.Name = "lblStatusCryptoKeys";
            lblStatusCryptoKeys.Size = new Size(23, 15);
            lblStatusCryptoKeys.TabIndex = 22;
            lblStatusCryptoKeys.Text = "0%";
            lblStatusCryptoKeys.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(304, 239);
            label17.Name = "label17";
            label17.Size = new Size(143, 15);
            label17.TabIndex = 51;
            label17.Text = "Unsafe File Manipulations";
            // 
            // lblStatusFile
            // 
            lblStatusFile.AutoSize = true;
            lblStatusFile.Location = new Point(255, 239);
            lblStatusFile.Name = "lblStatusFile";
            lblStatusFile.Size = new Size(23, 15);
            lblStatusFile.TabIndex = 50;
            lblStatusFile.Text = "0%";
            lblStatusFile.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(304, 159);
            label19.Name = "label19";
            label19.Size = new Size(126, 15);
            label19.TabIndex = 49;
            label19.Text = "JavaScript Tag Analysis";
            // 
            // lblStatusJSTag
            // 
            lblStatusJSTag.AutoSize = true;
            lblStatusJSTag.Location = new Point(255, 159);
            lblStatusJSTag.Name = "lblStatusJSTag";
            lblStatusJSTag.Size = new Size(23, 15);
            lblStatusJSTag.TabIndex = 48;
            lblStatusJSTag.Text = "0%";
            lblStatusJSTag.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(304, 174);
            label21.Name = "label21";
            label21.Size = new Size(99, 15);
            label21.TabIndex = 47;
            label21.Text = "Style Tag Analysis";
            // 
            // lblStatusStyleTag
            // 
            lblStatusStyleTag.AutoSize = true;
            lblStatusStyleTag.Location = new Point(255, 174);
            lblStatusStyleTag.Name = "lblStatusStyleTag";
            lblStatusStyleTag.Size = new Size(23, 15);
            lblStatusStyleTag.TabIndex = 46;
            lblStatusStyleTag.Text = "0%";
            lblStatusStyleTag.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(304, 189);
            label23.Name = "label23";
            label23.Size = new Size(96, 15);
            label23.TabIndex = 45;
            label23.Text = "Link Tag Analysis";
            // 
            // lblStatusLinkTag
            // 
            lblStatusLinkTag.AutoSize = true;
            lblStatusLinkTag.Location = new Point(255, 189);
            lblStatusLinkTag.Name = "lblStatusLinkTag";
            lblStatusLinkTag.Size = new Size(23, 15);
            lblStatusLinkTag.TabIndex = 44;
            lblStatusLinkTag.Text = "0%";
            lblStatusLinkTag.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(304, 121);
            label25.Name = "label25";
            label25.Size = new Size(164, 15);
            label25.TabIndex = 43;
            label25.Text = "Reflected XSS via IHtmlHelper";
            // 
            // lblStatusXssViaHelper
            // 
            lblStatusXssViaHelper.AutoSize = true;
            lblStatusXssViaHelper.Location = new Point(255, 121);
            lblStatusXssViaHelper.Name = "lblStatusXssViaHelper";
            lblStatusXssViaHelper.Size = new Size(23, 15);
            lblStatusXssViaHelper.TabIndex = 42;
            lblStatusXssViaHelper.Text = "0%";
            lblStatusXssViaHelper.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(304, 106);
            label27.Name = "label27";
            label27.Size = new Size(159, 15);
            label27.TabIndex = 41;
            label27.Text = "Reflected XSS via Html.Raw()";
            // 
            // lblStatusXssViaRaw
            // 
            lblStatusXssViaRaw.AutoSize = true;
            lblStatusXssViaRaw.Location = new Point(255, 106);
            lblStatusXssViaRaw.Name = "lblStatusXssViaRaw";
            lblStatusXssViaRaw.Size = new Size(23, 15);
            lblStatusXssViaRaw.TabIndex = 40;
            lblStatusXssViaRaw.Text = "0%";
            lblStatusXssViaRaw.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(304, 51);
            label29.Name = "label29";
            label29.Size = new Size(170, 15);
            label29.TabIndex = 39;
            label29.Text = "Mass Assignment/Overposting";
            // 
            // lblStatusOverposting
            // 
            lblStatusOverposting.AutoSize = true;
            lblStatusOverposting.Location = new Point(255, 51);
            lblStatusOverposting.Name = "lblStatusOverposting";
            lblStatusOverposting.Size = new Size(23, 15);
            lblStatusOverposting.TabIndex = 38;
            lblStatusOverposting.Text = "0%";
            lblStatusOverposting.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(304, 224);
            label31.Name = "label31";
            label31.Size = new Size(143, 15);
            label31.TabIndex = 37;
            label31.Text = "Cookie Misconfigurations";
            // 
            // lblStatusCookies
            // 
            lblStatusCookies.AutoSize = true;
            lblStatusCookies.Location = new Point(255, 224);
            lblStatusCookies.Name = "lblStatusCookies";
            lblStatusCookies.Size = new Size(23, 15);
            lblStatusCookies.TabIndex = 36;
            lblStatusCookies.Text = "0%";
            lblStatusCookies.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new Point(59, 175);
            label35.Name = "label35";
            label35.Size = new Size(172, 15);
            label35.TabIndex = 33;
            label35.Text = "Hard-Coded Connection String";
            // 
            // lblStatusConnectionString
            // 
            lblStatusConnectionString.AutoSize = true;
            lblStatusConnectionString.Location = new Point(10, 175);
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
            label6.Location = new Point(10, 36);
            label6.Name = "label6";
            label6.Size = new Size(42, 15);
            label6.TabIndex = 52;
            label6.Text = "Global";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label10.Location = new Point(10, 213);
            label10.Name = "label10";
            label10.Size = new Size(82, 15);
            label10.TabIndex = 53;
            label10.Text = "Cryptography";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label12.Location = new Point(10, 130);
            label12.Name = "label12";
            label12.Size = new Size(85, 15);
            label12.TabIndex = 54;
            label12.Text = "SQL/Database";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label14.Location = new Point(255, 144);
            label14.Name = "label14";
            label14.Size = new Size(40, 15);
            label14.TabIndex = 55;
            label14.Text = "HTML";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label16.Location = new Point(255, 36);
            label16.Name = "label16";
            label16.Size = new Size(49, 15);
            label16.TabIndex = 56;
            label16.Text = "Binding";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label18.Location = new Point(255, 91);
            label18.Name = "label18";
            label18.Size = new Size(29, 15);
            label18.TabIndex = 57;
            label18.Text = "XSS";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label20.Location = new Point(255, 209);
            label20.Name = "label20";
            label20.Size = new Size(73, 15);
            label20.TabIndex = 58;
            label20.Text = "Other/Misc.";
            // 
            // lblStatusStep
            // 
            lblStatusStep.AutoSize = true;
            lblStatusStep.Location = new Point(12, 9);
            lblStatusStep.Name = "lblStatusStep";
            lblStatusStep.Size = new Size(116, 15);
            lblStatusStep.TabIndex = 59;
            lblStatusStep.Text = "Current Step: (None)";
            // 
            // lblStatusFindings
            // 
            lblStatusFindings.AutoSize = true;
            lblStatusFindings.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusFindings.Location = new Point(12, 353);
            lblStatusFindings.Name = "lblStatusFindings";
            lblStatusFindings.Size = new Size(65, 15);
            lblStatusFindings.TabIndex = 60;
            lblStatusFindings.Text = "Findings: 0";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(59, 190);
            label22.Name = "label22";
            label22.Size = new Size(177, 15);
            label22.TabIndex = 62;
            label22.Text = "Unclosed Database Connections";
            // 
            // lblStatusUnclosedConnection
            // 
            lblStatusUnclosedConnection.AutoSize = true;
            lblStatusUnclosedConnection.Location = new Point(10, 190);
            lblStatusUnclosedConnection.Name = "lblStatusUnclosedConnection";
            lblStatusUnclosedConnection.Size = new Size(23, 15);
            lblStatusUnclosedConnection.TabIndex = 61;
            lblStatusUnclosedConnection.Text = "0%";
            lblStatusUnclosedConnection.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(304, 66);
            label24.Name = "label24";
            label24.Size = new Size(134, 15);
            label24.TabIndex = 64;
            label24.Text = "Missing Input Validation";
            // 
            // lblStatusInputValidation
            // 
            lblStatusInputValidation.AutoSize = true;
            lblStatusInputValidation.Location = new Point(255, 66);
            lblStatusInputValidation.Name = "lblStatusInputValidation";
            lblStatusInputValidation.Size = new Size(23, 15);
            lblStatusInputValidation.TabIndex = 63;
            lblStatusInputValidation.Text = "0%";
            lblStatusInputValidation.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(305, 288);
            label26.Name = "label26";
            label26.Size = new Size(127, 15);
            label26.TabIndex = 66;
            label26.Text = "JWT Misconfigurations";
            // 
            // lblStatusJWT
            // 
            lblStatusJWT.AutoSize = true;
            lblStatusJWT.Location = new Point(256, 288);
            lblStatusJWT.Name = "lblStatusJWT";
            lblStatusJWT.Size = new Size(23, 15);
            lblStatusJWT.TabIndex = 65;
            lblStatusJWT.Text = "0%";
            lblStatusJWT.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(60, 303);
            label28.Name = "label28";
            label28.Size = new Size(95, 15);
            label28.TabIndex = 68;
            label28.Text = "RSA Key Lengths";
            // 
            // lblStatusRSA
            // 
            lblStatusRSA.AutoSize = true;
            lblStatusRSA.Location = new Point(11, 303);
            lblStatusRSA.Name = "lblStatusRSA";
            lblStatusRSA.Size = new Size(23, 15);
            lblStatusRSA.TabIndex = 67;
            lblStatusRSA.Text = "0%";
            lblStatusRSA.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(60, 288);
            label30.Name = "label30";
            label30.Size = new Size(171, 15);
            label30.TabIndex = 70;
            label30.Text = "Deprecated Hashing Algorithm";
            // 
            // lblStatusHashingAlgorithm
            // 
            lblStatusHashingAlgorithm.AutoSize = true;
            lblStatusHashingAlgorithm.Location = new Point(11, 288);
            lblStatusHashingAlgorithm.Name = "lblStatusHashingAlgorithm";
            lblStatusHashingAlgorithm.Size = new Size(23, 15);
            lblStatusHashingAlgorithm.TabIndex = 69;
            lblStatusHashingAlgorithm.Text = "0%";
            lblStatusHashingAlgorithm.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(59, 160);
            label32.Name = "label32";
            label32.Size = new Size(190, 15);
            label32.TabIndex = 72;
            label32.Text = "SQL Injection via Entity Framework";
            // 
            // lblStatusSQLiViaEF
            // 
            lblStatusSQLiViaEF.AutoSize = true;
            lblStatusSQLiViaEF.Location = new Point(10, 160);
            lblStatusSQLiViaEF.Name = "lblStatusSQLiViaEF";
            lblStatusSQLiViaEF.Size = new Size(23, 15);
            lblStatusSQLiViaEF.TabIndex = 71;
            lblStatusSQLiViaEF.Text = "0%";
            lblStatusSQLiViaEF.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(305, 303);
            label33.Name = "label33";
            label33.Size = new Size(143, 15);
            label33.TabIndex = 74;
            label33.Text = "Password Lockout is False";
            // 
            // lblStatusPasswordLockout
            // 
            lblStatusPasswordLockout.AutoSize = true;
            lblStatusPasswordLockout.Location = new Point(256, 303);
            lblStatusPasswordLockout.Name = "lblStatusPasswordLockout";
            lblStatusPasswordLockout.Size = new Size(23, 15);
            lblStatusPasswordLockout.TabIndex = 73;
            lblStatusPasswordLockout.Text = "0%";
            lblStatusPasswordLockout.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(305, 318);
            label34.Name = "label34";
            label34.Size = new Size(159, 15);
            label34.TabIndex = 76;
            label34.Text = "IUserStore Misconfigurations";
            // 
            // lblStatusIUserStore
            // 
            lblStatusIUserStore.AutoSize = true;
            lblStatusIUserStore.Location = new Point(256, 318);
            lblStatusIUserStore.Name = "lblStatusIUserStore";
            lblStatusIUserStore.Size = new Size(23, 15);
            lblStatusIUserStore.TabIndex = 75;
            lblStatusIUserStore.Text = "0%";
            lblStatusIUserStore.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label36.Location = new Point(255, 273);
            label36.Name = "label36";
            label36.Size = new Size(171, 15);
            label36.TabIndex = 77;
            label36.Text = "Authentication/Authorization";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(59, 51);
            label1.Name = "label1";
            label1.Size = new Size(140, 15);
            label1.TabIndex = 79;
            label1.Text = "Database Access Analysis";
            // 
            // lblStatusDBAnalysis
            // 
            lblStatusDBAnalysis.AutoSize = true;
            lblStatusDBAnalysis.Location = new Point(10, 51);
            lblStatusDBAnalysis.Name = "lblStatusDBAnalysis";
            lblStatusDBAnalysis.Size = new Size(23, 15);
            lblStatusDBAnalysis.TabIndex = 78;
            lblStatusDBAnalysis.Text = "0%";
            lblStatusDBAnalysis.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ScanStatusForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(494, 384);
            Controls.Add(label1);
            Controls.Add(lblStatusDBAnalysis);
            Controls.Add(label36);
            Controls.Add(label34);
            Controls.Add(lblStatusIUserStore);
            Controls.Add(label33);
            Controls.Add(lblStatusPasswordLockout);
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
            Name = "ScanStatusForm";
            Text = "Start Scan";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
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
        private Label label33;
        private Label lblStatusPasswordLockout;
        private Label label34;
        private Label lblStatusIUserStore;
        private Label label36;
        private Label label1;
        private Label lblStatusDBAnalysis;
    }
}