using OnlineInventory.DbContext;
using OnlineInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineInventory.DAL
{
    public class CustomerDb
    {
        //Get CustomerList
        public static List<CustomerMD> GetCustomerList(int CustomerId)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {

                if (CustomerId == 0)
                {
                    var query = (from obj in dbContaxt.CustomerTbls
                                 select new
                                 {
                                     Name = obj.Name,
                                     Mobile = obj.Mobile,
                                     Address = obj.Address,
                                     Phone = obj.Phone,
                                     CustomerId = obj.CustomerId,
                                     OpeningBal = obj.OpeningBal
                                 });
                    var ItemsLst = new List<CustomerMD>();
                    foreach (var itm in query)
                    {
                        ItemsLst.Add(new CustomerMD()
                        {
                            Name = itm.Name,
                            Mobile = itm.Mobile,
                            Address = itm.Address,
                            Phone = itm.Phone,
                            CustomerId = itm.CustomerId,
                            OpeningBal = Convert.ToDouble ( itm.OpeningBal)
                        });
                    }
                    return ItemsLst;
                }
                else
                {
                    var query = (from obj in dbContaxt.CustomerTbls
                                 where obj.CustomerId == CustomerId
                                 select new
                                 {
                                     Name = obj.Name,
                                     Mobile = obj.Mobile,
                                     Address = obj.Address,
                                     Phone = obj.Phone,
                                     CustomerId = obj.CustomerId,
                                     OpeningBal = obj.OpeningBal
                                 });
                    var ItemsLst = new List<CustomerMD>();
                    foreach (var itm in query)
                    {
                        ItemsLst.Add(new CustomerMD()
                        {
                            Name = itm.Name,
                            Mobile = itm.Mobile,
                            Address = itm.Address,
                            Phone = itm.Phone,
                            CustomerId = itm.CustomerId,
                            OpeningBal = Convert.ToDouble(itm.OpeningBal)
                        });
                    }
                    return ItemsLst;
                }




            }
        }
        public static bool AddEditCustomer(CustomerMD obj)
        {
            try
            {
                using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
                {
                    if (obj.Name != null)
                    {
                        if (obj.CustomerId == 0)
                        {
                            int MaxId = 1;
                            var GetMaxId = dbContaxt.CustomerTbls.OrderByDescending(x => x.CustomerId).FirstOrDefault();
                            if (GetMaxId != null)
                            {
                                MaxId = 1+GetMaxId.CustomerId;
                            }

                            CustomerTbl objCustomer = new CustomerTbl();
                            objCustomer.CustomerId = MaxId;
                            objCustomer.Name = obj.Name;
                            objCustomer.Mobile = obj.Mobile;
                            objCustomer.Address = obj.Address;
                            objCustomer.Phone = obj.Phone;
                            objCustomer.OpeningBal = obj.OpeningBal;
                            objCustomer.CreatedBy = 1;
                            objCustomer.CreatedDate = DateTime.Now;
                            objCustomer.CustomerNo = "CTN-" + MaxId.ToString();
                            dbContaxt.CustomerTbls.Add(objCustomer);
                            dbContaxt.SaveChanges();
                        }
                        else
                        {
                            CustomerTbl objCustomer = dbContaxt.CustomerTbls.Where(a => a.CustomerId == obj.CustomerId).FirstOrDefault();

                            objCustomer.Name = obj.Name;
                            objCustomer.Mobile = obj.Mobile;
                            objCustomer.Address = obj.Address;
                            objCustomer.Phone = obj.Phone;
                            objCustomer.OpeningBal = obj.OpeningBal;
                            objCustomer.CreatedBy = 1;
                            objCustomer.CreatedDate = DateTime.Now;

                            dbContaxt.SaveChanges();
                        }
                    }
                    else
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}