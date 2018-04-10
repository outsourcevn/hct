using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HocCatToc.Models;
using System.Data.Entity;
namespace HocCatToc.Models
{
    public class DBContext
    {
        public static string addUpdatecustomer(customer cp)
        {
            try
            {
                using (var db = new hoccattocEntities())
                {
                    if (cp.id == 0)
                    {
                        db.customers.Add(cp);
                    }
                    else
                    {
                        db.Entry(cp).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Thất bại: " + ex.Message;
            }
        }
        public static string addUpdateGroup(group cp)
        {
            try
            {
                using (var db = new hoccattocEntities())
                {
                    if (cp.id == 0)
                    {
                        db.groups.Add(cp);
                    }
                    else
                    {
                        db.Entry(cp).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    if (cp.id != 0)
                    {
                        db.Database.ExecuteSqlCommand("update video set group_name=N'" + cp.group_name + "' where group_id=" + cp.id);
                        db.Database.ExecuteSqlCommand("update customers set group_name=N'" + cp.group_name + "' where group_id=" + cp.id);
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Thất bại: " + ex.Message;
            }
        }
        public static string deletecustomer(int cpId)
        {
            try
            {
                using (var db = new hoccattocEntities())
                {
                    var cp = new customer() { id = cpId };
                    db.Entry(cp).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Thất bại: " + ex.Message;
            }
        }
        public static string deletegroup(int cpId)
        {
            try
            {
                using (var db = new hoccattocEntities())
                {
                    var cp = new group() { id = cpId };
                    db.Entry(cp).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Thất bại: " + ex.Message;
            }
        }
        public static string deletevideo(int cpId)
        {
            try
            {
                using (var db = new hoccattocEntities())
                {
                    var cp = new video() { id = cpId };
                    db.Entry(cp).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Thất bại: " + ex.Message;
            }
        }
    }
}