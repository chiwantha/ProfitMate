using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.Office.Interop.Access.Dao;
using System.Windows.Forms;

namespace ProfitMate
{
    internal class Connection
    {
        public SqlConnection my_conn()
        {
            string Constr = "Data Source="+ Get_path() +";Initial Catalog=HMS;User id=sa;";
            SqlConnection con = new SqlConnection(Constr);
            return con;
        }

        public string Get_path()
        {
            string registryPath = "Software\\k-chord";

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);

                if (key != null)
                {
                    string registryValue = key.GetValue("Path") as string;

                    if (registryValue != null)
                    {
                        return registryValue;
                    }
                }
            }
            catch (Exception) { }

            return "";
        }

        public byte[] getphoto(string path)
        {
            byte[] imageBinary = null;

            // Load the original image
            Image originalImage = Image.FromFile(path);

            // Define the desired width and height for the resized image
            int desiredWidth = 400;  // Adjust this to your preference
            int desiredHeight = 400; // Adjust this to your preference

            // Create a new bitmap with the desired size
            Bitmap resizedImage = new Bitmap(originalImage, desiredWidth, desiredHeight);

            // Convert the resized image to a memory stream
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Adjust the format as needed

                // Convert the memory stream to a binary array
                imageBinary = ms.ToArray();
            }

            return imageBinary;
        }

        public byte[] putphoto(System.Drawing.Image backgroundImage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                backgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // You can use a different image format if needed
                return ms.ToArray();
            }
        }

        private async Task<string[]> Get_Api(string api_id)
        {
            string[] result = null;
            SqlConnection con = my_conn();

            try
            {
                await con.OpenAsync();

                string apisettiongs = "select * from Api where id='" + api_id +"'";
                SqlCommand cmd = new SqlCommand(apisettiongs, con);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    result = new string[3]; // Assuming there are 3 columns to retrieve

                    // Store the values in the array
                    result[0] = reader["apikey"].ToString();
                    result[1] = reader["apiscrect"].ToString();
                    result[2] = reader["state"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            // Return the result array
            return result;
        }

        public async Task<string> sms(string recipient, string message)
        {

            string[] api_values = await Get_Api("S");

            if (api_values[2].ToString() == "0")
            {
                return "";
            }

            if (api_values[0] == "" || api_values[1] == "")
            {
                MessageBox.Show("Please Update APi Settings !", "ApiError", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }


            string strSendData = String.Format("{{\"recipient\":\"{0}\",\"sender_id\":\"  " + api_values[1].ToString().Trim() +"  \",\"message\":\"{1}\"}}", recipient, message);

            using (HttpClient sms = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://sms.send.lk/api/v3/sms/send");
                request.Headers.Add("accept", "application/json");
                request.Headers.Add("authorization", "Bearer " + api_values[0].ToString().Trim());
                request.Headers.Add("cache-control", "no-cache");

                request.Content = new StringContent(strSendData, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await sms.SendAsync(request);

                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        return responseString;
                    }
                    else
                    {
                        // Handle non-successful response
                        return "Error: " + response.ReasonPhrase;
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle exception (if any) here
                    return "Error: " + ex.Message;
                }
            }
        }

        public async Task<string> GetSMSUnitDetails()
        {
            string[] api_values = await Get_Api("S");

            if (api_values[2].ToString() == "0")
            {
                return "No Inc.";
            }

            if (api_values[0] == "" || api_values[1] == "")
            {
                MessageBox.Show("Please Update APi Settings !", "ApiError", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "No Inc.";
            }

            using (HttpClient sms = new HttpClient())
            {
                sms.DefaultRequestHeaders.Add("accept", "application/json");
                sms.DefaultRequestHeaders.Add("authorization", "Bearer " + api_values[0].ToString().Trim());

                try
                {
                    byte[] responseData = sms.GetAsync("https://sms.send.lk/api/v3/balance")
                                                .Result.Content.ReadAsByteArrayAsync().Result;

                    string responseString = Encoding.UTF8.GetString(responseData);

                    // Parse JSON response using regular expressions
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("\"remaining_unit\":\"([\\d,]+)\"");
                    System.Text.RegularExpressions.Match match = regex.Match(responseString);

                    if (match.Success)
                    {
                        string remainingUnits = match.Groups[1].Value.Replace(",", ""); // Remove commas from the number
                        return "Remaining SMS units: " + remainingUnits;
                    }
                    else
                    {
                        return "Error: Invalid API response";
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle exception (if any) here
                    return "Error: " + ex.Message;
                }
            }
        }

        public async Task<string> GetSMSExpireDetails()
        {
            string[] api_values = await Get_Api("S");

            if (api_values[2].ToString() == "0")
            {
                return "No Inc.";
            }

            if (api_values[0] == "" || api_values[1] == "")
            {
                //MessageBox.Show("Please Update APi Settings !", "ApiError", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "No Inc.";
            }

            using (HttpClient sms = new HttpClient())
            {
                sms.DefaultRequestHeaders.Add("accept", "application/json");
                sms.DefaultRequestHeaders.Add("authorization", "Bearer " + api_values[0].ToString().Trim());

                try
                {
                    byte[] responseData = sms.GetAsync("https://sms.send.lk/api/v3/balance")
                                                .Result.Content.ReadAsByteArrayAsync().Result;

                    string responseString = Encoding.UTF8.GetString(responseData);

                    // Parse JSON response using regular expressions for expiry date
                    System.Text.RegularExpressions.Regex expiryRegex = new System.Text.RegularExpressions.Regex("\"expired_on\":\"([^\"]+)\"");
                    System.Text.RegularExpressions.Match expiryMatch = expiryRegex.Match(responseString);

                    if (expiryMatch.Success)
                    {
                        string expiryDate = expiryMatch.Groups[1].Value;
                        return "Expiry Date: " + expiryDate;
                    }
                    else
                    {
                        return "Error: Expiry date not found in the API response.";
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle exception (if any) here
                    return "Error: " + ex.Message;
                }
            }
        }

        internal object GetSMSUnitDetailsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
