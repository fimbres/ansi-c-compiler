using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.UnidadesLexicas;

namespace WindowsFormsApp1
{
    public partial class frmEditor : Form
    {
        string Archivo;
        int PosX = 0;
        int PosY = 0;
        public frmEditor()
        {
            InitializeComponent();
        }
        public int getWidth()
        {
            int ancho = 25;
            int linea = rTBEditor.Lines.Length;

            if (linea <= 99)
            {
                ancho = 20 + (int)rTBEditor.Font.Size;
            }
            else if (linea <= 99)
            {
                ancho = 30 + (int)rTBEditor.Font.Size;
            }
            else
            {
                ancho = 50 + (int)rTBEditor.Font.Size;
            }
            return ancho;
        }
        public void EnumeraLineasCodigo()
        {
            Point pt = new Point(0, 0);
            int PrimerIndice = rTBEditor.GetCharIndexFromPosition(pt);
            int PrimeraLinea = rTBEditor.GetLineFromCharIndex(PrimerIndice);
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            int UltimoIndice = rTBEditor.GetCharIndexFromPosition(pt);
            int UltimaLinea = rTBEditor.GetLineFromCharIndex(UltimoIndice);
            rTBNumLineas.SelectionAlignment = HorizontalAlignment.Center;
            rTBNumLineas.Text = "";
            rTBNumLineas.Width = getWidth();
            for (int i = PrimeraLinea; i <= UltimaLinea; i++)
            {
                rTBNumLineas.Text += i + 1 + "\n";
            }
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void rTBEditor_TextChanged(object sender, EventArgs e)
        {
            EnumeraLineasCodigo();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog AbrirArchivo = new OpenFileDialog();

            AbrirArchivo.Filter = "*Text|*.*";
            if (AbrirArchivo.ShowDialog() == DialogResult.OK)
            {
                Archivo = AbrirArchivo.FileName;
                using (StreamReader sr = new StreamReader(Archivo))
                {
                    rTBEditor.Text = sr.ReadToEnd();
                }
                EnumeraLineasCodigo();
                rTBEditor.SelectionStart = 1;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Archivo = string.Empty;
            rTBEditor.Clear();
            EnumeraLineasCodigo();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog GuardarArchivo = new SaveFileDialog();
            GuardarArchivo.Filter = "Fimbres C | *.C";

            if (Archivo != null)
            {
                using (StreamWriter sw = new StreamWriter(Archivo))
                {
                    sw.Write(rTBEditor.Text);
                }
            }
            else
            {
                if (GuardarArchivo.ShowDialog() == DialogResult.OK)
                {
                    Archivo = GuardarArchivo.FileName;
                    using (StreamWriter sw = new StreamWriter(Archivo))
                    {
                        sw.Write(rTBEditor.Text);
                    }
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog GuardarComo = new SaveFileDialog()
            {
                Title = "Guardar como: ",
                Filter = "Fimbres C | *.C",
                AddExtension = true
            };

            GuardarComo.ShowDialog();
            if (Archivo != null && GuardarComo.FileName != string.Empty)
            {
                Archivo = GuardarComo.FileName;
                using (StreamWriter sw = new StreamWriter(Archivo))
                {
                    sw.Write(rTBEditor.Text);
                    frmEditor.ActiveForm.Text = "Fimbres C | " + Archivo;
                    sw.Close();
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (rTBEditor.Text == string.Empty)
            {
                Consola.Text += "Compilación interrumpida: No se encontró texto en el archivo";
                Consola.ForeColor = Color.Red;
            }
            else
            {
                AnalizadorLexico AL = new AnalizadorLexico();
                List<Tokens> LstTokens = AL.AnalisisLexico(rTBEditor.Text);

                AnalizadorSintactico AS = new AnalizadorSintactico();
                string Error = AS.AnalisisSintactico(LstTokens);

                Consola.Text = string.Empty;
                if (Error == string.Empty)
                {
                    Consola.Text += "Compilación terminada exitosamente.";
                    Consola.ForeColor = Color.Green;
                }
                else
                {
                    Consola.Text += "Compilación interrumpida: " + Error;
                    Consola.ForeColor = Color.Red;
                }
                /*Consola.Text += "\n\n\n";

                for (int i = 0; i < LstTokens.Count; i++)
                {
                    Consola.Text += "Renglon: " + LstTokens[i].Linea.ToString() +
                        ", Lexema: ( " + LstTokens[i].Lexema +
                        " ), Token: ( " + LstTokens[i].Token + " )\n";
                }*/
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.CenterToScreen();
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.CenterToScreen();
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                PosX = e.X;
                PosY = e.Y;
            }
            else
            {
                Left = Left + (e.X - PosX);
                Top = Top + (e.Y - PosY);
            }
        }
    }
}
