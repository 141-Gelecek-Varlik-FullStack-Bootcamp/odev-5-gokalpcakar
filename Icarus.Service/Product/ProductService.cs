using AutoMapper;
using Icarus.DB.Entities.DataContext;
using Icarus.Model;
using Icarus.Model.Extensions;
using Icarus.Model.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public General<ListDeleteViewModel> GetProducts()
        {
            var result = new General<ListDeleteViewModel>();

            using (var context = new IcarusContext())
            {
                // eğer ürün aktif ve silinmemişse Id'sine göre listeliyoruz
                var data = context.Product.
                            Where(x => x.IsActive && !x.IsDeleted).
                            OrderBy(x => x.Id);

                // gelen veri varsa işlem başarılı yoksa belirttiğimiz mesaj dönüyor
                if (data.Any())
                {
                    result.List = mapper.Map<List<ListDeleteViewModel>>(data);
                    result.IsSuccess = true;
                }
                else
                {
                    result.ExceptionMessage = "Herhangi bir ürün bulunamadı.";
                }
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
        public General<ListDeleteViewModel> Delete(int id)
        {
            var result = new General<ListDeleteViewModel>();

            using (var context = new IcarusContext())
            {
                // Silme işlemi gerçekleştirilecek id'ye ait ürün var mı kontrol ediliyor
                // Varsa ürün siliniyor yoksa mesaj dönüyor
                var product = context.Product.SingleOrDefault(i => i.Id == id);

                if (product is not null)
                {
                    context.Product.Remove(product);
                    context.SaveChanges();

                    result.Entity = mapper.Map<ListDeleteViewModel>(product);
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
