using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace VovasKursach.Infrastructure.Services
{
    public class ExcelService
    {
        private int MaxLenValue(IEnumerable<Product> products, Func<Product, string> func)
        {


            return 0;
        }

        public void WriteToExcel(string path, IEnumerable<Product> products)
        {
            if (string.IsNullOrEmpty(path) && File.Exists(path))
            {
                System.Windows.MessageBox.Show("Неверный путь");
                return;
            }

            var excel = new Application();

            excel.Visible = false;
            excel.DisplayAlerts = false;

            _Workbook workBook = excel.Workbooks.Open(path);
            _Worksheet workSheet = (_Worksheet)workBook.ActiveSheet;
            workSheet.Name = "Products";

            workSheet.Cells.ClearContents();

            workSheet.Cells[1, 1] = "Id";
            workSheet.Cells[1, 1] = "Name";
            workSheet.Cells[1, 1] = "Product type";
            workSheet.Cells[1, 1] = "Product image path";
            workSheet.Cells[1, 1] = "Recipe text";
            workSheet.Cells[1, 1] = "Price";



            int index = 2;
            foreach (Product product in products)
            {
                workSheet.Cells[index, 1] = product.Id.ToString();
                workSheet.Cells[index, 2] = product.Name;
                workSheet.Cells[index, 3] = product.ProductType.TypeName;
                workSheet.Cells[index, 4] = product.ProductImagePath;
                workSheet.Cells[index, 5] = product.RecipeText;
                workSheet.Cells[index, 6] = product.Price.ToString();

                ++index;
            }

            workBook.Save();

            workBook?.Close();
        }
    }
}
