namespace Carga_Masiva_Suma
{
    partial class Form1
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
            this.btnProcesar = new System.Windows.Forms.Button();
            this.lstBuenos = new System.Windows.Forms.ListBox();
            this.lstMalos = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalBuenos = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalMalos = new System.Windows.Forms.Label();
            this.lblCedulas = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInicio = new System.Windows.Forms.Label();
            this.lblFin = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblPorcentaje = new System.Windows.Forms.Label();
            this.btnFase2 = new System.Windows.Forms.Button();
            this.btnFase3 = new System.Windows.Forms.Button();
            this.lblClientes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(12, 12);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(75, 23);
            this.btnProcesar.TabIndex = 0;
            this.btnProcesar.Text = "Fase 1";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // lstBuenos
            // 
            this.lstBuenos.FormattingEnabled = true;
            this.lstBuenos.Location = new System.Drawing.Point(12, 77);
            this.lstBuenos.Name = "lstBuenos";
            this.lstBuenos.Size = new System.Drawing.Size(1161, 264);
            this.lstBuenos.TabIndex = 1;
            // 
            // lstMalos
            // 
            this.lstMalos.FormattingEnabled = true;
            this.lstMalos.Location = new System.Drawing.Point(12, 371);
            this.lstMalos.Name = "lstMalos";
            this.lstMalos.Size = new System.Drawing.Size(1162, 225);
            this.lstMalos.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 344);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Registros:";
            // 
            // lblTotalBuenos
            // 
            this.lblTotalBuenos.AutoSize = true;
            this.lblTotalBuenos.Location = new System.Drawing.Point(90, 344);
            this.lblTotalBuenos.Name = "lblTotalBuenos";
            this.lblTotalBuenos.Size = new System.Drawing.Size(13, 13);
            this.lblTotalBuenos.TabIndex = 4;
            this.lblTotalBuenos.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 599);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Total Registros:";
            // 
            // lblTotalMalos
            // 
            this.lblTotalMalos.AutoSize = true;
            this.lblTotalMalos.Location = new System.Drawing.Point(90, 599);
            this.lblTotalMalos.Name = "lblTotalMalos";
            this.lblTotalMalos.Size = new System.Drawing.Size(13, 13);
            this.lblTotalMalos.TabIndex = 6;
            this.lblTotalMalos.Text = "0";
            // 
            // lblCedulas
            // 
            this.lblCedulas.AutoSize = true;
            this.lblCedulas.Location = new System.Drawing.Point(12, 38);
            this.lblCedulas.Name = "lblCedulas";
            this.lblCedulas.Size = new System.Drawing.Size(297, 13);
            this.lblCedulas.TabIndex = 7;
            this.lblCedulas.Text = "0 documentos de identificación en 172.20.7.26/Cards/Clients";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 615);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Inicio:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 631);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Fin:";
            // 
            // lblInicio
            // 
            this.lblInicio.AutoSize = true;
            this.lblInicio.Location = new System.Drawing.Point(54, 615);
            this.lblInicio.Name = "lblInicio";
            this.lblInicio.Size = new System.Drawing.Size(0, 13);
            this.lblInicio.TabIndex = 11;
            // 
            // lblFin
            // 
            this.lblFin.AutoSize = true;
            this.lblFin.Location = new System.Drawing.Point(54, 631);
            this.lblFin.Name = "lblFin";
            this.lblFin.Size = new System.Drawing.Size(0, 13);
            this.lblFin.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 647);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Total:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(54, 647);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 13);
            this.lblTotal.TabIndex = 14;
            // 
            // lblPorcentaje
            // 
            this.lblPorcentaje.AutoSize = true;
            this.lblPorcentaje.Location = new System.Drawing.Point(739, 599);
            this.lblPorcentaje.Name = "lblPorcentaje";
            this.lblPorcentaje.Size = new System.Drawing.Size(0, 13);
            this.lblPorcentaje.TabIndex = 15;
            // 
            // btnFase2
            // 
            this.btnFase2.Location = new System.Drawing.Point(93, 12);
            this.btnFase2.Name = "btnFase2";
            this.btnFase2.Size = new System.Drawing.Size(75, 23);
            this.btnFase2.TabIndex = 16;
            this.btnFase2.Text = "Fase 2";
            this.btnFase2.UseVisualStyleBackColor = true;
            this.btnFase2.Click += new System.EventHandler(this.btnFase2_Click);
            // 
            // btnFase3
            // 
            this.btnFase3.Location = new System.Drawing.Point(174, 12);
            this.btnFase3.Name = "btnFase3";
            this.btnFase3.Size = new System.Drawing.Size(75, 23);
            this.btnFase3.TabIndex = 17;
            this.btnFase3.Text = "Fase 3";
            this.btnFase3.UseVisualStyleBackColor = true;
            this.btnFase3.Click += new System.EventHandler(this.btnFase3_Click);
            // 
            // lblClientes
            // 
            this.lblClientes.AutoSize = true;
            this.lblClientes.Location = new System.Drawing.Point(12, 55);
            this.lblClientes.Name = "lblClientes";
            this.lblClientes.Size = new System.Drawing.Size(346, 13);
            this.lblClientes.TabIndex = 18;
            this.lblClientes.Text = "0 documentos de identificación en 172.20.1.23/SumaLealtad/CLIENTE";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 673);
            this.Controls.Add(this.lblClientes);
            this.Controls.Add(this.btnFase3);
            this.Controls.Add(this.btnFase2);
            this.Controls.Add(this.lblPorcentaje);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblFin);
            this.Controls.Add(this.lblInicio);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCedulas);
            this.Controls.Add(this.lblTotalMalos);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotalBuenos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstMalos);
            this.Controls.Add(this.lstBuenos);
            this.Controls.Add(this.btnProcesar);
            this.Name = "Form1";
            this.Text = "Carga Masiva Suma";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.ListBox lstBuenos;
        private System.Windows.Forms.ListBox lstMalos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalBuenos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTotalMalos;
        private System.Windows.Forms.Label lblCedulas;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblInicio;
        private System.Windows.Forms.Label lblFin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblPorcentaje;
        private System.Windows.Forms.Button btnFase2;
        private System.Windows.Forms.Button btnFase3;
        private System.Windows.Forms.Label lblClientes;
    }
}

