namespace CodeMachine
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("生成框架");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("导出菜单");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("生成实体类");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("开发", new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode28,
            treeNode29});
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("接口测试");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("pmass");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("测试", new System.Windows.Forms.TreeNode[] {
            treeNode31,
            treeNode32});
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("菜单路径查询");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("查询", new System.Windows.Forms.TreeNode[] {
            treeNode34});
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("自动联网");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("帮助", new System.Windows.Forms.TreeNode[] {
            treeNode36});
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("统计");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("其他", new System.Windows.Forms.TreeNode[] {
            treeNode38});
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.CreateEntityButton = new System.Windows.Forms.Button();
            this.ExportMenuButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据库链接测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.metroTabControl1.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(949, 509);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AllowDrop = true;
            this.tableLayoutPanel2.AutoScroll = true;
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(949, 484);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.AllowDrop = true;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(941, 455);
            this.splitContainer1.SplitterDistance = 203;
            this.splitContainer1.TabIndex = 6;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeView1.Indent = 7;
            this.treeView1.ItemHeight = 30;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode27.Name = "createForm";
            treeNode27.Text = "生成框架";
            treeNode28.Name = "exportSql";
            treeNode28.Text = "导出菜单";
            treeNode29.Name = "createEntity";
            treeNode29.Text = "生成实体类";
            treeNode30.Name = "KAIFA";
            treeNode30.Text = "开发";
            treeNode31.Name = "interfaceTest";
            treeNode31.Text = "接口测试";
            treeNode32.Name = "pmass";
            treeNode32.Text = "pmass";
            treeNode33.Name = "CESHI";
            treeNode33.Text = "测试";
            treeNode34.Name = "menuPath";
            treeNode34.Text = "菜单路径查询";
            treeNode35.Name = "CHAXUN";
            treeNode35.Text = "查询";
            treeNode36.Name = "autoConect";
            treeNode36.Text = "自动联网";
            treeNode37.Name = "BANGZHU";
            treeNode37.Text = "帮助";
            treeNode38.Name = "tongji";
            treeNode38.Text = "统计";
            treeNode39.Name = "QITA";
            treeNode39.Text = "其他";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode30,
            treeNode33,
            treeNode35,
            treeNode37,
            treeNode39});
            this.treeView1.ShowLines = false;
            this.treeView1.Size = new System.Drawing.Size(203, 455);
            this.treeView1.TabIndex = 5;
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TreeView1_MouseDoubleClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.metroTabControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 455F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 455);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.AllowDrop = true;
            this.metroTabControl1.Controls.Add(this.metroTabPage1);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(28, 3);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.ShowToolTips = true;
            this.metroTabControl1.Size = new System.Drawing.Size(703, 449);
            this.metroTabControl1.TabIndex = 10;
            this.metroTabControl1.UseSelectable = true;
            this.metroTabControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MetroTabControl1_MouseDoubleClick);
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.AllowDrop = true;
            this.metroTabPage1.Controls.Add(this.button3);
            this.metroTabPage1.Controls.Add(this.button1);
            this.metroTabPage1.Controls.Add(this.CreateEntityButton);
            this.metroTabPage1.Controls.Add(this.ExportMenuButton);
            this.metroTabPage1.HorizontalScrollbar = true;
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(695, 407);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "首页";
            this.metroTabPage1.VerticalScrollbar = true;
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.Location = new System.Drawing.Point(390, 53);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(123, 106);
            this.button3.TabIndex = 5;
            this.button3.Text = "PMASS";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(261, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 106);
            this.button1.TabIndex = 4;
            this.button1.Text = "生成框架";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // CreateEntityButton
            // 
            this.CreateEntityButton.BackColor = System.Drawing.Color.MediumTurquoise;
            this.CreateEntityButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CreateEntityButton.Location = new System.Drawing.Point(132, 53);
            this.CreateEntityButton.Name = "CreateEntityButton";
            this.CreateEntityButton.Size = new System.Drawing.Size(123, 106);
            this.CreateEntityButton.TabIndex = 3;
            this.CreateEntityButton.Text = "生成实体类";
            this.CreateEntityButton.UseVisualStyleBackColor = false;
            this.CreateEntityButton.Click += new System.EventHandler(this.CreateEntityButton_Click);
            // 
            // ExportMenuButton
            // 
            this.ExportMenuButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ExportMenuButton.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExportMenuButton.Location = new System.Drawing.Point(3, 53);
            this.ExportMenuButton.Name = "ExportMenuButton";
            this.ExportMenuButton.Size = new System.Drawing.Size(123, 106);
            this.ExportMenuButton.TabIndex = 2;
            this.ExportMenuButton.Text = "导出菜单";
            this.ExportMenuButton.UseVisualStyleBackColor = false;
            this.ExportMenuButton.Click += new System.EventHandler(this.ExportMenuButton_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.Location = new System.Drawing.Point(3, 155);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(19, 144);
            this.button2.TabIndex = 1;
            this.button2.Text = "《";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(446, 464);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "版权信息";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2);
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Margin = new System.Windows.Forms.Padding(1);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(1);
            this.menuStrip1.Size = new System.Drawing.Size(949, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据库链接测试ToolStripMenuItem});
            this.帮助ToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Padding = new System.Windows.Forms.Padding(1);
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(38, 23);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 数据库链接测试ToolStripMenuItem
            // 
            this.数据库链接测试ToolStripMenuItem.MergeIndex = 0;
            this.数据库链接测试ToolStripMenuItem.Name = "数据库链接测试ToolStripMenuItem";
            this.数据库链接测试ToolStripMenuItem.Padding = new System.Windows.Forms.Padding(1);
            this.数据库链接测试ToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.数据库链接测试ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.数据库链接测试ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.数据库链接测试ToolStripMenuItem.Text = "数据库链接测试";
            this.数据库链接测试ToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.数据库链接测试ToolStripMenuItem.Click += new System.EventHandler(this.数据库链接测试ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 589);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "CodeMachine";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.metroTabControl1.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据库链接测试ToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button2;
        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button CreateEntityButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ExportMenuButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
    }
}

