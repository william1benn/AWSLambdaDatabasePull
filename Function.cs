using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using MySql.Data.MySqlClient;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace pagoTypes
{
    public class Function
    {
        public object FunctionHandler(ILambdaContext context)
        {
            try
            {
                string id = Environment.GetEnvironmentVariable("ID");
                string password = Environment.GetEnvironmentVariable("PASSWORD");
                string server = Environment.GetEnvironmentVariable("SERVER");
                string database = Environment.GetEnvironmentVariable("DATABASE");

                using (var connection = new MySqlConnection($"Server={server};User ID={id};Password={password};Database={database}"))
                {
                    var paymentDatas = new List<PaymentData>();

                    connection.Open();

                    using (var command = new MySqlCommand("SELECT * FROM Pago;", connection))
                    using (var reader = command.ExecuteReader())

                        while (reader.Read())
                        {
                            paymentDatas.Add(new PaymentData { id = reader[0], pagoName = reader[1] });
                        }
                    if (paymentDatas.Count > 0)
                    {
                        return paymentDatas;
                    }
                    else
                    {
                        return "nothing found";
                    }

                }
            } catch (Exception e)
            {
                return e.ToString();
            }
        }

        }
    }
