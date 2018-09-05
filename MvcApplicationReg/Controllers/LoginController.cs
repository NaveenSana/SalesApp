using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplicationReg.Models;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Rotativa;

using System.Web.UI.HtmlControls;
using System.Web.UI;
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using NPOI.POIFS.FileSystem;
using NPOI.HPSF;
using Newtonsoft.Json;

namespace MvcApplicationReg.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        AdminModel objAdmin = new AdminModel();
        public ActionResult Index()
        {
            return View();

        }
        [HttpPost]
        public ActionResult CreateUser(string fname, string lname, string password, string state, string city, string address, int pincode)//
        {
            UserData objuserdata = new UserData();
            //objuserdata.UserId = userid;
            objuserdata.FisrtName = fname;
            objuserdata.lastName = lname;
            objuserdata.Password = password;
            objuserdata.State = state;
            objuserdata.city = city;
            objuserdata.Address = address;
            objuserdata.PinCode = pincode;
            int result = objAdmin.CreateUser(objuserdata);
            //GetAllUsers();
            return Json(result); ;
        }

        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            UserData objuserdata = new UserData();
            objuserdata.FisrtName = frm["Fname"];
            objuserdata.Password = frm["Password"];
            int result = objAdmin.checklogin(objuserdata);
            if (result == 1)
            {
                return RedirectToAction("UploadFile", "Login");
            }
            return View();
        }

        [HttpPost]
        public JsonResult ActivationLink(string email)
        {
            //string ActivationUrl = HttpContext.Request.Url.ToString();
            //objUser.ActivationUrl = ActivationUrl.Replace(HttpContext.Request.Url.AbsolutePath.ToString(), "");
            //objUser.EMAILID = email;
            int status = objAdmin.SendEmail(email);
            return Json(status);
        }

        public ActionResult GetAllUsers()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAllUserss()
        {
            List<UserData> userslist = new List<UserData>();
            userslist = objAdmin.getallusers();
            return Json(userslist);
        }

        [HttpPost]
        public ActionResult GetAllTestimonials()
        {
            List<testidata> testilist = new List<testidata>();
            testilist = objAdmin.GetAllTestimonials();
            return Json(testilist);
        }
        public ActionResult ExportToExcel()
        {

            int i = 0;
            int j = 0;
            string sql = null;
            string data = null;
            Excel.Application app = new Excel.Application();
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = false;
            xlWorkBook = (Microsoft.Office.Interop.Excel.Workbook)(xlApp.Workbooks.Add(System.Reflection.Missing.Value));
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.ActiveSheet;
            string conn = ConfigurationManager.ConnectionStrings["testDBConn"].ToString();
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand("allusers", con);
            SqlDataAdapter scmd = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            scmd.Fill(ds);
            int k = 0;

            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                data = column.ColumnName;
                xlWorkSheet.Cells[1, k + 1] = data;
                xlWorkSheet.Cells[1, k + 1].Interior.Color = System.Drawing.Color.Yellow;
                xlWorkSheet.Cells.Font.Bold = true;
                k++;
            }
            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                var newj = 0;
                for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                {
                    data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                    xlWorkSheet.Cells[i + 2, newj + 1] = data;
                    newj++;
                }
            }
            string AppLocation = "";
            AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            AppLocation = AppLocation.Replace("file:\\", "");
            string file = AppLocation + "\\Files\\SampleReport.xlsx";
            //xlWorkBook.SaveAs(file, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlApp.DisplayAlerts = false;
            xlWorkBook.SaveAs(file, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            //objAdmin.SendExcelReport("naveen_kumar@tecnics.com");
            return RedirectToAction("GetAllUsers", "Login");
        }


        [HttpPost]
        public ActionResult GetAllCountries()
        {
            int year = 1994, month = 12, day = 02;
            DateTime dt = new DateTime(year, month, day);
            int datetodays = DateTime.Now.Subtract(dt).Days;
            int datetoyears = Convert.ToInt32(DateTime.Now.Year) - dt.Year;
            //int datetomonths = DateTime.Now.Subtract(dt).Days;


            string[] strArray = new string[] { "Mahesh Chand", "Mike Gold", "Raj Beniwal", "Praveen Kumar", "Dinesh Beniwal" };
            string name = string.Empty;
            for (int i = 0; i < strArray.Length; i++)
            {
                name += strArray[i] + ",";
            }
            List<CountryList> countrylist = new List<CountryList>();
            countrylist = objAdmin.getallcountries();
            return Json(countrylist);
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();

        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, FormCollection frm)
        {

            filevalues objfiles = new filevalues();
            try
            {
                //if (file.ContentLength > 0)
                //{
                //if (files. > 0)
                //{

                ////}
                //    foreach (var file in files)
                //    {

                string _FileName = Path.GetFileName(file.FileName);
                string _Extension = Path.GetExtension(file.FileName);
                string _InputFileName = frm["text"].ToString();
                if (!System.IO.Directory.Exists(Server.MapPath("~/Images")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Images"));
                }
                if (!System.IO.File.Exists(Server.MapPath("~/Images")))
                {
                    string _path = Path.Combine(Server.MapPath("~/Images"), _FileName);
                    objfiles.Name = _FileName;
                    objfiles.Extension = _Extension;
                    objfiles.Path = _path;
                    objfiles.InputFileName = _InputFileName;
                    objfiles.UploadedDate = DateTime.UtcNow;
                    int result = objAdmin.InsertFile(objfiles);
                    if (result == 1)
                    {
                        file.SaveAs(_path);

                    }
                    else
                    {
                        ViewBag.Message = "Error with db";
                    }
                }
                //}
                //string _path = Path.Combine(Server.MapPath("~/Images"), _FileName);  
                //file.SaveAs(_path);  
                // }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

        //[HttpGet]                
        //public PartialViewResult GetAllFiles()
        //{
        //    List<filevalues> fileslist = new List<filevalues>();
        //    fileslist = objAdmin.getallFiles();
        //    return PartialView("_Fileslist", JsonConvert.SerializeObject(fileslist));
        //}

        [HttpPost]
        public ActionResult GetAllFiles()
        {
            List<filevalues> fileslist = new List<filevalues>();
            fileslist = objAdmin.getallFiles();
            return Json(fileslist);
        }

        [HttpPost]
        public JsonResult DeleteFile(int id)
        {
            //List<filevalues> fileslist = new List<filevalues>();
            int result = objAdmin.deletefile(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Testimonals(string Name, string Email, string Description)
        //public ActionResult Testimonals(FormCollection frmcoll)
        {
            //if (frmcoll != null)
            //{
            testidata objdata = new testidata();
            objdata.Name = Name;
            objdata.Email = Email;
            objdata.Description = Description;

            //objdata.Name = frmcoll["txtfname"];
            //objdata.Email = frmcoll["txtemail"];
            //objdata.Description = frmcoll["txtdesc"];
            int result = objAdmin.CreateTestimonals(objdata);
            //}
            //return Json(result);
            return Json(result);

        }

        [HttpGet]
        public ActionResult sample()
        {
            //fields objfield = new fields();
            //objfield.Matched = 0;
            //return View(objfield);
            return View("fdfbfd");
        }

        [HttpPost]
        public ActionResult sample(FormCollection formCollection)
        {
            //String roleValue1 = formCollection.Get("inputRole");
            String roleValue1 = formCollection.Get("on");
            return View(roleValue1);
        }


        //public ActionResult ExcelDownload()
        //{
        //    string conn = ConfigurationManager.ConnectionStrings["testDBConn"].ToString();
        //    SqlConnection con = new SqlConnection(conn);
        //    SqlCommand cmd = new SqlCommand("allusers", con);
        //    SqlDataAdapter scmd = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();

        //    scmd.Fill(ds);
        //    string AppLocation = "";
        //    AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //    AppLocation = AppLocation.Replace("file:\\", "");
        //    string file = AppLocation + "\\Files\\SampleReport.xlsx";

        //    List<UserData> lstStudents = new List<UserData>(){
        //    new UserData(){UserId = "10",FisrtName = "Naveen1"},
        //    new UserData(){UserId = "20",FisrtName = "Naveen2"},
        //    new UserData(){UserId = "30", FisrtName= "Naveen3"},
        //    new UserData(){UserId = "40", FisrtName= "Naveen4"}
        //};
        //    var lstuds = lstStudents.Select(s => new { Name = s.FisrtName, s.UserId }).ToList();
        //    //DataTable dt1 = new DataTable();
        //    DataTable dt = ToDataTable(lstuds);
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        //wb.Worksheets.Add(ds.Tables[0]);
        //        wb.Worksheets.Add(dt);
        //        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        wb.Style.Font.Bold = true;
        //        wb.SaveAs(file);
        //    }
        //    int status = objAdmin.SendExcelReport("naveen_kumar@tecnics.com");
        //    if (status == 1)
        //    {
        //        Response.Write("<script>alert('Mail sent succesfully')</script>");
        //    }
        //    return RedirectToAction("GetAllUsers", "Login");
        //}
        //static DataTable ToDataTable<T>(List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);
        //    //Get all the properties by using reflection   
        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo prop in Props)
        //    {
        //        //Setting column names as Property names  
        //        dataTable.Columns.Add(prop.Name);
        //    }
        //    foreach (T item in items)
        //    {
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {

        //            values[i] = Props[i].GetValue(item, null);
        //        }
        //        dataTable.Rows.Add(values);
        //    }

        //    return dataTable;
        //}      
        public ActionResult PrintAllReport()
        {
            //var report = new ActionAsPdf("Index");
            //return report;

            var FileName = new Rotativa.UrlAsPdf("http://www.Google.com")
            {
                FileName = "urltest.pdf",
            };
            return FileName;
        }

        [HttpPost]
        //public FileResult DownloadFile(string filename)
        //{            
        //    //string path = "~/images/" + filename;
        //    string path = @"C:\Users\Naveen\Documents\Visual Studio 2012\Projects\MvcApplicationReg\MvcApplicationReg\Images\download.png";            
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(path);
        //    string fileName = filename;
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        //    //return View();
        //}
        [HttpGet]
        public ActionResult DownloadFile(string filename)
        {
            byte[] fileBytes;
            try
            {
                filename = "download.png";
                //var destination = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Shipment_Documents/" + Shipment_No + "/"), fileName);
                //string fullName = Path.Combine(, filePath, fileName);
                //string fullName = @"C:\Users\Naveen\Documents\Visual Studio 2012\Projects\MvcApplicationReg\MvcApplicationReg\Images\download.png";            
                string fullName = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/"), filename);
                fileBytes = GetFile(fullName);
                return File(
                    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        byte[] GetFile(string s)
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.OpenRead(s);
                byte[] data = new byte[fs.Length];
                int br = fs.Read(data, 0, data.Length);
                if (br != fs.Length)
                    throw new System.IO.IOException(s);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult AssignRole(int SelectedUserId, string UserRole)
        {
            UserData objuser = new UserData();
            objuser.Role = UserRole;
            objuser.UserId = SelectedUserId.ToString();
            int result = objAdmin.updateuserrole(objuser);
            return Json(result);
        }

        [HttpPost]
        public ActionResult BindUserRole(string userid)
        {
            System.Data.DataTable result = objAdmin.BindUserRole(userid);
            Object Role = result.Rows[0]["Role"];
            return Json(Role);
 
        }

        [HttpPost]
        public ActionResult UpdatingUser(string Names)
        {
            UserData objuser = new UserData();
            objuser.Result = Names;            
            int result = objAdmin.updateuserrole(objuser);
            return Json(result);
        }
    }

}




