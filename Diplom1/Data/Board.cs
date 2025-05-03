using Project_Manager.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data.Entity;
using System.Collections.ObjectModel;

public partial class Board
{
    public Board()
    {
        this.Catalog = new ObservableCollection<Catalog>();
        this.User = new HashSet<User>();
    }

    [Key]
    public int Board_ID { get; set; }

    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Position { get; set; }

    public virtual ObservableCollection<Catalog> Catalog { get; set; }
    public virtual ICollection<User> User { get; set; }
}