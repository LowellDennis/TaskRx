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
            checkDevelopmentVMMenuItem = new ToolStripMenuItem();
            checkAllPostTasksMenuItem = new ToolStripMenuItem();
            checkAllPostTasksToolStripMenuItem = new ToolStripMenuItem();
            checkAllPostTasksToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripButton1 = new ToolStripButton();
            txtUsername = new TextBox();
            lblUsername = new Label();
            txtInitials = new TextBox();
            lblInitials = new Label();
            executionStrip.SuspendLayout();
            uncheckAllSplitButton.SuspendLayout();
            checkAllSplitButton.SuspendLayout();
            SuspendLayout();
            // 
            // setupTabControl
            // 
            setupTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            setupTabControl.Location = new Point(12, 175);
            setupTabControl.Name = "setupTabControl";
            setupTabControl.SelectedIndex = 0;
            setupTabControl.Size = new Size(832, 511);
            setupTabControl.TabIndex = 19;
            // 
            // executeButton
            // 
            executeButton.Location = new Point(12, 692);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(100, 30);
            executeButton.TabIndex = 20;
            executeButton.Text = "Execute";
            executeButton.Click += executeButton_Click;
            // 
            // executionProgress
            // 
            executionProgress.Location = new Point(118, 692);
            executionProgress.Name = "executionProgress";
            executionProgress.Size = new Size(726, 29);
            executionProgress.TabIndex = 21;
            // 
            // executionStrip
            // 
            executionStrip.ImageScalingSize = new Size(20, 20);
            executionStrip.Items.AddRange(new ToolStripItem[] { executionStatus });
            executionStrip.Location = new Point(0, 725);
            executionStrip.Name = "executionStrip";
            executionStrip.Size = new Size(856, 22);
            executionStrip.TabIndex = 22;
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
            lblFirst.Location = new Point(17, 15);
            lblFirst.Name = "lblFirst";
            lblFirst.Size = new Size(80, 20);
            lblFirst.TabIndex = 0;
            lblFirst.Text = "First Name";
            // 
            // txtFirst
            // 
            txtFirst.Location = new Point(102, 12);
            txtFirst.Name = "txtFirst";
            txtFirst.Size = new Size(225, 27);
            txtFirst.TabIndex = 1;
            txtFirst.TextChanged += txtFirst_Changed;
            // 
            // txtLast
            // 
            txtLast.Location = new Point(102, 45);
            txtLast.Name = "txtLast";
            txtLast.Size = new Size(225, 27);
            txtLast.TabIndex = 3;
            txtLast.TextChanged += txtLast_Changed;
            // 
            // lblLast
            // 
            lblLast.AutoSize = true;
            lblLast.Location = new Point(18, 48);
            lblLast.Name = "lblLast";
            lblLast.Size = new Size(79, 20);
            lblLast.TabIndex = 2;
            lblLast.Text = "Last Name";
            // 
            // txtDomain
            // 
            txtDomain.Location = new Point(102, 109);
            txtDomain.Name = "txtDomain";
            txtDomain.Size = new Size(227, 27);
            txtDomain.TabIndex = 7;
            txtDomain.TextChanged += txtUsername_TextChanged;
            // 
            // lblDomain
            // 
            lblDomain.AutoSize = true;
            lblDomain.Location = new Point(34, 116);
            lblDomain.Name = "lblDomain";
            lblDomain.Size = new Size(62, 20);
            lblDomain.TabIndex = 6;
            lblDomain.Text = "Domain";
            // 
            // txtPersonal
            // 
            txtPersonal.Location = new Point(436, 47);
            txtPersonal.Name = "txtPersonal";
            txtPersonal.Size = new Size(325, 27);
            txtPersonal.TabIndex = 13;
            txtPersonal.TextChanged += txtPersonal_TextChanged;
            // 
            // lblPersonal
            // 
            lblPersonal.AutoSize = true;
            lblPersonal.Location = new Point(339, 50);
            lblPersonal.Name = "lblPersonal";
            lblPersonal.Size = new Size(91, 20);
            lblPersonal.TabIndex = 12;
            lblPersonal.Text = "Home Email";
            // 
            // txtOutput
            // 
            txtOutput.DetectUrls = false;
            txtOutput.Location = new Point(12, 7);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            txtOutput.Size = new Size(832, 647);
            txtOutput.TabIndex = 23;
            txtOutput.Text = "";
            txtOutput.Visible = false;
            txtOutput.WordWrap = false;
            // 
            // txtBase
            // 
            txtBase.Location = new Point(436, 81);
            txtBase.Name = "txtBase";
            txtBase.Size = new Size(325, 27);
            txtBase.TabIndex = 15;
            txtBase.TextChanged += txtBase_TextChanged;
            // 
            // lblBase
            // 
            lblBase.AutoSize = true;
            lblBase.Location = new Point(345, 84);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(85, 20);
            lblBase.TabIndex = 14;
            lblBase.Text = "Repos Base";
            // 
            // btnBase
            // 
            btnBase.Location = new Point(767, 79);
            btnBase.Name = "btnBase";
            btnBase.Size = new Size(77, 30);
            btnBase.TabIndex = 16;
            btnBase.Text = "Browse";
            btnBase.Click += btnBase_Click;
            // 
            // lblWork
            // 
            lblWork.AutoSize = true;
            lblWork.Location = new Point(345, 17);
            lblWork.Name = "lblWork";
            lblWork.Size = new Size(84, 20);
            lblWork.TabIndex = 10;
            lblWork.Text = "Work Email";
            // 
            // txtWork
            // 
            txtWork.Location = new Point(435, 14);
            txtWork.Name = "txtWork";
            txtWork.Size = new Size(326, 27);
            txtWork.TabIndex = 11;
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
            uncheckAllSplitButton.Location = new Point(693, 142);
            uncheckAllSplitButton.Name = "uncheckAllSplitButton";
            uncheckAllSplitButton.RenderMode = ToolStripRenderMode.System;
            uncheckAllSplitButton.Size = new Size(108, 27);
            uncheckAllSplitButton.TabIndex = 18;
            uncheckAllSplitButton.Text = "Uncheck All";
            // 
            // checkAllSplitButton
            // 
            checkAllSplitButton.Dock = DockStyle.None;
            checkAllSplitButton.GripStyle = ToolStripGripStyle.Hidden;
            checkAllSplitButton.ImageScalingSize = new Size(20, 20);
            checkAllSplitButton.Items.AddRange(new ToolStripItem[] { checkWorkstationButton, toolStripButton1 });
            checkAllSplitButton.Location = new Point(435, 142);
            checkAllSplitButton.Name = "checkAllSplitButton";
            checkAllSplitButton.RenderMode = ToolStripRenderMode.System;
            checkAllSplitButton.Size = new Size(198, 27);
            checkAllSplitButton.TabIndex = 17;
            checkAllSplitButton.Text = "Check All";
            // 
            // checkWorkstationButton
            // 
            checkWorkstationButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            checkWorkstationButton.DropDownItems.AddRange(new ToolStripItem[] { checkAllTasksMenuItem, checkDevelopmentVMMenuItem, checkAllPostTasksMenuItem, checkAllPostTasksToolStripMenuItem, checkAllPostTasksToolStripMenuItem1 });
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
            // checkDevelopmentVMMenuItem
            // 
            checkDevelopmentVMMenuItem.Name = "checkDevelopmentVMMenuItem";
            checkDevelopmentVMMenuItem.Size = new Size(231, 26);
            checkDevelopmentVMMenuItem.Text = "DevelopmentVM Defaults";
            checkDevelopmentVMMenuItem.Click += checkDefaultDevelopmentVM_ButtonClick;
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
            txtUsername.Location = new Point(102, 142);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(227, 27);
            txtUsername.TabIndex = 9;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(21, 145);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(75, 20);
            lblUsername.TabIndex = 8;
            lblUsername.Text = "Username";
            // 
            // txtInitials
            // 
            txtInitials.Location = new Point(102, 78);
            txtInitials.Name = "txtInitials";
            txtInitials.Size = new Size(225, 27);
            txtInitials.TabIndex = 5;
            txtInitials.TextChanged += txtInitials_TextChanged;
            // 
            // lblInitials
            // 
            lblInitials.AutoSize = true;
            lblInitials.Location = new Point(44, 81);
            lblInitials.Name = "lblInitials";
            lblInitials.Size = new Size(52, 20);
            lblInitials.TabIndex = 4;
            lblInitials.Text = "Initials";
            // 
            // TaskRx
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(856, 747);
            Controls.Add(txtInitials);
            Controls.Add(lblInitials);
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
            Text = "TaskRx V1.1";
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
        private ToolStripButton toolStripButton1;
        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtInitials;
        private Label lblInitials;
        private ToolStripSplitButton checkWorkstationButton;
        private ToolStripMenuItem checkAllTasksMenuItem;
        private ToolStripMenuItem checkDevelopmentVMMenuItem;
        private ToolStripMenuItem checkAllPostTasksMenuItem;
        private ToolStripMenuItem checkAllPostTasksToolStripMenuItem;
        private ToolStripMenuItem checkAllPostTasksToolStripMenuItem1;
    }
}