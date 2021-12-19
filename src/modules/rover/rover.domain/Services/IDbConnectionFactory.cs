using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rover.domain.Services
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();
    }
}
