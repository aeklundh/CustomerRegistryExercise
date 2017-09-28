using albin_eklundh_registry.Extensions;
using albin_eklundh_registry.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace albin_eklundh_registry.Repositories
{
    public static class CustomerRepository
    {
        private static readonly string _connectionString;

        static CustomerRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        //gets the 100 latest customers
        public static List<Customer> GetRecentCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand getCustomers = new SqlCommand("SELECT TOP 100 * FROM [Customers] ORDER BY [CustomerId] DESC", conn))
            {
                conn.Open();
                using (SqlDataReader reader = getCustomers.ExecuteReader())
                    customers = reader.CreateListOfCustomers();
            }

            return customers;
        }

        //searches for a customer by the Phone or Email columns
        public static List<Customer> SearchForCustomer(string searchKey)
        {
            List<Customer> customers = new List<Customer>();
            SqlCommand searchByEmailOrPhone = new SqlCommand("SELECT TOP 100 * FROM [Customers] WHERE ([Phone] LIKE '%' + @searchKey + '%') OR ([Email] LIKE '%' + @searchKey + '%')");
            searchByEmailOrPhone.Parameters.Add("@searchKey", SqlDbType.VarChar).Value = searchKey;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                searchByEmailOrPhone.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = searchByEmailOrPhone.ExecuteReader())
                    customers = reader.CreateListOfCustomers();

            }

            return customers;
        }

        public static void AddCustomer(Customer customer)
        {
            SqlCommand insertCustomer = new SqlCommand("INSERT INTO [Customers] VALUES (@contactPerson, @companyName, @dateOfBirth, @address, @area, @postalCode, @phone, @email, @wantsNewsletter, @notes)");
            insertCustomer.Parameters.Add("@contactPerson", SqlDbType.NVarChar).Value = customer.ContactPerson;
            insertCustomer.Parameters.Add("@companyName", SqlDbType.NVarChar).Value = customer.IsCompany ? customer.CompanyName : (object)DBNull.Value;
            insertCustomer.Parameters.Add("@dateOfBirth", SqlDbType.SmallDateTime).Value = customer.DateOfBirth ?? (object)DBNull.Value;
            insertCustomer.Parameters.Add("@address", SqlDbType.NVarChar).Value = customer.Address;
            insertCustomer.Parameters.Add("@area", SqlDbType.NVarChar).Value = customer.Area;
            insertCustomer.Parameters.Add("@postalCode", SqlDbType.Char).Value = customer.PostalCode;
            insertCustomer.Parameters.Add("@phone", SqlDbType.VarChar).Value = customer.Phone;
            insertCustomer.Parameters.Add("@email", SqlDbType.VarChar).Value = customer.Email;
            insertCustomer.Parameters.Add("@wantsNewsletter", SqlDbType.Bit).Value = customer.WantsNewsletter;
            insertCustomer.Parameters.Add("@notes", SqlDbType.NVarChar).Value = customer.Notes;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (insertCustomer)
            {
                insertCustomer.Connection = conn;
                conn.Open();

                insertCustomer.ExecuteNonQuery();
            }
        }

        //creates a list of customers using a datareader that has selected all columns from Customers
        private static List<Customer> CreateListOfCustomers(this SqlDataReader reader)
        {
            List<Customer> retVal = new List<Customer>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Customer customer = new Customer()
                    {
                        CustomerId = (int)reader["CustomerId"],
                        CompanyName = reader["CompanyName"].ToString(),
                        ContactPerson = (string)reader["ContactPerson"],
                        DateOfBirth = reader.GetNullableDateTime("DateOfBirth"),
                        Address = (string)reader["Address"],
                        Area = (string)reader["Area"],
                        PostalCode = (string)reader["PostalCode"],
                        Phone = reader["Phone"].ToString(),
                        Email = (string)reader["Email"],
                        WantsNewsletter = (bool)reader["WantsNewsletter"],
                        Notes = reader["Notes"].ToString()
                    };
                    if (String.IsNullOrEmpty(customer.CompanyName))
                        customer.IsCompany = false;
                    else
                        customer.IsCompany = true;

                    retVal.Add(customer);
                }
            }
            
            return retVal;
        }
    }
}
