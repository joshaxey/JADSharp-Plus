using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JAD
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFilename.Text = openFileDialog1.FileName;
            }
        }

        private void Parse(string filename)
        {
            if (filename.Equals("")) return;

            txtFilename.Text = filename;
            
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("jadit.exe", "-p \"" + filename + "\"");

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();
            result = Regex.Replace(result, "// Decompiled by Jad v1.5.8g. Copyright 2001 Pavel Kouznetsov.\r\n", "// Decompiled with JADSharp Plus by Josh_Axey\r\n");
            result = Regex.Replace(result, "// Jad home page: http://www.kpdus.com/jad.html\r\n", "");
            result = Regex.Replace(result, "// Decompiler options: packimports\\(3\\) \r\n", "");
            string fileName = txtFilename.Text.Substring(txtFilename.Text.LastIndexOf("\\") + 1).Replace(".class", ".java");
            result = Regex.Replace(result, "// Source File Name:   " + fileName + "\r\n\r\n", "// Source File Name: " + fileName + "\r\n\r\n");
            txtConsole.Text = result;

        }

        private void btnDecompile_Click(object sender, EventArgs e)
        {
            Parse(txtFilename.Text);
        }

        private void txtFilename_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string File in FileList)
                Parse(File);
        }

        private void txtFilename_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtConsole.Text = "";
            txtFilename.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string dir = "C:\\";
            string fileName = "";
            if (!txtFilename.Text.Equals(""))
            {
                dir = txtFilename.Text.Substring(0, txtFilename.Text.LastIndexOf("\\"));
                fileName = txtFilename.Text.Substring(txtFilename.Text.LastIndexOf("\\") + 1).Replace(".class", ".java");
            }
            saveFileDialog1.InitialDirectory = dir;
            saveFileDialog1.FileName = fileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, txtConsole.Text);
            }
        }
    }
}
