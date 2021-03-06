namespace Model;

using Interfaces;
using DAO;
using DTO;
using Microsoft.EntityFrameworkCore;

public class Address : IValidateDataObject, IDataController<AddressDTO, Address>
{

    private int id;
    private String street ;
    private String city ;
    private String state ;
    private String country ;
    private String postal_code ;
    public List<AddressDTO> addressDTO = new List<AddressDTO>();

    public Address(String street, String city, String state, String country, String postal_code)
    {
        this.street = street;
        this.city = city;
        this.state = state;
        this.country = country;
        this.postal_code = postal_code;
    }

    public static Address convertDTOToModel(AddressDTO obj)
    {
        Address address = new Address(obj.street, obj.city, obj.state, obj.country, obj.postal_code);
        address.id = obj.id;
        return address;
    }

    public static AddressDTO ConvertDAOToDTO(DAO.Address addressDAO){
        var addressDTO = new AddressDTO();
        addressDTO.postal_code = addressDAO.postal_code;
        addressDTO.id = addressDAO.id;
        addressDTO.street = addressDAO.street;
        addressDTO.state = addressDAO.state;
       addressDTO.city = addressDAO.city;
       addressDTO.country = addressDAO.country;
        return addressDTO;
    }
    public void delete()
    {
        using( var context =  new DAOContext()){
            var addressToRemove =  context.addresses.FirstOrDefault(x => x.id == this.id);
            context.addresses.Remove(addressToRemove);
            context.SaveChanges();
        }

    }
    public int save()
    {
        var id = 0;

        using(var context = new DAOContext())
        {
            var address = new DAO.Address{
                street = this.street,
                city = this.city,
                state = this.state,
                country = this.country,
                postal_code = this.postal_code
            };



            context.addresses.Add(address);

            context.SaveChanges();

            id = address.id;

        }
         return id;
    }

    public void update(AddressDTO obj)
    {
        using(var context = new DAOContext()){
            var address = context.addresses.FirstOrDefault(i=> i.id == obj.id);

            if( address != null){
                context.Entry(address).State = EntityState.Modified;
                address.street = obj.street;
                address.city = obj.city;
                address.state = obj.state;
                address.country = obj.country;
                address.postal_code = obj.postal_code;
            }
            context.SaveChanges();
        }
    }

    public AddressDTO findById(int id)
    {

        return new AddressDTO();
    }

    public List<AddressDTO> getAll()
    {
        return this.addressDTO;
    }


    public AddressDTO convertModelToDTO()
    {
        var addressDTO = new AddressDTO();

        addressDTO.street = this.street;

        addressDTO.state = this.state;

        addressDTO.city = this.city;

        addressDTO.country = this.country;

        addressDTO.postal_code = this.postal_code;

        return addressDTO;
    }

    public Boolean validateObject()
    {

        if (this.getCity() == null) return false;
        if (this.getCountry() == null) return false;
        if (this.getStreet() == null) return false;
        if (this.getState() == null) return false;
        if (this.getPostalCode() == null) return false;
        return true;
    }


    public void setStreet(String street) { this.street = street; }
    public String getStreet() { return this.street; }


    public void setCity(String city) { this.city = city; }
    public String getCity() { return this.city; }


    public void setState(String state) { this.state = state; }
    public String getState() { return this.state; }


    public void setCountry(String country) { this.country = country; }
    public String getCountry() { return this.country; }


    public void setPostalCode(String postal_code) { this.postal_code = postal_code; }
    public String getPostalCode() { return this.postal_code; }


}










