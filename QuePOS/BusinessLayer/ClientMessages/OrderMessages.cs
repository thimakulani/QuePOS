using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ClientMessages
{
    public static class OrderMessages
    {
        public const string OrderNotFound = "Order Bulunamadı.!";
        public const string OrderNotCreated = "Order Oluşturulamadı.!";
        public const string OrderCreated = "Order Oluşturuldu.!";
        public const string OrderNotUpdated = "Order Güncellenemedi.!";
        public const string OrderUpdated = "Order Güncellendi.!";
        public const string OrderNotDeleted = "Order Silinemedi.!";
        public const string OrderDeleted = "Order Silindi.!";
        public const string OrderNotListed = "Order Listelenemedi.!";
        public const string OrderListed = "Order Listelendi.!";
        public const string OrderIsNotNull = "Order Boş Olamaz.!";
        public const string OrderNameLength = "Minimum 3 Karakter Olmalı ve En Fazla 100 Karakter Olmalıdır.!";
        public const string OrderNameExists = "Order Adı Kullanılmaktadır.!";
    }
}
