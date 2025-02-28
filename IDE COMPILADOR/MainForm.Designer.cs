namespace IDE_COMPILADOR
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controles del formulario
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panelMain;

        // Editor de texto
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.Panel lineNumberPanel;
        private System.Windows.Forms.RichTextBox txtEditor;

        // Panel central de análisis
        private System.Windows.Forms.Panel panelAnalysis;
        private System.Windows.Forms.TabControl tabAnalysis;
        private System.Windows.Forms.TabPage tabLexico;
        private System.Windows.Forms.TabPage tabSintactico;
        private System.Windows.Forms.TabPage tabSemantico;
        private System.Windows.Forms.TabPage tabHashTable;
        private System.Windows.Forms.TabPage tabCodigoIntermedio;

        // Explorador de archivos
        private System.Windows.Forms.Panel panelFileExplorer;
        private System.Windows.Forms.FlowLayoutPanel panelFileExplorerButtons;
        private System.Windows.Forms.Button btnAgregarArchivo;
        private System.Windows.Forms.Button btnEliminarArchivo;
        private System.Windows.Forms.TreeView fileExplorer;

        // Área inferior (TabControl) para resultados
        private System.Windows.Forms.TabControl tabOutput;
        private System.Windows.Forms.Label lblStatus;

        // ToolTip para mostrar textos al pasar el cursor
        private System.Windows.Forms.ToolTip toolTip1;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si se deben desechar los recursos administrados; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador.
        /// No modificar el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panelMain = new System.Windows.Forms.Panel();

            this.panelEditor = new System.Windows.Forms.Panel();
            this.lineNumberPanel = new System.Windows.Forms.Panel();
            this.txtEditor = new System.Windows.Forms.RichTextBox();

            this.panelAnalysis = new System.Windows.Forms.Panel();
            this.tabAnalysis = new System.Windows.Forms.TabControl();
            this.tabLexico = new System.Windows.Forms.TabPage();
            this.tabSintactico = new System.Windows.Forms.TabPage();
            this.tabSemantico = new System.Windows.Forms.TabPage();
            this.tabHashTable = new System.Windows.Forms.TabPage();
            this.tabCodigoIntermedio = new System.Windows.Forms.TabPage();

            this.panelFileExplorer = new System.Windows.Forms.Panel();
            this.panelFileExplorerButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregarArchivo = new System.Windows.Forms.Button();
            this.btnEliminarArchivo = new System.Windows.Forms.Button();
            this.fileExplorer = new System.Windows.Forms.TreeView();

            this.tabOutput = new System.Windows.Forms.TabControl();
            this.lblStatus = new System.Windows.Forms.Label();

            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);

            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip.TabIndex = 0;

            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1200, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";

            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 55);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1200, 345);
            this.panelMain.TabIndex = 2;

            // 
            // panelEditor
            // 
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Size = new System.Drawing.Size(600, 345);
            this.panelEditor.TabIndex = 0;

            // 
            // lineNumberPanel
            // 
            this.lineNumberPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.lineNumberPanel.Width = 40;
            this.lineNumberPanel.BackColor = System.Drawing.Color.LightGray;

            // 
            // txtEditor
            // 
            this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditor.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtEditor.AcceptsTab = true;
            this.txtEditor.WordWrap = false;
            this.txtEditor.Location = new System.Drawing.Point(40, 0);
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.Size = new System.Drawing.Size(560, 345);
            this.txtEditor.TabIndex = 0;
            this.txtEditor.Text = "";

            // Se añaden txtEditor y lineNumberPanel al panelEditor
            this.panelEditor.Controls.Add(this.txtEditor);
            this.panelEditor.Controls.Add(this.lineNumberPanel);

            // 
            // panelAnalysis
            // 
            this.panelAnalysis.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelAnalysis.Width = 400;
            this.panelAnalysis.Name = "panelAnalysis";
            this.panelAnalysis.BackColor = System.Drawing.Color.DimGray;

            // 
            // tabAnalysis
            // 
            this.tabAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAnalysis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tabAnalysis.Name = "tabAnalysis";

            this.tabLexico.Text = "Léxico";
            this.tabSintactico.Text = "Sintáctico";
            this.tabSemantico.Text = "Semántico";
            this.tabHashTable.Text = "Hash Table";
            this.tabCodigoIntermedio.Text = "Código Intermedio";

            // Se agregan las pestañas al TabControl
            this.tabAnalysis.TabPages.Add(this.tabLexico);
            this.tabAnalysis.TabPages.Add(this.tabSintactico);
            this.tabAnalysis.TabPages.Add(this.tabSemantico);
            this.tabAnalysis.TabPages.Add(this.tabHashTable);
            this.tabAnalysis.TabPages.Add(this.tabCodigoIntermedio);

            // Añadimos el TabControl al panelAnalysis
            this.panelAnalysis.Controls.Add(this.tabAnalysis);

            // 
            // panelFileExplorer
            // 
            this.panelFileExplorer.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelFileExplorer.Width = 200;
            this.panelFileExplorer.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelFileExplorer.Name = "panelFileExplorer";

            // 
            // panelFileExplorerButtons
            // 
            this.panelFileExplorerButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFileExplorerButtons.Height = 40;
            this.panelFileExplorerButtons.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelFileExplorerButtons.Padding = new System.Windows.Forms.Padding(5);

            // 
            // btnAgregarArchivo
            // 
            this.btnAgregarArchivo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregarArchivo.Text = "";
            this.btnAgregarArchivo.AutoSize = true;
            this.btnAgregarArchivo.Margin = new System.Windows.Forms.Padding(5);

            // 
            // btnEliminarArchivo
            // 
            this.btnEliminarArchivo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminarArchivo.Text = "";
            this.btnEliminarArchivo.AutoSize = true;
            this.btnEliminarArchivo.Margin = new System.Windows.Forms.Padding(5);

            // Se añaden los botones al panel
            this.panelFileExplorerButtons.Controls.Add(this.btnAgregarArchivo);
            this.panelFileExplorerButtons.Controls.Add(this.btnEliminarArchivo);

            // 
            // fileExplorer
            // 
            this.fileExplorer.Dock = System.Windows.Forms.DockStyle.Fill;

            // Se añaden el TreeView y el panel de botones al panelFileExplorer
            this.panelFileExplorer.Controls.Add(this.fileExplorer);
            this.panelFileExplorer.Controls.Add(this.panelFileExplorerButtons);

            // 
            // tabOutput
            // 
            this.tabOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabOutput.Height = 150;
            this.tabOutput.BackColor = System.Drawing.Color.Black;

            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Height = 20;

            // 
            // toolTip1
            // 
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);

            // 
            // Orden de adición de paneles a panelMain
            // 
            this.panelMain.Controls.Add(this.panelEditor);
            this.panelMain.Controls.Add(this.panelAnalysis);
            this.panelMain.Controls.Add(this.panelFileExplorer);

            // 
            // Se agregan panelMain, tabOutput, lblStatus, toolStrip1 y menuStrip al formulario
            // 
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.tabOutput);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip);

            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Name = "MainForm";
            this.Text = "Compilador";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        #endregion
    }
}
