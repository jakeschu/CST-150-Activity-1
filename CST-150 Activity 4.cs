namespace CST_150_Activity_4._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            selectFileDialog.InitialDirectory = Application.StartupPath + @"Data";
            selectFileDialog.Title = "Browse Text Files";
            selectFileDialog.DefaultExt = "txt";
            selectFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files(*.*)|*.*";
            lblResults.Visible = false;
            lblSelectedFile.Visible = false;
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            string txtFile = "";
            string dirlocation = "";
            const int PadSpace = 20;
            string header1 = "Type", header2 = "Color", header3 = "Qty";
            string headerLine1 = "----", headerLine2 = "-----", headerLine3 = "---";
            int numberRows = 1;
            int qtyValue = -1;
            int rowSelected = -1;

            if (this.selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFile = this.selectFileDialog.FileName;
                dirlocation = Path.GetFullPath(selectFileDialog.FileName);
                lblSelectedFile.Text = txtFile;
                lblSelectedFile.Visible = true;

                cmbSelectRow.Visible = false;
                lblSelectRow.Visible = false;

                string[] lines = File.ReadAllLines(txtFile);
                lblResults.Text = "";

                lblResults.Text = string.Format("{0}{1}{2}\n", header1.PadRight(PadSpace), header2.PadRight(PadSpace), header3.PadRight(PadSpace));
                lblResults.Text += string.Format("{0}{1}{2}\n", headerLine1.PadRight(PadSpace), headerLine2.PadRight(PadSpace), headerLine3.PadRight(PadSpace));
                foreach (string line in lines)
                {
                    cmbSelectRow.Items.Add(numberRows);
                    numberRows++;

                    string[] inventoryList = line.Split(",");
                    for (int i = 0; i < inventoryList.Length; i++)
                    {
                        ConvertLowerCase(inventoryList[i]);

                    }
                    lblResults.Text += "\n";
                }

                lblResults.Visible = true;

                cmbSelectRow.Visible = true;
                lblSelectRow.Visible = true;

                rowSelected = SelectedRow();
                if (rowSelected <= 0)
                {
                    qtyValue = GetQty(lines, rowSelected);

                    IncDisplayQty(lines, rowSelected, qtyValue, txtFile);
                }
            }
        }

        /// <summary>
        /// Convert input string to all lower case character.
        /// Then send the results to be displayed.
        /// </summary>
        /// <param name="textToConvert"></param>

        private void ConvertLowerCase(string textToConvert)
        {
            ResultsToLabel(textToConvert.ToLower());
        }

        private void ResultsToLabel(string results)
        {
            const int PadSpace = 20;
            lblResults.Text += results.PadRight(PadSpace);
        }

        private int SelectedRow()
        {
            if (cmbSelectRow.SelectedIndex >= 0)
            {
                return cmbSelectRow.SelectedIndex;
            }
            else
            {
                return -1;
            }
        }

        /// <param name="lines"></param>
        /// <param name="selectedRow"></param>
        /// <returns></returns>
        private int GetQty(string[] lines, int selectedRow)
        {
            int qty = -1;

            for (int x = 1; x < lines.Length; x++)
            {
                if (x == selectedRow)
                {
                    string[] invRow = lines[x].Split(",");

                    try
                    {
                        qty = int.Parse(invRow[2].Trim());
                        return qty;
                    }

                    catch (FormatException e)
                    {
                        lblResults.Text = e.Message;
                    }
                }
            }
            return qty;
        }

        /// <param name="lines"></param>
        /// <param name="invRowToUpdate"></param>
        /// <param name="qty"></param>
        /// <param name="txtFile"></param>
        private void IncDisplayQty(string[] lines, int invRowToUpdate, int qty, string txtFile)
        {
            string updateLine = "";

            qty++;

            string[] invRow = lines[invRowToUpdate].Split(",");
            invRow[2] = qty.ToString();
            updateLine = invRow[0].Trim() + ", " + invRow[1].Trim() + ", " + invRow[2].Trim();
            lines[invRowToUpdate] = updateLine;
            File.WriteAllLines(txtFile, lines);
        }
    }
}
