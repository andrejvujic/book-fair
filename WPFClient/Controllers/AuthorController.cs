using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO;
using Core.Models;

namespace WPFClient.Controllers
{
    public class AuthorController: Controller<Author>
    {
        public AuthorController(AuthorDAO authorDAO): base(authorDAO) { }

    }
}
