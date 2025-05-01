    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
namespace Project_Manager.Data
{


    public partial class Catalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Catalog()
        {
            this.Card = new ObservableCollection<Card>();
        }
    
        public int Catalog_ID { get; set; }
        public int Board_ID { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
    
        public virtual Board Board { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Card> Card { get; set; }
    }
}
