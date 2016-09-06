using FileHelpers;
using FileHelpers.Options;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConvertBankStatementToYnab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();

            if (diag.ShowDialog() == true)
            {
                try
                {


                    //var engine = new FileHelpers.CsvEngine("Test", ';', 11);

                    var res = FileHelpers.CsvEngine.CsvToDataTable(diag.FileName, ';');

                    DataTable createTable = new DataTable("Bank");
                    createTable.Columns.Add("a");
                    createTable.Columns.Add("b");
                    createTable.Columns.Add("c");
                    createTable.Columns.Add("d");
                    createTable.Columns.Add("e");
                    createTable.Columns.Add("f");
                                     
                    if (res.Rows.Count > 0)
                    {
                        //Header
                        var headerRow = createTable.NewRow();

                        headerRow[0] = "Date";
                        headerRow[1] = "Payee";
                        headerRow[2] = "Category";
                        headerRow[3] = "Memo";
                        headerRow[4] = "Outflow";
                        headerRow[5] = "Inflow";

                        createTable.Rows.Add(headerRow);
                        createTable.AcceptChanges();


                        foreach (System.Data.DataRow inRow in res.Rows)
                        {

                            var newRow = createTable.NewRow();
                            
                            newRow[0] = DateTime.Parse((string)inRow[2]).ToString("MM'/'dd'/'yy");
                            newRow[1] = inRow[5].ToString().Replace(',', '.');
                            newRow[2] = "";
                            newRow[3] = inRow[4].ToString().Replace(',', '.');

                            if (float.Parse((string)inRow[8]) < 0f)
                            {
                                newRow[4] = ((string)inRow[8]).Replace("-", "").Replace(',', '.');
                            }
                            else
                            {
                                newRow[5] = ((string)inRow[8]).Replace(',', '.');
                            }

                            
                            createTable.Rows.Add(newRow);
                            createTable.AcceptChanges();

                        }

                        FileHelpers.CsvEngine.DataTableToCsv(createTable, diag.FileName + ".cnvrt.csv", ',');

                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error", ex.ToString());
                }
            }

        }
    }

    public class BankStatement
    {
        public string Name;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Use;

    }
}
