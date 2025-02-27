namespace IDE_COMPILADOR
{
    partial class MainForm : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        private System.Windows.Forms.RichTextBox txtEditor;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.Panel lineNumberPanel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.TreeView fileExplorer;
        private System.Windows.Forms.Panel panelFileExplorer;
        private System.Windows.Forms.Button btnAgregarArchivo;
        private System.Windows.Forms.Button btnEliminarArchivo;

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
            menuStrip = new MenuStrip();
            lineNumberPanel = new Panel();
            txtEditor = new RichTextBox();
            lblStatus = new Label();
            txtOutput = new RichTextBox();
            panelFileExplorer = new Panel();
            btnAgregarArchivo = new Button();
            btnEliminarArchivo = new Button();
            fileExplorer = new TreeView();
            panelFileExplorer.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1153, 24);
            menuStrip.TabIndex = 0;
            // 
            // lineNumberPanel
            // 
            lineNumberPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lineNumberPanel.BackColor = Color.LightGray;
            lineNumberPanel.Location = new Point(0, 60);
            lineNumberPanel.Name = "lineNumberPanel";
            lineNumberPanel.Size = new Size(50, 195);
            lineNumberPanel.TabIndex = 1;
            lineNumberPanel.Paint += LineNumberPanel_Paint;
            // 
            // txtEditor
            // 
            txtEditor.AcceptsTab = true;
            txtEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtEditor.Font = new Font("Consolas", 10F);
            txtEditor.Location = new Point(50, 60);
            txtEditor.Name = "txtEditor";
            txtEditor.Size = new Size(282, 195);
            txtEditor.TabIndex = 2;
            txtEditor.Text = "";
            txtEditor.WordWrap = false;
            txtEditor.VScroll += TxtEditor_VScroll;
            txtEditor.TextChanged += TxtEditor_TextChanged;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Location = new Point(0, 510);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1153, 20);
            lblStatus.TabIndex = 3;
            // 
            // txtOutput
            // 
            txtOutput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOutput.BackColor = Color.Black;
            txtOutput.Font = new Font("Consolas", 10F);
            txtOutput.ForeColor = Color.White;
            txtOutput.Location = new Point(0, 261);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtOutput.Size = new Size(282, 92);
            txtOutput.TabIndex = 4;
            txtOutput.Text = "";
            // 
            // panelFileExplorer
            // 
            panelFileExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panelFileExplorer.BackColor = Color.LightSteelBlue;
            panelFileExplorer.Controls.Add(btnAgregarArchivo);
            panelFileExplorer.Controls.Add(btnEliminarArchivo);
            panelFileExplorer.Controls.Add(fileExplorer);
            panelFileExplorer.Location = new Point(282, 24);
            panelFileExplorer.Name = "panelFileExplorer";
            panelFileExplorer.Size = new Size(200, 253);
            panelFileExplorer.TabIndex = 5;
            // 
            // btnAgregarArchivo
            // 
            btnAgregarArchivo.AutoSize = true;
            btnAgregarArchivo.Location = new Point(0, 0);
            btnAgregarArchivo.Name = "btnAgregarArchivo";
            btnAgregarArchivo.Size = new Size(127, 30);
            btnAgregarArchivo.TabIndex = 0;
            btnAgregarArchivo.Text = "Agregar Archivo";
            btnAgregarArchivo.Click += BtnAgregarArchivo_Click;
            // 
            // btnEliminarArchivo
            // 
            btnEliminarArchivo.AutoSize = true;
            btnEliminarArchivo.Location = new Point(110, 0);
            btnEliminarArchivo.Name = "btnEliminarArchivo";
            btnEliminarArchivo.Size = new Size(127, 30);
            btnEliminarArchivo.TabIndex = 1;
            btnEliminarArchivo.Text = "Eliminar Archivo";
            btnEliminarArchivo.Click += BtnEliminarArchivo_Click;
            // 
            // fileExplorer
            // 
            fileExplorer.Dock = DockStyle.Fill;
            fileExplorer.Location = new Point(0, 0);
            fileExplorer.Name = "fileExplorer";
            fileExplorer.Size = new Size(200, 253);
            fileExplorer.TabIndex = 2;
            fileExplorer.NodeMouseDoubleClick += FileExplorer_NodeMouseDoubleClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1153, 530);
            Controls.Add(menuStrip);
            Controls.Add(lineNumberPanel);
            Controls.Add(txtEditor);
            Controls.Add(lblStatus);
            Controls.Add(txtOutput);
            Controls.Add(panelFileExplorer);
            Name = "MainForm";
            Text = "Compilador";
            WindowState = FormWindowState.Maximized;
            Resize += MainForm_Resize;
            panelFileExplorer.ResumeLayout(false);
            panelFileExplorer.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion
    }
}
