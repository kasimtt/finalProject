using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;  //injection,  bağımlılığı azaltır.
        public ProductManager(IProductDal productDal)  //ilgili veri tabanı alınır.
        {
            _productDal = productDal;
        }

        public IResult Add(Product product)
        {
            //business code
           
            if(product.ProductName.Length<2)
            {
                return new ErrorResult(Messages.ProductNameInvalid);  // Messages sınıfından "geçersiz isimlendirme" uyarısı gelir
            }
           
            _productDal.Add(product);
           return  new SuccessResult(Messages.ProductAdded); // Messages sınıfından "proje eklendi" uyarısı gelir
        }

        public IDataResult<List<Product>> GetAll()
        {
            if(DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductListed); //verileri DataAccess katmanından çağırır.
        }

        public IDataResult<List<Product>>GetAllByCategoryId(int Id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == Id));
        }

        public IDataResult<List<Product>> GetAllByPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice <= max && p.UnitPrice >= min));
        }

        public IDataResult<List<ProductDetailsDto>> GetProductDetails()
        {
            if(DateTime.Now.Hour==14)
            {
                return new ErrorDataResult<List<ProductDetailsDto>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<ProductDetailsDto>>(_productDal.getProductDetails(),Messages.ProductListed);
        }

        public IDataResult<Product> GetProductById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

      
    }
}
