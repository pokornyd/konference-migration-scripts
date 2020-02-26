using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Konference.Interfaces
{
    interface IMigrator
    {
        Task Migrate();
    }
}
