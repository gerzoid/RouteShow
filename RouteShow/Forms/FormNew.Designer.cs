namespace RouteShow.Forms
{
    partial class FormNew
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNew));
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nMetric = new System.Windows.Forms.NumericUpDown();
            this.tbHop = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMask = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDest = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbPersistent = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbInterfaces = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nMetric)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Image = global::RouteShow.Properties.Resources.cancel;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(145, 210);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(44, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 22);
            this.button1.TabIndex = 19;
            this.button1.Text = "Ок";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Метрика";
            // 
            // nMetric
            // 
            this.nMetric.Location = new System.Drawing.Point(148, 76);
            this.nMetric.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nMetric.Name = "nMetric";
            this.nMetric.Size = new System.Drawing.Size(54, 20);
            this.nMetric.TabIndex = 17;
            this.nMetric.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbHop
            // 
            this.tbHop.Location = new System.Drawing.Point(17, 76);
            this.tbHop.Name = "tbHop";
            this.tbHop.Size = new System.Drawing.Size(107, 20);
            this.tbHop.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Шлюз";
            // 
            // tbMask
            // 
            this.tbMask.Location = new System.Drawing.Point(145, 33);
            this.tbMask.Name = "tbMask";
            this.tbMask.Size = new System.Drawing.Size(107, 20);
            this.tbMask.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Маска";
            // 
            // tbDest
            // 
            this.tbDest.Location = new System.Drawing.Point(17, 33);
            this.tbDest.Name = "tbDest";
            this.tbDest.Size = new System.Drawing.Size(107, 20);
            this.tbDest.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Сетевой адрес";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Постоянный:";
            // 
            // cbPersistent
            // 
            this.cbPersistent.FormattingEnabled = true;
            this.cbPersistent.Items.AddRange(new object[] {
            "Нет",
            "Да"});
            this.cbPersistent.Location = new System.Drawing.Point(93, 166);
            this.cbPersistent.Name = "cbPersistent";
            this.cbPersistent.Size = new System.Drawing.Size(56, 21);
            this.cbPersistent.TabIndex = 21;
            this.cbPersistent.Text = "Нет";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Интерфейс";
            // 
            // cbInterfaces
            // 
            this.cbInterfaces.FormattingEnabled = true;
            this.cbInterfaces.Location = new System.Drawing.Point(17, 128);
            this.cbInterfaces.Name = "cbInterfaces";
            this.cbInterfaces.Size = new System.Drawing.Size(245, 21);
            this.cbInterfaces.TabIndex = 24;
            this.cbInterfaces.SelectedIndexChanged += new System.EventHandler(this.cbInterfaces_SelectedIndexChanged);
            // 
            // FormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 250);
            this.Controls.Add(this.cbInterfaces);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbPersistent);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nMetric);
            this.Controls.Add(this.tbHop);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbMask);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDest);
            this.Controls.Add(this.label1);
            this.Name = "FormNew";
            this.Text = "Добавить маршрут";
            this.Shown += new System.EventHandler(this.FormNew_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nMetric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown nMetric;
        public System.Windows.Forms.MaskedTextBox tbHop;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.MaskedTextBox tbMask;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.MaskedTextBox tbDest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox cbPersistent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbInterfaces;
    }
}