using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MecatredzStoredProceurecrudmvc.Models;

namespace MecatredzStoredProceurecrudmvc.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public void InsertRecord(StuentClass ss)
        {
            string msg = "";
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-7C38DCLJ\SQLEXPRESS;Initial Catalog=CRUDDB;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("InsertData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", ss.name);
                cmd.Parameters.AddWithValue("@mob", ss.mob);
                cmd.Parameters.AddWithValue("@email", ss.email);
                cmd.Parameters.AddWithValue("@dob", ss.dob);
                if (ConnectionState.Closed == conn.State)
                {
                    conn.Open();
                }
               int n = cmd.ExecuteNonQuery();
                if(n > 0)
                {
                    msg = "Data Successfully Saved";
                }
                else
                {
                    msg = "Data not Saved";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
        public List<StuentClass> GetAllData()
        {
            List<StuentClass> lst = new List<StuentClass>();
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-7C38DCLJ\SQLEXPRESS;Initial Catalog=CRUDDB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("SelectData", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if(ConnectionState.Closed == conn.State)
            {
                conn.Open();
            }
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                lst.Add(new StuentClass
                {
                    name = dr["name"].ToString(),
                    mob = dr["mob"].ToString(),
                    email = dr["email"].ToString(),
                    dob = dr["dob"].ToString()
                }); 
            }
            conn.Close();
            return lst;
        }

        public List<StuentClass> GetSingleData(string email)
        {
            List<StuentClass> lst = new List<StuentClass>();
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-7C38DCLJ\SQLEXPRESS;Initial Catalog=CRUDDB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("SelectSingleRecord", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            if (ConnectionState.Closed == conn.State)
            {
                conn.Open();
            }
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lst.Add(new StuentClass
                {
                    rid = int.Parse(dr["rid"].ToString()),
                    name = dr["name"].ToString(),
                    mob = dr["mob"].ToString(),
                    email = dr["email"].ToString(),
                    dob = dr["dob"].ToString()
                }); ;
            }
            conn.Close();
            return lst;
        }
        public ActionResult Display()
        {
            return View();
        }
        public JsonResult getList()
        {
            return Json(GetAllData(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getSingleRow(string email)
        {
            return Json(GetSingleData(email), JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteRecord(string email)
        {
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-7C38DCLJ\SQLEXPRESS;Initial Catalog=CRUDDB;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("DeleteData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", email);
                if (ConnectionState.Closed == conn.State)
                {
                    conn.Open();
                }
                 cmd.ExecuteNonQuery();
                conn.Close();
                return Json("Record Deleted", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Record Not Deleted", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateRecord(string rid, string n, string m, string e, string d)
        {
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-7C38DCLJ\SQLEXPRESS;Initial Catalog=CRUDDB;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UpdateData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@rid", int.Parse(rid));
                cmd.Parameters.AddWithValue("@name", n);
                cmd.Parameters.AddWithValue("@mob", m);
                cmd.Parameters.AddWithValue("@email", e);
                cmd.Parameters.AddWithValue("@dob", d);
                if (ConnectionState.Closed == conn.State)
                {
                    conn.Open();
                }
                 cmd.ExecuteNonQuery();
                conn.Close();
                return Json("Record Updated Successfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json("Record Not Updated", JsonRequestBehavior.AllowGet);
            }
        }
    }
}