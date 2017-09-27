using albin_eklundh_registry.Models;
using albin_eklundh_registry.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
            CustomerRepository.GetCustomers().ForEach(x => _customers.Add(x));

            DataContext = _customers;
        }

        private void SaveCustomer(object sender, EventArgs e)
        {
            Customer customer = new Customer()
            {
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
                _customers.Add(customer);
            }
        }

        private void GetCustomers(object sender, EventArgs e)
        {
            _customers.Clear();
            CustomerRepository.GetCustomers().ForEach(x => _customers.Add(x));
        }

        private void Search(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SearchInput.Text))
            {
                _customers.Clear();
                CustomerRepository.SearchForCustomer(SearchInput.Text).ForEach(x => _customers.Add(x));
            }
        }

        private bool Validate(Customer customer)
        {
            ValidationSummary.Children.Clear();

            ValidationContext validationContext = new ValidationContext(customer, null, null);
            List<System.ComponentModel.DataAnnotations.ValidationResult> results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool valid = Validator.TryValidateObject(customer, validationContext, results);
            if (!valid)
            {
                foreach (var result in results)
                {
                    ValidationSummary.Children.Add(new Label() { Content = result });
                }
            }

            return valid;
        }
    }
}
