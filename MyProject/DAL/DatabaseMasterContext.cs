
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyProject.Models;

namespace MyProject.DAL
{
    public class DatabaseMasterContext : DbContext
    {
        public DatabaseMasterContext() : base("DefaultConnection")
        {
            
        }
    }
}