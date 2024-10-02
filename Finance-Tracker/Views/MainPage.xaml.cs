using MySql.Data.MySqlClient;
using System.ComponentModel;
using Finance_Tracker.Models;

namespace Finance_Tracker.Views
{
    public partial class MainPage : ContentPage
    {
        private string connectionString = "Server=51.20.113.113;Port=3306;Database=finance_tracker;uid=finance_user;pwd=finance6879;";
        private List<ExpenseIncome> _expenses; //store data For search
        private bool _isTypeAscending = true;
        private bool _isAmountAscending = true;
        private bool _isDateAscending = true;
        private bool _isCategoryAscending = true;
        private bool _isTagAscending = true;
        private bool _isMethodAscending = true;
        private bool _isDescriptionAscending = true;

        public MainPage()
        {
            InitializeComponent();
            LoadFromDb();
        }

        async void LoadFromDb()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    //opening connection
                    await connection.OpenAsync();

                    //Sql query to fetch the data
                    string query = @"SELECT ei.id, 
                                            ei.Description,
                                            ei.Amount,
                                            ei.Time,
                                            m.method_name,
                                            t.type_name,
                                            tg.tag_name,
                                            c.category_name
                                     FROM 
                                            expense_income ei
                                     JOIN
                                            types t on ei.type_id = t.id
                                     JOIN
                                            methods m on ei.method_id = m.id
                                     JOIN
                                            tags tg on ei.tag_id = tg.id
                                     JOIN
                                            categories c on tg.category_id = c.id";
                                                                    

                    //creating mysql command with the querry above
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute the query
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        _expenses = new List<ExpenseIncome>();

                        while (await reader.ReadAsync())
                        {
                            //Handle nulllll
                            var id = reader.GetInt32(0);
                            var description = reader.IsDBNull(1) ? "No Description" : reader.GetString(1);
                            var amount = (float)reader.GetDouble(2);
                            var time = reader.GetDateTime(3);
                            var method_name = reader.GetString(4);
                            var type_name = reader.GetString(5);
                            var tag_name = reader.GetString(6);
                            var category_name = reader.GetString(7);

                            _expenses.Add(new ExpenseIncome
                            {
                                Id = id,
                                Description = description,
                                Amount = amount,
                                Time = time,
                                MethodName = method_name,
                                TypeName = type_name,
                                TagName = tag_name,
                                CategoryName = category_name,
                            });
                        }

                        //Bind the results to the listview
                        expensesCollectionView.ItemsSource = _expenses;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }

        private void expensesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedExpense = e.CurrentSelection.FirstOrDefault() as ExpenseIncome;

            if (selectedExpense != null)
            {
                // Handle the selection (e.g., show details, delete, etc.)
                DisplayAlert("Selected", $"You selected: {selectedExpense.Description}, Amount: {selectedExpense.Amount}", "OK");
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Checking if the searched value is null or empty
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                //if it is then set the ItemsSource to the original value
                expensesCollectionView.ItemsSource = _expenses;
            }
            else
            {
                var searchTerm = e.NewTextValue.ToLower();

                var filteredExpenses = _expenses.Where(expense =>
                {
                    // Use reflection to search all fields of the object
                    foreach (var prop in expense.GetType().GetProperties())
                    {
                        var value = prop.GetValue(expense)?.ToString()?.ToLower();
                        if (!string.IsNullOrEmpty(value) && value.Contains(searchTerm))
                        {
                            return true;
                        }
                    }
                    return false;
                }).ToList();

                expensesCollectionView.ItemsSource = filteredExpenses;
            }
        }

        #region Headers Tapped
        private void OnTypeHeaderTapped (object sender, EventArgs e)
        {
            if (_isTypeAscending)
            {
                _expenses = _expenses.OrderBy(exp => exp.TypeName).ToList();
            }
            else
            {
                _expenses = _expenses.OrderByDescending(exp => exp.TypeName).ToList();
            }
            _isTypeAscending = !_isTypeAscending;

            expensesCollectionView.ItemsSource = _expenses;
        }
        private void OnAmountHeaderTapped (object sender, EventArgs e)
        {
            if (_isAmountAscending)
            {
                _expenses = _expenses.OrderBy(exp => exp.Amount).ToList();
            }
            else
            {
                _expenses = _expenses.OrderByDescending(exp => exp.Amount).ToList();
            }
            _isAmountAscending = !_isAmountAscending;

            expensesCollectionView.ItemsSource = _expenses;
        }
        private void OnDateHeaderTapped (object sender, EventArgs e)
        {
            if (_isDateAscending)
            {
                _expenses = _expenses.OrderBy(exp => exp.Time).ToList();
            }
            else
            {
                _expenses = _expenses.OrderByDescending(exp => exp.Time).ToList();
            }
            _isDateAscending = !_isDateAscending;

            expensesCollectionView.ItemsSource = _expenses;
        }
        private void OnCategoryHeaderTapped (object sender, EventArgs e)
        {
            if (_isCategoryAscending)
            {
                _expenses = _expenses.OrderBy(exp => exp.CategoryName).ToList();
            }
            else
            {
                _expenses = _expenses.OrderByDescending(exp => exp.CategoryName).ToList();
            }
            _isCategoryAscending = !_isCategoryAscending;

            expensesCollectionView.ItemsSource = _expenses;
        }
        private void OnTagHeaderTapped (object sender, EventArgs e)
        {
            if (_isTagAscending)
            {
                _expenses = _expenses.OrderBy(exp => exp.TagName).ToList();
            }
            else
            {
                _expenses = _expenses.OrderByDescending(exp => exp.TagName).ToList();
            }
            _isTagAscending = !_isTagAscending;

            expensesCollectionView.ItemsSource = _expenses;
        }
        private void OnMethodHeaderTapped (object sender, EventArgs e)
        {
            if (_isMethodAscending)
            {
                _expenses = _expenses.OrderBy(exp => exp.MethodName).ToList();
            }
            else
            {
                _expenses = _expenses.OrderByDescending(exp => exp.MethodName).ToList();
            }
            _isMethodAscending = !_isMethodAscending;

            expensesCollectionView.ItemsSource = _expenses;
        }
        private void OnDescriptionHeaderTapped (object sender, EventArgs e)
        {
            if (_isDescriptionAscending)
            {
                _expenses = _expenses.OrderBy(exp => exp.Description).ToList();
            }
            else
            {
                _expenses = _expenses.OrderByDescending(exp => exp.Description).ToList();
            }
            _isDescriptionAscending = !_isDescriptionAscending;

            expensesCollectionView.ItemsSource = _expenses;
        }
        #endregion

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var expenseItem = button.BindingContext as ExpenseIncome;

            // Implement the logic to navigate to the Edit page
            // Or open a modal to edit the selected item
            DisplayAlert("Edit", $"You clicked edit for {expenseItem.Description}", "OK");

        }
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var expenseItem = button.BindingContext as ExpenseIncome;

            // Implement the logic to delete the selected item
            // Confirm before deleting
            bool delete = await DisplayAlert("Delete", $"Are you sure you want to delete {expenseItem.Description}?", "Yes", "No");
            if (delete)
            {
                // Logic to remove the item from the data source
                // and update the CollectionView
            }
        }
    }

}
