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
        public static int AddEditItems(ItemsMD obj)
        {
            int ItemId = 0;
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
                            ItemId = objItems.ItemId;
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
                            ItemId = objItems.ItemId;
                            dbContaxt.SaveChanges();
                        }
                    }
                    else
                        return ItemId;
                }
                return ItemId;
            }
            catch
            {
                return ItemId;
            }

        }
        public static string SaveItemPhoto(String Url, int ItemId)
        {
            using (OnlineInvoiceSystemDBEntities dbContaxt = new OnlineInvoiceSystemDBEntities())
            {
                var UpdateQuery = dbContaxt.ItemsTbls.Where(x => x.ItemId == ItemId).FirstOrDefault();
                if (UpdateQuery != null)
                {
                    UpdateQuery.PhotoURL = Url;
                    dbContaxt.SaveChanges();
                    return "Save Successfully";
                }
                else {
                    return "Failed to Save image";
                }
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
                                     PhotoURL = obj.PhotoURL
                                 });
                    var ItemsLst = new List<ItemsMD>();
                    foreach (var itm in query)
                    {
                        ItemsLst.Add(new ItemsMD()
                        {
                            ItemId = itm.ItemId,
                            ItemName = itm.ItemName,
                            SaleRate = Convert.ToDouble(itm.SaleRate),
                            PhotoURL= itm.PhotoURL,

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
                                     ItemCode = obj.ItemCode,
                                     PhotoURL = obj.PhotoURL
                                 });
                    var ItemsLst = new List<ItemsMD>();
                    foreach (var itm in query)
                    {
                        var ImageUrl = "/Content/assets/avatars/avatar5.png";
                        if (itm.PhotoURL != null)
                        {
                            ImageUrl = itm.PhotoURL;
                        }
                        ItemsLst.Add(new ItemsMD()
                        {
                            ItemId = itm.ItemId,
                            ItemName = itm.ItemName,
                            SaleRate = Convert.ToDouble(itm.SaleRate),
                            ItemCode = itm.ItemCode,
                            PhotoURL = ImageUrl
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