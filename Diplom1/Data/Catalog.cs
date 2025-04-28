using Project_Manager.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

public partial class Catalog
{
    public Catalog()
    {
        Card = new HashSet<Card>();
    }

    [Key]
    public int Catalog_ID { get; set; }

    public string Title { get; set; }
    public int Position { get; set; }


    // Связь с Board
    public int Board_ID { get; set; }
    public virtual Board Board { get; set; }

    // Коллекция карточек
    public virtual ICollection<Card> Card { get; set; }
}
