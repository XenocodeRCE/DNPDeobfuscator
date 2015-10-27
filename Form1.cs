#region Dependencies

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

#endregion

namespace DNPD
{
    public partial class Form1 : Form
    {

        #region Declarations

        public static Assembly asm;
        public string DirectoryName = "";
        public static MethodDef Methoddecryption;
        public static ModuleDefMD module;
        public static MethodDef AntitampMethodDef;
        public static MethodDef AntidebugMethodDef;
        public static MethodDef AntidumpMethodDef;

        public static int DeobedString;
        public static int DeobedInts;
        public static int PrunedMembers;
        #endregion

        #region FormAcions
        public Form1()
        {
            InitializeComponent();
        }

        private void TextBox1DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void TextBox1DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array array = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (array != null)
                {
                    string text = array.GetValue(0).ToString();
                    int num = text.LastIndexOf(".", StringComparison.Ordinal);
                    if (num != -1)
                    {
                        string text2 = text.Substring(num);
                        text2 = text2.ToLower();
                        if (text2 == ".exe" || text2 == ".dll")
                        {
                            Activate();
                            textBox1.Text = text;
                            int num2 = text.LastIndexOf("\\", StringComparison.Ordinal);
                            if (num2 != -1)
                            {
                                DirectoryName = text.Remove(num2, text.Length - num2);
                            }
                            if (DirectoryName.Length == 2)
                            {
                                DirectoryName += "\\";
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion
       
        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Browse for target assembly";
            openFileDialog.InitialDirectory = "c:\\";
            if (DirectoryName != "")
            {
                openFileDialog.InitialDirectory = this.DirectoryName;
            }
            openFileDialog.Filter = "All files (*.exe,*.dll)|*.exe;*.dll";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                textBox1.Text = fileName;
                int num = fileName.LastIndexOf("\\", StringComparison.Ordinal);
                if (num != -1)
                {
                    DirectoryName = fileName.Remove(num, fileName.Length - num);
                }
                if (DirectoryName.Length == 2)
                {
                    DirectoryName += "\\";
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            module = ModuleDefMD.Load(textBox1.Text);
            asm = Assembly.LoadFile(textBox1.Text);
            if (!Checker.IsDNP.Check(module))
            {
                label2.Text = "Not a DNP protectedt file !";
                return;
            }
                

            //Remove Anti
            if (chk_antitamp.Checked)
            {
                Helpers.GetAntitamper(module);
                if (AntitampMethodDef != null) Helpers.NopCall(module, AntitampMethodDef);
            }
            if (chk_antidebug.Checked)
            {
                Helpers.GetAntidebug(module);
                if (AntidebugMethodDef != null) Helpers.NopCall(module, AntidebugMethodDef);
            }
            if (chk_antidump.Checked)
            {
                Helpers.GetAntidump(module);
                if (AntidumpMethodDef != null) Helpers.NopCall(module, AntidumpMethodDef);
            }



            if (chk_Integers.Checked)
            {
                //Decrypt Integers
                Helpers.ResolveMathPow(module);
            }
            if (chk_str.Checked)
            {
                //Decrypt Strings
                Helpers.GetStrDecMeth(module);
            }
            if (chk_Integers.Checked)
            {
                //Decrypt remaining integers
                Helpers.ResolveLastInt(module);
                Helpers.ExtractIntFromRes(module);
            }

            if (chk_prune.Checked)
            {
                //Prune Assembly
                Helpers.PruneModule(module);
            }
            
            var text2 = Path.GetDirectoryName(textBox1.Text);
            if (text2 != null && !text2.EndsWith("\\"))
            {
                text2 += "\\";
            }
            var path = text2 + Path.GetFileNameWithoutExtension(textBox1.Text) + "_DNPDeob" +
                          Path.GetExtension(textBox1.Text);
            var opts = new ModuleWriterOptions(module) {Logger = DummyLogger.NoThrowInstance};
            module.Write(path, opts);
            label2.Text = "Successfully deobfuscated " + DeobedString + " String, " + DeobedInts +" Integers, and " + PrunedMembers + " members has been removed !";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
      
    }
}
