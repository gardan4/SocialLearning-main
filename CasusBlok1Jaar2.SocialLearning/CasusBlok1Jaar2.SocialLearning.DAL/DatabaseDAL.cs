using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CasusBlok1Jaar2.SocialLearning.DAL
{
    public abstract class DatabaseDAL
    {
        #region Fields
        protected readonly string _connectionString;
        #endregion

        #region Constructors
        internal DatabaseDAL(string connectionString)
        {
            this._connectionString = connectionString;
        }
        #endregion

        #region Methods
        protected void ExecuteQuery(string query, Action<SqlDataReader> readerAction, params SqlParameter[] sqlParameters)
        {
            if (readerAction == null)
                return;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    if (sqlParameters != null)
                        command.Parameters.AddRange(sqlParameters);

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        readerAction(dataReader);

                        dataReader.Close();
                    }

                    command.Dispose();
                }

                conn.Close();
            }
        }
        
        protected string GetHashString(string input)
        {
            byte[] hash;

            using (HashAlgorithm algrthm = SHA256.Create())
                hash = algrthm.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        #endregion
    }
}
