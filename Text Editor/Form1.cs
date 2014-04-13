using System;
using System.IO;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Text_Editor
{
    public partial class frmTextEditor : Form
    {
        bool Save = false;
        public frmTextEditor()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process NewForm = new Process();
            NewForm.StartInfo.FileName = Application.ExecutablePath;
            NewForm.Start();
        }

        private void saveCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFileIndex = new SaveFileDialog(); //creates instance of SaveFileDialog
            SaveFileIndex.Filter = "Rich Text File (*.rtf)|*.rtf|Text Documents (*.txt)|*.txt|All files (*.*)|*.*";
            if (SaveFileIndex.ShowDialog() == DialogResult.OK) //Get's correct location and checks if it is valid
            {
                rtfContent.SaveFile(SaveFileIndex.FileName); //Saves content to file in PlainText for no encoding
                Save = true;
            }
        }

        private void openCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog(); //get the file save location
            OpenFile.Filter = "Rich Text File (*.rtf)|*.rtf|Text Documents (*.txt)|*.txt|All files (*.*)|*.*"; //filter
            if (OpenFile.ShowDialog() == DialogResult.OK) //dialog for find path
            {
                if (Path.GetExtension(OpenFile.FileName) == ".rtf")
                {
                    rtfContent.LoadFile(OpenFile.FileName);
                }
                else
                {
                    rtfContent.LoadFile(OpenFile.FileName, RichTextBoxStreamType.PlainText);
                }
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyCtrlCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtfContent.Copy();
        }

        private void cutCtrlXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtfContent.Cut();
        }

        private void pasteCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtfContent.Paste();
        }

        private void undoCtrlZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtfContent.Undo();
        }

        private void redoCtrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtfContent.Redo();
        }

        private void fontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog FontSelect = new FontDialog(); //Font dialog
            if (FontSelect.ShowDialog() == DialogResult.OK)
            {
                rtfContent.Font = FontSelect.Font; //sets font
            }
        }

        private void theEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Editor is an open source Text Editor with little features currently but there will be more to come as worked on. - Ryan Guarascia");
        }

        private void fontColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog FontColour = new ColorDialog(); //colour dialog
            if (FontColour.ShowDialog() == DialogResult.OK)
            {
                rtfContent.ForeColor = FontColour.Color; //sets colour
            }
        }
        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //add left
            rtfContent.SelectAll();
            rtfContent.SelectionAlignment = HorizontalAlignment.Left;
            rtfContent.DeselectAll();
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtfContent.SelectAll();
            rtfContent.SelectionAlignment = HorizontalAlignment.Center;
            rtfContent.DeselectAll();
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtfContent.SelectAll();
            rtfContent.SelectionAlignment = HorizontalAlignment.Right;
            rtfContent.DeselectAll();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Save == false)
            {
                DialogResult MessageboxDialog = MessageBox.Show("Document has not been saved, save the file now?"
                                                                 , "Prompt for Saving", MessageBoxButtons.YesNoCancel);
                if (MessageboxDialog == DialogResult.Yes)
                {
                    Save = true;
                    saveCtrlSToolStripMenuItem_Click(null, null);
                    e.Cancel = false;
                }
                else if (MessageboxDialog == DialogResult.No)
                {
                    e.Cancel = false;
                }
                else if (MessageboxDialog == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void rtfContent_TextChanged(object sender, EventArgs e)
        {
            Save = false;
        }

        private void rtfContent_KeyPress(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'C')
            {
                MessageBox.Show("C");
                e.Handled = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //PrintDialog associate with PrintDocument;
            printDialog1.Document = printDocument1;
            printDocument1.PrintPage += new PrintPageEventHandler(PrintPageOut);

            printPreviewDialog1.Document = printDocument1;

            // Show PrintPreview Dialog
            printPreviewDialog1.ShowDialog();

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1170);
                Margins margins = new Margins(100, 100, 100, 100);
                printDocument1.DefaultPageSettings.Margins = margins;
                
                printDocument1.Print(); //prints document
            }
        }
        private void PrintPageOut(object sender, PrintPageEventArgs e)
        {
            float LinesOnPage = 0;
            int count = 0;
            float yval = 0;

            StringFormat format = new StringFormat(StringFormatFlags.LineLimit);
            int Lines = rtfContent.Lines.Count(); //gets line count

            LinesOnPage = e.MarginBounds.Height / rtfContent.Font.GetHeight(e.Graphics); //page height / font height = lines on page for each font size

   
            yval = e.MarginBounds.Top + (count * rtfContent.Font.GetHeight(e.Graphics));
            SolidBrush FontColor = new SolidBrush(rtfContent.ForeColor);
            e.Graphics.DrawString(rtfContent.Text, rtfContent.Font, FontColor, 25, yval, format);
        }
    }
}
