namespace Model;
using Interfaces;
using DAO;
using DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore;


public class Store: IValidateDataObject, IDataController<StoreDTO, Store>
{
    private String name;
    private String CNPJ;
    private Owner owner;
    private List<Purchase> purchases = new List<Purchase>();
    public List<StoreDTO> storeDTO = new List<StoreDTO>();

    private Store(Owner owner) { this.owner = owner; }

    public Store(){}

    public static Store convertDTOToModel(StoreDTO obj)
    {
        // var store = new Store(Owner.convertDTOToModel(obj.owner));
        var store = new Store();
        store.setName(obj.name);
        store.setCNPJ(obj.CNPJ);       
        foreach(var purch in obj.purchase){
            store.addNewPurchase(Purchase.convertDTOToModel(purch));
        }

        return store;
    }

    public void delete()
    {

    }
    public int save(int ownerid)
    {
        
        var id = 0;      
        using(var context = new DAOContext())
        {
            
            var ownerDAO = context.owners.FirstOrDefault(o => o.id == ownerid);            

            var store = new DAO.Store{
                name = this.name,
                CNPJ = this.CNPJ,
                owner = ownerDAO
            };

            context.stores.Add(store);
            context.Entry(store.owner).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            context.SaveChanges();

            id = store.id;

        }
         return id;
    }

    public void update(StoreDTO obj)
    {

    }

    public StoreDTO findById(int id)
    {
        return new StoreDTO();
    }



    public static object find(string CNPJ){

        using (var context = new DAOContext())
        {

            var storeDAO = context.stores. Include(i => i.owner).Include(i => i.owner.address).FirstOrDefault(o => o.CNPJ == CNPJ);

            return new
            {
                name = storeDAO.name,
                CNPJ = storeDAO.CNPJ,
                owner = storeDAO.owner
            };
        };
    }

    public static Store findStore(string CNPJ){

        using (var context = new DAOContext())
        {

            var storeDAO = context.stores.Include(i => i.owner).Include(i => i.owner.address).FirstOrDefault(o => o.CNPJ == CNPJ);

           
            Store storeModel = Model.Store.convertDTOToModel(Model.Store.ConvertDaoToDTO(storeDAO));

            return storeModel;
        };
    }

    public int getID(){
         using(var context = new DAOContext()){
            var ID  = context.stores.FirstOrDefault(p => p.CNPJ == this.CNPJ).id;
            Console.WriteLine(ID);
            return ID;
        }
    }

    public static List<object> findAll(){
    using (var context = new DAOContext())
        {

            var storeDAO = context.stores. Include(i => i.owner).Include(i => i.owner.address);

            List<object> stores = new List<object>();
            foreach(object store in storeDAO){
                stores.Add(store);
            }

            return stores;

            
        };
    }

    public static List<dynamic> findAllByOwner(string document){
    using (var context = new DAOContext())
        {

            var storeDAO = context.stores.Include(i => i.owner).Include(i => i.owner.address).Where(c => c.owner.document == document);

            List<object> stores = new List<dynamic>();
            foreach(object store in storeDAO){
                stores.Add(store);
            }

            return stores;

            
        };
    }


    public static int GetOwnerId(Owner ownerModel){

         using(var context = new DAOContext())
        {

            Console.WriteLine(ownerModel.getDocument());
            var ownerDAO = context.owners.FirstOrDefault(o => o.document == ownerModel.getDocument()); 
            return ownerDAO.id;
        }
    }


    
    public List<StoreDTO> getAll()
    {        
        return this.storeDTO;      
    }

   
    public StoreDTO convertModelToDTO()
    {
        var storeDTO = new StoreDTO();

        storeDTO.name = this.name;
        storeDTO.CNPJ = this.CNPJ;
        storeDTO.owner = this.owner.convertModelToDTO();

        foreach(var purch in this.purchases){
            storeDTO.purchase.Add(purch.convertModelToDTO());
        }

        return storeDTO;
    }


    public static StoreDTO ConvertDaoToDTO(DAO.Store storeDao)
    {
        var storeDTO = new StoreDTO();
        storeDTO.name = storeDao.name;
        storeDTO.CNPJ = storeDao.CNPJ;
        storeDTO.owner = Owner.ConvertDaoToDTO(storeDao.owner);

        return storeDTO;
    }


   public Boolean validateObject()
    {             
        
        if (this.getName() == null) return false;
        
        if (this.getCNPJ() == null) return false;
        //if(this.owner.validateObject())return false;
        
       
        return true;
    }


    public String getName() { return this.name; }
    public void setName(string name) { this.name = name; }

    public String getCNPJ() { return this.CNPJ; }
    public void setCNPJ(String CNPJ) { this.CNPJ = CNPJ; }

    public Owner getOwner() { return this.owner; }


    public List<Purchase> getPurchases() { return purchases; }
    public void addNewPurchase(Purchase purchase)
    { //add list to wishlist
        if (!getPurchases().Contains(purchase))
        { // se a lista nao conter o produto na lusta
            this.purchases.Add(purchase);// adicionar ela na lista
        }
    }


}
