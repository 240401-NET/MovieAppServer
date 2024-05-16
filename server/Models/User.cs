using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace server.Models;

public partial class User : IdentityUser{ 
    public int Age {get;set;}
    public List<int>? MovieIds { get; set; }
    public Movie? Movie { get; set; }
}