using MySql.Data.MySqlClient;
using System.ComponentModel;
using Finance_Tracker.Models;

namespace Finance_Tracker.Views
{
    public partial class MainPage : ContentPage
    {
        private string connectionString = "Server=51.20.113.113;Port=3306;Database=finance_tracker;uid=finance_user;pwd=finance6879;";

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
                    string query = "SELECT Description, Amount, Time FROM expense_income";

                    //creating mysql command with the querry above
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute the query
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var expenses = new List<ExpenseIncome>();

                        while (await reader.ReadAsync())
                        {
                            //Handle nulllll
                            var description = reader.IsDBNull(0) ? "No Description" : reader.GetString(0);
                            var amount = (float)reader.GetDouble(1);
                            var time = reader.GetDateTime(2);

                            expenses.Add(new ExpenseIncome
                            {
                                Description = description,
                                Amount = amount,
                                Time = time,
                            });
                        }

                        //Bind the results to the listview
                        expensesListView.ItemsSource = expenses;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }
    }

}
