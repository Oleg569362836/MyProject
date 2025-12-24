using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Уч.практика_2.Учет_продаж_в_супермаркете.Form1;

namespace Уч.практика_2.Учет_продаж_в_супермаркете
{
    public partial class Form1 : Form
    {
        private DataGridView dataGridViewSales;
        private DataGridView dataGridViewCashiers;
        private TabControl tabControlMain;
        private Button btnAddSale;
        private Button btnEditSale;
        private Button btnDeleteSale;
        private Button btnAddCashier;
        private Button btnEditCashier;
        private Button btnDeleteCashier;
        private Button btnCalculateRevenue;
        private Label lblTotalRevenue;
        private ComboBox comboBoxCashierFilter;
        private ComboBox comboBoxProductFilter;
        private DateTimePicker dateTimePickerFrom;
        private DateTimePicker dateTimePickerTo;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnGenerateReport;

        // Хранилища данных
        private List<ClassFormSales> sales = new List<ClassFormSales>();
        private List<ClassCashier> cashiers = new List<ClassCashier>();
        private int nextSaleId = 1;
        private int nextCashierId = 1;

        //public class ClassFormSales
        //{
        //    public int Id { get; set; }
        //    public DateTime Date { get; set; }
        //    public int CashierId { get; set; }
        //    public string Product { get; set; }
        //    public int Quantity { get; set; }
        //    public decimal Price { get; set; }
        //    public decimal Total => Quantity * Price;
        //}

        //public class ClassCashier
        //{
        //public int Id { get; set; }
        //public string FullName { get; set; }
        //public string CashRegister { get; set; }
        //public string Shift { get; set; }
        //}

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupForm();
            AddTestData();
            RefreshDataGridViews();
            UpdateTotalRevenue();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Учет продаж в супермаркете";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создаем TabControl
            tabControlMain = new TabControl();
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Padding = new Point(15, 10);

            // Вкладка продаж
            TabPage tabSales = new TabPage("Продажи");
            TabPage tabCashiers = new TabPage("Кассиры");
            TabPage tabReports = new TabPage("Отчеты");

            // Создаем DataGridView для продаж
            dataGridViewSales = new DataGridView();
            dataGridViewSales.Dock = DockStyle.Fill;
            dataGridViewSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSales.MultiSelect = false;
            dataGridViewSales.ReadOnly = true;

            // Добавляем колонки
            dataGridViewSales.Columns.Add("Id", "ID");
            dataGridViewSales.Columns.Add("Date", "Дата продажи");
            dataGridViewSales.Columns.Add("CashierName", "Кассир");
            dataGridViewSales.Columns.Add("Product", "Товар");
            dataGridViewSales.Columns.Add("Quantity", "Количество");
            dataGridViewSales.Columns.Add("Price", "Цена");
            dataGridViewSales.Columns.Add("Total", "Сумма");

            // Создаем DataGridView для кассиров
            dataGridViewCashiers = new DataGridView();
            dataGridViewCashiers.Dock = DockStyle.Fill;
            dataGridViewCashiers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCashiers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCashiers.MultiSelect = false;
            dataGridViewCashiers.ReadOnly = true;

            dataGridViewCashiers.Columns.Add("Id", "ID");
            dataGridViewCashiers.Columns.Add("FullName", "ФИО кассира");
            dataGridViewCashiers.Columns.Add("CashRegister", "Номер кассы");
            dataGridViewCashiers.Columns.Add("Shift", "Смена");

            // Панель с кнопками для продаж
            Panel panelSalesButtons = new Panel();
            panelSalesButtons.Dock = DockStyle.Bottom;
            panelSalesButtons.Height = 60;
            panelSalesButtons.Padding = new Padding(10);

            btnAddSale = new Button();
            btnAddSale.Text = "Добавить продажу";
            btnAddSale.Size = new Size(150, 35);
            btnAddSale.Location = new Point(20, 10);
            btnAddSale.Click += BtnAddSale_Click;

            btnEditSale = new Button();
            btnEditSale.Text = "Редактировать";
            btnEditSale.Size = new Size(150, 35);
            btnEditSale.Location = new Point(180, 10);
            btnEditSale.Click += BtnEditSale_Click;
            btnEditSale.Enabled = false;

            btnDeleteSale = new Button();
            btnDeleteSale.Text = "Удалить";
            btnDeleteSale.Size = new Size(150, 35);
            btnDeleteSale.Location = new Point(340, 10);
            btnDeleteSale.Click += BtnDeleteSale_Click;
            btnDeleteSale.Enabled = false;

