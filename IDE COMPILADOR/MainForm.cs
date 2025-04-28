using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using IDE_COMPILADOR.AnalizadorLexico; 


namespace IDE_COMPILADOR
{
    public partial class MainForm : Form
    {
        private string currentFilePath = string.Empty;

        public MainForm()
        {
            InitializeComponent();

            // Texto inicial en el editor:
            txtEditor.Text = "Escriba aqu�...";

            // Eventos del editor
            txtEditor.SelectionChanged += TxtEditor_SelectionChanged;
            txtEditor.TextChanged += TxtEditor_TextChanged;
            txtEditor.VScroll += TxtEditor_VScroll;

            // Evento de panel de n�meros de l�nea
            lineNumberPanel.Paint += LineNumberPanel_Paint;

            // Eventos de explorador de archivos
            btnAgregarArchivo.Click += BtnAgregarArchivo_Click;
            btnEliminarArchivo.Click += BtnEliminarArchivo_Click;
            fileExplorer.NodeMouseDoubleClick += FileExplorer_NodeMouseDoubleClick;

            // Inicializaciones gr�ficas y l�gicas
            // (Opcional: Si deseas mantener el men�, d�jalo; de lo contrario, puedes comentarlo)
            InicializarMenuPersonalizado();

            // ToolStrip: Agregamos los botones con �conos, tooltips y el nuevo bot�n "New Project"
            InicializarToolStrip();

            // Moderniza botones del explorador
            ModernizarBotonesFileExplorer();

            // TabControl inferior para errores/resultados
            InicializarTabOutput();
        }

        #region Men� (Opcional)

        private void InicializarMenuPersonalizado()
        {
            // Limpiamos cualquier �tem existente
            menuStrip.Items.Clear();

            // �conos de ejemplo
            Image openIcon = SystemIcons.Application.ToBitmap();
            Image saveIcon = SystemIcons.Information.ToBitmap();
            Image saveAsIcon = SystemIcons.Warning.ToBitmap();

            // Men� "File"
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Open", openIcon, (s, e) => OpenFile()));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Save", saveIcon, (s, e) => SaveFile()));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Save As", saveAsIcon, (s, e) => SaveFileAs()));

            // �conos para el men� "Compile"
            Image lexicalIcon = SystemIcons.Information.ToBitmap();
            Image syntaxIcon = SystemIcons.Question.ToBitmap();
            Image semanticIcon = SystemIcons.Shield.ToBitmap();
            Image intermediateIcon = SystemIcons.WinLogo.ToBitmap();
            Image executionIcon = SystemIcons.Exclamation.ToBitmap();

            // Men� "Compile"
            ToolStripMenuItem compileMenu = new ToolStripMenuItem("Compile");
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Lexical Analysis", lexicalIcon, (s, e) => EjecutarFase("Lexical Analysis")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Syntax Analysis", syntaxIcon, (s, e) => EjecutarFase("Syntax Analysis")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Semantic Analysis", semanticIcon, (s, e) => EjecutarFase("Semantic Analysis")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Intermediate Code", intermediateIcon, (s, e) => EjecutarFase("Intermediate Code")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Execution", executionIcon, (s, e) => EjecutarFase("Execution")));

