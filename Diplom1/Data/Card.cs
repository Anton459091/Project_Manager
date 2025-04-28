using Project_Manager.Data;
using System.ComponentModel.DataAnnotations;

public partial class Card
{
    [Key]
    public int Card_ID { get; set; }

    public int Catalog_ID { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public int Position { get; set; }

    public virtual Catalog Catalog { get; set; }
}
