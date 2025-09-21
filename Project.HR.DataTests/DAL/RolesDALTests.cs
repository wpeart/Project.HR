using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project.HR.Data.DAL;
using Project.HR.Data.Models;
using Project.HR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Data.DAL.Tests
{
    [TestClass()]
    public class RolesDALTests
    {
        private readonly HRDbContext _context;

        [TestMethod()]
        public void GetAllRolesAsyncTest()
        {

            IRoleService roleService = new RolesDAL(_context);

        }
    }
}