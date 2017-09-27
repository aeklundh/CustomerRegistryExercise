using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace albin_eklundh_registry.Extensions
{
    public static class DataReaderExtensions
    {

        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string columnName)
        {
            int column = reader.GetOrdinal(columnName);
            return reader.IsDBNull(column) ? null : (DateTime?)reader.GetDateTime(column);
        }
    }
}
