using AccountService.Entities;
using AccountService.DTO.Address;

namespace AccountService.Mappers
{
    public static class AddressMapper
    {
        public static Address ToAddress(AddressDTO addressDTO)
        {
            return new Address 
            {
                IdAddress = addressDTO.IdAddress,
                City = addressDTO.City,
                Country = addressDTO.Country,
                Street = addressDTO.Street,
                StreetNumber = addressDTO.StreetNumber,
                PostalCode = addressDTO.PostalCode,
            };
        }

        public static Address ToAddress(CreateAddressDTO createAddressDTO)
        {
            return new Address
            {
                City = createAddressDTO.City,
                Country = createAddressDTO.Country,
                Street = createAddressDTO.Street,
                StreetNumber = createAddressDTO.StreetNumber,
                PostalCode = createAddressDTO.PostalCode,
            };
        }

        public static Address ToAddress(UpdateAddressDTO updateAddressDTO)
        {
            return new Address
            {
                City = updateAddressDTO.City,
                Country = updateAddressDTO.Country,
                Street = updateAddressDTO.Street,
                StreetNumber = updateAddressDTO.StreetNumber,
                PostalCode = updateAddressDTO.PostalCode,
            };
        }

        public static AddressDTO ToAddressDTO(Address address)
        {
            return new AddressDTO
            {
                IdAddress = address.IdAddress,
                City = address.City,
                Country = address.Country,
                Street = address.Street,
                StreetNumber = address.StreetNumber,
                PostalCode = address.PostalCode,
            };
        }
    }
}
