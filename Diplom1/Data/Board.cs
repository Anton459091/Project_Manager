using Project_Manager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Board
{
    public Board()
    {
        Catalogs = new HashSet<Catalog>();
        Users = new HashSet<User>();
    }

    [Key]
    public int Board_ID { get; set; }

    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Position { get; set; }


    public virtual User User { get; set; }
    public virtual Catalog Catalog { get; set; }

    public virtual ICollection<Catalog> Catalogs { get; set; }
    public virtual ICollection<User> Users { get; set; }
}
