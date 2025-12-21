namespace SneakerShop.Models
{
    public class Collection
    {
        #region Class Fields
        
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        
        #endregion
        
        #region Constructors
        
        public Collection(string name, string brand, string description)
        {
            Name = name;
            Brand = brand;
            Description = description;
        }
        
        #endregion
        
        //TODO: Make an extent for the class
        //TODO: Implement the associations
    }
}