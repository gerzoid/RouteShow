﻿namespace RouteShow
{
    partial class FormEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbDest = new System.Windows.Forms.MaskedTextBox();
            this.tbMask = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbHop = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nMetric = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nMetric)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Сетевой адрес";
            // 
            // tbDest
            // 
            this.tbDest.Location = new System.Drawing.Point(12, 38);
            this.tbDest.Name = "tbDest";
            this.tbDest.Size = new System.Drawing.Size(107, 20);
            this.tbDest.TabIndex = 2;
            // 
            // tbMask
            // 
            this.tbMask.Location = new System.Drawing.Point(140, 38);
            this.tbMask.Name = "tbMask";
            this.tbMask.Size = new System.Drawing.Size(107, 20);
            this.tbMask.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Маска";
            // 
            // tbHop
            // 
            this.tbHop.Location = new System.Drawing.Point(12, 92);
            this.tbHop.Name = "tbHop";
            this.tbHop.Size = new System.Drawing.Size(107, 20);
            this.tbHop.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Шлюз";
            // 
            // nMetric
            // 
            this.nMetric.Location = new System.Drawing.Point(143, 92);
            this.nMetric.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nMetric.Name = "nMetric";
            this.nMetric.Size = new System.Drawing.Size(54, 20);
            this.nMetric.TabIndex = 7;
            this.nMetric.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Метрика";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(60, 206);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 22);
            this.button1.TabIndex = 9;
            this.button1.Text = "Ок";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(143, 206);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 250);
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
            this.Name = "FormEdit";
            this.Text = "FormEdit";
            ((System.ComponentModel.ISupportInitialize)(this.nMetric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.MaskedTextBox tbDest;
        public System.Windows.Forms.MaskedTextBox tbMask;
        public System.Windows.Forms.MaskedTextBox tbHop;
        public System.Windows.Forms.NumericUpDown nMetric;

    }
}