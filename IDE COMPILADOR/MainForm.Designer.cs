using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Reflection;

namespace IDE_COMPILADOR
{
    partial class MainForm : Form
    {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuStrip;
        private ToolStrip toolStrip1;
        private SplitContainer splitContainer;
        private Panel panelEditor;
        private Panel lineNumberPanel;
        private RichTextBox txtEditor;
        private Panel panelAnalysis;
        private TabControl tabAnalysis;
        private TabPage tabLexico;
        private TabPage tabSintactico;
        private TabPage tabSemantico;
        private TabPage tabHashTable;
        private TabPage tabCodigoIntermedio;
        private Panel panelFileExplorer;
        private FlowLayoutPanel panelFileExplorerButtons;
        private Button btnAgregarArchivo;
        private Button btnEliminarArchivo;
        private TreeView fileExplorer;
        private TabControl tabOutput;
        private Label lblStatus;
        private ToolTip toolTip1;

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
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new MenuStrip();
            this.toolStrip1 = new ToolStrip();
            this.splitContainer = new SplitContainer();
            this.panelEditor = new Panel();
            this.lineNumberPanel = new Panel();
            this.txtEditor = new RichTextBox();
            this.panelAnalysis = new Panel();
            this.tabAnalysis = new TabControl();
            this.tabLexico = new TabPage();
            this.tabSintactico = new TabPage();
            this.tabSemantico = new TabPage();
            this.tabHashTable = new TabPage();
            this.tabCodigoIntermedio = new TabPage();
            this.panelFileExplorer = new Panel();
            this.panelFileExplorerButtons = new FlowLayoutPanel();
            this.btnAgregarArchivo = new Button();
            this.btnEliminarArchivo = new Button();
            this.fileExplorer = new TreeView();
            this.tabOutput = new TabControl();
            this.lblStatus = new Label();
            this.toolTip1 = new ToolTip(this.components);

            // menuStrip
            this.menuStrip.Dock = DockStyle.Top;
            this.menuStrip.BackColor = Color.FromArgb(18, 18, 30);
            this.menuStrip.ForeColor = Color.White;
            this.menuStrip.Font = new Font("Segoe UI", 10F);

            // toolStrip1
            this.toolStrip1.Dock = DockStyle.Top;
            this.toolStrip1.BackColor = Color.FromArgb(28, 28, 48);
            this.toolStrip1.ForeColor = Color.White;
            this.toolStrip1.ImageScalingSize = new Size(24, 24);
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.Add(CreateToolStripButton("🚀", "Compilar"));
            this.toolStrip1.Items.Add(CreateToolStripButton("📂", "Abrir"));
            this.toolStrip1.Items.Add(CreateToolStripButton("💾", "Guardar"));

            // splitContainer
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Orientation = Orientation.Vertical;
            this.splitContainer.SplitterDistance = 600;
            this.splitContainer.BackColor = Color.Transparent;
            this.splitContainer.IsSplitterFixed = false;
            this.splitContainer.BorderStyle = BorderStyle.FixedSingle;
            this.splitContainer.SplitterWidth = 6;
            this.splitContainer.Cursor = Cursors.VSplit;

            // panelEditor
            this.panelEditor.Dock = DockStyle.Fill;
            this.panelEditor.BackColor = Color.Transparent;
            this.panelEditor.Padding = new Padding(5);
            this.panelEditor.Paint += PanelEditor_Paint;

            // lineNumberPanel
            this.lineNumberPanel.Dock = DockStyle.Left;
            this.lineNumberPanel.Width = 40;
            this.lineNumberPanel.BackColor = Color.FromArgb(50, 50, 70);

            // txtEditor
            this.txtEditor.Dock = DockStyle.Fill;
            this.txtEditor.Font = new Font("JetBrains Mono", 10F);
            this.txtEditor.AcceptsTab = true;
            this.txtEditor.WordWrap = false;
            this.txtEditor.BackColor = Color.FromArgb(20, 20, 20);
            this.txtEditor.ForeColor = Color.FromArgb(220, 220, 220);
            this.txtEditor.BorderStyle = BorderStyle.None;

            this.panelEditor.Controls.Add(this.txtEditor);
            this.panelEditor.Controls.Add(this.lineNumberPanel);

