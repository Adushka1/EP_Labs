using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Phones.Interfaces;

namespace EPAM_3.Phones
{
    public class Phone : IPhone
    {
        public int Id { get; }

        public Phone(int id)
        {
            Id = id;
        }

        public int CompareTo(IPhone other)
        {
            return Id.CompareTo(other.Id);
        }

        public override bool Equals(object obj)
        {
            return Id.Equals((obj as IPhone)?.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
