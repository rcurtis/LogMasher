using System.Collections.Generic;

namespace LogMasher.Library
{
    public interface IInput
    {
        IEnumerable<string> GetLines();
    }
}