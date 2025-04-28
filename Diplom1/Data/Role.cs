using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Project_Manager.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role
    {
        public Role()
        {
            this.User = new HashSet<User>();
        }
    
        public int Role_ID { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<User> User { get; set; } = new ObservableCollection<User>();
    }
}