            // Панель с кнопками для кассиров
            Panel panelCashiersButtons = new Panel();
            panelCashiersButtons.Dock = DockStyle.Bottom;
            panelCashiersButtons.Height = 60;
            panelCashiersButtons.Padding = new Padding(10);

            btnAddCashier = new Button();
            btnAddCashier.Text = "Добавить кассира";
            btnAddCashier.Size = new Size(150, 35);
            btnAddCashier.Location = new Point(20, 10);
            btnAddCashier.Click += BtnAddCashier_Click;

            btnEditCashier = new Button();
            btnEditCashier.Text = "Редактировать";
            btnEditCashier.Size = new Size(150, 35);
            btnEditCashier.Location = new Point(180, 10);
            btnEditCashier.Click += BtnEditCashier_Click;
            btnEditCashier.Enabled = false;

            btnDeleteCashier = new Button();
            btnDeleteCashier.Text = "Удалить";
            btnDeleteCashier.Size = new Size(150, 35);
            btnDeleteCashier.Location = new Point(340, 10);
            btnDeleteCashier.Click += BtnDeleteCashier_Click;
            btnDeleteCashier.Enabled = false;

            // Элементы для вкладки отчетов
            Panel panelFilters = new Panel();
            panelFilters.Dock = DockStyle.Top;
            panelFilters.Height = 120;
            panelFilters.BorderStyle = BorderStyle.FixedSingle;
            panelFilters.Padding = new Padding(10);

            label1 = new Label();
            label1.Text = "Кассир:";
            label1.Location = new Point(20, 20);
            label1.Size = new Size(60, 20);

            comboBoxCashierFilter = new ComboBox();
            comboBoxCashierFilter.Location = new Point(85, 17);
            comboBoxCashierFilter.Size = new Size(200, 25);
            comboBoxCashierFilter.DropDownStyle = ComboBoxStyle.DropDownList;

            label2 = new Label();
            label2.Text = "Товар:";
            label2.Location = new Point(300, 20);
            label2.Size = new Size(60, 20);

            comboBoxProductFilter = new ComboBox();
            comboBoxProductFilter.Location = new Point(365, 17);
            comboBoxProductFilter.Size = new Size(200, 25);
            comboBoxProductFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProductFilter.Items.AddRange(new string[] { "Все товары", "Хлеб", "Молоко", "Колбаса", "Сыр", "Вода", "Чай", "Кофе", "Сахар" });

            label3 = new Label();
            label3.Text = "С:";
            label3.Location = new Point(20, 60);
            label3.Size = new Size(30, 20);

            dateTimePickerFrom = new DateTimePicker();
            dateTimePickerFrom.Location = new Point(50, 57);
            dateTimePickerFrom.Size = new Size(150, 25);
            dateTimePickerFrom.Value = DateTime.Today.AddDays(-7);

            label4 = new Label();
            label4.Text = "По:";
            label4.Location = new Point(210, 60);
            label4.Size = new Size(40, 20);

            dateTimePickerTo = new DateTimePicker();
            dateTimePickerTo.Location = new Point(250, 57);
            dateTimePickerTo.Size = new Size(150, 25);
            dateTimePickerTo.Value = DateTime.Today;

            btnCalculateRevenue = new Button();
            btnCalculateRevenue.Text = "Рассчитать выручку";
            btnCalculateRevenue.Size = new Size(150, 35);
            btnCalculateRevenue.Location = new Point(420, 50);
            btnCalculateRevenue.Click += BtnCalculateRevenue_Click;

            btnGenerateReport = new Button();
            btnGenerateReport.Text = "Сформировать отчет";
            btnGenerateReport.Size = new Size(150, 35);
            btnGenerateReport.Location = new Point(580, 50);
            btnGenerateReport.Click += BtnGenerateReport_Click;

            Panel panelResults = new Panel();
            panelResults.Dock = DockStyle.Top;
            panelResults.Height = 100;
            panelResults.Top = 120;
            panelResults.BorderStyle = BorderStyle.FixedSingle;
            panelResults.Padding = new Padding(20);

            lblTotalRevenue = new Label();
            lblTotalRevenue.Text = "Общая выручка: 0 руб.";
            lblTotalRevenue.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTotalRevenue.ForeColor = Color.Green;
            lblTotalRevenue.Location = new Point(20, 20);
            lblTotalRevenue.Size = new Size(400, 30);