            // panelAnalysis
            this.panelAnalysis.Dock = DockStyle.Fill;
            this.panelAnalysis.BackColor = Color.FromArgb(30, 30, 50);

            // tabAnalysis
            this.tabAnalysis.Dock = DockStyle.Fill;
            this.tabAnalysis.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.tabAnalysis.Appearance = TabAppearance.FlatButtons;
            this.tabAnalysis.ItemSize = new Size(120, 30);
            this.tabAnalysis.SizeMode = TabSizeMode.Normal;
            this.tabAnalysis.Multiline = true;

            this.tabLexico.Text = "🧩 Léxico";
            this.tabSintactico.Text = "📐 Sintáctico";
            this.tabSemantico.Text = "🧠 Semántico";
            this.tabHashTable.Text = "🔑 Hash Table";
            this.tabCodigoIntermedio.Text = "💻 Código Intermedio";

            this.tabAnalysis.TabPages.AddRange(new TabPage[] {
                this.tabLexico, this.tabSintactico, this.tabSemantico, this.tabHashTable, this.tabCodigoIntermedio });

            this.panelAnalysis.Controls.Add(this.tabAnalysis);

            // splitContainer panels
            this.splitContainer.Panel1.Controls.Add(this.panelEditor);
            this.splitContainer.Panel2.Controls.Add(this.panelAnalysis);

            // panelFileExplorer
            this.panelFileExplorer.Dock = DockStyle.Right;
            this.panelFileExplorer.Width = 200;
            this.panelFileExplorer.BackColor = Color.FromArgb(40, 40, 60);

            // panelFileExplorerButtons
            this.panelFileExplorerButtons.Dock = DockStyle.Top;
            this.panelFileExplorerButtons.Height = 40;
            this.panelFileExplorerButtons.FlowDirection = FlowDirection.LeftToRight;
            this.panelFileExplorerButtons.Padding = new Padding(5);

            ConfigureModernButton(this.btnAgregarArchivo, "📄");
            ConfigureModernButton(this.btnEliminarArchivo, "❌");
            this.panelFileExplorerButtons.Controls.Add(this.btnAgregarArchivo);
            this.panelFileExplorerButtons.Controls.Add(this.btnEliminarArchivo);

            // fileExplorer
            this.fileExplorer.Dock = DockStyle.Fill;
            this.fileExplorer.BackColor = Color.FromArgb(30, 30, 45);
            this.fileExplorer.ForeColor = Color.White;

            this.panelFileExplorer.Controls.Add(this.fileExplorer);
            this.panelFileExplorer.Controls.Add(this.panelFileExplorerButtons);

            // tabOutput
            this.tabOutput.Dock = DockStyle.Bottom;
            this.tabOutput.Height = 150;
            this.tabOutput.BackColor = Color.FromArgb(15, 15, 25);
            this.tabOutput.ForeColor = Color.LightGreen;

            // lblStatus
            this.lblStatus.Dock = DockStyle.Bottom;
            this.lblStatus.Height = 20;
            this.lblStatus.ForeColor = Color.White;
            this.lblStatus.BackColor = Color.FromArgb(25, 25, 35);

            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelFileExplorer);
            this.Controls.Add(this.tabOutput);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip);

            this.ClientSize = new Size(1200, 600);
            this.Name = "MainForm";
            this.Text = "Compilador ";
            this.WindowState = FormWindowState.Maximized;
        }

        private void ConfigureModernButton(Button button, string text)
        {
            button.Text = text;
            button.FlatStyle = FlatStyle.Flat;
            button.ForeColor = Color.White;
            button.BackColor = Color.FromArgb(60, 60, 90);
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.AutoSize = true;
            button.Margin = new Padding(5);
            button.Cursor = Cursors.Hand;

            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(80, 80, 120);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(60, 60, 90);
        }

        private ToolStripButton CreateToolStripButton(string icon, string tooltip)
        {
            return new ToolStripButton
            {
                Text = icon,
                ToolTipText = tooltip,
                Font = new Font("Segoe UI", 14F),
                ForeColor = Color.White,
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                AutoSize = false,
                Width = 40,
                Height = 40
            };
        }

        private void PanelEditor_Paint(object sender, PaintEventArgs e)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddRectangle(panelEditor.ClientRectangle);
                using (Pen pen = new Pen(Color.FromArgb(100, 100, 150), 2))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
