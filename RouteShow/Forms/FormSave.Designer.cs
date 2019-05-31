namespace RouteShow.Forms
{
    partial class FormSave
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSave));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioCSV = new System.Windows.Forms.RadioButton();
            this.radioBAT = new System.Windows.Forms.RadioButton();
            this.radioXLS = new System.Windows.Forms.RadioButton();
            this.radioTXT = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioCSV);
            this.groupBox1.Controls.Add(this.radioBAT);
            this.groupBox1.Controls.Add(this.radioXLS);
            this.groupBox1.Controls.Add(this.radioTXT);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(80, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип файла";
            // 
            // radioCSV
            // 
            this.radioCSV.AutoSize = true;
            this.radioCSV.Location = new System.Drawing.Point(16, 70);
            this.radioCSV.Name = "radioCSV";
            this.radioCSV.Size = new System.Drawing.Size(46, 17);
            this.radioCSV.TabIndex = 2;
            this.radioCSV.TabStop = true;
            this.radioCSV.Text = "CSV";
            this.radioCSV.UseVisualStyleBackColor = true;
            // 
            // radioBAT
            // 
            this.radioBAT.AutoSize = true;
            this.radioBAT.Location = new System.Drawing.Point(16, 47);
            this.radioBAT.Name = "radioBAT";
            this.radioBAT.Size = new System.Drawing.Size(46, 17);
            this.radioBAT.TabIndex = 1;
            this.radioBAT.Text = "BAT";
            this.radioBAT.UseVisualStyleBackColor = true;
            // 
            // radioXLS
            // 
            this.radioXLS.AutoSize = true;
            this.radioXLS.Location = new System.Drawing.Point(16, 94);
            this.radioXLS.Name = "radioXLS";
            this.radioXLS.Size = new System.Drawing.Size(45, 17);
            this.radioXLS.TabIndex = 1;
            this.radioXLS.Text = "XLS";
            this.radioXLS.UseVisualStyleBackColor = true;
            // 
            // radioTXT
            // 
            this.radioTXT.AutoSize = true;
            this.radioTXT.Checked = true;
            this.radioTXT.Location = new System.Drawing.Point(16, 22);
            this.radioTXT.Name = "radioTXT";
            this.radioTXT.Size = new System.Drawing.Size(46, 17);
            this.radioTXT.TabIndex = 1;
            this.radioTXT.TabStop = true;
            this.radioTXT.Text = "TXT";
            this.radioTXT.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Image = global::RouteShow.Properties.Resources.apply;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(114, 181);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Ок";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(214, 181);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "По сетевым интерфейсам:";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point(101, 28);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(314, 124);
            this.checkedListBox1.TabIndex = 4;
            // 
            // FormSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 216);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormSave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Экспорт маршрутов";
            this.Shown += new System.EventHandler(this.FormSave_Shown);
            this.Resize += new System.EventHandler(this.FormSave_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBAT;
        private System.Windows.Forms.RadioButton radioXLS;
        private System.Windows.Forms.RadioButton radioTXT;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton radioCSV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
    }
}