            // Agregamos ambos men�s al menuStrip
            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(compileMenu);
        }

        #endregion

        #region ToolStrip con botones con �conos y ToolTip

        private void InicializarToolStrip()
        {
            // Limpiamos el toolStrip existente
            toolStrip1.Items.Clear();

            // --- Bot�n "New Project" para limpiar/abrir otro proyecto ---
            ToolStripButton newProjectButton = new ToolStripButton();
            newProjectButton.Image = SystemIcons.Application.ToBitmap(); // Puedes reemplazarlo por otro �cono personalizado
            newProjectButton.ToolTipText = "New Project";
            newProjectButton.Click += (s, e) =>
            {
                // Abre una nueva instancia de MainForm (nuevo proyecto)
                MainForm newForm = new MainForm();
                newForm.Show();
            };
            // Insertamos el bot�n al inicio del ToolStrip
            toolStrip1.Items.Insert(0, newProjectButton);

            // Bot�n "Edit"
            ToolStripButton editButton = new ToolStripButton();
            editButton.Image = SystemIcons.Information.ToBitmap();
            editButton.ToolTipText = "Edit";
            editButton.Click += (s, e) => { /* Aqu� podr�as implementar l�gica de edici�n (Deshacer, Rehacer, etc.) */ };
            toolStrip1.Items.Add(editButton);

            // Bot�n "Build and Debug"
            ToolStripButton buildDebugButton = new ToolStripButton();
            buildDebugButton.Image = SystemIcons.Shield.ToBitmap();
            buildDebugButton.ToolTipText = "Build and Debug";
            buildDebugButton.Click += (s, e) => { /* L�gica de compilaci�n y depuraci�n */ };
            toolStrip1.Items.Add(buildDebugButton);

            // Bot�n "Open File"
            ToolStripButton openFileButton = new ToolStripButton();
            openFileButton.Image = SystemIcons.WinLogo.ToBitmap();
            openFileButton.ToolTipText = "Open File";
            openFileButton.Click += (s, e) => OpenFile();
            toolStrip1.Items.Add(openFileButton);

            // Bot�n "Close" (Cerrar ventana)
            ToolStripButton closeButton = new ToolStripButton();
            closeButton.Image = SystemIcons.Error.ToBitmap();
            closeButton.ToolTipText = "Close Window";
            closeButton.Click += (s, e) => this.Close();
            toolStrip1.Items.Add(closeButton);

            // Bot�n "L�xico" (Lexical Analysis)
            ToolStripButton lexicoButton = new ToolStripButton();
            lexicoButton.Image = SystemIcons.Information.ToBitmap();
            lexicoButton.ToolTipText = "Lexical Analysis";
            lexicoButton.Click += (s, e) => EjecutarFase("Lexical Analysis");
            toolStrip1.Items.Add(lexicoButton);

            // Bot�n "Sint�ctico" (Syntax Analysis)
            ToolStripButton sintacticoButton = new ToolStripButton();
            sintacticoButton.Image = SystemIcons.Question.ToBitmap();
            sintacticoButton.ToolTipText = "Syntax Analysis";
            sintacticoButton.Click += (s, e) => EjecutarFase("Syntax Analysis");
            toolStrip1.Items.Add(sintacticoButton);

            // Bot�n "Sem�ntico" (Semantic Analysis)
            ToolStripButton semanticoButton = new ToolStripButton();
            semanticoButton.Image = SystemIcons.Shield.ToBitmap();
            semanticoButton.ToolTipText = "Semantic Analysis";
            semanticoButton.Click += (s, e) => EjecutarFase("Semantic Analysis");
            toolStrip1.Items.Add(semanticoButton);

            // Bot�n "Compilar" (Compile)
            ToolStripButton compilarButton = new ToolStripButton();
            compilarButton.Image = SystemIcons.Exclamation.ToBitmap();
            compilarButton.ToolTipText = "Compile";
            compilarButton.Click += (s, e) => EjecutarFase("Intermediate Code");
            toolStrip1.Items.Add(compilarButton);
        }

        #endregion

        #region Botones del Explorador de Archivos

        private void ModernizarBotonesFileExplorer()
        {
            // Ajustes para "Agregar Archivo"
            btnAgregarArchivo.FlatStyle = FlatStyle.Flat;
            btnAgregarArchivo.FlatAppearance.BorderSize = 0;
            btnAgregarArchivo.BackColor = Color.FromArgb(45, 45, 48);
            btnAgregarArchivo.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            btnAgregarArchivo.Size = new Size(32, 32);
            btnAgregarArchivo.Image = CrearIconoConPlus();

            // Ajustes para "Eliminar Archivo"
            btnEliminarArchivo.FlatStyle = FlatStyle.Flat;
            btnEliminarArchivo.FlatAppearance.BorderSize = 0;
            btnEliminarArchivo.BackColor = Color.FromArgb(45, 45, 48);
            btnEliminarArchivo.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            btnEliminarArchivo.Size = new Size(32, 32);
            btnEliminarArchivo.Image = new Bitmap(SystemIcons.Error.ToBitmap(), new Size(24, 24));
        }

        // Crea un �cono base con un "plus" verde en la esquina
        private Bitmap CrearIconoConPlus()
        {
            int baseW = 24, baseH = 24;
            Bitmap baseIcon = new Bitmap(SystemIcons.WinLogo.ToBitmap(), new Size(baseW, baseH));
            Bitmap plusIcon = new Bitmap(12, 12);
            using (Graphics g = Graphics.FromImage(plusIcon))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.LimeGreen, 2))
                {
                    int c = plusIcon.Width / 2;
                    g.DrawLine(pen, c, 2, c, plusIcon.Height - 2);
                    g.DrawLine(pen, 2, c, plusIcon.Width - 2, c);
                }
            }
            Bitmap composite = new Bitmap(baseIcon.Width, baseIcon.Height);
            using (Graphics g = Graphics.FromImage(composite))
            {
                g.DrawImage(baseIcon, 0, 0);
                int x = baseIcon.Width - plusIcon.Width;
                int y = baseIcon.Height - plusIcon.Height;
                g.DrawImage(plusIcon, x, y);
            }
            return composite;
        }

        #endregion

        #region TabControl inferior para errores/resultados

        private void InicializarTabOutput()
        {
            tabOutput.TabPages.Clear();
            string[] nombresPesta�as = { "Errores Lexicos", "Errores Sintacticos", "Errores Semanticos", "Resultados" };

            foreach (string nombre in nombresPesta�as)
            {
                TabPage pagina = new TabPage(nombre);
                RichTextBox rtb = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 10),
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    ReadOnly = true,
                    Text = $"Contenido para {nombre}..."
                };
                pagina.Controls.Add(rtb);
                tabOutput.TabPages.Add(pagina);
            }
        }

        #endregion

        #region Eventos del Editor y N�meros de L�nea

        private void TxtEditor_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLineColumn();
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
            if (txtEditor == null) return;

            int firstIndex = txtEditor.GetCharIndexFromPosition(new Point(0, 0));
            int firstLine = txtEditor.GetLineFromCharIndex(firstIndex);
            int lastIndex = txtEditor.GetCharIndexFromPosition(new Point(0, txtEditor.Height));
            int lastLine = txtEditor.GetLineFromCharIndex(lastIndex);

            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (Font font = new Font("Consolas", 10))
            {
                int yOffset = 0;
                for (int i = firstLine; i <= lastLine; i++)
                {
                    e.Graphics.DrawString((i + 1).ToString(), font, brush, new PointF(5, yOffset));
                    yOffset += 20;
                }
            }
        }

        private void UpdateLineColumn()
        {
            if (txtEditor == null || lblStatus == null) return;

            int line = txtEditor.GetLineFromCharIndex(txtEditor.SelectionStart) + 1;
            int column = txtEditor.SelectionStart - txtEditor.GetFirstCharIndexOfCurrentLine() + 1;
            lblStatus.Text = $"L�nea: {line}, Columna: {column}";
        }

        #endregion

        #region L�gica de Men�/Compilaci�n


