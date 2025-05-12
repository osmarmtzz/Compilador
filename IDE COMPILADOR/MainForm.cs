﻿using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using IDE_COMPILADOR.AnalizadorLexico; 


namespace IDE_COMPILADOR
{
    public partial class MainForm : Form
    {
        private string currentFilePath = string.Empty;
        private RichTextBox rtbLexico;


        public MainForm()
        {
            InitializeComponent();

            // Texto inicial en el editor:
            txtEditor.Text = "Escriba aquí...";

            // Eventos del editor
            txtEditor.SelectionChanged += TxtEditor_SelectionChanged;
            txtEditor.TextChanged += TxtEditor_TextChanged;
            txtEditor.VScroll += TxtEditor_VScroll;
            txtEditor.KeyDown += TxtEditor_KeyDown;


            // Evento de panel de números de línea
            lineNumberPanel.Paint += LineNumberPanel_Paint;

            // Eventos de explorador de archivos
            btnAgregarArchivo.Click += BtnAgregarArchivo_Click;
            btnEliminarArchivo.Click += BtnEliminarArchivo_Click;
            fileExplorer.NodeMouseDoubleClick += FileExplorer_NodeMouseDoubleClick;

            // Inicializaciones gráficas y lógicas
            // (Opcional: Si deseas mantener el menú, déjalo; de lo contrario, puedes comentarlo)
            InicializarMenuPersonalizado();

            // ToolStrip: Agregamos los botones con íconos, tooltips y el nuevo botón "New Project"
            InicializarToolStrip();

            // Moderniza botones del explorador
            ModernizarBotonesFileExplorer();
            // Cargar ícono para los nodos del explorador de archivos
            Image iconArchivo = Image.FromFile("Resources/Icons/archivo.png");
            Bitmap smallIcon = new Bitmap(iconArchivo, new Size(16, 16));
            fileExplorer.ImageList = new ImageList();
            fileExplorer.ImageList.Images.Add("archivo", smallIcon);
            // Cargar los íconos una sola vez


            fileExplorer.ImageList.Images.Add("php", new Bitmap(Image.FromFile("Resources/Icons/php.png"), new Size(16, 16)));



            // TabControl inferior para errores/resultados
            InicializarTabOutput();
        }

        #region Menú (Opcional)

        private void InicializarMenuPersonalizado()
        {
            // Limpiamos cualquier ítem existente
            menuStrip.Items.Clear();

            // Íconos de ejemplo
            Image openIcon = SystemIcons.Application.ToBitmap();
            Image saveIcon = SystemIcons.Information.ToBitmap();
            Image saveAsIcon = SystemIcons.Warning.ToBitmap();

            // Menú "File"
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Open", openIcon, (s, e) => OpenFile()));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Save", saveIcon, (s, e) => SaveFile()));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Save As", saveAsIcon, (s, e) => SaveFileAs()));

            // Íconos para el menú "Compile"
            Image lexicalIcon = SystemIcons.Information.ToBitmap();
            Image syntaxIcon = SystemIcons.Question.ToBitmap();
            Image semanticIcon = SystemIcons.Shield.ToBitmap();
            Image intermediateIcon = SystemIcons.WinLogo.ToBitmap();
            Image executionIcon = SystemIcons.Exclamation.ToBitmap();

