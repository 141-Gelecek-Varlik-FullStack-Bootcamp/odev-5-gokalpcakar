using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus.Model
{
    // Geri dönüş değerlerinde hata yazdırmak,
    // işlem başarılı mı kontrolü yapmak,
    // Tek kullanıcı üzerinde işlem yapıldıysa o kullanıcıyı görmek,
    // Birden fazla kullanıcı üzerinde işlem yapıldıysa o kullanıcıları görmek
    // için oluşturulan bir class
    public class General<T>
    {
        public bool IsSuccess { get; set; }
        public T Entity { get; set; }
        public List<T> List { get; set; }
        public decimal TotalCount { get; set; }
        public decimal totalPage { get; set; }
        public string ExceptionMessage { get; set; }
        public string SuccessfulMessage { get; set; }
    }
}
