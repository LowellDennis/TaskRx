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
            txtUsername = new TextBox();
            lblUsername = new Label();
            txtPersonal = new TextBox();
            lblPersonal = new Label();
            txtOutput = new RichTextBox();
            checkAllSplitButton = new ToolStrip();
            checkDefaultButton = new ToolStripSplitButton();
            checkAllTasksMenuItem = new ToolStripMenuItem();
            checkAllPostTasksMenuItem = new ToolStripMenuItem();
            checkAllPostTasksToolStripMenuItem = new ToolStripMenuItem();
            toolStripButton1 = new ToolStripButton();
            uncheckAllSplitButton = new ToolStrip();
            uncheckAllButton = new ToolStripSplitButton();
            uncheckAllTasksMenuItem = new ToolStripMenuItem();
            uncheckAllPostTasksMenuItem = new ToolStripMenuItem();
            txtBase = new TextBox();
            lblBase = new Label();
            btnBase = new Button();
            lblWork = new Label();
            txtWork = new TextBox();
            executionStrip.SuspendLayout();
            checkAllSplitButton.SuspendLayout();
            uncheckAllSplitButton.SuspendLayout();
            SuspendLayout();
            // 
            // setupTabControl
            // 
            setupTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            setupTabControl.Location = new Point(12, 142);
            setupTabControl.Name = "setupTabControl";
            setupTabControl.SelectedIndex = 0;
            setupTabControl.Size = new Size(734, 373);
            setupTabControl.TabIndex = 12;
            // 
            // executeButton
            // 
            executeButton.Location = new Point(12, 528);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(100, 30);
            executeButton.TabIndex = 18;
            executeButton.Text = "Execute";
            executeButton.Click += executeButton_Click;
            // 
            // executionProgress
            // 
            executionProgress.Location = new Point(125, 528);
            executionProgress.Name = "executionProgress";
            executionProgress.Size = new Size(618, 29);
            executionProgress.TabIndex = 19;
            // 
            // executionStrip
            // 
            executionStrip.ImageScalingSize = new Size(20, 20);
            executionStrip.Items.AddRange(new ToolStripItem[] { executionStatus });
            executionStrip.Location = new Point(0, 566);
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
            // txtUsername
            // 
            txtUsername.Location = new Point(100, 72);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(227, 27);
            txtUsername.TabIndex = 5;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(19, 77);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(75, 20);
            lblUsername.TabIndex = 4;
            lblUsername.Text = "Username";
            // 
            // txtPersonal
            // 
            txtPersonal.Location = new Point(436, 39);
            txtPersonal.Name = "txtPersonal";
            txtPersonal.Size = new Size(227, 27);
            txtPersonal.TabIndex = 9;
            txtPersonal.TextChanged += txtPersonal_TextChanged;
            // 
            // lblPersonal
            // 
            lblPersonal.AutoSize = true;
            lblPersonal.Location = new Point(339, 42);
            lblPersonal.Name = "lblPersonal";
            lblPersonal.Size = new Size(91, 20);
            lblPersonal.TabIndex = 8;
            lblPersonal.Text = "Home Email";
            // 
            // txtOutput
            // 
            txtOutput.DetectUrls = false;
            txtOutput.Location = new Point(19, 7);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            txtOutput.Size = new Size(734, 508);
            txtOutput.TabIndex = 17;
            txtOutput.Text = "";
            txtOutput.Visible = false;
            txtOutput.WordWrap = false;
            // 
            // checkAllSplitButton
            // 
            checkAllSplitButton.Dock = DockStyle.None;
            checkAllSplitButton.GripStyle = ToolStripGripStyle.Hidden;
            checkAllSplitButton.ImageScalingSize = new Size(20, 20);
            checkAllSplitButton.Items.AddRange(new ToolStripItem[] { checkDefaultButton, toolStripButton1 });
            checkAllSplitButton.Location = new Point(19, 109);
            checkAllSplitButton.Name = "checkAllSplitButton";
            checkAllSplitButton.RenderMode = ToolStripRenderMode.System;
            checkAllSplitButton.Size = new Size(152, 27);
            checkAllSplitButton.TabIndex = 13;
            checkAllSplitButton.Text = "Check All";
            // 
            // checkDefaultButton
            // 
            checkDefaultButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            checkDefaultButton.DropDownItems.AddRange(new ToolStripItem[] { checkAllTasksMenuItem, checkAllPostTasksMenuItem, checkAllPostTasksToolStripMenuItem });
            checkDefaultButton.ImageTransparentColor = Color.Magenta;
            checkDefaultButton.Name = "checkDefaultButton";
            checkDefaultButton.Size = new Size(120, 24);
            checkDefaultButton.Text = "Check Default";
            checkDefaultButton.ButtonClick += checkDefaultButton_ButtonClick;
            // 
            // checkAllTasksMenuItem
            // 
            checkAllTasksMenuItem.Name = "checkAllTasksMenuItem";
            checkAllTasksMenuItem.Size = new Size(221, 26);
            checkAllTasksMenuItem.Text = "Check All";
            checkAllTasksMenuItem.Click += checkAllMenuItem_ButtonClick;
            // 
            // checkAllPostTasksMenuItem
            // 
            checkAllPostTasksMenuItem.Name = "checkAllPostTasksMenuItem";
            checkAllPostTasksMenuItem.Size = new Size(221, 26);
            checkAllPostTasksMenuItem.Text = "Check All Tasks";
            checkAllPostTasksMenuItem.Click += checkAllTasksMenuItem_Click;
            // 
            // checkAllPostTasksToolStripMenuItem
            // 
            checkAllPostTasksToolStripMenuItem.Name = "checkAllPostTasksToolStripMenuItem";
            checkAllPostTasksToolStripMenuItem.Size = new Size(221, 26);
            checkAllPostTasksToolStripMenuItem.Text = "Check All Post Tasks";
            checkAllPostTasksToolStripMenuItem.Click += checkAllPostTasksMenuItem_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(29, 24);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // uncheckAllSplitButton
            // 
            uncheckAllSplitButton.Dock = DockStyle.None;
            uncheckAllSplitButton.GripStyle = ToolStripGripStyle.Hidden;
            uncheckAllSplitButton.ImageScalingSize = new Size(20, 20);
            uncheckAllSplitButton.Items.AddRange(new ToolStripItem[] { uncheckAllButton });
            uncheckAllSplitButton.Location = new Point(175, 109);
            uncheckAllSplitButton.Name = "uncheckAllSplitButton";
            uncheckAllSplitButton.RenderMode = ToolStripRenderMode.System;
            uncheckAllSplitButton.Size = new Size(108, 27);
            uncheckAllSplitButton.TabIndex = 14;
            uncheckAllSplitButton.Text = "Uncheck All";
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
            // txtBase
            // 
            txtBase.Location = new Point(436, 72);
            txtBase.Name = "txtBase";
            txtBase.Size = new Size(227, 27);
            txtBase.TabIndex = 11;
            txtBase.TextChanged += txtBase_TextChanged;
            // 
            // lblBase
            // 
            lblBase.AutoSize = true;
            lblBase.Location = new Point(345, 75);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(85, 20);
            lblBase.TabIndex = 10;
            lblBase.Text = "Repos Base";
            // 
            // btnBase
            // 
            btnBase.Location = new Point(669, 72);
            btnBase.Name = "btnBase";
            btnBase.Size = new Size(77, 30);
            btnBase.TabIndex = 12;
            btnBase.Text = "Browse";
            btnBase.Click += btnBase_Click;
            // 
            // lblWork
            // 
            lblWork.AutoSize = true;
            lblWork.Location = new Point(346, 10);
            lblWork.Name = "lblWork";
            lblWork.Size = new Size(84, 20);
            lblWork.TabIndex = 6;
            lblWork.Text = "Work Email";
            // 
            // txtWork
            // 
            txtWork.Location = new Point(436, 7);
            txtWork.Name = "txtWork";
            txtWork.Size = new Size(227, 27);
            txtWork.TabIndex = 7;
            txtWork.TextChanged += txtWork_TextChanged;
            // 
            // TaskRx
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(758, 588);
            Controls.Add(btnBase);
            Controls.Add(txtBase);
            Controls.Add(lblBase);
            Controls.Add(txtPersonal);
            Controls.Add(lblPersonal);
            Controls.Add(txtWork);
            Controls.Add(lblWork);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
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
            Text = "TaskRx V1.0";
            Load += TaskRx_Load;
            executionStrip.ResumeLayout(false);
            executionStrip.PerformLayout();
            checkAllSplitButton.ResumeLayout(false);
            checkAllSplitButton.PerformLayout();
            uncheckAllSplitButton.ResumeLayout(false);
            uncheckAllSplitButton.PerformLayout();
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
        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtPersonal;
        private Label lblPersonal;
        private RichTextBox txtOutput;
        private ToolStrip checkAllSplitButton;
        private ToolStripSplitButton checkDefaultButton;
        private ToolStripMenuItem checkAllTasksMenuItem;
        private ToolStripMenuItem checkAllPostTasksMenuItem;
        private ToolStrip uncheckAllSplitButton;
        private ToolStripSplitButton uncheckAllButton;
        private ToolStripMenuItem uncheckAllTasksMenuItem;
        private ToolStripMenuItem uncheckAllPostTasksMenuItem;
        private TextBox txtBase;
        private Label lblBase;
        private Button btnBase;
        private Label lblWork;
        private TextBox txtWork;
        private ToolStripButton toolStripButton1;
        private ToolStripMenuItem checkAllPostTasksToolStripMenuItem;
    }
}