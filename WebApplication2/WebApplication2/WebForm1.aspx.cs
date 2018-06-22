using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public Hashtable h1 = new Hashtable();
        private string accountNo = "111111111";
        protected void button_Click(object sender, EventArgs e)
        {
            h1.Clear();
            Postcode.Lookup obj = new Postcode.Lookup();
            Postcode.InterimResult result;
            Postcode.InterimResults results;

            if (Code.Text.Length >= 3)
            {
                results = obj.ByPostCode(Code.Text.Trim(), accountNo);
                if (results.Results.Length != 0)
                {
                    result = (Postcode.InterimResult)results.Results[0];

                    for (int i = 0; i <= results.Results.Length - 1; i++)
                    {

                        result = (Postcode.InterimResult)results.Results[i];

                        h1.Add(i, result.id);
                        ViewState["h1"] = h1;
                        showList.Items.Add(result.Description);

                    }

                }
            }
        }

        protected void SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Hashtable H2 = new Hashtable();
            H2 = (Hashtable)ViewState["h1"];
            StringBuilder str = new StringBuilder();

            Postcode.Lookup x = new Postcode.Lookup();
            Postcode.AddressResult result;
            Postcode.AddressResults results;

            results = x.AddressByID(H2[showList.SelectedIndex].ToString());
            result = (Postcode.AddressResult)results.Results[0];

            str.Remove(0, str.Length);
            if (result.OrganisationName.Trim() != "") {
                str.Append(result.OrganisationName.Trim() + "<br>");
                TextBox5.Text = result.OrganisationName.Trim();
            }
            if (result.BuildingNumber.Trim() != "0")
            {
                str.Append(result.BuildingNumber.Trim() + " ");

            }
            str.Remove(0, str.Length);
            if (result.SubbuildingName.Trim() != "")
                str.Append(result.SubbuildingName.Trim() + ", ");
            if (result.BuildingName.Trim() != "")
                str.Append(result.BuildingName.Trim() + ", ");
            if (result.ThoroughfareName.Trim() != "")
                str.Append(result.ThoroughfareName.Trim() + ", ");
            str.Append(result.ThoroughfareDescriptor.Trim() + ", ");
            if (result.DependentLocality.Trim() != "") {
                str.Append(result.DependentLocality.Trim());

            }
            TextBox6.Text = str.ToString();
            str.Remove(0, str.Length);
            if (result.Posttown.Trim() != "")
            {
                str.Append(result.Posttown.Trim());
                TextBox7.Text = result.Posttown.Trim();
            }
            str.Remove(0, str.Length);
            if (result.County.Trim() != "")
            {
                str.Append(result.County.Trim());
                TextBox7.Text = result.County.Trim();
            }
            str.Remove(0, str.Length);
            if (result.Outcome.Trim() != "")
                str.Append(result.Outcome.Trim() + " ");
            str.Append(result.Incode.Trim());
            TextBox9.Text = str.ToString();
            spanId.InnerHtml = str.ToString();
        }



        protected void AddToDatabase(object sender, EventArgs e)       {
            string FirstName = TextBox2.Text;
            string LastName = TextBox3.Text;
            string Email = TextBox4.Text;
            string OrganisationName = TextBox5.Text;
            string Address = TextBox6.Text;
            string Town = TextBox7.Text;
            string County = TextBox8.Text;
            string PostCode = TextBox9.Text;
            SqlConnection objSqlConnection = new SqlConnection(WebConfigurationManager.AppSettings["ConnectionString"]);
            
            string sqlQuery = "INSERT INTO  [Table](FirstName,LastName,Email,OrganisationName,Address,Town,County,PostCode) VALUES (@FirstName,@LastName,@Email,@OrganisationName,@Address,@Town,@County,@PostCode)";

            SqlCommand cmd = new SqlCommand(sqlQuery, objSqlConnection); cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@LastName",LastName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@OrganisationName", OrganisationName);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Town", Town);
            cmd.Parameters.AddWithValue("@County",  County);
            cmd.Parameters.AddWithValue("@PostCode", PostCode);
            cmd.Connection = objSqlConnection;
                objSqlConnection.Open();
                cmd.ExecuteNonQuery();
                objSqlConnection.Close();
        }
        protected void FindCoordinates(object sender, EventArgs e)
        {
            
            string url = "http://maps.google.com/maps/api/geocode/xml?address=" + TextBox6.Text + "&sensor=false";
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    DataTable dtCoordinates = new DataTable();
                    dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                    new DataColumn("Address", typeof(string)),
                    new DataColumn("Latitude",typeof(string)),
                    new DataColumn("Longitude",typeof(string)) });
                    if(dsResult.Tables["result"]!=null)
                    foreach (DataRow row in dsResult.Tables["result"].Rows)
                    {
                        string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                        DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                        dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                    }
                    if (dtCoordinates.Rows.Count > 0)
                    {
                        pnlScripts.Visible = true;
                        rptMarkers.DataSource = dtCoordinates;
                        rptMarkers.DataBind();
                    }
                }
            }
        }
    }
}