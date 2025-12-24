using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Уч.практика_2.Учет_продаж_в_супермаркете
{
    /// <summary>
    /// Класс продаж
    /// </summary>
    public class ClassFormSales
    {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int CashierId { get; set; }
            public string Product { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Total => Quantity * Price;

        // Форма для добавления/редактирования продажи
        //public class AddEditSaleForm : Form
        //{
        //}
    }
}