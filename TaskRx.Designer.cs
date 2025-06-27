﻿﻿﻿﻿﻿﻿﻿namespace TaskRx
{
    partial class TaskRx
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskRx));
            setupTabControl = new TabControl();
            executeButton = new Button();
            executionProgress = new ProgressBar();
            executionStrip = new StatusStrip();
            executionStatus = new ToolStripStatusLabel();
            lblFirst = new Label();
            txtFirst = new TextBox();
            txtLast = new TextBox();
            lblLast = new Label();
            txtDomain = new TextBox();
            lblDomain = new Label();
            txtPersonal = new TextBox();
            lblPersonal = new Label();
            txtOutput = new RichTextBox();
            txtBase = new TextBox();
            lblBase = new Label();
            btnBase = new Button();
            lblWork = new Label();
            txtWork = new TextBox();
            uncheckAllButton = new ToolStripSplitButton();
            uncheckAllTasksMenuItem = new ToolStripMenuItem();
            uncheckAllPostTasksMenuItem = new ToolStripMenuItem();
            uncheckAllSplitButton = new ToolStrip();
            checkAllSplitButton = new ToolStrip();
            checkWorkstationButton = new ToolStripSplitButton();
            checkAllTasksMenuItem = new ToolStripMenuItem();
            checkAllPostTasksMenuItem = new ToolStripMenuItem();
            checkAllPostTasksToolStripMenuItem = new ToolStripMenuItem();
            checkAllPostTasksToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripButton1 = new ToolStripButton();
            txtUsername = new TextBox();
            lblUsername = new Label();
            executionStrip.SuspendLayout();
            uncheckAllSplitButton.SuspendLayout();
            checkAllSplitButton.SuspendLayout();
            SuspendLayout();
            // 
            // setupTabControl
            // 
            setupTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            setupTabControl.Location = new Point(12, 142);
            setupTabControl.Name = "setupTabControl";
            setupTabControl.SelectedIndex = 0;
            setupTabControl.Size = new Size(734, 510);
            setupTabControl.TabIndex = 17;
            // 
            // executeButton
            // 
            executeButton.Location = new Point(12, 658);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(100, 30);
            executeButton.TabIndex = 18;
            executeButton.Text = "Execute";
            executeButton.Click += executeButton_Click;
            // 
            // executionProgress
            // 
            executionProgress.Location = new Point(118, 659);
            executionProgress.Name = "executionProgress";
            executionProgress.Size = new Size(628, 29);
            executionProgress.TabIndex = 19;
            // 
            // executionStrip
            // 
            executionStrip.ImageScalingSize = new Size(20, 20);
            executionStrip.Items.AddRange(new ToolStripItem[] { executionStatus });
            executionStrip.Location = new Point(0, 691);
            executionStrip.Name = "executionStrip";
            executionStrip.Size = new Size(758, 22);
            executionStrip.TabIndex = 20;
            executionStrip.Text = "statusStrip1";
            // 
            // executionStatus
            // 
            executionStatus.Name = "executionStatus";
            executionStatus.Size = new Size(0, 16);
            // 
            // lblFirst
            // 
            lblFirst.AutoSize = true;
            lblFirst.Location = new Point(16, 12);
            lblFirst.Name = "lblFirst";
            lblFirst.Size = new Size(80, 20);
            lblFirst.TabIndex = 0;
            lblFirst.Text = "First Name";
            // 
            // txtFirst
            // 
            txtFirst.Location = new Point(102, 7);
            txtFirst.Name = "txtFirst";
            txtFirst.Size = new Size(225, 27);
            txtFirst.TabIndex = 1;
            txtFirst.TextChanged += txtFirst_Changed;
            // 
            // txtLast
            // 
            txtLast.Location = new Point(102, 39);
            txtLast.Name = "txtLast";
            txtLast.Size = new Size(225, 27);
            txtLast.TabIndex = 3;
            txtLast.TextChanged += txtLast_Changed;
            // 
            // lblLast
            // 
            lblLast.AutoSize = true;
            lblLast.Location = new Point(17, 42);
            lblLast.Name = "lblLast";
            lblLast.Size = new Size(79, 20);
            lblLast.TabIndex = 2;
            lblLast.Text = "Last Name";
            // 
            // txtDomain
            // 
            txtDomain.Location = new Point(100, 72);
            txtDomain.Name = "txtDomain";
            txtDomain.Size = new Size(227, 27);
            txtDomain.TabIndex = 5;
            txtDomain.TextChanged += txtUsername_TextChanged;
            // 
            // lblDomain
            // 
            lblDomain.AutoSize = true;
            lblDomain.Location = new Point(19, 77);
            lblDomain.Name = "lblDomain";
            lblDomain.Size = new Size(62, 20);
            lblDomain.TabIndex = 4;
            lblDomain.Text = "Domain";
            // 
            // txtPersonal
            // 
            txtPersonal.Location = new Point(436, 39);
            txtPersonal.Name = "txtPersonal";
            txtPersonal.Size = new Size(227, 27);
            txtPersonal.TabIndex = 11;
            txtPersonal.TextChanged += txtPersonal_TextChanged;
            // 
            // lblPersonal
            // 
            lblPersonal.AutoSize = true;
            lblPersonal.Location = new Point(339, 42);
            lblPersonal.Name = "lblPersonal";
            lblPersonal.Size = new Size(91, 20);
            lblPersonal.TabIndex = 10;
            lblPersonal.Text = "Home Email";
            // 
            // txtOutput
            // 
            txtOutput.DetectUrls = false;
            txtOutput.Location = new Point(12, 7);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            txtOutput.Size = new Size(734, 647);
            txtOutput.TabIndex = 17;
            txtOutput.Text = "";
            txtOutput.Visible = false;
            txtOutput.WordWrap = false;
            // 
            // txtBase
            // 
            txtBase.Location = new Point(436, 72);
            txtBase.Name = "txtBase";
            txtBase.Size = new Size(227, 27);
            txtBase.TabIndex = 13;
            txtBase.TextChanged += txtBase_TextChanged;
            // 
            // lblBase
            // 
            lblBase.AutoSize = true;
            lblBase.Location = new Point(345, 75);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(85, 20);
            lblBase.TabIndex = 12;
            lblBase.Text = "Repos Base";
            // 
            // btnBase
            // 
            btnBase.Location = new Point(669, 72);
            btnBase.Name = "btnBase";
            btnBase.Size = new Size(77, 30);
            btnBase.TabIndex = 14;
            btnBase.Text = "Browse";
            btnBase.Click += btnBase_Click;
            // 
            // lblWork
            // 
            lblWork.AutoSize = true;
            lblWork.Location = new Point(346, 10);
            lblWork.Name = "lblWork";
            lblWork.Size = new Size(84, 20);
            lblWork.TabIndex = 8;
            lblWork.Text = "Work Email";
            // 
            // txtWork
            // 
            txtWork.Location = new Point(436, 7);
            txtWork.Name = "txtWork";
            txtWork.Size = new Size(227, 27);
            txtWork.TabIndex = 9;
            txtWork.TextChanged += txtWork_TextChanged;
            // 
            // uncheckAllButton
            // 
            uncheckAllButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            uncheckAllButton.DropDownItems.AddRange(new ToolStripItem[] { uncheckAllTasksMenuItem, uncheckAllPostTasksMenuItem });
            uncheckAllButton.ImageTransparentColor = Color.Magenta;
            uncheckAllButton.Name = "uncheckAllButton";
            uncheckAllButton.Size = new Size(105, 24);
            uncheckAllButton.Text = "Uncheck All";
            uncheckAllButton.ButtonClick += uncheckAllButton_ButtonClick;
            // 
            // uncheckAllTasksMenuItem
            // 
            uncheckAllTasksMenuItem.Name = "uncheckAllTasksMenuItem";
            uncheckAllTasksMenuItem.Size = new Size(237, 26);
            uncheckAllTasksMenuItem.Text = "Uncheck All Tasks";
            uncheckAllTasksMenuItem.Click += uncheckAllTasksMenuItem_Click;
            // 
            // uncheckAllPostTasksMenuItem
            // 
            uncheckAllPostTasksMenuItem.Name = "uncheckAllPostTasksMenuItem";
            uncheckAllPostTasksMenuItem.Size = new Size(237, 26);
            uncheckAllPostTasksMenuItem.Text = "Uncheck All Post Tasks";
            uncheckAllPostTasksMenuItem.Click += uncheckAllPostTasksMenuItem_Click;
            // 
            // uncheckAllSplitButton
            // 
            uncheckAllSplitButton.Dock = DockStyle.None;
            uncheckAllSplitButton.GripStyle = ToolStripGripStyle.Hidden;
            uncheckAllSplitButton.ImageScalingSize = new Size(20, 20);
            uncheckAllSplitButton.Items.AddRange(new ToolStripItem[] { uncheckAllButton });
            uncheckAllSplitButton.Location = new Point(630, 109);
            uncheckAllSplitButton.Name = "uncheckAllSplitButton";
            uncheckAllSplitButton.RenderMode = ToolStripRenderMode.System;
            uncheckAllSplitButton.Size = new Size(108, 27);
            uncheckAllSplitButton.TabIndex = 16;
            uncheckAllSplitButton.Text = "Uncheck All";
            // 
            // checkAllSplitButton
            // 
            checkAllSplitButton.Dock = DockStyle.None;
            checkAllSplitButton.GripStyle = ToolStripGripStyle.Hidden;
            checkAllSplitButton.ImageScalingSize = new Size(20, 20);
            checkAllSplitButton.Items.AddRange(new ToolStripItem[] { checkWorkstationButton, toolStripButton1 });
            checkAllSplitButton.Location = new Point(430, 109);
            checkAllSplitButton.Name = "checkAllSplitButton";
            checkAllSplitButton.RenderMode = ToolStripRenderMode.System;
            checkAllSplitButton.Size = new Size(198, 27);
            checkAllSplitButton.TabIndex = 15;
            checkAllSplitButton.Text = "Check All";
            // 
            // checkWorkstationButton
            // 
            checkWorkstationButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            checkWorkstationButton.DropDownItems.AddRange(new ToolStripItem[] { checkAllTasksMenuItem, checkAllPostTasksMenuItem, checkAllPostTasksToolStripMenuItem, checkAllPostTasksToolStripMenuItem1 });
            checkWorkstationButton.ImageTransparentColor = Color.Magenta;
            checkWorkstationButton.Name = "checkWorkstationButton";
            checkWorkstationButton.Size = new Size(166, 24);
            checkWorkstationButton.Text = "Workstation Defaults";
            checkWorkstationButton.ToolTipText = "Check Defaults for Workstation";
            checkWorkstationButton.ButtonClick += checkDefaultWorkstation_ButtonClick;
            // 
            // checkAllTasksMenuItem
            // 
            checkAllTasksMenuItem.Name = "checkAllTasksMenuItem";
            checkAllTasksMenuItem.Size = new Size(231, 26);
            checkAllTasksMenuItem.Text = "Jumpstation Defaults";
            checkAllTasksMenuItem.Click += checkDefaultJumpstation_ButtonClick;
            // 
            // checkAllPostTasksMenuItem
            // 
            checkAllPostTasksMenuItem.Name = "checkAllPostTasksMenuItem";
            checkAllPostTasksMenuItem.Size = new Size(231, 26);
            checkAllPostTasksMenuItem.Text = "Check All";
            checkAllPostTasksMenuItem.Click += checkAllMenuItem_ButtonClick;
            // 
            // checkAllPostTasksToolStripMenuItem
            // 
            checkAllPostTasksToolStripMenuItem.Name = "checkAllPostTasksToolStripMenuItem";
            checkAllPostTasksToolStripMenuItem.Size = new Size(231, 26);
            checkAllPostTasksToolStripMenuItem.Text = "Check All Tasks";
            checkAllPostTasksToolStripMenuItem.Click += checkAllTasksMenuItem_Click;
            // 
            // checkAllPostTasksToolStripMenuItem1
            // 
            checkAllPostTasksToolStripMenuItem1.Name = "checkAllPostTasksToolStripMenuItem1";
            checkAllPostTasksToolStripMenuItem1.Size = new Size(231, 26);
            checkAllPostTasksToolStripMenuItem1.Text = "Check All Post Tasks";
            checkAllPostTasksToolStripMenuItem1.Click += checkAllPostTasksMenuItem_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(29, 24);
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(100, 105);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(227, 27);
            txtUsername.TabIndex = 7;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(19, 110);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(75, 20);
            lblUsername.TabIndex = 6;
            lblUsername.Text = "Username";
            // 
            // TaskRx
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(758, 713);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Controls.Add(btnBase);
            Controls.Add(txtBase);
            Controls.Add(lblBase);
            Controls.Add(txtPersonal);
            Controls.Add(lblPersonal);
            Controls.Add(txtWork);
            Controls.Add(lblWork);
            Controls.Add(txtDomain);
            Controls.Add(lblDomain);
            Controls.Add(txtLast);
            Controls.Add(lblLast);
            Controls.Add(txtFirst);
            Controls.Add(lblFirst);
            Controls.Add(executionStrip);
            Controls.Add(executionProgress);
            Controls.Add(setupTabControl);
            Controls.Add(executeButton);
            Controls.Add(checkAllSplitButton);
            Controls.Add(uncheckAllSplitButton);
            Controls.Add(txtOutput);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TaskRx";
            Text = "TaskRx V1.0.5";
            Load += TaskRx_Load;
            executionStrip.ResumeLayout(false);
            executionStrip.PerformLayout();
            uncheckAllSplitButton.ResumeLayout(false);
            uncheckAllSplitButton.PerformLayout();
            checkAllSplitButton.ResumeLayout(false);
            checkAllSplitButton.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TabControl setupTabControl;
        private System.Windows.Forms.Button executeButton;
        private ProgressBar executionProgress;
        private StatusStrip executionStrip;
        private ToolStripStatusLabel executionStatus;
        private Label lblFirst;
        private TextBox txtFirst;
        private TextBox txtLast;
        private Label lblLast;
        private TextBox txtDomain;
        private Label lblDomain;
        private TextBox txtPersonal;
        private Label lblPersonal;
        private RichTextBox txtOutput;
        private TextBox txtBase;
        private Label lblBase;
        private Button btnBase;
        private Label lblWork;
        private TextBox txtWork;
        private ToolStripSplitButton uncheckAllButton;
        private ToolStripMenuItem uncheckAllTasksMenuItem;
        private ToolStripMenuItem uncheckAllPostTasksMenuItem;
        private ToolStrip uncheckAllSplitButton;
        private ToolStrip checkAllSplitButton;
        private ToolStripSplitButton checkWorkstationButton;
        private ToolStripMenuItem checkAllTasksMenuItem;
        private ToolStripMenuItem checkAllPostTasksMenuItem;
        private ToolStripMenuItem checkAllPostTasksToolStripMenuItem;
        private ToolStripMenuItem checkAllPostTasksToolStripMenuItem1;
        private ToolStripButton toolStripButton1;
        private TextBox txtUsername;
        private Label lblUsername;
    }
}