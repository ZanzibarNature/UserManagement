namespace UserAPI.Domain.DTO
{
    public class RetrieveUserDTO : UserBase
    {
        public int ID { get; set; }

        public DateTime LastActive { get; set; }
    }
}
