using System.Collections.Generic;
using System.IO;

namespace LogMasher.Library
{
    public interface IInput
    {
        IEnumerable<string> GetLines();
    }

    public class FileInput : IInput
    {
        private readonly string _path;

        public FileInput(string path)
        {
            if(!File.Exists(path))
                throw new FileNotFoundException(path);

            _path = path;
        }

        public IEnumerable<string> GetLines()
        {
            return File.ReadLines(_path);
        }
    }

    public class MemoryInput : IInput
    {
        public List<string> Lines { get; private set; }

        public MemoryInput(List<string> lines)
        {
            Lines = lines ?? new List<string>();
        }

        public IEnumerable<string> GetLines()
        {
            return Lines;
        }
    }
}