private void EjecutarFase(string fase)
    {
        string tabName;

        switch (fase)
        {
            case "Lexical Analysis":
                {
                    tabName = "Errores Lexicos";

                    LexicalAnalyzer analizador = new LexicalAnalyzer();
                    var (tokens, errores) = analizador.Analizar(txtEditor.Text);

                    foreach (TabPage pagina in tabOutput.TabPages)
                    {
                        if (pagina.Text.Equals(tabName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (pagina.Controls.Count > 0 && pagina.Controls[0] is RichTextBox rtb)
                            {
                                if (errores.Count > 0)
                                {
                                    rtb.Text = "Errores L�xicos Encontrados:\n\n" + string.Join(Environment.NewLine, errores);
                                }
                                else
                                {
                                    rtb.Text = "An�lisis L�xico Correcto:\n\n" +
                                               string.Join(Environment.NewLine, tokens.Select(t => t.ToString()));
                                }
                            }
                            tabOutput.SelectedTab = pagina;
                            break;
                        }
                    }
                    break;
                }
            case "Syntax Analysis":
                tabName = "Errores Sintacticos";
                MostrarMensajeTemporal(tabName, "Ejecutando an�lisis sint�ctico...");
                break;
            case "Semantic Analysis":
                tabName = "Errores Semanticos";
                MostrarMensajeTemporal(tabName, "Ejecutando an�lisis sem�ntico...");
                break;
            case "Intermediate Code":
                tabName = "Resultados";
                MostrarMensajeTemporal(tabName, "Generando c�digo intermedio...");
                break;
            case "Execution":
                tabName = "Resultados";
                MostrarMensajeTemporal(tabName, "Ejecutando programa...");
                break;
            default:
                tabName = "Resultados";
                MostrarMensajeTemporal(tabName, $"Fase '{fase}' en ejecuci�n...");
                break;
        }
    }

    private void MostrarMensajeTemporal(string tabName, string mensaje)
    {
        foreach (TabPage pagina in tabOutput.TabPages)
        {
            if (pagina.Text.Equals(tabName, StringComparison.OrdinalIgnoreCase))
            {
                if (pagina.Controls.Count > 0 && pagina.Controls[0] is RichTextBox rtb)
                {
                    rtb.Text = mensaje;
                }
                tabOutput.SelectedTab = pagina;
                break;
            }
        }
    }


    #endregion

    #region Abrir/Guardar Archivos

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
                SaveFileAs();
            else
                File.WriteAllText(currentFilePath, txtEditor.Text);
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

        #endregion

        #region Explorador de Archivos

        private void BtnAgregarArchivo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
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

        private void BtnEliminarArchivo_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = fileExplorer.SelectedNode;
            if (selectedNode != null)
            {
                DialogResult result = MessageBox.Show("�Est�s seguro de que deseas eliminar este archivo del explorador?",
                                                      "Confirmar eliminaci�n", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string filePathToDelete = selectedNode.Tag as string;
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
                MessageBox.Show("Por favor, selecciona un archivo para eliminar.", "Eliminar Archivo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

        #endregion
    }
}