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

        public MainForm()
        {
            InitializeComponent();
            InitializeMenu();
            txtEditor.SelectionChanged += TxtEditor_SelectionChanged;
        }
        private void InitializeMenu()
        {
            Image openIcon = SystemIcons.Application.ToBitmap();  // Ícono genérico para "Open"
            Image saveIcon = SystemIcons.Information.ToBitmap();  // Alternativa para "Save"
            Image saveAsIcon = SystemIcons.Warning.ToBitmap();    // Alternativa para "Save As"

            // ---------- MENÚ SUPERIOR (MenuStrip) ---------- //
            menuStrip.Dock = DockStyle.Top;  // Asegura que el menú quede arriba

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");

            ToolStripMenuItem openItem = new ToolStripMenuItem("Open", openIcon, (s, e) => OpenFile());
            ToolStripMenuItem saveItem = new ToolStripMenuItem("Save", saveIcon, (s, e) => SaveFile());
            ToolStripMenuItem saveAsItem = new ToolStripMenuItem("Save As", saveAsIcon, (s, e) => SaveFileAs());

            fileMenu.DropDownItems.Add(openItem);
            fileMenu.DropDownItems.Add(saveItem);
            fileMenu.DropDownItems.Add(saveAsItem);
            menuStrip.Items.Add(fileMenu);

            // Menú Compilar (Compile)
            ToolStripMenuItem compileMenu = new ToolStripMenuItem("Compile");
            compileMenu.DropDownItems.Add("Lexical Analysis", null, (s, e) => CompilePhase("Lexical"));
            compileMenu.DropDownItems.Add("Syntax Analysis", null, (s, e) => CompilePhase("Syntax"));
            compileMenu.DropDownItems.Add("Semantic Analysis", null, (s, e) => CompilePhase("Semantic"));
            compileMenu.DropDownItems.Add("Intermediate Code", null, (s, e) => CompilePhase("Intermediate"));
            compileMenu.DropDownItems.Add("Execution", null, (s, e) => CompilePhase("Execution"));
            menuStrip.Items.Add(compileMenu);

            // Agregar el menú al formulario
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;

            // ---------- BARRA DE HERRAMIENTAS (ToolStrip) ---------- //
            ToolStrip toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.None;  // NO usar DockStyle.Top, porque lo sobrepone al menuStrip
            toolStrip.Location = new Point(0, menuStrip.Height); // Se coloca justo debajo del menú
            toolStrip.ImageScalingSize = new Size(24, 24); // Tamaño de los iconos

            // Crear botones del ToolStrip
            ToolStripButton openButton = new ToolStripButton();
            openButton.Image = openIcon;
            openButton.ToolTipText = "Open File";
            openButton.Click += (s, e) => OpenFile();

            ToolStripButton saveButton = new ToolStripButton();
            saveButton.Image = saveIcon;
            saveButton.ToolTipText = "Save File";
            saveButton.Click += (s, e) => SaveFile();

            ToolStripButton saveAsButton = new ToolStripButton();
            saveAsButton.Image = saveAsIcon;
            saveAsButton.ToolTipText = "Save As";
            saveAsButton.Click += (s, e) => SaveFileAs();

            // Agregar botones al ToolStrip
            toolStrip.Items.Add(openButton);
            toolStrip.Items.Add(saveButton);
            toolStrip.Items.Add(saveAsButton);

            // Agregar la barra de herramientas al formulario
            this.Controls.Add(toolStrip);
        }


        private void TxtEditor_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLineColumn();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Evita que el código se ejecute en el diseñador
            if (DesignMode) return;

            int outputHeight = 100;

            if (panelFileExplorer != null)
            {
                panelFileExplorer.Size = new Size(200, this.ClientSize.Height - menuStrip.Height - lblStatus.Height - outputHeight);
                panelFileExplorer.Location = new Point(this.ClientSize.Width - panelFileExplorer.Width, menuStrip.Height);
            }

            int editorWidth = this.ClientSize.Width - lineNumberPanel.Width - (panelFileExplorer != null ? panelFileExplorer.Width : 0);
            int editorHeight = this.ClientSize.Height - menuStrip.Height - lblStatus.Height - outputHeight;
            txtEditor.Size = new Size(editorWidth, editorHeight);
            lineNumberPanel.Height = editorHeight;

            txtOutput.Size = new Size(this.ClientSize.Width, outputHeight);
            txtOutput.Location = new Point(0, this.ClientSize.Height - outputHeight);
        }


        /*private void InitializeMenu()
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
            // Panel para números de línea
            lineNumberPanel = new Panel();
            lineNumberPanel.Width = 50;
            lineNumberPanel.Dock = DockStyle.None;
            lineNumberPanel.Location = new Point(0, menuStrip.Height);
            lineNumberPanel.Height = this.ClientSize.Height - menuStrip.Height - (lblStatus?.Height ?? 20);
            lineNumberPanel.BackColor = Color.LightGray;
            lineNumberPanel.Paint += LineNumberPanel_Paint;
            this.Controls.Add(lineNumberPanel);

            // Editor de texto
            txtEditor = new RichTextBox();
            txtEditor.Multiline = true;
            txtEditor.ScrollBars = RichTextBoxScrollBars.Both;
            txtEditor.WordWrap = false;
            txtEditor.AcceptsTab = true;
            txtEditor.Dock = DockStyle.None;
            // Se descuenta el ancho del panel lateral (200) para que no se solapen
            int editorWidth = this.ClientSize.Width - lineNumberPanel.Width - 200;
            int editorHeight = this.ClientSize.Height - menuStrip.Height - (lblStatus?.Height ?? 20) - 100; // 100 es la altura del panel de salida
            txtEditor.Location = new Point(lineNumberPanel.Width, menuStrip.Height);
            txtEditor.Size = new Size(editorWidth, editorHeight);
            txtEditor.Font = new Font("Consolas", 10);
            txtEditor.TextChanged += TxtEditor_TextChanged;
            txtEditor.VScroll += TxtEditor_VScroll;
            this.Controls.Add(txtEditor);

            // Label de estado
            lblStatus = new Label();
            lblStatus.Height = 20;
            lblStatus.Dock = DockStyle.Bottom;
            this.Controls.Add(lblStatus);

            // RichTextBox para la salida
            txtOutput = new RichTextBox();
            txtOutput.Multiline = true;
            txtOutput.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtOutput.WordWrap = false;
            txtOutput.ReadOnly = true;
            txtOutput.Dock = DockStyle.None;
            txtOutput.Location = new Point(0, this.ClientSize.Height - 100);
            txtOutput.Size = new Size(this.ClientSize.Width, 100);
            txtOutput.Font = new Font("Consolas", 10);
            txtOutput.BackColor = Color.Black;
            txtOutput.ForeColor = Color.White;
            this.Controls.Add(txtOutput);
        }

        private void InitializeFileExplorer()
        {
            // Panel lateral derecho para el explorador de archivos
            panelFileExplorer = new Panel();
            panelFileExplorer.Width = 200;
            panelFileExplorer.Height = this.ClientSize.Height - menuStrip.Height - lblStatus.Height - 100;
            panelFileExplorer.Location = new Point(this.ClientSize.Width - panelFileExplorer.Width, menuStrip.Height);
            panelFileExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panelFileExplorer.BackColor = Color.LightSteelBlue;
            this.Controls.Add(panelFileExplorer);

            // Panel para los botones del explorador
            Panel panelButtons = new Panel();
            panelButtons.Height = 30;
            panelButtons.Dock = DockStyle.Top;
            panelFileExplorer.Controls.Add(panelButtons);

            // Botón para agregar archivos
            btnAgregarArchivo = new Button();
            btnAgregarArchivo.Text = "Agregar Archivo";
            btnAgregarArchivo.AutoSize = true;
            btnAgregarArchivo.Click += BtnAgregarArchivo_Click;
            panelButtons.Controls.Add(btnAgregarArchivo);

            // Botón para eliminar archivos
            Button btnEliminarArchivo = new Button();
            btnEliminarArchivo.Text = "Eliminar Archivo";
            btnEliminarArchivo.AutoSize = true;
            btnEliminarArchivo.Click += BtnEliminarArchivo_Click;
            // Ubicar el botón junto al de "Agregar Archivo"
            btnEliminarArchivo.Location = new Point(btnAgregarArchivo.Width + 5, 0);
            panelButtons.Controls.Add(btnEliminarArchivo);

            // TreeView para mostrar los archivos agregados
            fileExplorer = new TreeView();
            fileExplorer.Dock = DockStyle.Fill;
            fileExplorer.NodeMouseDoubleClick += FileExplorer_NodeMouseDoubleClick;
            panelFileExplorer.Controls.Add(fileExplorer);
        }*/
        private void BtnEliminarArchivo_Click(object sender, EventArgs e)
        {
            // Obtener el nodo seleccionado del TreeView
            TreeNode selectedNode = fileExplorer.SelectedNode;
            if (selectedNode != null)
            {
                // Preguntar al usuario si está seguro de eliminar el archivo del explorador
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este archivo del explorador?",
                                                          "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Guardar la ruta del archivo antes de eliminar el nodo
                    string filePathToDelete = selectedNode.Tag as string;
                    // Si el archivo eliminado es el que está cargado en el editor, lo limpiamos
                    if (!string.IsNullOrEmpty(filePathToDelete) && filePathToDelete == currentFilePath)
                    {
                        txtEditor.Clear();
                        currentFilePath = string.Empty;
                    }
                    fileExplorer.Nodes.Remove(selectedNode);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un archivo para eliminar.", "Eliminar Archivo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAgregarArchivo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    // Agregar el archivo al TreeView si no existe ya
                    if (!FileNodeExists(filePath))
                    {
                        TreeNode node = new TreeNode(Path.GetFileName(filePath));
                        node.Tag = filePath;
                        fileExplorer.Nodes.Add(node);
                    }
                }
            }
        }

        private bool FileNodeExists(string filePath)
        {
            foreach (TreeNode node in fileExplorer.Nodes)
            {
                if (node.Tag != null && node.Tag.ToString() == filePath)
                    return true;
            }
            return false;
        }

        private void FileExplorer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                string filePath = e.Node.Tag.ToString();
                if (File.Exists(filePath))
                {
                    txtEditor.Text = File.ReadAllText(filePath);
                    currentFilePath = filePath;
                }
                else
                {
                    MessageBox.Show("El archivo no existe.");
                }
            }
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
            if (txtEditor == null || lblStatus == null)
                return;

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