            // Добавляем элементы на вкладки
            panelSalesButtons.Controls.Add(btnAddSale);
            panelSalesButtons.Controls.Add(btnEditSale);
            panelSalesButtons.Controls.Add(btnDeleteSale);
            tabSales.Controls.Add(dataGridViewSales);
            tabSales.Controls.Add(panelSalesButtons);

            panelCashiersButtons.Controls.Add(btnAddCashier);
            panelCashiersButtons.Controls.Add(btnEditCashier);
            panelCashiersButtons.Controls.Add(btnDeleteCashier);
            tabCashiers.Controls.Add(dataGridViewCashiers);
            tabCashiers.Controls.Add(panelCashiersButtons);

            panelFilters.Controls.Add(label1);
            panelFilters.Controls.Add(comboBoxCashierFilter);
            panelFilters.Controls.Add(label2);
            panelFilters.Controls.Add(comboBoxProductFilter);
            panelFilters.Controls.Add(label3);
            panelFilters.Controls.Add(dateTimePickerFrom);
            panelFilters.Controls.Add(label4);
            panelFilters.Controls.Add(dateTimePickerTo);
            panelFilters.Controls.Add(btnCalculateRevenue);
            panelFilters.Controls.Add(btnGenerateReport);

            panelResults.Controls.Add(lblTotalRevenue);

            tabReports.Controls.Add(panelResults);
            tabReports.Controls.Add(panelFilters);

            tabControlMain.TabPages.Add(tabSales);
            tabControlMain.TabPages.Add(tabCashiers);
            tabControlMain.TabPages.Add(tabReports);

            this.Controls.Add(tabControlMain);

            // Обработчики событий для DataGridViews
            dataGridViewSales.SelectionChanged += (s, e) =>
            {
                bool hasSelection = dataGridViewSales.SelectedRows.Count > 0;
                btnEditSale.Enabled = hasSelection;
                btnDeleteSale.Enabled = hasSelection;
            };

            dataGridViewCashiers.SelectionChanged += (s, e) =>
            {
                bool hasSelection = dataGridViewCashiers.SelectedRows.Count > 0;
                btnEditCashier.Enabled = hasSelection;
                btnDeleteCashier.Enabled = hasSelection;
            };
        }

        private void AddTestData()
        {
            // Добавляем тестовых кассиров
            cashiers.Add(new ClassCashier
            {
                Id = nextCashierId++,
                FullName = "Иванов Иван Иванович",
                CashRegister = "Касса 1",
                Shift = "Утренняя"
            });

            cashiers.Add(new ClassCashier
            {
                Id = nextCashierId++,
                FullName = "Петров Петр Петрович",
                CashRegister = "Касса 2",
                Shift = "Дневная"
            });

            cashiers.Add(new ClassCashier
            {
                Id = nextCashierId++,
                FullName = "Сидорова Анна Сергеевна",
                CashRegister = "Касса 3",
                Shift = "Вечерняя"
            });

            // Добавляем тестовые продажи
            sales.Add(new ClassFormSales
            {
                Id = nextSaleId++,
                Date = DateTime.Today,
                CashierId = 1,
                Product = "Хлеб",
                Quantity = 2,
                Price = 50
            });

            sales.Add(new ClassFormSales
            {
                Id = nextSaleId++,
                Date = DateTime.Today,
                CashierId = 1,
                Product = "Молоко",
                Quantity = 1,
                Price = 80
            });

            sales.Add(new ClassFormSales
            {
                Id = nextSaleId++,
                Date = DateTime.Today.AddDays(-1),
                CashierId = 2,
                Product = "Колбаса",
                Quantity = 1,
                Price = 300
            });

            sales.Add(new ClassFormSales
            {
                Id = nextSaleId++,
                Date = DateTime.Today.AddDays(-1),
                CashierId = 3,
                Product = "Сыр",
                Quantity = 2,
                Price = 250
            });

            sales.Add(new ClassFormSales
            {
                Id = nextSaleId++,
                Date = DateTime.Today.AddDays(-2),
                CashierId = 1,
                Product = "Вода",
                Quantity = 3,
                Price = 60
            });

            // Обновляем фильтры
            UpdateCashierFilter();
            UpdateTotalRevenue();
        }

        private void UpdateCashierFilter()
        {
            comboBoxCashierFilter.Items.Clear();
            comboBoxCashierFilter.Items.Add("Все кассиры");
            foreach (var cashier in cashiers)
            {
                comboBoxCashierFilter.Items.Add(cashier.FullName);
            }
            if (comboBoxCashierFilter.Items.Count > 0)
                comboBoxCashierFilter.SelectedIndex = 0;
        }