            // Menú "Compile"
            ToolStripMenuItem compileMenu = new ToolStripMenuItem("Compile");
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Lexical Analysis", lexicalIcon, (s, e) => EjecutarFase("Lexical Analysis")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Syntax Analysis", syntaxIcon, (s, e) => EjecutarFase("Syntax Analysis")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Semantic Analysis", semanticIcon, (s, e) => EjecutarFase("Semantic Analysis")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Intermediate Code", intermediateIcon, (s, e) => EjecutarFase("Intermediate Code")));
            compileMenu.DropDownItems.Add(new ToolStripMenuItem("Execution", executionIcon, (s, e) => EjecutarFase("Execution")));

            // Agregamos ambos menús al menuStrip
            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(compileMenu);
        }

        #endregion

        #region ToolStrip con botones con íconos y ToolTip

        private void InicializarToolStrip()
        {
            Image iconNuevoProyecto = Image.FromFile("Resources/Icons/guardar.png");
            Image iconBuildDebug = Image.FromFile("Resources/Icons/debug.png");
            Image iconCerrarVentana = Image.FromFile("Resources/Icons/cerrar.png");
            Image iconCorrerAnalizador = Image.FromFile("Resources/Icons/lexico.png");
            Image iconCorrerSintactico = Image.FromFile("Resources/Icons/sintactico.png");
            Image iconCorrerSemantico = Image.FromFile("Resources/Icons/semantico.png");
            Image iconGenerarCodigo = Image.FromFile("Resources/Icons/codigo-intermedio.png");





            toolStrip1.Items.Clear();

            // Botón "New Project" con input
            ToolStripButton newProjectButton = new ToolStripButton
            {
                Image = iconNuevoProyecto,
                ToolTipText = "Nuevo Proyecto"
            };

            newProjectButton.Click += (s, e) =>
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Selecciona la ubicación para el nuevo proyecto";

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string nombreProyecto = Microsoft.VisualBasic.Interaction.InputBox(
                            "Escribe el nombre del nuevo proyecto:", "Nuevo Proyecto", "MiProyecto");

                        if (!string.IsNullOrWhiteSpace(nombreProyecto))
                        {
                            string rutaCompleta = Path.Combine(folderDialog.SelectedPath, nombreProyecto);

                            try
                            {
                                Directory.CreateDirectory(rutaCompleta);
                                MessageBox.Show($"Proyecto creado en:\n{rutaCompleta}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                MainForm nuevoForm = new MainForm
                                {
                                    Text = nombreProyecto // Cambia el título de la ventana
                                };

                                // Si deseas guardar la ruta, puedes hacerlo aquí:
                                nuevoForm.currentFilePath = rutaCompleta;

                                nuevoForm.Show();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error al crear la carpeta:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            };
            toolStrip1.Items.Add(newProjectButton);


            // Botón "Build and Debug"
            ToolStripButton buildDebugButton = new ToolStripButton
            {
                Image = iconBuildDebug,
                ToolTipText = "Build and Debug"
            };
            buildDebugButton.Click += (s, e) => { /* lógica */ };
            toolStrip1.Items.Add(buildDebugButton);

            // Botón "Cerrar ventana"
            ToolStripButton closeButton = new ToolStripButton
            {
                Image = iconCerrarVentana,
                ToolTipText = "Cerrar ventana"
            };
            closeButton.Click += (s, e) => this.Close();
            toolStrip1.Items.Add(closeButton);

            // Botón "Léxico"
            ToolStripButton lexicoButton = new ToolStripButton
            {
                Image = iconCorrerAnalizador,
                ToolTipText = "Análisis Léxico"
            };
            lexicoButton.Click += (s, e) => EjecutarFase("Lexical Analysis");
            toolStrip1.Items.Add(lexicoButton);

            // Botón "Sintáctico"
            ToolStripButton sintacticoButton = new ToolStripButton
            {
                Image = iconCorrerSintactico,
                ToolTipText = "Análisis Sintáctico"
            };
            sintacticoButton.Click += (s, e) => EjecutarFase("Syntax Analysis");
            toolStrip1.Items.Add(sintacticoButton);

            // Botón "Semántico"
            ToolStripButton semanticoButton = new ToolStripButton
            {
                Image = iconCorrerSemantico,
                ToolTipText = "Análisis Semántico"
            };
            semanticoButton.Click += (s, e) => EjecutarFase("Semantic Analysis");
            toolStrip1.Items.Add(semanticoButton);

            // Botón "Compilar"
            ToolStripButton compilarButton = new ToolStripButton
            {
                Image = iconGenerarCodigo,
                ToolTipText = "Generar Código Intermedio"
            };
            compilarButton.Click += (s, e) => EjecutarFase("Intermediate Code");
            toolStrip1.Items.Add(compilarButton);
        }

        #endregion

        #region Botones del Explorador de Archivos

        private void ModernizarBotonesFileExplorer()
        {

            Image iconOriginal2 = Image.FromFile("Resources/Icons/agregar-archivo.png");
            Image iconAgregar = new Bitmap(iconOriginal2, new Size(24, 24));
            Image iconOriginal = Image.FromFile("Resources/Icons/basura.png");
            Image iconBasura = new Bitmap(iconOriginal, new Size(24, 24));

            // Ajustes para "Agregar Archivo"
            btnAgregarArchivo.FlatStyle = FlatStyle.Flat;
            btnAgregarArchivo.FlatAppearance.BorderSize = 0;
            btnAgregarArchivo.BackColor = Color.FromArgb(45, 45, 48);
            btnAgregarArchivo.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            btnAgregarArchivo.Size = new Size(32, 32);
            btnAgregarArchivo.Image = iconAgregar;
            btnEliminarArchivo.Text = ""; // Asegura que no haya texto ni emoji


            // Ajustes para "Eliminar Archivo"
            btnEliminarArchivo.FlatStyle = FlatStyle.Flat;
            btnEliminarArchivo.FlatAppearance.BorderSize = 0;
            btnEliminarArchivo.BackColor = Color.FromArgb(45, 45, 48);
            btnEliminarArchivo.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            btnEliminarArchivo.Size = new Size(32, 32);
            btnEliminarArchivo.Image = iconBasura;
            btnEliminarArchivo.Text = ""; // Asegura que no haya texto ni emoji

        }


        // Crea un ícono base con un "plus" verde en la esquina
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
            string[] nombresPestañas = { "Errores Lexicos", "Errores Sintacticos", "Errores Semanticos", "Resultados" };

            foreach (string nombre in nombresPestañas)
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

        #region Eventos del Editor y Números de Línea

        private void TxtEditor_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLineColumn();
        }
        // 1) Flag para detectar pegado
        private bool pasteDetected = false;

        // ??? 3) TxtEditor_TextChanged: colorear sólo la línea activa ??????????????
        private void TxtEditor_TextChanged(object sender, EventArgs e)
        {
            UpdateLineColumn();
            lineNumberPanel.Invalidate();

            var analizador = new LexicalAnalyzer();

            if (pasteDetected)
            {
                // full coloreado tras pegar
                pasteDetected = false;
                var (tokens, _) = analizador.Analizar(txtEditor.Text);
                AplicarColoreadoCompleto(tokens);
            }
            else
            {
                // 1) Si no hay texto, limpiar colores y salir
                if (string.IsNullOrEmpty(txtEditor.Text))
                {
                    // simplemente elimina cualquier color previo
                    txtEditor.SelectAll();
                    txtEditor.SelectionColor = Color.White;
                    return;
                }

                // 2) obtener índices
                int lineIndex = txtEditor.GetLineFromCharIndex(txtEditor.SelectionStart);
                int start = txtEditor.GetFirstCharIndexFromLine(lineIndex);
                int end;

                // si es la última línea
                if (lineIndex == txtEditor.Lines.Length - 1)
                    end = txtEditor.Text.Length;
                else
                    end = txtEditor.GetFirstCharIndexFromLine(lineIndex + 1);

                // 3) validar
                if (start < 0) start = 0;
                if (end < 0) end = txtEditor.Text.Length;
                if (end < start) end = start;

                // 4) extraer y colorear sólo esa línea
                int length = end - start;
                string lineaTexto = txtEditor.Text.Substring(start, length);
                var (tokens, _) = analizador.Analizar(lineaTexto);
                AplicarColoreadoLinea(tokens, start);
            }
        }

        // ??? 4) Método para colorear TODO el documento (al cargar) ?????????????????
        private void AplicarColoreadoCompleto(List<Token> tokens)
        {
            int selStart = txtEditor.SelectionStart;
            int selLen = txtEditor.SelectionLength;

            txtEditor.TextChanged -= TxtEditor_TextChanged;
            txtEditor.SuspendLayout();

            // 1) limpiar a blanco
            txtEditor.SelectAll();
            txtEditor.SelectionColor = Color.White;

            // 2) pintar cada token por su posición
            foreach (var t in tokens)
            {
                int lineIdx = t.Linea - 1;
                int pos = txtEditor.GetFirstCharIndexFromLine(lineIdx) + (t.Columna - 1);
                if (pos < 0 || pos + t.Valor.Length > txtEditor.Text.Length) continue;

                txtEditor.Select(pos, t.Valor.Length);
                txtEditor.SelectionColor = ColorForToken(t.Tipo, t.Valor);
            }

            // 3) restaurar selección
            txtEditor.Select(selStart, selLen);
            txtEditor.SelectionColor = Color.White;
            txtEditor.ResumeLayout();
            txtEditor.TextChanged += TxtEditor_TextChanged;
        }
        // ??? 5) Método para colorear sólo 1 línea (al tipear) 
        private void AplicarColoreadoLinea(List<Token> tokens, int offset)
        {
            int selStart = txtEditor.SelectionStart;
            int selLen = txtEditor.SelectionLength;

            txtEditor.TextChanged -= TxtEditor_TextChanged;
            txtEditor.SuspendLayout();

            foreach (var t in tokens)
            {
                int pos = offset + (t.Columna - 1);
                if (pos < 0 || pos + t.Valor.Length > txtEditor.Text.Length) continue;

                txtEditor.Select(pos, t.Valor.Length);
                txtEditor.SelectionColor = ColorForToken(t.Tipo, t.Valor);
            }

            txtEditor.Select(selStart, selLen);
            txtEditor.SelectionColor = Color.White;
            txtEditor.ResumeLayout();
            txtEditor.TextChanged += TxtEditor_TextChanged;
        }
        // 3) Captura Ctrl+V y Shift+Insert
        private void TxtEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
                pasteDetected = true;
        }


        // ??? 6) ColorForToken: define tus colores según el tipo ????????????????????
        // ─── Firma nueva para recibir tipo y valor ────────────────────────────────
        private Color ColorForToken(string tipo, string valor)
        {
            return tipo switch
            {
                // Color 1: Números enteros y reales (con y sin signo)
                "Numero" or "PuntoFlotante" => Color.LightGreen,

                // Color 2: Identificadores (letras y dígitos, sin comenzar por dígito)
                "Identificador" => Color.Cyan,

                // Color 3: Comentarios (inline "//…" y multilínea "/*…*/")
                "ComentarioInline" or "ComentarioExtenso" => Color.Gray,

                // Color 4: Palabras reservadas: if, else, end, do, while, switch,
                // case, int, float, main, cin, cout
                "PalabraReservada" => Color.Orange,

                // Color 5: Operadores aritméticos: +, -, *, /, %, ^, ++, --
                "OperadorAritmetico" => Color.Yellow,

                // Color 6: 
              
                "OperadorRelacional" or
                "OperadorLogico" or
                "Asignacion" or
                "Simbolo" => Color.Red,

                // Por defecto: color de texto normal
                _ => Color.White,
            };
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
            lblStatus.Text = $"Línea: {line}, Columna: {column}";
        }

        #endregion

        #region Lógica de Menú/Compilación


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
                        AplicarColoreado(tokens);

                        // Mostramos errores en tabOutput como antes
                        foreach (TabPage pagina in tabOutput.TabPages)
                        {
                            if (pagina.Text.Equals(tabName, StringComparison.OrdinalIgnoreCase))
                            {
                                if (pagina.Controls.Count > 0 && pagina.Controls[0] is RichTextBox rtb)
                                {
                                    if (errores.Count > 0)
                                    {
                                        rtb.Text = "Errores Léxicos Encontrados:\n\n" + string.Join(Environment.NewLine, errores);
                                    }
                                    else
                                    {
                                        rtb.Text = "Sin errores léxicos encontrados.";
                                    }
                                }
                                tabOutput.SelectedTab = pagina;
                                break;
                            }
                        }

                        // Mostramos los tokens válidos en la pestaña "?? Léxico"
                        rtbLexico.Clear();
                        rtbLexico.Text = string.Join(Environment.NewLine, tokens.Select(t => t.ToString()));
                        tabAnalysis.SelectedTab = tabLexico;

                        break;
                    }

                case "Syntax Analysis":
                    tabName = "Errores Sintacticos";
                    MostrarMensajeTemporal(tabName, "Ejecutando análisis sintáctico...");
                    break;

                case "Semantic Analysis":
                    tabName = "Errores Semanticos";
                    MostrarMensajeTemporal(tabName, "Ejecutando análisis semántico...");
                    break;

                case "Intermediate Code":
                    tabName = "Resultados";
                    MostrarMensajeTemporal(tabName, "Generando código intermedio...");
                    break;

                case "Execution":
                    tabName = "Resultados";
                    MostrarMensajeTemporal(tabName, "Ejecutando programa...");
                    break;

                default:
                    tabName = "Resultados";
                    MostrarMensajeTemporal(tabName, $"Fase '{fase}' en ejecución...");
                    break;
            }
        }

        private void AplicarColoreado(List<Token> tokens)
        {
            int originalSelectionStart = txtEditor.SelectionStart;
            int originalSelectionLength = txtEditor.SelectionLength;

            txtEditor.TextChanged -= TxtEditor_TextChanged; // Evitar loops infinitos
            txtEditor.SuspendLayout(); // Pausar redibujo

            txtEditor.SelectAll();
            txtEditor.SelectionColor = Color.White;

            foreach (var token in tokens)
            {
                try
                {
                    int start = txtEditor.GetFirstCharIndexFromLine(token.Linea - 1) + token.Columna - 1;
                    txtEditor.Select(start, token.Valor.Length);

                    switch (token.Tipo)
                    {
                        case "Numero":
                        case "PuntoFlotante":
                            txtEditor.SelectionColor = Color.LightGreen;
                            break;
                        case "Identificador":
                            txtEditor.SelectionColor = Color.Cyan;
                            break;
                        case "ComentarioInline":
                        case "ComentarioExtenso":
                            txtEditor.SelectionColor = Color.Gray;
                            break;
                        case "PalabraReservada":
                            txtEditor.SelectionColor = Color.Orange;
                            break;
                        case "OperadorAritmetico":
                            txtEditor.SelectionColor = Color.Yellow;
                            break;
                        case "OperadorRelacional":
                        case "OperadorLogico":
                        case "Asignacion":
                            txtEditor.SelectionColor = Color.Red;
                            break;
                    }
                }
                catch
                {
                    // Silenciar errores de selección fuera de rango
                }
            }

            // Restaurar selección y evento
            txtEditor.Select(originalSelectionStart, originalSelectionLength);
            txtEditor.SelectionColor = Color.White;
            txtEditor.ResumeLayout();
            txtEditor.TextChanged += TxtEditor_TextChanged;
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

        // ??? 1) OpenFile: cargar y colorear TODO ?????????????????????????????????????
        private void OpenFile()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Archivos permitidos (*.txt;*.html;*.php;*.java;*.cs)|*.txt;*.html;*.php;*.java;*.cs|Todos los archivos (*.*)|*.*";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                currentFilePath = dlg.FileName;
                txtEditor.Text = File.ReadAllText(currentFilePath);

                var analizador = new LexicalAnalyzer();
                var (tokens, _) = analizador.Analizar(txtEditor.Text);
                AplicarColoreadoCompleto(tokens);
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
                openFileDialog.Filter = "Archivos permitidos (*.txt;*.html;*.php;*.java;*.cs)|*.txt;*.html;*.php;*.java;*.cs|Todos los archivos (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    if (!FileNodeExists(filePath))
                    {
                        string extension = Path.GetExtension(filePath).ToLower();
                        string iconKey = "default"; // Icono por defecto

                        switch (extension)
                        {
                            case ".php":
                                iconKey = "php";
                                break;
                            case ".html":
                                iconKey = "html";
                                break;
                            case ".java":
                                iconKey = "java";
                                break;
                            case ".cs":
                                iconKey = "cs";
                                break;
                        }

                        TreeNode node = new TreeNode(Path.GetFileName(filePath))
                        {
                            Tag = filePath,
                            ImageKey = iconKey,
                            SelectedImageKey = iconKey
                        };

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
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este archivo del explorador?",
                                                      "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        // ??? 2) Double-click en el TreeView: cargar y colorear TODO ????????????????
        private void FileExplorer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;

            string filePath = e.Node.Tag.ToString();
            if (!File.Exists(filePath))
            {
                MessageBox.Show("El archivo no existe.");
                return;
            }

            currentFilePath = filePath;
            txtEditor.Text = File.ReadAllText(filePath);

            var analizador = new LexicalAnalyzer();
            var (tokens, _) = analizador.Analizar(txtEditor.Text);
            AplicarColoreadoCompleto(tokens);
        }


        #endregion
    }
}