using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineInventory.Models;
using OnlineInventory.DbContext;

namespace OnlineInventory.DAL
{
    public class InventoryDB
    {
        public static bool AddEditItems(ItemsMD obj)
        {
            try
            {
                using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
                {
                    if (obj.ItemName != null)
                    {
                        if (obj.ItemId == 0)
                        {
                            ItemsTbl objItems = new ItemsTbl();
                            objItems.ItemName = obj.ItemName;
                            objItems.CustomerId = obj.CustomerId;
                            objItems.SaleRate = obj.SaleRate;
                            objItems.CreatedBy = 1;
                            objItems.CreatedDate = DateTime.Now;
                            objItems.ItemCode = obj.ItemCode;
                            dbContaxt.ItemsTbls.Add(objItems);
                            dbContaxt.SaveChanges();
                        }
                        else
                        {
                            ItemsTbl objItems = dbContaxt.ItemsTbls.Where(a => a.ItemId == obj.ItemId).FirstOrDefault();
                            objItems.ItemCode = obj.ItemCode;
                            objItems.ItemName = obj.ItemName;
                            objItems.CustomerId = obj.CustomerId;
                            objItems.SaleRate = obj.SaleRate;
                            objItems.CreatedBy = 1;
                            objItems.CreatedDate = DateTime.Now;

                            dbContaxt.SaveChanges();
                        }
                    }
                    else
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static List<ItemsMD> GetItemsList(int CustomerId)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {

                if (CustomerId == 0)
                {
                    var query = (from obj in dbContaxt.ItemsTbls
                                 select new
                                 {
                                     ItemId = obj.ItemId,
                                     ItemName = obj.ItemName,
                                     SaleRate = obj.SaleRate,
                                 });
                    var ItemsLst = new List<ItemsMD>();
                    foreach (var itm in query)
                    {
                        ItemsLst.Add(new ItemsMD()
                        {
                            ItemId = itm.ItemId,
                            ItemName = itm.ItemName,
                            SaleRate = Convert.ToDouble(itm.SaleRate),
                        });
                    }
                    return ItemsLst;
                }
                else
                {
                    var query = (from obj in dbContaxt.ItemsTbls
                                 where obj.CustomerId == CustomerId
                                 select new
                                 {
                                     ItemId = obj.ItemId,
                                     ItemName = obj.ItemName,
                                     SaleRate = obj.SaleRate,
                                     ItemCode = obj.ItemCode
                                 });
                    var ItemsLst = new List<ItemsMD>();
                    foreach (var itm in query)
                    {
                        ItemsLst.Add(new ItemsMD()
                        {
                            ItemId = itm.ItemId,
                            ItemName = itm.ItemName,
                            SaleRate = Convert.ToDouble(itm.SaleRate),
                            ItemCode = itm.ItemCode
                        });
                    }
                    return ItemsLst;
                }




            }
        }
        public static List<ItemsMD> GetItemsListByID(int ItemId)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var query = (from obj in dbContaxt.ItemsTbls
                             where obj.ItemId == ItemId
                             select new
                             {
                                 ItemId = obj.ItemId,
                                 ItemName = obj.ItemName,
                                 SaleRate = obj.SaleRate,
                                 ItemCode=obj.ItemCode
                             });
                var ItemsLst = new List<ItemsMD>();
                foreach (var itm in query)
                {
                    ItemsLst.Add(new ItemsMD()
                    {
                        ItemId = itm.ItemId,
                        ItemName = itm.ItemName,
                        SaleRate = Convert.ToDouble(itm.SaleRate),
                        ItemCode = itm.ItemCode
                    });
                }
                return ItemsLst;
            }
        }


        public static List<ItemsMD> GetItembyCustomers(int CustomerId)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {

               
                    var query = (from obj in dbContaxt.ItemsTbls
                                 where obj.CustomerId==CustomerId
                                 select new
                                 {
                                     ItemId = obj.ItemId,
                                     ItemName = obj.ItemName,
                                     SaleRate=obj.SaleRate,
                                     ItemCode=obj.ItemCode
                                 });
                    var ItemsLst = new List<ItemsMD>();
                    foreach (var itm in query)
                    {
                        ItemsLst.Add(new ItemsMD()
                        {
                            ItemId = itm.ItemId,
                            ItemName = itm.ItemName,
                            SaleRate = (double)itm.SaleRate,
                            ItemCode = itm.ItemCode
                        });
                    }
                    return ItemsLst;
            }
        }


    }
}