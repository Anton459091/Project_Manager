using Project_Manager.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Catalog
{
    public Catalog()
    {
        Cards = new HashSet<Card>();
    }

    [Key]
    public int Catalog_ID { get; set; }

    public string Title { get; set; }
    public int Position { get; set; }

    // Связь с Board
    public int Board_ID { get; set; }
    public virtual Board Board { get; set; }

    public virtual Card Card { get; set; }

    public virtual ICollection<Card> Cards { get; set; }
}
