namespace Model;
using DAO;
using DTO;
using Interfaces;
using Microsoft.EntityFrameworkCore;

public class Product : IValidateDataObject, IDataController<ProductDTO, Product>
{
    private String name;
    private String bar_code;

    private String image;
    private String description;



    public static Product convertDTOToModel(ProductDTO obj)
    {
        
        var product = new Model.Product
        {
            name = obj.name,
            bar_code = obj.bar_code,
            image  = obj.image,
            description = obj.description
        };

        return product;
    }

    public void delete()
    {
        using (var context = new DAOContext())
        {

            var product = context.products.FirstOrDefault(c => c.bar_code == this.bar_code);
            var stocks = context.stocks.Where(s => s.product.id == product.id);
            var wishlists = context.wishlists.Where(s=>s.stocks.product.id == product.id);
            

            if(product != null){
                if(stocks != null){
                    // foreach(var stock in stocks){
                    //     context.stocks.Remove(stock);
                    // }
                    context.stocks.RemoveRange(stocks);

                }
                if(wishlists != null){
                    // foreach(var WishList in wishlists){
                        
                    // }
                    context.wishlists.RemoveRange(wishlists);
                }
            }
            context.products.Remove(product);
            context.SaveChanges();
        }
    }
    public int save()
    {
        var id = 0;

        using (var context = new DAOContext())
        {
            var product = context.products
                .FirstOrDefault(c => c.bar_code == this.bar_code);
            if (product != null)
            {
                return -1;
            }
            product = new DAO.Product
            {
                name = this.name,
                bar_code = this.bar_code,
                image = this.image,
                description = this.description
            };

            context.products.Add(product);

            context.SaveChanges();

            id = product.id;

        }
        return id;
    }


    public int findId()
    {

        using (var context = new DAOContext())
        {
            var productDAO = context.products.FirstOrDefault(o => o.bar_code == this.bar_code);
            return productDAO.id;
        }
    }


    public void update(ProductDTO obj)
    {
        using (var context = new DAOContext())
        {
            var product = context.products.FirstOrDefault(i => i.bar_code == obj.bar_code);

            if (product != null)
            {
                context.Entry(product).State = EntityState.Modified;
                product.name = obj.name;
                product.description = obj.name;
                product.image = obj.image;
            }
            context.SaveChanges();
        }
    }

    public ProductDTO findById(int id)
    {
        return new ProductDTO();
    }

    public List<ProductDTO> getAll()
    {
        return new List<ProductDTO>();
    }

    public int getID()
    {
        using (var context = new DAOContext())
        {
            var product = context.products.FirstOrDefault(p => p.bar_code == this.bar_code);
            Console.WriteLine("Bar_code: ",this.bar_code);
            return product.id;
        }
    }

    public static List<object> getAllStaticTeste(){
    using (var context = new DAOContext())
        {

            var stocksDAO = context.stocks.Include(i => i.product).Include(i => i.store);

            List<object> stocks = new List<object>();
            foreach(object stock in stocksDAO){
                stocks.Add(stock);
            }

            return stocks;

            
        };
    }

    public static List<ProductResponseDTO> getAllStatic()
    {
        Console.WriteLine("Inicio");
        using (var context = new DAOContext())
        {
            var TodosOsProdutos = context.products.Join(context.stocks.Include(i=> i.store),
            produto => produto.id,
            estoque => estoque.product.id,
            (p,e) => new {
                Product = p,
                Stock = e
            }
            );
            var productsDTO = new List<ProductResponseDTO>();
            Console.WriteLine("meio");
            foreach (var item in TodosOsProdutos)
            {
                var TransitionDAO = new DTO.ProductResponseDTO();
                TransitionDAO.Id = item.Product.id;
                TransitionDAO.bar_code = item.Product.bar_code;
                TransitionDAO.name = item.Product.name;
                TransitionDAO.image = item.Product.image;
                TransitionDAO.description = item.Product.description;
                TransitionDAO.Quantity = item.Stock.quantity;
                TransitionDAO.Unit_price = item.Stock.unit_price;  
                TransitionDAO.CNPJ = item.Stock.store.CNPJ;
                TransitionDAO.IdStocks = item.Stock.id;
                productsDTO.Add(TransitionDAO);
            }
            Console.WriteLine("Fim");
            return productsDTO;
        }
    }

    public static object getProductById(int id){
          using(var context = new DAOContext()){
            var stocks = context.stocks.Include(i=> i.store).Include(i=>i.product).FirstOrDefault(x=> x.product.id == id);
            var TransitionDAO = new DTO.ProductResponseDTO();

           
            TransitionDAO.Id = stocks.product.id;
            TransitionDAO.bar_code = stocks.product.bar_code;
            TransitionDAO.name = stocks.product.name;
            TransitionDAO.image = stocks.product.image;
            TransitionDAO.description = stocks.product.description;
            TransitionDAO.Quantity = stocks.quantity;
            TransitionDAO.Unit_price = stocks.unit_price;  
            TransitionDAO.CNPJ = stocks.store.CNPJ;
            TransitionDAO.IdStocks = stocks.id;
            TransitionDAO.StoreCNPJ = int.Parse(stocks.store.CNPJ);
            
            return TransitionDAO;
        }

    }

    public static object getProductByBarCode(string barCode){
          using(var context = new DAOContext()){
            var product = context.products.FirstOrDefault(x=> x.bar_code == barCode);
 
            return product;
        }

    }


    public ProductDTO convertModelToDTO()
    {
        var productDTO = new ProductDTO();

        productDTO.name = this.name;

        productDTO.bar_code = this.bar_code;
        productDTO.image = this.image;
        productDTO.description = this.description;

        return productDTO;
    }




    public void setName(String name) { this.name = name; }
    public String getName() { return this.name; }


    public void setBarCode(String bar_code) { this.bar_code = bar_code; }
    public String getBarCode() { return this.bar_code; }


    public void setImage(String Image) { this.image = Image;}
    public String getImage() { return this.image; }


    public void setDescription(String Description){this.description = Description;}
    public String getDescription(){ return this.description;}
    public Boolean validateObject()
    {
        if (this.getName() == null) return false;
        if (this.getBarCode() == null) return false;
        if(this.getImage() == null) return false;
        if(this.getDescription() == null) return false;
        return true;
    }


    public static Product convertDAOToModel( DAO.Product productDAO){
        var product = new Product();
        product.setName(productDAO.name);
        product.setBarCode(productDAO.bar_code);
        product.setImage(productDAO.image);
        product.setDescription(productDAO.description);
        return product;

    }
}