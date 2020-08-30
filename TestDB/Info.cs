using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDB
{
    class Info : INotifyPropertyChanged
    {

        public string NameOfPlace { get; set; }
        public int CodeFil { get; set; }
        public int CodePred { get; set; }
        public string APILogin { get; set; }
        public string APIPass { get; set; }
        public string User { get; set; }
        public string CompanyGuid { get; set; }
        public string PlaceGuid { get; set; }
        public string APIKey { get; set; }
        public string Master { get; set; }
        public string Title { get; set; }
        public string AppID { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableCollection<Info> GetProducts(string connectionString)
        {
            const string GetProductsQuery = "Select S.CodM,M.Master,S.CodB,K.Title,S.Login,S.Passw,S.GUID,S.GUIDP,S.APIKey,S.AppID,S.Initiator,S.Name" +
          " From Merc_xs S,Master M, Skls K" +
          " Where S.CodM = M.CodM and S.CodB = K.Skl and S.Login <> '' and S.Passw <> '' and S.GUID <> '' and S.GUIDP <> '' and S.APIKey <> ''" +
          " Order by M.Master,K.Title";
            

            var products = new ObservableCollection<Info>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetProductsQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var product = new Info();
                                    product.CodeFil = reader.GetInt16(0);
                                    product.Master = reader.GetString(1);
                                    product.CodePred = reader.GetInt16(2);
                                    product.Title = reader.GetString(3);
                                    product.APILogin = reader.GetString(4);
                                    product.APIPass = reader.GetString(5);
                                    product.CompanyGuid = reader.GetString(6);
                                    product.PlaceGuid = reader.GetString(7);
                                    product.APIKey = reader.GetString(8);
                                    //product.AppID = reader.GetString(9);
                                    product.User = reader.GetString(10);
                                    product.NameOfPlace = reader.GetString(11);
                                    products.Add(product);
                                }
                            }
                        }
                    }
                }
                return products;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }
    }
}
