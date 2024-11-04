using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Archive.Repositores
{
    internal interface IPublicServices
    {
        bool Login();
        bool Logout();
        void LoadConfig();
    }
}
