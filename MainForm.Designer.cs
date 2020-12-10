namespace DataBase
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this._folderOpenBtn = new System.Windows.Forms.Button();
            this.dataGridViewMain = new System.Windows.Forms.DataGridView();
            this._deleteRowBtn = new System.Windows.Forms.Button();
            this._nameTbx = new System.Windows.Forms.TextBox();
            this._nameLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).BeginInit();
            this.SuspendLayout();
            // 
            // _folderOpenBtn
            // 
            this._folderOpenBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._folderOpenBtn.Location = new System.Drawing.Point(12, 484);
            this._folderOpenBtn.Name = "_folderOpenBtn";
            this._folderOpenBtn.Size = new System.Drawing.Size(152, 55);
            this._folderOpenBtn.TabIndex = 0;
            this._folderOpenBtn.Text = "Открыть папку с файлами";
            this._folderOpenBtn.UseVisualStyleBackColor = true;
            this._folderOpenBtn.Click += new System.EventHandler(this._folderOpenBtn_Click);
            // 
            // dataGridViewMain
            // 
            this.dataGridViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMain.Location = new System.Drawing.Point(28, 12);
            this.dataGridViewMain.Name = "dataGridViewMain";
            this.dataGridViewMain.RowHeadersWidth = 51;
            this.dataGridViewMain.RowTemplate.Height = 24;
            this.dataGridViewMain.Size = new System.Drawing.Size(801, 277);
            this.dataGridViewMain.TabIndex = 1;
            // 
            // _deleteRowBtn
            // 
            this._deleteRowBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._deleteRowBtn.Location = new System.Drawing.Point(679, 491);
            this._deleteRowBtn.Name = "_deleteRowBtn";
            this._deleteRowBtn.Size = new System.Drawing.Size(141, 41);
            this._deleteRowBtn.TabIndex = 2;
            this._deleteRowBtn.Text = "Удалить запись";
            this._deleteRowBtn.UseVisualStyleBackColor = true;
            this._deleteRowBtn.Click += new System.EventHandler(this._deleteRowBtn_Click);
            // 
            // _nameTbx
            // 
            this._nameTbx.Location = new System.Drawing.Point(30, 322);
            this._nameTbx.Name = "_nameTbx";
            this._nameTbx.Size = new System.Drawing.Size(134, 22);
            this._nameTbx.TabIndex = 3;
            this._nameTbx.TextChanged += new System.EventHandler(this._nameTbx_TextChanged);
            // 
            // _nameLbl
            // 
            this._nameLbl.AutoSize = true;
            this._nameLbl.Location = new System.Drawing.Point(27, 302);
            this._nameLbl.Name = "_nameLbl";
            this._nameLbl.Size = new System.Drawing.Size(137, 17);
            this._nameLbl.TabIndex = 4;
            this._nameLbl.Text = "Найти по названию";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 561);
            this.Controls.Add(this._nameLbl);
            this.Controls.Add(this._nameTbx);
            this.Controls.Add(this._deleteRowBtn);
            this.Controls.Add(this.dataGridViewMain);
            this.Controls.Add(this._folderOpenBtn);
            this.Name = "MainForm";
            this.Text = "DataBase";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button _folderOpenBtn;
        private System.Windows.Forms.DataGridView dataGridViewMain;
        private System.Windows.Forms.Button _deleteRowBtn;
        private System.Windows.Forms.TextBox _nameTbx;
        private System.Windows.Forms.Label _nameLbl;
    }
}

