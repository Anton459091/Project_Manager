// UserBoard.cs
using Project_Manager.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System;

public class UserBoard
{
    [Key, Column(Order = 0)]
    [ForeignKey("User")]
    public int Users_ID { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey("Board")]
    public int Boards_ID { get; set; }

    // Навигационные свойства
    public virtual User User { get; set; }
    public virtual Board Board { get; set; }

    // Дополнительные свойства связи (если нужно)
    public DateTime JoinedDate { get; set; } = DateTime.Now;
}