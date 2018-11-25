using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM_3.Phones.Interfaces
{
    public interface IPhone : IComparable<IPhone>
    {
        int Id { get; }
    }
}
