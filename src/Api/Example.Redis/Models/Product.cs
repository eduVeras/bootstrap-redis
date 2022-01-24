namespace Example.Redis.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set;}
    }
}
