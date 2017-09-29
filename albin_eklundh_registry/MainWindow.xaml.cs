using albin_eklundh_registry.Models;
using albin_eklundh_registry.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace albin_eklundh_registry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();

        public MainWindow()
        {
            InitializeComponent();
            CustomerRepository.GetRecentCustomers().ForEach(x => _customers.Add(x));

            DataContext = _customers;
        }

        //creates a customer object, validates and saves to DB if it's valid
        private void SaveCustomer(object sender, EventArgs e)
        {
            Customer customer = new Customer()
            {
                IsCompany = (bool)IsCompany.IsChecked,
                ContactPerson = ContactPersonInput.Text,
                CompanyName = CompanyNameInput.Text,
                Address = AddressInput.Text,
                PostalCode = PostalCodeInput.Text,
                Area = AreaInput.Text,
                DateOfBirth = DateOfBirthInput.SelectedDate,
                Email = EmailInput.Text,
                Phone = PhoneInput.Text,
                WantsNewsletter = (bool)WantsNewsLetterInput.IsChecked,
                Notes = NotesInput.Text
            };

            if (Validate(customer))
            {
                CustomerRepository.AddCustomer(customer);
                _customers.Clear();
                CustomerRepository.GetRecentCustomers().ForEach(x => _customers.Add(x));

                ClearTextBoxes(RegistryWindow);
            }
        }

        private void GetRecentCustomers(object sender, EventArgs e)
        {
            _customers.Clear();
            CustomerRepository.GetRecentCustomers().ForEach(x => _customers.Add(x));
        }

        private void Search(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SearchInput.Text))
            {
                _customers.Clear();
                CustomerRepository.SearchForCustomer(SearchInput.Text).ForEach(x => _customers.Add(x));
            }
        }

        private void IsPrivate_Checked(object sender, RoutedEventArgs e)
        {
            CompanyNameInput.Text = String.Empty;
        }

        //validates the current customer input using its data annotations and adds potential error messages to the view
        private bool Validate(Customer customer)
        {
            ValidationSummary.Children.Clear();

            ValidationContext validationContext = new ValidationContext(customer, null, null);
            List<System.ComponentModel.DataAnnotations.ValidationResult> results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool valid = Validator.TryValidateObject(customer, validationContext, results);
            if (!valid)
            {
                foreach (var result in results)
                    ValidationSummary.Children.Add(new TextBlock() { Text = $"*{ result.ErrorMessage }" });
            }

            return valid;
        }

        //clears every textbox in the view
        private void ClearTextBoxes(Visual myMainWindow)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(myMainWindow);
            for (int i = 0; i < childrenCount; i++)
            {
                var visualChild = (Visual)VisualTreeHelper.GetChild(myMainWindow, i);
                if (visualChild is TextBox)
                {
                    TextBox tb = (TextBox)visualChild;
                    tb.Clear();
                }
                ClearTextBoxes(visualChild);
            }
        }
    }
}
