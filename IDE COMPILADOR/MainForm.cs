using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace IDE_COMPILADOR
{
    public partial class MainForm : Form
    {
        private string currentFilePath = string.Empty;
        private RichTextBox txtEditor;
        private RichTextBox txtOutput; // Nuevo RichTextBox para la salida
        private Panel lineNumberPanel;
        private Label lblStatus;
        private MenuStrip menuStrip;
        private TreeView fileExplorer;
        private SplitContainer splitContainer;

        public MainForm()
        {
            InitializeComponent();
            InitializeMenu();
            InitializeEditor();
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.WindowState = FormWindowState.Maximized;
            this.Resize += new EventHandler(MainForm_Resize);
            this.Text = "Compilador";
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            int outputHeight = 100; // Altura del panel de salida
            txtEditor.Size = new Size(this.ClientSize.Width - lineNumberPanel.Width, this.ClientSize.Height - menuStrip.Height - lblStatus.Height - outputHeight);
            lineNumberPanel.Height = this.ClientSize.Height - menuStrip.Height - lblStatus.Height - outputHeight;
            txtOutput.Size = new Size(this.ClientSize.Width, outputHeight);
            txtOutput.Location = new Point(0, this.ClientSize.Height - outputHeight);
        }

        private void InitializeMenu()
        {
            menuStrip = new MenuStrip();
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Open", null, (s, e) => OpenFile());
            fileMenu.DropDownItems.Add("Save", null, (s, e) => SaveFile());
            fileMenu.DropDownItems.Add("Save As", null, (s, e) => SaveFileAs());
            menuStrip.Items.Add(fileMenu);

            ToolStripMenuItem compileMenu = new ToolStripMenuItem("Compile");
            compileMenu.DropDownItems.Add("Lexical Analysis", null, (s, e) => CompilePhase("Lexical"));
            compileMenu.DropDownItems.Add("Syntax Analysis", null, (s, e) => CompilePhase("Syntax"));
            compileMenu.DropDownItems.Add("Semantic Analysis", null, (s, e) => CompilePhase("Semantic"));
            compileMenu.DropDownItems.Add("Intermediate Code", null, (s, e) => CompilePhase("Intermediate"));
            compileMenu.DropDownItems.Add("Execution", null, (s, e) => CompilePhase("Execution"));
            menuStrip.Items.Add(compileMenu);
        }

        private void InitializeEditor()
        {
            lineNumberPanel = new Panel();
            lineNumberPanel.Width = 50;
            lineNumberPanel.Dock = DockStyle.None;
            lineNumberPanel.Location = new Point(0, menuStrip.Height);
            lineNumberPanel.Height = this.ClientSize.Height - menuStrip.Height - (lblStatus?.Height ?? 20);
            lineNumberPanel.BackColor = Color.LightGray;
            lineNumberPanel.Paint += LineNumberPanel_Paint;
            this.Controls.Add(lineNumberPanel);

            txtEditor = new RichTextBox();
            txtEditor.Multiline = true;
            txtEditor.ScrollBars = RichTextBoxScrollBars.Both;
            txtEditor.WordWrap = false;
            txtEditor.AcceptsTab = true;
            txtEditor.Dock = DockStyle.None;
            txtEditor.Location = new Point(lineNumberPanel.Width, menuStrip.Height);
            txtEditor.Size = new Size(this.ClientSize.Width - lineNumberPanel.Width, this.ClientSize.Height - menuStrip.Height - (lblStatus?.Height ?? 20));
            txtEditor.Font = new Font("Consolas", 10);
            txtEditor.TextChanged += TxtEditor_TextChanged;
            txtEditor.VScroll += TxtEditor_VScroll;
            this.Controls.Add(txtEditor);

            lblStatus = new Label();
            lblStatus.Height = 20;
            lblStatus.Dock = DockStyle.Bottom;
            this.Controls.Add(lblStatus);

            // Inicializar el nuevo RichTextBox para la salida
            txtOutput = new RichTextBox();
            txtOutput.Multiline = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtOutput.WordWrap = false;
            txtOutput.ReadOnly = true;
            txtOutput.Dock = DockStyle.None;
            txtOutput.Location = new Point(0, this.ClientSize.Height - 100); // Ajustar la ubicación
            txtOutput.Size = new Size(this.ClientSize.Width, 100); // Ajustar el tamaño
            txtOutput.Font = new Font("Consolas", 10);
            txtOutput.BackColor = Color.Black;
            txtOutput.ForeColor = Color.White;
            this.Controls.Add(txtOutput);
        }

        private void TxtEditor_TextChanged(object sender, EventArgs e)
        {
            UpdateLineColumn();
            lineNumberPanel.Invalidate();
        }

        private void TxtEditor_VScroll(object sender, EventArgs e)
        {
            lineNumberPanel.Invalidate();
        }

        private void LineNumberPanel_Paint(object sender, PaintEventArgs e)
        {
            int firstIndex = txtEditor.GetCharIndexFromPosition(new Point(0, 0));
            int firstLine = txtEditor.GetLineFromCharIndex(firstIndex);
            int lastIndex = txtEditor.GetCharIndexFromPosition(new Point(0, txtEditor.Height));
            int lastLine = txtEditor.GetLineFromCharIndex(lastIndex);

            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (Font font = new Font("Consolas", 10))
            {
                for (int i = firstLine, y = 0; i <= lastLine; i++, y += 20)
                {
                    e.Graphics.DrawString((i + 1).ToString(), font, brush, new PointF(5, y));
                }
            }
        }

        private void UpdateLineColumn()
        {
            int line = txtEditor.GetLineFromCharIndex(txtEditor.SelectionStart) + 1;
            int column = txtEditor.SelectionStart - txtEditor.GetFirstCharIndexOfCurrentLine() + 1;
            lblStatus.Text = $"Línea: {line}, Columna: {column}";
        }

        private void CompilePhase(string phase)
        {
            MessageBox.Show($"Ejecutando {phase} analysis...");
        }

        private void OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = openFileDialog.FileName;
                    txtEditor.Text = File.ReadAllText(currentFilePath);
                }
            }
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileAs();
            }
            else
            {
                File.WriteAllText(currentFilePath, txtEditor.Text);
            }
        }

        private void SaveFileAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                    File.WriteAllText(currentFilePath, txtEditor.Text);
                }
            }
        }
    }
}
