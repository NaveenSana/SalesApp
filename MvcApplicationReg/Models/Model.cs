using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace MvcApplicationReg.Models
{
    public class UserData
    {
        [Required]
        public string UserId { get; set; }
        public string FisrtName { get; set; }
        public string lastName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string city { get; set; }
        public int PinCode { get; set; }
        public string Role { get; set; }
        public int IsActive { get; set; }
        public string Result { get; set; }
    }

    public class CountryList
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class testidata
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }

    public class fields
    {
        //public int<null> Matched;
        public int? Matched = null;
    }

    public class filevalues
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string InputFileName { get; set; }
        public string Extension { get; set; }
        public DateTime UploadedDate { get; set; }
        public string Path { get; set; }
    }


    public class AdminModel
    {
        string objConn = ConfigurationManager.ConnectionStrings["testDBConn"].ToString();

        public int CreateUser(UserData _objUserData)
        {
            int Status = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("register", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FirstName", _objUserData.FisrtName);
                        //cmd.Parameters.AddWithValue("@userID", _objUserData.UserId);
                        cmd.Parameters.AddWithValue("@LastName", _objUserData.lastName);
                        cmd.Parameters.AddWithValue("@Password", _objUserData.Password);
                        cmd.Parameters.AddWithValue("@Zip", _objUserData.PinCode);
                        cmd.Parameters.AddWithValue("@StreetAdd1", _objUserData.Address);
                        cmd.Parameters.AddWithValue("@State", _objUserData.State);
                        cmd.Parameters.AddWithValue("@City", _objUserData.city);
                        cmd.Parameters.AddWithValue("@Role", _objUserData.Role);
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@ProcResult";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        outPutParameter.Size = 10;
                        cmd.Parameters.Add(outPutParameter);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Status = Convert.ToInt16(outPutParameter.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Status;
        }

        public List<UserData> getallusers()
        {
            List<UserData> userslist = new List<UserData>();
            DataSet dsusers = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("allusers", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            //con.Open();   

                            da.Fill(dsusers);
                            //dsusers.Tables[0].AsEnumerable().AsQueryable();
                            foreach (DataRow dr in dsusers.Tables[0].Rows)
                            {
                                userslist.Add(new UserData
                                {
                                    UserId = dr["userID"].ToString(),
                                    FisrtName = dr["FirstName"].ToString(),
                                    lastName = dr["LastName"].ToString(),
                                    State = dr["State"].ToString(),
                                    PinCode = Convert.ToInt32(dr["ZIPCode"]),
                                    Role = dr["Role"].ToString()
                                });
                            }
                            // con.Close();
                        }
                    }
                }
                return userslist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CountryList> getallcountries()
        {
            List<CountryList> Countrylist = new List<CountryList>();
            DataSet dsusers = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getcountries", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsusers);
                            foreach (DataRow dr in dsusers.Tables[0].Rows)
                            {
                                Countrylist.Add(new CountryList
                                {
                                    CountryName = dr["CountryName"].ToString(),
                                    CountryId = Convert.ToInt32(dr["CountryID"])
                                });
                            }
                        }
                    }
                }
                return Countrylist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int CreateTestimonals(testidata _objUserData)
        {
            int Status = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_testidate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@name", _objUserData.Name);
                        cmd.Parameters.AddWithValue("@email", _objUserData.Email);
                        cmd.Parameters.AddWithValue("@description", _objUserData.Description);
                        con.Open();
                        Status = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Status;
        }

        public List<testidata> GetAllTestimonials()
        {
            try
            {
                List<testidata> testdata = new List<testidata>();
                DataTable dtdata = new DataTable();
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("select * from testimonals where id !=0", con))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtdata);
                            foreach (DataRow dr in dtdata.Rows)
                            {
                                testdata.Add(new testidata
                                {
                                    Name = dr["Name"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    Description = dr["Description"].ToString(),
                                });
                            }
                        }
                        return testdata;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SendEmail(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("sananaveen2@gmail.com", "test");
                mail.Subject = "Open it bro!!!";

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("sananaveen2@gmail.com", "ranks1234");
                NetworkCredential NetworkCred = new NetworkCredential("sananaveen2@gmail.com", "ranks1234");
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = NetworkCred;

                SmtpServer.EnableSsl = true;
                string myString;
                //if (actiontype == "activation link")//Activation Link
                //{
                mail.To.Add(new MailAddress("rajsekhar1209@gmail.com"));//rajsekhar1209@gmail.com
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Images/ActivationLink_Template.html")))
                {
                    myString = reader.ReadToEnd();
                }
                //myString = myString.Replace("{Password}", "sana@123");
                //myString = myString.Replace("{Url}", "");
                //myString = myString.Replace("{img}", @"C:\Users\Naveen\Documents\Visual Studio 2012\Projects\MvcApplicationReg\MvcApplicationReg\Images\IMG_0721.JPG");
                mail.CC.Add(new MailAddress("naveen_kumar@tecnics.com"));
                //mail.CC.Add(new MailAddress("rabia.princess999@gmail.com"));
                string FileName = @"C:\Users\Naveen\Documents\Visual Studio 2012\Projects\MvcApplicationReg\MvcApplicationReg\Images\IMG_0721.JPG";
                mail.Attachments.Add(new Attachment(FileName));
                mail.Body = myString.ToString();
                mail.IsBodyHtml = true;
                SmtpServer.Send(mail);
                //}
                //else if (actiontype == "forgotpwd")//Forgot Password request
                //{
                //    mail.To.Add(new MailAddress(objUser.EMAILID));
                //    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplates/ForgotPassword_Template.html")))
                //    {
                //        myString = reader.ReadToEnd();
                //    }
                //    myString = myString.Replace("{Url}", objUser.ActivationUrl + ConfigurationManager.AppSettings["ForgotPageUrl"].ToString() + "?Token=" + objCom.Encrypt(objUser.EMAILID) + "&Key=" + objUser.ActiveKey);
                //    mail.Body = myString.ToString();
                //    mail.IsBodyHtml = true;
                //    SmtpServer.Send(mail);
                //}
                return 1;
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;
            }
        }


        public int SendExcelReport(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("sananaveen2@gmail.com", "test");
                mail.Subject = "Open it bro!!!";

                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("sananaveen2@gmail.com", "ranks1234");
                NetworkCredential NetworkCred = new NetworkCredential("sananaveen2@gmail.com", "Ranks1234");
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Credentials = NetworkCred;
                SmtpServer.EnableSsl = true;
                string myString;
                mail.To.Add(new MailAddress("naveen_kumar@tecnics.com"));
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Images/ActivationLink_Template.html")))
                {
                    myString = reader.ReadToEnd();
                }
                //mail.CC.Add(new MailAddress("naveen_kumar@tecnics.com"));
                //mail.CC.Add(new MailAddress("naveen_kumar@tecnics.com"));
                //string FileName = @"C:\Users\Naveen\Documents\Visual Studio 2012\Projects\MvcApplicationReg\MvcApplicationReg\Images\IMG_0721.JPG";
                string FileName = @"C:\Users\Naveen\Documents\Visual Studio 2012\Projects\MvcApplicationReg\MvcApplicationReg\bin\Files\SampleReport.xlsx";
                mail.Attachments.Add(new Attachment(FileName));
                mail.Body = myString.ToString();
                mail.IsBodyHtml = true;
                SmtpServer.Send(mail);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public int InsertFile(filevalues _objfiledata)
        {
            int Status = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertFileData", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@Id", 0);
                        cmd.Parameters.AddWithValue("@FileName", _objfiledata.Name);
                        cmd.Parameters.AddWithValue("@FileExtension", _objfiledata.Extension);
                        cmd.Parameters.AddWithValue("@Filepath", _objfiledata.Path);
                        cmd.Parameters.AddWithValue("@UploadDate", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@InputFileName", _objfiledata.InputFileName);
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@ProcResult";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        outPutParameter.Size = 10;
                        cmd.Parameters.Add(outPutParameter);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Status = Convert.ToInt16(outPutParameter.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Status;
        }

        public List<filevalues> getallFiles()
        {
            List<filevalues> Fileslist = new List<filevalues>();
            DataSet dsusers = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    string sql = "select * from FileData";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            //con.Open();   

                            da.Fill(dsusers);
                            //dsusers.Tables[0].AsEnumerable().AsQueryable();
                            foreach (DataRow dr in dsusers.Tables[0].Rows)
                            {
                                Fileslist.Add(new filevalues
                                {
                                    id = Convert.ToInt32(dr["Id"]),
                                    Name = dr["FileName"].ToString(),
                                    Path = dr["FilePath"].ToString(),
                                    InputFileName = dr["InputFileName"].ToString()
                                    //lastName = dr["LastName"].ToString(),
                                    //State = dr["State"].ToString(),
                                    //PinCode = Convert.ToInt32(dr["ZIPCode"])
                                });
                            }
                            // con.Close();
                        }
                    }
                }
                return Fileslist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int deletefile(int id)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    string sql = "delete from filedata where id = " + id + "";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            con.Open();
                            result = cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int checklogin(UserData objuser)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    string sql = "select * from mvcUser where FirstName = '" + objuser.FisrtName + "' and PasswordConfirm = '" + objuser.Password + "'";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            con.Open();
                            result = cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int updateuserrole(UserData objuser)
        {
            UserData _objUser = new UserData();
            int Status = 0;
            try
            {
                //using (SqlConnection con = new SqlConnection(objConn))
                //{
                //    using (SqlCommand cmd = new SqlCommand("SP_UPDATE_USER", con))
                //    {
                //        //cmd.commandtext = "manageusers";
                //        cmd.commandtype = commandtype.storedprocedure;
                //        cmd.parameters.addwithvalue("@role", objuser.role);
                //        cmd.parameters.addwithvalue("@userid", objuser.userid);
                //        cmd.parameters.addwithvalue("@result", objuser.result);
                //        cmd.parameters.addwithvalue("@actiontype", 2);
                //        //cmd.parameters["@status"].direction = parameterdirection.output;
                //        //con.open();
                //        //cmd.executenonquery();
                //        //con.close();
                //        //status = convert.toint32(cmd.parameters["@status"].value);

                //        sqlparameter outputparameter = new sqlparameter();
                //        outputparameter.parametername = "@procresult";
                //        outputparameter.sqldbtype = system.data.sqldbtype.int;
                //        outputparameter.direction = system.data.parameterdirection.output;
                //        outputparameter.size = 10;
                //        cmd.parameters.add(outputparameter);
                //        con.open();
                //        cmd.executenonquery();
                //        con.close();
                //        status = convert.toint16(outputparameter.value);



                //    }
                //}

                using (SqlConnection con = new SqlConnection(objConn))
                {
                    //string sql = "delete from filedata where id = " + id + "";
                    string sql = "update mvcUser set isActive = 1 where FirstName in (select item from [dbo].[SplitString] ('" + objuser.Result + "',','))";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            con.Open();
                            Status = cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Status;
        }



        public DataTable BindUserRole(string userId)
        {
            int result = 0;
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    string sql = "select Role from mvcUser where userID = '" + userId + "'";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            con.Open();
                            da.Fill(dt);
                            //result = cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int updateuserrole1(UserData objuser)
        {
            UserData _objUser = new UserData();
            int Status = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UPDATE_USER", con))
                    {
                        //cmd.CommandText = "ManageUsers";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@role", objuser.Role);
                        cmd.Parameters.AddWithValue("@userID", objuser.UserId);
                        SqlParameter outPutParameter = new SqlParameter();
                        outPutParameter.ParameterName = "@ProcResult";
                        outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                        outPutParameter.Direction = System.Data.ParameterDirection.Output;
                        outPutParameter.Size = 10;
                        cmd.Parameters.Add(outPutParameter);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Status = Convert.ToInt16(outPutParameter.Value);

                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Status;
        }


    }



    #region // Sales \\
    public class SalesData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public DateTime Orderdate { get; set; }
        public string ProductName { get; set; }
        public Decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Decimal TotalPrice { get; set; }
        public Decimal TotalAmount { get; set; }
        public int TotalOrders { get; set; }
        public int OrderID { get; set; }
    }

    public class SalesModel
    {
        string objConn = ConfigurationManager.ConnectionStrings["testDBConn1"].ToString();

        public List<SalesData> getSalesData(int Searchtext)
        {
            List<SalesData> salesdata = new List<SalesData>();
            DataSet dsSalesDate = new DataSet();
            string sumObject;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SalesDate", con))
                    {
                        cmd.Parameters.AddWithValue("@id", Searchtext);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsSalesDate);
                            int count = dsSalesDate.Tables[0].Rows.Count;
                            sumObject = dsSalesDate.Tables[0].Compute("Sum(TOTALPRICE)", string.Empty).ToString();

                            //DataRow toInsert = dsSalesDate.Tables[0].NewRow();                            
                            //dsSalesDate.Tables[0].Rows.InsertAt(toInsert, count + 1);   
                            //toInsert["TOTALPRICE"] = sumObject;

                            DataTable dt = dsSalesDate.Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    salesdata.Add(new SalesData
                                    {
                                        Name = dr["NAME"].ToString(),
                                        Id = Convert.ToInt16(dr["Id"]),
                                        Phone = dr["PHONE"].ToString(),
                                        Country = dr["ADRESS"].ToString(),
                                        TotalOrders = Convert.ToInt32(dr["TOTALORDERS"]),
                                        TotalPrice = Convert.ToDecimal(Convert.ToDecimal(dr["TOTALPRICE"]).ToString("#,##0.00")),
                                    });
                                }
                                salesdata.Add(new SalesData { TotalAmount = Convert.ToDecimal(Convert.ToDecimal(sumObject).ToString("#,##0.00")) });
                            }
                        }
                    }
                }
                return salesdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesData> getUserSalesData(int Searchtext)
        {
            List<SalesData> salesdata = new List<SalesData>();
            DataSet dsSalesDate = new DataSet();
            string sumObject;
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SalesDate", con))
                    {
                        cmd.Parameters.AddWithValue("@id", Searchtext);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsSalesDate);
                            int count = dsSalesDate.Tables[0].Rows.Count;
                            sumObject = dsSalesDate.Tables[0].Compute("Sum(TOTALPRICE)", string.Empty).ToString();

                            //DataRow toInsert = dsSalesDate.Tables[0].NewRow();                            
                            //dsSalesDate.Tables[0].Rows.InsertAt(toInsert, count + 1);   
                            //toInsert["TOTALPRICE"] = sumObject;

                            DataTable dt = dsSalesDate.Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    salesdata.Add(new SalesData
                                    {
                                        Name = dr["NAME"].ToString(),
                                        Country = dr["COUNTRY"].ToString(),
                                        OrderID = Convert.ToInt16(dr["ORDERID"]),
                                        Phone = dr["PHONE"].ToString(),
                                        Orderdate = Convert.ToDateTime(dr["DATEOFORDER"]),
                                        ProductName = dr["PRODUCTNAME"].ToString(),
                                        UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["UNITPRICE"]).ToString()),
                                        Quantity = Convert.ToInt32(dr["QUANTITY"]),
                                        TotalPrice = Convert.ToDecimal(Convert.ToDecimal(dr["TOTALPRICE"]).ToString("#,##0.00")),
                                    });
                                }
                                salesdata.Add(new SalesData { TotalAmount = Convert.ToDecimal(Convert.ToDecimal(sumObject).ToString("#,##0.00")) });
                            }
                        }
                    }
                }
                return salesdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesData> getallusers()
        {
            List<SalesData> userslist = new List<SalesData>();
            DataSet dsusers = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(objConn))
                {
                    using (SqlCommand cmd = new SqlCommand("select id, FirstName + ' '+LastName as Name from  customer order by id asc", con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsusers);
                            foreach (DataRow dr in dsusers.Tables[0].Rows)
                            {
                                userslist.Add(new SalesData
                                {
                                    Id = Convert.ToInt16(dr["Id"]),
                                    Name = dr["Name"].ToString(),
                                });
                            }
                        }
                    }
                }
                return userslist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    #endregion
}





