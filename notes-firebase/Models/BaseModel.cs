namespace notes_firebase.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
