using AccountService.DTO.Address;

namespace AccountService.Handlers.Address
{
    public interface IAddressHandler
    {
        public Task<AddressDTO> CreateAddress(CreateAddressDTO createAddressDTO);
        public Task<List<AddressDTO>> GetAddresses();
        public Task<AddressDTO> GetAddressById(int idAddress);
        public Task<AddressDTO> UpdateAddress(UpdateAddressDTO updateAddressDTO, int idAddress);
        public Task<bool> DeleteAddress(int idAddress);

    }
}
