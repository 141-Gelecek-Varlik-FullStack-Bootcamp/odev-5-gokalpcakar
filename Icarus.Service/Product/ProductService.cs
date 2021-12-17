using AutoMapper;
using Icarus.DB.Entities.DataContext;
using Icarus.Model;
using Icarus.Model.Extensions;
using Icarus.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icarus.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        public ProductService(IMapper _mapper)
        {
            mapper = _mapper;
        }

        // Ürün listelemesini gerçekleştiren metot
        public General<ProductViewModel> GetProducts()
        {
            var result = new General<ProductViewModel>();

            using (var context = new IcarusContext())
            {
                // eğer ürün aktif ve silinmemişse Id'sine göre listeliyoruz
                var data = context.Product.
                            Where(x => x.IsActive && !x.IsDeleted).
                            OrderBy(x => x.Id);

                // gelen veri varsa işlem başarılı yoksa belirttiğimiz mesaj dönüyor
                if (data.Any())
                {
                    result.List = mapper.Map<List<ProductViewModel>>(data);
                    result.IsSuccess = true;
                }
                else
                {
                    result.ExceptionMessage = "Herhangi bir ürün bulunamadı.";
                }
            }

            return result;
        }

        // Ürünlerin sayfalama işleminin gerçekleştirildiği metot
        // Parametre olarak sayfa başına ürün sayısı ve sayfa numarasını alıyor
        public General<ProductViewModel> ProductPagination(int productByPage, int displayPageNo)
        {
            var result = new General<ProductViewModel>();

            int _totalCount = 0;
            int _totalPage = 0;
            int remaining = 0;

            try
            {
                using (var context = new IcarusContext())
                {
                    // toplam ürün sayısı alınıyor
                    _totalCount = context.Product.Count();

                    // Eğer sayfa başına ürün sayısı 1'den küçük ya da toplam üründen fazlaysa
                    // aşağıdaki mesaj dönüyor
                    if (productByPage < 1 || productByPage > _totalCount)
                    {
                        result.ExceptionMessage = $"Lütfen 1 ile {_totalCount} arasında bir değer giriniz";
                    }

                    _totalPage = _totalCount / productByPage;
                    remaining = _totalCount % productByPage;

                    // Eğer sayfa başına ürün, toplam ürüne tam bölünmüyorsa
                    // Kalan ürünler de ekleniyor
                    if (remaining != 0)
                    {
                        _totalPage += remaining;
                    }

                    // Girilen parametrelere göre sayfalama gerçekleştiriliyor
                    var _products = context.Product
                                            .Where(x => !x.IsDeleted)
                                            .OrderBy(i => i.Id)
                                            .Skip((displayPageNo - 1) * productByPage)
                                            .Take(productByPage).ToList();

                    result.List = mapper.Map<List<ProductViewModel>>(_products);
                    result.IsSuccess = true;

                    result.TotalCount = _totalCount;
                    result.totalPage = _totalPage;
                    result.SuccessfulMessage = "Sayfalama işlemi başarıyla gerçekleştirilmiştir";
                }
            }
            catch (Exception)
            {
                result.ExceptionMessage = "Beklenmedik bir hata oluştu. Lütfen tekrar deneyiniz";
            }

            return result;
        }

        // Ürün adlarının ne ile başladığına göre filtreleme yapan metodumuz
        public General<ProductViewModel> FilterProducts(string filterByName)
        {
            var result = new General<ProductViewModel>();
            using (var context = new IcarusContext())
            {
                var products = context.Product.Where(x => !x.IsDeleted);

                if (!String.IsNullOrEmpty(filterByName))
                {
                    products = products.Where(i => i.Name.StartsWith(filterByName));
                }
                else
                {
                    result.ExceptionMessage = "Lütfen arama işleminizi tekrardan gerçekleştiriniz";
                    return result;
                }

                result.List = mapper.Map<List<ProductViewModel>>(products);
                result.SuccessfulMessage = "Filtreleme işleminiz başarıyla gerçekleştirilmiştir";
            }

            return result;
        }

        // Sıralama metodu neye göre sıralama yapacağına dair parametre alıyor
        public General<ProductViewModel> SortProducts(string sortingParameter)
        {
            var result = new General<ProductViewModel>();
            using (var context = new IcarusContext())
            {
                var products = context.Product.Where(x => x.IsActive && !x.IsDeleted);

                switch (sortingParameter)
                {
                    case "Name":
                        products = products.OrderBy(x => x.Name);
                        break;
                    case "DescendingName":
                        products = products.OrderByDescending(p => p.Name);
                        break;
                    case "Price":
                        products = products.OrderBy(p => p.Price);
                        break;
                    case "DescendingPrice":
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    case "InsertDate":
                        products = products.OrderBy(p => p.Idate);
                        break;
                    case "DescendingInsertDate":
                        products = products.OrderByDescending(p => p.Idate);
                        break;
                    // Eğer yukarıdaki değerlere göre sıralama yapılmayacaksa 
                    // varsayılan olarak eklenme tarihine göre sıralama işlemi gerçekleştiriliyor
                    default:
                        products = products.OrderBy(p => p.Idate);
                        break;
                }

                result.List = mapper.Map<List<ProductViewModel>>(products);
                result.SuccessfulMessage = "Sıralama işleminiz başarıyla gerçekleştirilmiştir";
            }

            return result;
        }
        // Ürün ekleme işlemini gerçekleştiren metot
        public General<InsertProductViewModel> Insert(InsertProductViewModel newProduct)
        {
            var result = new General<InsertProductViewModel>();

            try
            {
                var model = mapper.Map<Icarus.DB.Entities.Product>(newProduct);

                using (var context = new IcarusContext())
                {
                    // Eğer gelen modeldeki id veritabanındaki kullanıcılardan birinin id'si ise,
                    // kullanıcı login işlemini gerçekleştirmiş ve IsDeleted değeri false ise
                    // ekleme işlemini gerçekleştirebilecek yetkisi oluyor
                    var isAuth = context.User.Any(x => x.Id == model.Iuser &&
                                                               x.IsActive &&
                                                               !x.IsDeleted);

                    // Kullanıcı yetkiliyse ekleme gerçekleşiyor değilse aşağıdaki mesajı dönüyor
                    if (isAuth)
                    {
                        model.Idate = DateTime.Now;
                        context.Product.Add(model);
                        context.SaveChanges();

                        result.Entity = mapper.Map<InsertProductViewModel>(model);
                        result.IsSuccess = true;
                        result.SuccessfulMessage = "Ürün başarıyla eklenmiştir";
                    }
                    else
                    {
                        result.ExceptionMessage = "Ürün ekleme yetkiniz bulunmamaktadır.";
                    }
                }
            }
            catch (Exception)
            {
                result.ExceptionMessage = "Ürün ekleme işlemi başarısızlıkla sonuçlandı";
            }

            return result;
        }
        // Ürün güncelleme işlemini gerçekleştiren metot
        public General<UpdateProductViewModel> Update(int id, UpdateProductViewModel product)
        {
            var result = new General<UpdateProductViewModel>();

            using (var context = new IcarusContext())
            {
                // Güncelleme işlemini gerçekleştiren kişi
                // daha önceden ürünü eklemiş kullanıcıysa yetkili konuma geliyor
                var isAuth = context.Product.Any(x => x.Iuser == product.Iuser);
                var updateProduct = context.Product.SingleOrDefault(i => i.Id == id);

                // Kullanıcı yetkiliyse ürün güncelleniyor değilse mesaj dönüyor
                if (isAuth)
                {
                    if (updateProduct is not null)
                    {
                        // güncelleme işleminin vaktini alıyoruz
                        var trTimeZone = DateTime.Now;

                        updateProduct.Name = product.Name;
                        updateProduct.DisplayName = product.DisplayName;
                        updateProduct.Description = product.Description;
                        updateProduct.Price = product.Price;
                        updateProduct.Stock = product.Stock;
                        updateProduct.Udate = trTimeZone;

                        // Güncellemeler tokyo ve londra saatine göre ne zaman yapılmış onu ekliyoruz
                        updateProduct.UlondonDate = ProductExtensions.toLondonTimeZone(trTimeZone);
                        updateProduct.UtokyoDate = ProductExtensions.toTokyoTimeZone(trTimeZone);

                        context.SaveChanges();

                        result.Entity = mapper.Map<UpdateProductViewModel>(updateProduct);
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.ExceptionMessage = "Ürün bulunamadı.";
                    }
                }
                else
                {
                    result.ExceptionMessage = "Güncelleme yetkiniz bulunmamaktadır.";
                }
            }

            return result;
        }
        // Ürün silme işleminin gerçekleştiği metot
        public General<ProductViewModel> Delete(int id)
        {
            var result = new General<ProductViewModel>();

            using (var context = new IcarusContext())
            {
                // Silme işlemi gerçekleştirilecek id'ye ait ürün var mı kontrol ediliyor
                // Varsa ürün siliniyor yoksa mesaj dönüyor
                var product = context.Product.SingleOrDefault(i => i.Id == id);

                if (product is not null)
                {
                    context.Product.Remove(product);
                    context.SaveChanges();

                    result.Entity = mapper.Map<ProductViewModel>(product);
                    result.IsSuccess = true;
                }
                else
                {
                    result.ExceptionMessage = "Ürün bulunamadı.";
                }
            }

            return result;
        }
    }
}