        private void UpdateTotalRevenue()
        {
            decimal totalRevenue = sales.Sum(s => s.Total);
            lblTotalRevenue.Text = $"Общая выручка: {totalRevenue:N2} руб.";
        }

        private void RefreshDataGridViews()
        {
            // Обновляем DataGridView продаж
            dataGridViewSales.Rows.Clear();
            foreach (var sale in sales.OrderByDescending(s => s.Date).ThenBy(s => s.Id))
            {
                var cashier = cashiers.FirstOrDefault(c => c.Id == sale.CashierId);
                string cashierName = cashier != null ? cashier.FullName : "Неизвестно";

                dataGridViewSales.Rows.Add(
                    sale.Id,
                    sale.Date.ToString("dd.MM.yyyy"),
                    cashierName,
                    sale.Product,
                    sale.Quantity,
                    sale.Price.ToString("N2"),
                    sale.Total.ToString("N2")
                );
            }

            // Обновляем DataGridView кассиров
            dataGridViewCashiers.Rows.Clear();
            foreach (var cashier in cashiers.OrderBy(c => c.Id))
            {
                dataGridViewCashiers.Rows.Add(
                    cashier.Id,
                    cashier.FullName,
                    cashier.CashRegister,
                    cashier.Shift
                );
            }

            UpdateTotalRevenue();
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void BtnAddSale_Click(object sender, EventArgs e)
        {
            using (var form = new AddEditSaleForm(null, cashiers))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var newSale = form.Sale;
                    newSale.Id = nextSaleId++;
                    sales.Add(newSale);
                    RefreshDataGridViews();
                    MessageBox.Show("Продажа успешно добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEditSale_Click(object sender, EventArgs e)
        {
            if (dataGridViewSales.SelectedRows.Count == 0) return;

            int saleId = Convert.ToInt32(dataGridViewSales.SelectedRows[0].Cells["Id"].Value);
            var saleToEdit = sales.FirstOrDefault(s => s.Id == saleId);

            if (saleToEdit != null)
            {
                using (var form = new AddEditSaleForm(saleToEdit, cashiers))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshDataGridViews();
                        MessageBox.Show("Продажа успешно обновлена!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnDeleteSale_Click(object sender, EventArgs e)
        {
            if (dataGridViewSales.SelectedRows.Count == 0) return;

            int saleId = Convert.ToInt32(dataGridViewSales.SelectedRows[0].Cells["Id"].Value);

            var result = MessageBox.Show("Вы уверены, что хотите удалить эту продажу?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var saleToDelete = sales.FirstOrDefault(s => s.Id == saleId);
                if (saleToDelete != null)
                {
                    sales.Remove(saleToDelete);
                    RefreshDataGridViews();
                    MessageBox.Show("Продажа успешно удалена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnAddCashier_Click(object sender, EventArgs e)
        {
            using (var form = new AddEditCashierForm(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var newCashier = form.Cashier;
                    newCashier.Id = nextCashierId++;
                    cashiers.Add(newCashier);
                    RefreshDataGridViews();
                    UpdateCashierFilter();
                    MessageBox.Show("Кассир успешно добавлен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnEditCashier_Click(object sender, EventArgs e)
        {
            if (dataGridViewCashiers.SelectedRows.Count == 0) return;

            int cashierId = Convert.ToInt32(dataGridViewCashiers.SelectedRows[0].Cells["Id"].Value);
            var cashierToEdit = cashiers.FirstOrDefault(c => c.Id == cashierId);

            if (cashierToEdit != null)
            {
                using (var form = new AddEditCashierForm(cashierToEdit))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        RefreshDataGridViews();
                        UpdateCashierFilter();
                        MessageBox.Show("Кассир успешно обновлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void BtnDeleteCashier_Click(object sender, EventArgs e)
        {
            if (dataGridViewCashiers.SelectedRows.Count == 0) return;

            int cashierId = Convert.ToInt32(dataGridViewCashiers.SelectedRows[0].Cells["Id"].Value);

            // Проверяем, есть ли связанные продажи
            bool hasRelatedSales = sales.Any(s => s.CashierId == cashierId);
            if (hasRelatedSales)
            {
                MessageBox.Show("Невозможно удалить кассира, так как у него есть связанные продажи!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить этого кассира?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var cashierToDelete = cashiers.FirstOrDefault(c => c.Id == cashierId);
                if (cashierToDelete != null)
                {
                    cashiers.Remove(cashierToDelete);
                    RefreshDataGridViews();
                    UpdateCashierFilter();
                    MessageBox.Show("Кассир успешно удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnCalculateRevenue_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем даты с корректной обработкой времени
                DateTime fromDate = dateTimePickerFrom.Value.Date;
                DateTime toDate = dateTimePickerTo.Value.Date.AddDays(1).AddSeconds(-1); // Конец дня

                // Начинаем с полного списка продаж
                IEnumerable<ClassFormSales> filteredSales = sales;

                // Фильтрация по дате
                filteredSales = filteredSales.Where(s => s.Date >= fromDate && s.Date <= toDate);

                // Фильтрация по кассиру (если выбран не "Все кассиры")
                if (comboBoxCashierFilter.SelectedIndex > 0 && comboBoxCashierFilter.SelectedItem != null)
                {
                    string selectedCashierName = comboBoxCashierFilter.SelectedItem.ToString();
                    var selectedCashier = cashiers.FirstOrDefault(c => c.FullName == selectedCashierName);

                    if (selectedCashier != null)
                    {
                        filteredSales = filteredSales.Where(s => s.CashierId == selectedCashier.Id);
                    }
                }

                // Фильтрация по товару (если выбран не "Все товары")
                if (comboBoxProductFilter.SelectedIndex > 0 && comboBoxProductFilter.SelectedItem != null)
                {
                    string selectedProduct = comboBoxProductFilter.SelectedItem.ToString();
                    if (selectedProduct != "Все товары")
                    {
                        filteredSales = filteredSales.Where(s => s.Product == selectedProduct);
                    }
                }

                // Вычисляем общую выручку
                decimal totalRevenue = filteredSales.Sum(s => s.Total);

                // Получаем информацию о фильтрах для сообщения
                string cashierFilter = comboBoxCashierFilter.SelectedIndex > 0 ?
                    comboBoxCashierFilter.SelectedItem.ToString() : "Все кассиры";

                string productFilter = comboBoxProductFilter.SelectedIndex > 0 ?
                    comboBoxProductFilter.SelectedItem.ToString() : "Все товары";

                // Обновляем метку
                lblTotalRevenue.Text = $"Выручка за период: {totalRevenue:N2} руб.";

                // Показываем информацию о результатах
                MessageBox.Show($"Выручка успешно рассчитана!\n\n" +
                              $"Период: {fromDate:dd.MM.yyyy} - {dateTimePickerTo.Value:dd.MM.yyyy}\n" +
                              $"Кассир: {cashierFilter}\n" +
                              $"Товар: {productFilter}\n" +
                              $"Количество продаж: {filteredSales.Count()}\n" +
                              $"Общая сумма: {totalRevenue:N2} руб.",
                              "Результаты расчета",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расчете выручки: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            // Получаем значения фильтров
            string cashierFilter = comboBoxCashierFilter.SelectedIndex > 0 ?
                comboBoxCashierFilter.SelectedItem.ToString() : "Все кассиры";

            string productFilter = comboBoxProductFilter.SelectedIndex > 0 ?
                comboBoxProductFilter.SelectedItem.ToString() : "Все товары";

            using (var reportForm = new ReportForm(sales, cashiers,
                   dateTimePickerFrom.Value, dateTimePickerTo.Value,
                   cashierFilter, productFilter))
            {
                reportForm.ShowDialog();
            }
        }
    }

    /// <summary>
    /// Добавление/редактирование продажи
    /// </summary>
    public class AddEditSaleForm : Form
    {
        private ClassFormSales sale;
        private List<ClassCashier> cashiers;

        private DateTimePicker dateTimePicker;
        private ComboBox comboBoxCashier;
        private ComboBox comboBoxProduct;
        private NumericUpDown numericQuantity;
        private NumericUpDown numericPrice;
        private Button btnSave;
        private Button btnCancel;
        private Label lblTotal;

        public ClassFormSales Sale => sale;

        public AddEditSaleForm(ClassFormSales existingSale, List<ClassCashier> cashiersList)
        {
            this.cashiers = cashiersList;
            this.sale = existingSale ?? new ClassFormSales();
            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            this.Text = sale.Id == 0 ? "Добавить продажу" : "Редактировать продажу";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Дата продажи
            Label lblDate = new Label { Text = "Дата продажи:", Location = new Point(20, 20), Size = new Size(120, 20) };
            dateTimePicker = new DateTimePicker
            {
                Location = new Point(150, 17),
                Size = new Size(200, 25),
                Value = DateTime.Now
            };

            // Кассир
            Label lblCashier = new Label { Text = "Кассир:", Location = new Point(20, 60), Size = new Size(120, 20) };
            comboBoxCashier = new ComboBox
            {
                Location = new Point(150, 57),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var cashier in cashiers)
            {
                comboBoxCashier.Items.Add(cashier.FullName);
            }

            // Товар
            Label lblProduct = new Label { Text = "Товар:", Location = new Point(20, 100), Size = new Size(120, 20) };
            comboBoxProduct = new ComboBox
            {
                Location = new Point(150, 97),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDown
            };
            comboBoxProduct.Items.AddRange(new string[] { "Хлеб", "Молоко", "Колбаса", "Сыр", "Вода", "Чай", "Кофе", "Сахар" });

            // Количество
            Label lblQuantity = new Label { Text = "Количество:", Location = new Point(20, 140), Size = new Size(120, 20) };
            numericQuantity = new NumericUpDown
            {
                Location = new Point(150, 137),
                Size = new Size(100, 25),
                Minimum = 1,
                Maximum = 1000,
                Value = 1
            };

            // Цена
            Label lblPrice = new Label { Text = "Цена:", Location = new Point(20, 180), Size = new Size(120, 20) };
            numericPrice = new NumericUpDown
            {
                Location = new Point(150, 177),
                Size = new Size(100, 25),
                Minimum = 0.01m,
                Maximum = 100000,
                DecimalPlaces = 2,
                Value = 1.00m
            };

            // Итоговая сумма
            Label lblTotalLabel = new Label { Text = "Итоговая сумма:", Location = new Point(20, 220), Size = new Size(120, 20) };
            lblTotal = new Label
            {
                Text = "0.00 руб.",
                Location = new Point(150, 217),
                Size = new Size(120, 20),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Green
            };

            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(150, 260),
                Size = new Size(100, 30),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(260, 260),
                Size = new Size(100, 30),
                DialogResult = DialogResult.Cancel
            };

            // Добавляем элементы на форму
            this.Controls.AddRange(new Control[]
            {
                lblDate, dateTimePicker,
                lblCashier, comboBoxCashier,
                lblProduct, comboBoxProduct,
                lblQuantity, numericQuantity,
                lblPrice, numericPrice,
                lblTotalLabel, lblTotal,
                btnSave, btnCancel
            });

            // Обработчики для расчета суммы
            numericQuantity.ValueChanged += CalculateTotal;
            numericPrice.ValueChanged += CalculateTotal;
        }
        /// <summary>
        /// Загрузка информации продуктов
        /// </summary>
        private void LoadData()
        {
            if (sale.Id != 0)
            {
                dateTimePicker.Value = sale.Date;

                var cashier = cashiers.FirstOrDefault(c => c.Id == sale.CashierId);
                if (cashier != null)
                    comboBoxCashier.SelectedItem = cashier.FullName;

                comboBoxProduct.Text = sale.Product;
                numericQuantity.Value = sale.Quantity;
                numericPrice.Value = sale.Price;
            }
            else
            {
                if (comboBoxCashier.Items.Count > 0)
                    comboBoxCashier.SelectedIndex = 0;
                if (comboBoxProduct.Items.Count > 0)
                    comboBoxProduct.SelectedIndex = 0;
            }
            CalculateTotal(null, EventArgs.Empty);
        }
        /// <summary>
        /// Считает цену продуктов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateTotal(object sender, EventArgs e)
        {
            decimal total = numericQuantity.Value * numericPrice.Value;
            lblTotal.Text = $"{total:N2} руб.";
        }
        /// <summary>
        /// Показывает цену, кассира и товаров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxCashier.Text))
            {
                MessageBox.Show("Выберите кассира!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(comboBoxProduct.Text))
            {
                MessageBox.Show("Введите товар!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedCashier = cashiers.FirstOrDefault(c => c.FullName == comboBoxCashier.Text);
            if (selectedCashier == null)
            {
                MessageBox.Show("Выбранный кассир не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (numericPrice.Value <= 0)
            {
                MessageBox.Show("Цена должна быть больше 0!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (numericQuantity.Value <= 0)
            {
                MessageBox.Show("Количество должно быть больше 0!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sale.Date = dateTimePicker.Value.Date;
            sale.CashierId = selectedCashier.Id;
            sale.Product = comboBoxProduct.Text;
            sale.Quantity = (int)numericQuantity.Value;
            sale.Price = numericPrice.Value;
        }
    }

    /// <summary>
    /// Добавление/редактирование кассира
    /// </summary>
    public class AddEditCashierForm : Form
    {
        private ClassCashier cashier;

        private TextBox textBoxFullName;
        private ComboBox comboBoxCashRegister;
        private ComboBox comboBoxShift;
        private Button btnSave;
        private Button btnCancel;

        public ClassCashier Cashier => cashier;

        public AddEditCashierForm(ClassCashier existingCashier)
        {
            this.cashier = existingCashier ?? new ClassCashier();
            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            this.Text = cashier.Id == 0 ? "Добавить кассира" : "Редактировать кассира";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // ФИО
            Label lblFullName = new Label { Text = "ФИО:", Location = new Point(20, 20), Size = new Size(120, 20) };
            textBoxFullName = new TextBox { Location = new Point(150, 17), Size = new Size(200, 25) };

            // Номер кассы
            Label lblCashRegister = new Label { Text = "Номер кассы:", Location = new Point(20, 60), Size = new Size(120, 20) };
            comboBoxCashRegister = new ComboBox
            {
                Location = new Point(150, 57),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxCashRegister.Items.AddRange(new string[] { "Касса 1", "Касса 2", "Касса 3", "Касса 4", "Касса 5", "Касса 6" });

            // Смена
            Label lblShift = new Label { Text = "Смена:", Location = new Point(20, 100), Size = new Size(120, 20) };
            comboBoxShift = new ComboBox
            {
                Location = new Point(150, 97),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxShift.Items.AddRange(new string[] { "Утренняя", "Дневная", "Вечерняя", "Ночная" });

            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(150, 150),
                Size = new Size(100, 30),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(260, 150),
                Size = new Size(100, 30),
                DialogResult = DialogResult.Cancel
            };

            // Добавляем элементы на форму
            this.Controls.AddRange(new Control[]
            {
                lblFullName, textBoxFullName,
                lblCashRegister, comboBoxCashRegister,
                lblShift, comboBoxShift,
                btnSave, btnCancel
            });
        }
        /// <summary>
        /// Загружает информацию о кассире
        /// </summary>
        private void LoadData()
        {
            if (cashier.Id != 0)
            {
                textBoxFullName.Text = cashier.FullName;
                comboBoxCashRegister.SelectedItem = cashier.CashRegister;
                comboBoxShift.SelectedItem = cashier.Shift;
            }
            else
            {
                if (comboBoxCashRegister.Items.Count > 0)
                    comboBoxCashRegister.SelectedIndex = 0;
                if (comboBoxShift.Items.Count > 0)
                    comboBoxShift.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Показывают информацию о выборе смены и номере кассы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFullName.Text))
            {
                MessageBox.Show("Введите ФИО кассира!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxCashRegister.SelectedItem == null)
            {
                MessageBox.Show("Выберите номер кассы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxShift.SelectedItem == null)
            {
                MessageBox.Show("Выберите смену!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cashier.FullName = textBoxFullName.Text.Trim();
            cashier.CashRegister = comboBoxCashRegister.SelectedItem.ToString();
            cashier.Shift = comboBoxShift.SelectedItem.ToString();
        }
    }

    /// <summary>
    /// Форма для просмотра отчета
    /// </summary>
    public class ReportForm : Form
    {
        private DataGridView dataGridViewReport;
        private Label lblReportSummary;
        private Button btnPrint;
        private Button btnExport;
        /// <summary>
        /// Класс отчета
        /// </summary>
        /// <param name="sales"></param>
        /// <param name="cashiers"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="cashierFilter"></param>
        /// <param name="productFilter"></param>
        public ReportForm(List<ClassFormSales> sales, List<ClassCashier> cashiers,
                         DateTime fromDate, DateTime toDate,
                         string cashierFilter, string productFilter)
        {
            InitializeComponents();
            GenerateReport(sales, cashiers, fromDate, toDate, cashierFilter, productFilter);
        }

        private void InitializeComponents()
        {
            this.Text = "Отчет по продажам";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;

            dataGridViewReport = new DataGridView();
            dataGridViewReport.Dock = DockStyle.Fill;
            dataGridViewReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewReport.ReadOnly = true;
            dataGridViewReport.AllowUserToAddRows = false;

            Panel panelTop = new Panel();
            panelTop.Dock = DockStyle.Top;
            panelTop.Height = 80;
            panelTop.Padding = new Padding(10);
            panelTop.BorderStyle = BorderStyle.FixedSingle;

            lblReportSummary = new Label();
            lblReportSummary.Dock = DockStyle.Fill;
            lblReportSummary.Font = new Font("Arial", 10);
            lblReportSummary.TextAlign = ContentAlignment.MiddleLeft;

            Panel panelBottom = new Panel();
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Height = 60;
            panelBottom.Padding = new Padding(10);

            btnPrint = new Button { Text = "Печать", Size = new Size(120, 35) };
            btnPrint.Click += (s, e) => MessageBox.Show("Функция печати в разработке", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnExport = new Button { Text = "Экспорт в Excel", Size = new Size(120, 35), Left = 140 };
            btnExport.Click += (s, e) => MessageBox.Show("Функция экспорта в разработке", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            panelTop.Controls.Add(lblReportSummary);
            panelBottom.Controls.Add(btnPrint);
            panelBottom.Controls.Add(btnExport);

            this.Controls.Add(dataGridViewReport);
            this.Controls.Add(panelTop);
            this.Controls.Add(panelBottom);
        }
        /// <summary>
        /// Отчет по продажам
        /// </summary>
        /// <param name="sales"></param>
        /// <param name="cashiers"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="cashierFilter"></param>
        /// <param name="productFilter"></param>
        private void GenerateReport(List<ClassFormSales> sales, List<ClassCashier> cashiers,
                                   DateTime fromDate, DateTime toDate,
                                   string cashierFilter, string productFilter)
        {
            // Столбцы отчета
            dataGridViewReport.Columns.Clear();
            dataGridViewReport.Columns.Add("Date", "Дата продажи");
            dataGridViewReport.Columns.Add("Cashier", "Кассир");
            dataGridViewReport.Columns.Add("Product", "Товар");
            dataGridViewReport.Columns.Add("Quantity", "Кол-во");
            dataGridViewReport.Columns.Add("Price", "Цена");
            dataGridViewReport.Columns.Add("Total", "Сумма");

            dataGridViewReport.Columns["Price"].DefaultCellStyle.Format = "N2";
            dataGridViewReport.Columns["Total"].DefaultCellStyle.Format = "N2";

            // Корректно обрабатываем даты
            DateTime from = fromDate.Date;
            DateTime to = toDate.Date.AddDays(1).AddSeconds(-1); // Конец дня

            // Фильтруем продажи
            var filteredSales = sales.Where(s => s.Date >= from && s.Date <= to);

            // Фильтрация по кассиру
            if (!string.IsNullOrEmpty(cashierFilter) && cashierFilter != "Все кассиры")
            {
                var cashier = cashiers.FirstOrDefault(c => c.FullName == cashierFilter);
                if (cashier != null)
                {
                    filteredSales = filteredSales.Where(s => s.CashierId == cashier.Id);
                }
            }

            // Фильтрация по товару
            if (!string.IsNullOrEmpty(productFilter) && productFilter != "Все товары")
            {
                filteredSales = filteredSales.Where(s => s.Product == productFilter);
            }

            // Добавляем строки с данными
            decimal totalRevenue = 0;
            int totalQuantity = 0;
            var orderedSales = filteredSales.OrderBy(s => s.Date).ThenBy(s => s.CashierId);

            foreach (var sale in orderedSales)
            {
                var cashier = cashiers.FirstOrDefault(c => c.Id == sale.CashierId);
                string cashierName = cashier != null ? cashier.FullName : "Неизвестно";

                dataGridViewReport.Rows.Add(
                    sale.Date.ToString("dd.MM.yyyy"),
                    cashierName,
                    sale.Product,
                    sale.Quantity,
                    sale.Price,
                    sale.Total
                );

                totalRevenue += sale.Total;
                totalQuantity += sale.Quantity;
            }

            // Добавляем итоговую строку
            if (filteredSales.Any())
            {
                dataGridViewReport.Rows.Add(
                    "ИТОГО:",
                    "",
                    "",
                    totalQuantity,
                    "",
                    totalRevenue
                );

                var lastRow = dataGridViewReport.Rows[dataGridViewReport.Rows.Count - 1];
                lastRow.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                lastRow.DefaultCellStyle.BackColor = Color.LightYellow;
            }

            // Обновляем информацию в заголовке
            string filterInfo = "";
            if (!string.IsNullOrEmpty(cashierFilter) && cashierFilter != "Все кассиры")
                filterInfo += $" | Кассир: {cashierFilter}";
            if (!string.IsNullOrEmpty(productFilter) && productFilter != "Все товары")
                filterInfo += $" | Товар: {productFilter}";

            lblReportSummary.Text = $"Отчет за период: {fromDate:dd.MM.yyyy} - {toDate:dd.MM.yyyy}{filterInfo}\n" +
                                   $"Количество продаж: {filteredSales.Count()} | " +
                                   $"Общая выручка: {totalRevenue:N2} руб. | " +
                                   $"Общее количество товаров: {totalQuantity} шт.";
        }
    }
}